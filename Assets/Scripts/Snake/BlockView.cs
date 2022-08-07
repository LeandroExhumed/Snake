using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BlockView : MonoBehaviour
    {
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void SetParent (Transform parent) => transform.SetParent(parent);
    }
}