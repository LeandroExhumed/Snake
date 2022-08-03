using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ISnakeModel : ICollector
    {
        event Action<ISnakeModel, Vector2Int> OnPositionChanged;
        Vector2Int Position { get; }

        void Initialize (Vector2Int initialPosition);
        void LookTo (Vector2Int direction);
        void Grow ();
        void Tick ();
    }
}