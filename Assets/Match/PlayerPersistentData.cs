using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;

namespace LeandroExhumed.SnakeGame.Match
{
    [Serializable]
    public class PlayerPersistentData
    {
        public int Number { get; set; }
        public InputPersistentData Input { get; set; }
        public SnakePersistentData Snake { get; set; }
        
        public PlayerPersistentData (int number, InputPersistentData input, SnakePersistentData snake)
        {
            Number = number;
            Input = input;
            Snake = snake;
        }
    }
}