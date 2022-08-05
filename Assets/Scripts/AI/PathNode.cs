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

        public PathNode CameFromNode { get; set; }

        public PathNode (int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void CalculateFCost ()
        {
            FCost = GCost + HCost;
        }
    }
}