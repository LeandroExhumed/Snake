using LeandroExhumed.SnakeGame.Collectables;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BlockModel : CollectableModel, IBlockModel
    {
        public event Action<Transform> OnAttached;
        public event Action OnBenefitRemoved;

        public int ID => data.ID;
        public bool IsAttached { get; private set; }
        public bool HasBenefit { get; private set; }

        private readonly BlockData data;

        public BlockModel (BlockData data)
        {
            this.data = data;
        }

        public override void Initialize (Vector2Int startPosition, ICollector owner = null)
        {
            base.Initialize(startPosition, owner);
            HasBenefit = true;
        }

        public void Attach (Transform owner)
        {
            IsAttached = true;
            OnAttached?.Invoke(owner);
        }

        public void RemoveBenefit ()
        {
            HasBenefit = false;
            OnBenefitRemoved?.Invoke();
        }
    }
}