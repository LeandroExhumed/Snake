using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class CollectableFacade : MonoBehaviour, ICollectableModel
    {
        public event Action OnCollected
        {
            add => model.OnCollected += value;
            remove => model.OnCollected -= value;
        }

        private ICollectableModel model;
        private CollectableController controller;

        [Inject]
        public void Constructor (ICollectableModel model, CollectableController controller)
        {
            this.model = model;
            this.controller = controller;

            controller.Setup();
        }

        public void TryBeCollected (Collider other)
        {
            model.TryBeCollected(other);
        }

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}