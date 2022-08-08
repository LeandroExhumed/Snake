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

        protected ICollector owner;

        public virtual void Initialize (Vector2Int startPosition, ICollector owner = null)
        {
            Position = startPosition;
            this.owner = owner;
        }

        public virtual void BeCollected ()
        {
            OnCollected?.Invoke();
        }

        public virtual void ApplyEffect () { }
    }
}