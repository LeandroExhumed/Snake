using System;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeController : IDisposable
    {
        private readonly ISnakeModel model;
        private readonly SnakeView view;

        private readonly IMovementRequester input;

        public SnakeController (ISnakeModel model, SnakeView view, IMovementRequester input)
        {
            this.model = model;
            this.view = view;
            this.input = input;
        }

        public void Setup ()
        {
            model.OnBlockAttached += HandleBlockAttached;
            model.OnHit += HandleHit;
            view.OnUpdate += HandleViewUpdate;
            input.OnMovementRequested += HandleMovementInputPerformed;
        }

        private void HandleBlockAttached (IBlockModel block)
        {
            block.Attach(view.Transform);
        }

        private void HandleHit ()
        {
            view.PlayBlinkingEffect();
        }

        private void HandleViewUpdate ()
        {
            model.Tick();
        }

        private void HandleMovementInputPerformed (int value)
        {

            model.LookTo(value);
        }

        public void Dispose ()
        {
            model.OnHit -= HandleHit;
            view.OnUpdate -= HandleViewUpdate;
            input.OnMovementRequested -= HandleMovementInputPerformed;
        }
    }
}