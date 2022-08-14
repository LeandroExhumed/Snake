using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchView : MonoBehaviour
    {
        public event Action OnBlockFocusOver;
        public event Action OnRewindEffectOver;

        [SerializeField]
        private TextMeshProUGUI winnerMessageText;
        [SerializeField]
        private Vector2 guideOffset;

        [SerializeField]
        private float cameraSizeDiffOnFocus = 15f;
        [SerializeField]
        private float cameraMovementSpeed = 0.05f;
        [SerializeField]
        private float shakeDuration = 0.15f;
        [SerializeField]
        private Vector3 shakeMagnitude = new(0.1f, 0.1f);
        [SerializeField]
        private new Camera camera;
        [SerializeField]
        private CameraShake cameraShake;

        [SerializeField]
        private float rewindScreenDuration = 3f;
        [SerializeField]
        private GameObject rewindScreen;

        [SerializeField]
        private Transform canvas;

        private readonly Dictionary<int, TextMeshProUGUI> guides = new();

        private const float SLOW_MOTION_TIME_SCALE = 0.1F;
        private const float SLOW_MOTION_DURATION = 2F;

        private Vector3 originalCameraPosition;
        private float originalCameraSize;

        private void Start ()
        {
            originalCameraPosition = camera.transform.position;
            originalCameraSize = camera.orthographicSize;
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

        public void FocusBlockHit (Vector2Int position)
        {
            Vector3 targetPosition = new(position.x, position.y);
            float targetCameraSize = camera.orthographicSize - cameraSizeDiffOnFocus;
            StartCoroutine(SmoothLerp(targetPosition, targetCameraSize, cameraMovementSpeed));
        }

        public void PlayRewindEffect ()
        {
            StartCoroutine(RewindEffectRoutine());
        }

        public void LeaveFocus ()
        {
            camera.transform.position = originalCameraPosition;
            camera.orthographicSize = originalCameraSize;
        }

        private TextMeshProUGUI GetNewGuide ()
        {
            return Resources.Load<TextMeshProUGUI>("Guide");
        }

        private IEnumerator DeathEffectRoutine ()
        {
            Time.timeScale = SLOW_MOTION_TIME_SCALE;
            cameraShake.Shake(shakeDuration, shakeMagnitude);
            yield return new WaitForSecondsRealtime(SLOW_MOTION_DURATION);
            Time.timeScale = 1f;
        }

        private IEnumerator SmoothLerp (Vector3 targetPosition, float targetSize, float time)
        {
            Vector3 startingPos = camera.transform.position;
            float startingSize = camera.orthographicSize;
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                camera.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / time));
                camera.orthographicSize = Mathf.Lerp(startingSize, targetSize, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSecondsRealtime(SLOW_MOTION_DURATION);

            OnBlockFocusOver?.Invoke();
        }

        private IEnumerator RewindEffectRoutine ()
        {
            rewindScreen.SetActive(true);
            yield return new WaitForSeconds(rewindScreenDuration);
            OnRewindEffectOver?.Invoke();
            rewindScreen.SetActive(false);
        }
    }
}