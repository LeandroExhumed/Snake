using LeandroExhumed.SnakeGame.Grid;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class PathNodeModel : IPathNodeModel
    {
        public event Action<INodeModel, Vector2Int> OnPositionChanged;

        public Vector2Int Position
        {
            get => position;
            set
            {
                position = value;
                OnPositionChanged?.Invoke(this, value);
            }
        }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;
        public bool IsWalkable { get; }

        public IPathNodeModel CameFrom { get; set; }

        private Vector2Int position;

        public PathNodeModel (Vector2Int position, bool isWalkable)
        {
            Position = position;
            IsWalkable = isWalkable;
        }
    }
}