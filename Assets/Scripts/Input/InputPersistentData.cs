using System;

namespace LeandroExhumed.SnakeGame.Input
{
    [Serializable]
    public class InputPersistentData
    {
        public char LeftKey { get; private set; }
        public char RightKey { get; private set; }
        
        public InputPersistentData (char leftKey, char rightKey)
        {
            LeftKey = leftKey;
            RightKey = rightKey;
        }
    }
}