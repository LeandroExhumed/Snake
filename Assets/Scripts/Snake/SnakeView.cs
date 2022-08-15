using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeView : MonoBehaviour
    {
        public event Action OnUpdate;

        public Transform Transform => transform;
        public AudioClip HitSound => hitSound;

        [SerializeField]
        private AudioClip hitSound;

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
            SpriteRenderer[] bodyPartSprites = GetComponentsInChildren<SpriteRenderer>();
            Color[] originalColors = bodyPartSprites.Select(x => x.color).ToArray();
            int blinksExecuted = 0;

            while (blinksExecuted <= BLINK_AMOUNT)
            {
                for (int i = 0; i < bodyPartSprites.Length; i++)
                {
                    bodyPartSprites[i].color = Color.red;
                }
                yield return new WaitForSeconds(BLINK_RATE);
                for (int i = 0; i < bodyPartSprites.Length; i++)
                {
                    bodyPartSprites[i].color = originalColors[i];
                }
                yield return new WaitForSeconds(BLINK_RATE);

                blinksExecuted++;
            }

            Destroy();
        }
    }
}