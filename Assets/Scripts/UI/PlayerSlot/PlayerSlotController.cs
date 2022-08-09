using LeandroExhumed.SnakeGame.Input;
using System;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public class PlayerSlotController : IDisposable
    {
        private IMovementRequester Input
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

        private readonly IPlayerSlotModel model;
        private readonly PlayerSlotView view;

        private IMovementRequester input;

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
            view.OnConfirmationKeyPressed += HandleConfirmationKeyPressed;
        }

        private void HandleEnabled (IMovementRequester input)
        {
            Input = input;

            view.ShowKeysIcons();
            view.ShowSnakePreview();
            view.SetSelectSnakeTextActive(true);
        }

        private void HandleSnakeShown (int[] blockIDs)
        {
            view.ShowSnake(blockIDs);
        }

        private void HandleSnakeSelected (int selectedSnakeID, IMovementRequester input)
        {
            view.SetSelectSnakeTextActive(false);
            view.ShowOkIcon();
        }

        private void HandleConfirmationKeyPressed ()
        {
            if (model.IsAvailable)
            {
                return;
            }

            model.Confirm();
        }

        private void HandleMovementRequested (int direction)
        {
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
            view.OnConfirmationKeyPressed -= HandleConfirmationKeyPressed;
            Input.OnMovementRequested -= HandleMovementRequested;
        }
    }
}