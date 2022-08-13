using LeandroExhumed.SnakeGame.Block;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    [Serializable]
    public class SnakePersistentData
    {
        public int ID { get; set; }
        public Vector2Int Position { get; set; }
        public Vector2Int Direction { get; set; }
        public float TimeToMove { get; set; }
        public BlockPersistentData[] Blocks { get; set; }
    }
}