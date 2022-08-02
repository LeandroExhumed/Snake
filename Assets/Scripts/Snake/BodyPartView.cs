using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BodyPartView : MonoBehaviour
    {
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}