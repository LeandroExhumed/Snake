using LeandroExhumed.SnakeGame.Input;
using System;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public interface IPlayerSlotModel
    {
        bool IsAvailable { get; }

        event Action<IMovementRequester> OnEnabled;
        event Action<int[]> OnSnakeShown;
        event Action<int, IMovementRequester> OnSnakeSelected;

        void Enable (IMovementRequester input);
        void Confirm ();
        void ShowPreviousSnake ();
        void ShowNextSnake ();
    }
}