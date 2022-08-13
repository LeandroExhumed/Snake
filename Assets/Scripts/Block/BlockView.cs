using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void SetParent (Transform parent) => transform.SetParent(parent);

        public void SetNoBenefitVisual ()
        {
            Color normal = spriteRenderer.color;
            normal.a /= 2;
            spriteRenderer.color = normal;
        }

        public void Destroy () => Destroy(gameObject);
    }
}