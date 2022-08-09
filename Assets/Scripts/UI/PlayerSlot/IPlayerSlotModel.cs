using LeandroExhumed.SnakeGame.Input;
using System;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public interface IPlayerSlotModel
    {
        SlotState State { get; }

        event Action<IMovementRequester> OnEnabled;
        event Action<int[]> OnSnakeShown;
        event Action<int, int, IMovementRequester> OnSnakeSelected;
        event Action OnDisabled;

        void Initialize (int playerNumber);
        void Enable (IMovementRequester input);
        void ShowPreviousSnake ();
        void ShowNextSnake ();
        void Confirm ();
        void Disable ();
    }
}