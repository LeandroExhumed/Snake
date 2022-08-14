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
        public event Action OnRewindEffectOver;

        [SerializeField]
        private TextMeshProUGUI winnerMessageText;
        [SerializeField]
        private Vector2 guideOffset;

        [SerializeField]
        private float shakeDuration = 0.15f;
        [SerializeField]
        private Vector3 shakeMagnitude = new(0.1f, 0.1f);
        [SerializeField]
        private CameraShake cameraShake;
        [SerializeField]
        private Transform canvas;

        private readonly Dictionary<int, TextMeshProUGUI> guides = new();

        private const float SLOW_MOTION_TIME_SCALE = 0.1F;
        private const float SLOW_MOTION_DURATION = 2F;

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

        public void PlayRewindEffect ()
        {
            StartCoroutine(RewindEffectRoutine());
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
        
        private IEnumerator RewindEffectRoutine ()
        {
            yield return new WaitForSeconds(2f);
            OnRewindEffectOver?.Invoke();
        }
    }
}