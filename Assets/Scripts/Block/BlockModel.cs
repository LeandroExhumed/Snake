using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    public class BlockModel : IBlockModel
    {
        public event Action<INode, Vector2Int> OnPositionChanged;
        public event Action<IBlockModel> OnCollected;
        public event Action<Transform> OnAttached;
        public event Action OnBenefitRemoved;
        public event Action OnDestroyed;

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
        public bool HasBenefit
        {
            get => hasBenefit;
            private set
            {
                hasBenefit = value;
                if (!value)
                {
                    OnBenefitRemoved?.Invoke();
                }
            }
        }

        protected ICollector owner;

        private readonly BlockData data;

        private Vector2Int position;
        private bool hasBenefit;

        public BlockModel (BlockData data)
        {
            this.data = data;
        }

        public virtual void Initialize (Vector2Int startPosition, bool hasBenefit, ICollector owner = null)
        {
            Position = startPosition;
            HasBenefit = hasBenefit;
            this.owner = owner;
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
        }

        public void Destroy ()
        {
            OnDestroyed?.Invoke();
        }

        public bool IsEqual (IBlockModel other)
        {
            return this == other;
        }
    }
}