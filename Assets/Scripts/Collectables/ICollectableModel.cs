using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public interface ICollectableModel
    {
        event Action OnCollected;

        void TryBeCollected (Collider other);
    }
}