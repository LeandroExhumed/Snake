using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Linq;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public class PlayerSlotModel : IPlayerSlotModel
    {
        public event Action<IMovementRequester> OnEnabled;
        public event Action<int[]> OnSnakeShown;
        public event Action<int, int, IMovementRequester> OnSnakeSelected;

        public SlotState State { get; private set; }

        private int playerNumber;

        private int currentSnake = 0;
        private IMovementRequester input;

        private readonly SnakeData[] snakes;

        public PlayerSlotModel (SnakeData[] snakes)
        {
            this.snakes = snakes;
        }

        public void Initialize (int playerNumber)
        {
            this.playerNumber = playerNumber;
        }

        public void Enable (IMovementRequester input)
        {
            this.input = input;

            State = SlotState.Selection;
            OnEnabled?.Invoke(input);
        }

        public void ShowNextSnake ()
        {
            if (currentSnake == snakes.Length - 1)
            {
                currentSnake = 0;
            }

            ShowSnake();
        }

        public void ShowPreviousSnake ()
        {
            if (currentSnake == 0)
            {
                currentSnake = snakes.Length - 1;
            }

            ShowSnake();
        }

        public void Confirm ()
        {
            State = SlotState.Playing;
            OnSnakeSelected?.Invoke(snakes[currentSnake].ID, playerNumber, input);
        }

        private void ShowSnake ()
        {
            int[] blockIDs = snakes[currentSnake].StartingBlocks.Select(x => x.ID).ToArray();
            OnSnakeShown?.Invoke(blockIDs);
        }
    }

    public enum SlotState
    {
        Waiting,
        Selection,
        Playing
    }
}