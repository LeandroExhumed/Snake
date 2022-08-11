using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    [Serializable]
    public class BlockPersistentData
    {
        public int ID { get; private set; }
        public Vector2Int Position { get; private set; }
        public bool HasBenefit { get; private set; }

        public BlockPersistentData (int iD, Vector2Int position, bool hasBenefit)
        {
            ID = iD;
            Position = position;
            HasBenefit = hasBenefit;
        }
    }
}