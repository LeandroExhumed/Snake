using System;

namespace LeandroExhumed.SnakeGame.Input
{
    public interface IGameInputModel
    {
        event Action<int> OnMovementRequested;

        void Initialize ();
    }
}