using System;
using System.Collections;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeView : MonoBehaviour
    {
        public event Action OnUpdate;

        public Transform Transform => transform;

        private const int BLINK_AMOUNT = 5;
        private const float BLINK_RATE = 0.1F;

        public void PlayBlinkingEffect ()
        {
            StartCoroutine(SpriteBlinkingEffectRoutine());
        }

        public void Destroy () => Destroy(gameObject);

        private void Update ()
        {
            OnUpdate?.Invoke();
        }

        private IEnumerator SpriteBlinkingEffectRoutine ()
        {
            yield return new WaitForSeconds(3);

            Destroy();
        }
    }
}