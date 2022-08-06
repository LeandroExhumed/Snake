using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface IMovementRequester
    {
        event Action<int> OnMovementRequested;

        void Initialize ();
    }
}