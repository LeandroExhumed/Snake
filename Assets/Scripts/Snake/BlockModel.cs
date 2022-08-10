using LeandroExhumed.SnakeGame.Grid;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BlockModel : IBlockModel
    {
        public event Action<INode, Vector2Int> OnPositionChanged;
        public event Action<IBlockModel> OnCollected;
        public event Action<Transform> OnAttached;
        public event Action OnBenefitRemoved;

        public Vector2Int Position
        {
            get => position;
            set
            {
                position = value;
                OnPositionChanged?.Invoke(this, value);
            }
        }

        public int ID => data.ID;
        public bool IsAttached { get; private set; }
        public bool HasBenefit { get; private set; }

        protected ICollector owner;

        private readonly BlockData data;

        private Vector2Int position;

        public BlockModel (BlockData data)
        {
            this.data = data;
        }

        public virtual void Initialize (Vector2Int startPosition, ICollector owner = null)
        {
            Position = startPosition;
            this.owner = owner;
            HasBenefit = true;
        }

        public void Attach (Transform owner)
        {
            IsAttached = true;
            OnAttached?.Invoke(owner);
        }

        public virtual void BeCollected ()
        {
            OnCollected?.Invoke(this);
        }

        public virtual void ApplyEffect () { }

        public void RemoveBenefit ()
        {
            HasBenefit = false;
            OnBenefitRemoved?.Invoke();
        }
    }
}