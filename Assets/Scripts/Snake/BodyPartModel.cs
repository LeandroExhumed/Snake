using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BodyPartModel : IBodyPartModel
    {
        public event Action<Vector2Int> OnPositionChanged;

        public Vector2Int Position
        {
            get => position;
            set
            {
                position = value;
                OnPositionChanged?.Invoke(value);
            }
        }

        private Vector2Int position;

        public void Initialize (Vector2Int initialPosition)
        {
            Position = initialPosition;
        }
    }
}