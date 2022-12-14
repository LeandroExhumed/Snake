using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Linq;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public class PlayerSlotModel : IPlayerSlotModel
    {
        public event Action<IPlayerInputModel> OnEnabled;
        public event Action<int[]> OnSnakeShown;
        public event Action<int, int, IPlayerInputModel> OnSnakeSelected;
        public event Action OnDisabled;

        public SlotState State { get; private set; }

        private int playerNumber;

        private int currentSnake = 0;
        private IPlayerInputModel input;

        private readonly SnakeData[] snakes;

        public PlayerSlotModel (SnakeData[] snakes)
        {
            this.snakes = snakes;
        }

        public void Initialize (int playerNumber)
        {
            this.playerNumber = playerNumber;
        }

        public void Enable (ISnakeModel snake, IPlayerInputModel input)
        {
            this.input = input;

            State = SlotState.Playing;
            currentSnake = Array.FindIndex(snakes, x => x.ID == snake.ID);
            ShowSnake();

            OnEnabled?.Invoke(input);
        }

        public void Enable (IPlayerInputModel input)
        {
            this.input = input;

            State = SlotState.Selection;
            ShowSnake();

            OnEnabled?.Invoke(input);
        }

        public void ShowNextSnake ()
        {
            if (currentSnake == snakes.Length - 1)
            {
                currentSnake = 0;
            }
            else
            {
                currentSnake++;
            }

            ShowSnake();
        }

        public void ShowPreviousSnake ()
        {
            if (currentSnake == 0)
            {
                currentSnake = snakes.Length - 1;
            }
            else
            {
                currentSnake--;
            }

            ShowSnake();
        }

        public void Confirm ()
        {
            State = SlotState.Playing;
            OnSnakeSelected?.Invoke(snakes[currentSnake].ID, playerNumber, input);
        }

        public void Disable ()
        {
            State = SlotState.Waiting;
            OnDisabled?.Invoke();
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