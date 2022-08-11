using LeandroExhumed.SnakeGame.Collectables;
using LeandroExhumed.SnakeGame.Grid;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BlockFacade : MonoBehaviour, IBlockModel
    {
        public event Action<INode, Vector2Int> OnPositionChanged
        {
            add => model.OnPositionChanged += value;
            remove => model.OnPositionChanged -= value;
        }
        public event Action<IBlockModel> OnCollected
        {
            add => model.OnCollected += value;
            remove => model.OnCollected -= value;
        }
        public event Action<Transform> OnAttached
        {
            add => model.OnAttached += value;
            remove => model.OnAttached -= value;
        }
        public event Action OnBenefitRemoved
        {
            add => model.OnBenefitRemoved += value;
            remove => model.OnBenefitRemoved -= value;
        }
        public event Action OnDestroyed
        {
            add => model.OnDestroyed += value;
            remove => model.OnDestroyed -= value;
        }

        public int ID => model.ID;
        public Vector2Int Position { get => model.Position; set => model.Position = value; }
        public bool IsAttached => model.IsAttached;
        public bool HasBenefit => model.HasBenefit;

        private IBlockModel model;
        private BlockController controller;

        [Inject]
        public void Constructor (IBlockModel model, BlockController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        public void Initialize (Vector2Int initialPosition, ICollector owner = null)
        {
            controller.Setup();
            model.Initialize(initialPosition, owner);
        }

        public void BeCollected () => model.BeCollected();

        public void ApplyEffect () => model.ApplyEffect();

        public void Attach (Transform owner) => model.Attach(owner);

        public void RemoveBenefit () => model.RemoveBenefit();

        public void Destroy () => model.Destroy();

        public override bool Equals (object other)
        {
            return model != null ? model.Equals(other) : base.Equals(other);
        }

        public override int GetHashCode ()
        {
            return model != null ? model.GetHashCode() : base.GetHashCode();
        }

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}