using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Grid
{
    public interface INode
    {
        event Action<INode, Vector2Int> OnPositionChanged;

        Vector2Int Position { get; set; }
    }
}