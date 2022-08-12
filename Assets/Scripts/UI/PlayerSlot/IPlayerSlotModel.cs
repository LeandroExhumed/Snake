using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public interface IPlayerSlotModel
    {
        SlotState State { get; }

        event Action<IPlayerInput> OnEnabled;
        event Action<int[]> OnSnakeShown;
        event Action<int, int, IPlayerInput> OnSnakeSelected;
        event Action OnDisabled;

        void Initialize (int playerNumber);
        void Enable (IPlayerInput input);
        void Enable (ISnakeModel snake);
        void ShowPreviousSnake ();
        void ShowNextSnake ();
        void Confirm ();
        void Disable ();
    }
}