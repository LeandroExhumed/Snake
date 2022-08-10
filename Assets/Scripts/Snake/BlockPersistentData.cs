using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    [Serializable]
    public class BlockPersistentData
    {
        public int ID { get; set; }
        public Vector2Int Position { get; set; }
        
        public BlockPersistentData (int iD, Vector2Int position)
        {
            ID = iD;
            Position = position;
        }
    }
}