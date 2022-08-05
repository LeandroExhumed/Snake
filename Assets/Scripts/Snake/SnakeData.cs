using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    [CreateAssetMenu(fileName = "Snake", menuName = "Data/Snake")]
    public class SnakeData : ScriptableObject
    {
        public int Size => size;
        public float Speed => speed;

        [SerializeField]
        private int size;
        [SerializeField]
        private float speed;
    }
}