using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class CollectableController : IDisposable
    {
        private readonly CollectableModel model;
        private readonly CollectableView view;

        public void Setup ()
        {
            model.OnCollected += HandleCollected;
            view.OnCollision += HandleCollision;
        }

        private void HandleCollision (Collider other)
        {
            model.TryBeCollected(other);
        }

        private void HandleCollected ()
        {
            view.Destroy();
        }

        public void Dispose ()
        {
            model.OnCollected -= HandleCollected;
            view.OnCollision -= HandleCollision;
        }
    }
}
