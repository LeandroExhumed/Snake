using System;

namespace LeandroExhumed.SnakeGame.Input
{
    public interface IGameInput
    {
        event Action<int> OnMovementRequested;

        void Initialize ();
    }
}