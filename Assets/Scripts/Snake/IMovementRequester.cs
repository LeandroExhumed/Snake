using System;

namespace LeandroExhumed.SnakeGame.Input
{
    public interface IMovementRequester
    {
        event Action<int> OnMovementRequested;

        void Initialize ();
    }
}