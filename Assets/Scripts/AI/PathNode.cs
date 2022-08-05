using LeandroExhumed.SnakeGame.Grid;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class PathNode : INode
    {
        public Vector2Int Position { get => position; set => position = value; }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }
        public bool IsWalkable { get; }

        public PathNode CameFromNode { get; set; }

        private Vector2Int position;

        public PathNode (Vector2Int position, bool isWalkable)
        {
            Position = position;
            IsWalkable = isWalkable;
        }

        public void CalculateFCost ()
        {
            FCost = GCost + HCost;
        }
    }
}