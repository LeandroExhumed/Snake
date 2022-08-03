using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class CollectableFacade : MonoBehaviour, ICollectableModel
    {
        public event Action<Vector2Int> OnPositionChanged
        {
            add => model.OnPositionChanged += value;
            remove => model.OnPositionChanged -= value;
        }
        public event Action OnCollected
        {
            add => model.OnCollected += value;
            remove => model.OnCollected -= value;
        }

        public Vector2Int Position { get => model.Position; set => model.Position = value; }

        private ICollectableModel model;
        private CollectableController controller;

        [Inject]
        public void Constructor (ICollectableModel model, CollectableController controller)
        {
            this.model = model;
            this.controller = controller;

            controller.Setup();
        }

        public void Initialize (Vector2Int startPosition) => model.Initialize(startPosition);

        public void BeCollected (ICollector collector) => model.BeCollected(collector);

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}