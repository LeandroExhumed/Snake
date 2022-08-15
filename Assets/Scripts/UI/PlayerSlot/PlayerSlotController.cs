using LeandroExhumed.SnakeGame.Input;
using System;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public class PlayerSlotController : IDisposable
    {
        private IPlayerInputModel Input
        {
            get => input;
            set
            {
                if (input != null)
                {
                    input.OnMovementRequested -= HandleMovementRequested;
                }

                input = value;
                input.OnMovementRequested += HandleMovementRequested;
            }
        }

        private bool IsOnSelection => model.State == SlotState.Selection;

        private const string SELECT_SNAKE_MESSAGE = "Select your snake.";
        private const string PLAYING_MESSAGE = "Playing.";

        private readonly IPlayerSlotModel model;
        private readonly PlayerSlotView view;

        private IPlayerInputModel input;

        public PlayerSlotController (IPlayerSlotModel model, PlayerSlotView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Setup ()
        {
            model.OnEnabled += HandleEnabled;
            model.OnSnakeShown += HandleSnakeShown;
            model.OnSnakeSelected += HandleSnakeSelected;
            model.OnDisabled += HandleDisabled;
            view.OnConfirmationKeyPressed += HandleConfirmationKeyPressed;
        }

        private void HandleEnabled (IPlayerInputModel input)
        {
            Input = input;

            view.SetInputKeys(input.LeftKey, input.RightKey);
            view.SetKeysIconsActive(true);
            view.SetSnakePreviewActive(true);
            if (model.State == SlotState.Selection)
            {
                view.SetArrowsActive(true);
                view.SetSlotMessageText(SELECT_SNAKE_MESSAGE);
            }
            else
            {
                view.SetSlotMessageText(PLAYING_MESSAGE);
            }

            view.SetSlotMessageActive(true);
        }

        private void HandleSnakeShown (int[] blockIDs)
        {
            view.ShowSnake(blockIDs);
        }

        private void HandleSnakeSelected (int selectedSnakeID, int playerNumber, IGameInputModel input)
        {
            view.SetArrowsActive(false);
            view.SetSlotMessageText(PLAYING_MESSAGE);
        }

        private void HandleDisabled ()
        {
            view.SetKeysIconsActive(false);
            view.SetSnakePreviewActive(false);
            view.SetSlotMessageActive(false);
        }

        private void HandleConfirmationKeyPressed ()
        {
            if (!IsOnSelection)
            {
                return;
            }

            model.Confirm();
        }

        private void HandleMovementRequested (int direction)
        {
            if (!IsOnSelection)
            {
                return;
            }

            if (direction > 0)
            {
                model.ShowNextSnake();
                view.HighlightRightArrow();
            }
            else
            {
                model.ShowPreviousSnake();
                view.HighlightLeftArrow();
            }
        }

        public void Dispose ()
        {
            model.OnEnabled -= HandleEnabled;
            model.OnSnakeShown -= HandleSnakeShown;
            model.OnSnakeSelected -= HandleSnakeSelected;
            model.OnDisabled -= HandleDisabled;
            view.OnConfirmationKeyPressed -= HandleConfirmationKeyPressed;
            if (Input != null)
            {
                Input.OnMovementRequested -= HandleMovementRequested; 
            }
        }
    }
}