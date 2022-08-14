using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private Sprite brokenSprite;

        [SerializeField]
        private GameObject explosionVFX;

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void SetParent (Transform parent) => transform.SetParent(parent);

        public void SetNoBenefitVisual ()
        {
            spriteRenderer.sprite = brokenSprite;
        }

        public void PlayExplosionVFX ()
        {
            explosionVFX.SetActive(true);
        }

        public void Destroy () => Destroy(gameObject);
    }
}