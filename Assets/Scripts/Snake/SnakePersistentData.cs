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
        public bool HasTimeTravel { get; set; }
        public BlockPersistentData[] Blocks { get; set; }

        //public SnakePersistentData (
        //    int id,
        //    Vector2Int position,
        //    Vector2Int direction,
        //    float timeToMove,
        //    bool hasTimeTravel,
        //    BlockPersistentData[] blocks)
        //{
        //    ID = id;
        //    Position = position;
        //    Direction = direction;
        //    TimeToMove = timeToMove;
        //    HasTimeTravel = hasTimeTravel;
        //    Blocks = blocks;
        //}
    }
}