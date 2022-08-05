using LeandroExhumed.SnakeGame.Grid;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class PathNode : INode
    {
        public Vector2Int Position {
            get => new(x, y);
            set => throw new System.NotImplementedException();
        
        }

        public int x;
        public int y;

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }
        public bool IsWalkable { get; }

        public PathNode CameFromNode { get; set; }

        public PathNode (int x, int y, bool isWalkable)
        {
            this.x = x;
            this.y = y;
            IsWalkable = isWalkable;
        }

        public void CalculateFCost ()
        {
            FCost = GCost + HCost;
        }
    }
}