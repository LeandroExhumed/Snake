using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    [Serializable]
    public class PlayerPersistentData
    {
        public PlayerPersistentData (int number, InputPersistentData input, Vector2Int snake)
        {
            Number = number;
            Input = input;
            Snake = snake;
        }

        public int Number { get; set; }
        public InputPersistentData Input { get; set; }
        public Vector2Int Snake { get; set; }
    }
}