using System.Collections;
using UnityEngine;

namespace LeandroExhumed.SnakeGame
{
    public class CameraShake : MonoBehaviour
    {
        public void Shake (float duration, Vector3 magnitude)
        {
            StartCoroutine(ShakingProcess(duration, magnitude));
        }

        private IEnumerator ShakingProcess (float duration, Vector3 magnitude)
        {
            Vector3 originalPos = transform.localPosition;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude.x;
                float y = Random.Range(-1f, 1f) * magnitude.y;
                float z = Random.Range(-1f, 1f) * magnitude.z;

                transform.localPosition += new Vector3(x, y, z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = originalPos;
        }
    }
}