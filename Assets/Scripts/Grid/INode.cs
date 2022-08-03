using UnityEngine;

namespace LeandroExhumed.SnakeGame.Grid
{
    public interface INode
    {
        Vector2Int Position { get; set; }
    }
}