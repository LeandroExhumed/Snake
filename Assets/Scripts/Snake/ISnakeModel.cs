using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ISnakeModel
    {
        event Action<Vector2Int> OnPositionChanged;
        Vector2Int Position { get; set; }

        void Initialize (Vector2Int initialPosition, float size);
        void LookTo (Vector2Int direction);
        void Grow ();
        void IncreaseSpeed (float speedAddition);
        void ApplyBatteringRamEffect ();
        void Tick ();
    }
}