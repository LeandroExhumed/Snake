using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Grid
{
    public interface INodeModel
    {
        event Action<INodeModel, Vector2Int> OnPositionChanged;

        Vector2Int Position { get; set; }
    }
}