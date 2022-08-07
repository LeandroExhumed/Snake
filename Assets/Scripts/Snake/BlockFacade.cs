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
        public event Action OnCollected
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

        public void Initialize (Vector2Int initialPosition)
        {
            controller.Setup();
            model.Initialize(initialPosition);
        }

        public void BeCollected (ICollector collector) => model.BeCollected(collector);

        public void Attach (Transform owner) => model.Attach(owner);

        public void RemoveBenefit () => model.RemoveBenefit();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}