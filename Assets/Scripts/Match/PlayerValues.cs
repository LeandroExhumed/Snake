using LeandroExhumed.SnakeGame.Input;

namespace LeandroExhumed.SnakeGame.Match
{
    public struct PlayerValues
    {
        public int Number { get; private set; }
        public IPlayerInputModel Input { get; private set; }

        public PlayerValues (int number, IPlayerInputModel input)
        {
            Number = number;
            Input = input;
        }
    }
}