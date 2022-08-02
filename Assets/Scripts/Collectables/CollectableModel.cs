using LeandroExhumed.SnakeGame.Constants;
using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public abstract class CollectableModel : ICollectableModel
    {
        public event Action OnCollected;

        public void TryBeCollected (Collider other)
        {
            if (other.CompareTag(GameTags.PLAYER))
            {
                BeCollected(other.GetComponent<ICollector>());
                OnCollected?.Invoke();
            }
        }

        protected abstract void BeCollected (ICollector collector);
    }
}