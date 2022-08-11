using LeandroExhumed.SnakeGame.Snake;
using System.Collections.Generic;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchPersistentData
    {
        public Dictionary<int, PlayerPersistentData> Players { get; private set; }
        public List<SnakePersistentData> Snakes { get; private set; }
        public List<BlockPersistentData> Blocks { get; private set; }
        
        public MatchPersistentData ()
        {
            Players = new Dictionary<int, PlayerPersistentData>();
            Snakes = new List<SnakePersistentData>();
            Blocks = new List<BlockPersistentData>();
        }
    }
}