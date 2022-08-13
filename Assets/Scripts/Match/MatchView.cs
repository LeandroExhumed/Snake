using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchView : MonoBehaviour
    {
        public event Action OnRewindEffectOver;

        [SerializeField]
        private TextMeshProUGUI winnerMessageText;
        [SerializeField]
        private Vector2 guideOffset;

        [SerializeField]
        private Transform canvas;

        private readonly Dictionary<int, TextMeshProUGUI> guides = new();

        private const float CAMERA_SIZE_ON_ZOOM = 15F;
        private const float DYING_SNAKE_HIGHLIGHTED_DURATION = 2F;
        private const float ZOOMING_DURATION = 0.05F;
        private Vector3 originalCameraPosition;
        private float originalOrtographicSize;

        private void Start ()
        {
            originalCameraPosition = Camera.main.transform.position;
            originalOrtographicSize = Camera.main.orthographicSize;
        }

        public void SyncGuidePosition (int player, Vector2Int targetPosition)
        {
            if (!guides.ContainsKey(player))
            {
                TextMeshProUGUI guide = Instantiate(GetNewGuide(), canvas);
                guide.text = $"P{player}";
                guides.Add(player, guide);
            }

            Vector3 fixedPosition = new(targetPosition.x + guideOffset.x, targetPosition.y + guideOffset.y, 0);
            guides[player].transform.position = Camera.main.WorldToScreenPoint(fixedPosition);
        }

        public void PlayDeathEffect ()
        {
            StartCoroutine(DeathEffectRoutine());
        }

        public void ZoomIntoSnake (Vector2Int position)
        {
            StartCoroutine(SmoothLerp(new(position.x, position.y, Camera.main.transform.position.z), originalOrtographicSize - CAMERA_SIZE_ON_ZOOM, ZOOMING_DURATION));
        }

        public void SetWinnerMessage (string winner)
        {
            winnerMessageText.text = $"{winner} WON!";
        }

        public void SetWinnerMessageActive (bool value)
        {
            winnerMessageText.gameObject.SetActive(value);
        }

        public void RemoveGuide (int player)
        {
            Destroy(guides[player].gameObject);
            guides.Remove(player);
        }

        public void PlayRewindEffect ()
        {
            StartCoroutine(RewindEffectRoutine());
        }

        private TextMeshProUGUI GetNewGuide ()
        {
            return Resources.Load<TextMeshProUGUI>("Guide");
        }

        private IEnumerator SmoothLerp (Vector3 targetPosition, float targetSize, float time)
        {
            Vector3 startingPos = Camera.main.transform.position;
            float startingSize = Camera.main.orthographicSize;
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                Camera.main.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / time));
                Camera.main.orthographicSize = Mathf.Lerp(startingSize, targetSize, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator DeathEffectRoutine ()
        {
            Time.timeScale = 0.1f;
            yield return new WaitForSecondsRealtime(DYING_SNAKE_HIGHLIGHTED_DURATION);
            Time.timeScale = 1f;
            StartCoroutine(SmoothLerp(originalCameraPosition, originalOrtographicSize, ZOOMING_DURATION));
        }
        
        private IEnumerator RewindEffectRoutine ()
        {
            yield return new WaitForSeconds(2f);
            OnRewindEffectOver?.Invoke();
        }
    }
}