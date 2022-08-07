using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public abstract class CollectableModel : ICollectableModel
    {
        public event Action<INode, Vector2Int> OnPositionChanged;
        public event Action OnCollected;

        public Vector2Int Position
        {
            get => position;
            set
            {
                position = value;
                OnPositionChanged?.Invoke(this, value);
            }
        }

        private Vector2Int position;

        public void Initialize (Vector2Int startPosition)
        {
            Position = startPosition;
        }

        public virtual void BeCollected (ICollector collector)
        {
            OnCollected?.Invoke();
        }
    }
}