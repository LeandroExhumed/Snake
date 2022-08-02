using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface IBodyPartModel
    {
        event Action<Vector2Int> OnPositionChanged;

        Vector2Int Position { get; set; }
    }
}