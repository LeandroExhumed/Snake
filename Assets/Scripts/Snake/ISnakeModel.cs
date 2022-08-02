using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ISnakeModel
    {
        event Action<Vector2Int> OnPositionChanged;
        Vector2Int Position { get; }

        void Initialize (Vector2Int initialPosition);
        void LookTo (Vector2Int direction);
        void Grow ();
        void IncreaseSpeed (float speedAddition);
        void ApplyBatteringRamEffect ();
        void Tick ();
    }
}