using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface IMovementRequester
    {
        event Action<Vector2Int> OnMovementRequested;

        void Initialize ();
    }
}