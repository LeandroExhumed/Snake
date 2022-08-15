using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Input;
using System;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeController : IDisposable
    {
        private readonly ISnakeModel model;
        private readonly SnakeView view;

        private IGameInputModel Input
        {
            get => input;
            set
            {
                if (input != null)
                {
                    input.OnMovementRequested -= HandleMovementInputPerformed;
                }

                input = value;
                input.OnMovementRequested += HandleMovementInputPerformed;
            }
        }

        private IGameInputModel input;

        public SnakeController (ISnakeModel model, SnakeView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Setup ()
        {
            model.OnInitialized += HandleInitialized;
            model.OnBlockAttached += HandleBlockAttached;
            model.OnHit += HandleHit;
            model.OnDestroyed += HandleDestroyed;
            view.OnUpdate += HandleViewUpdate;
        }

        private void HandleInitialized (IGameInputModel input)
        {
            Input = input;
        }

        private void HandleBlockAttached (IBlockModel block)
        {
            block.Attach(view.Transform);
        }

        private void HandleHit (ISnakeModel model, IBlockModel timeTravelBlock)
        {
            view.PlayBlinkingEffect();
        }

        private void HandleDestroyed ()
        {
            view.Destroy();
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
            model.OnInitialized -= HandleInitialized;
            model.OnBlockAttached -= HandleBlockAttached;
            model.OnHit -= HandleHit;
            model.OnDestroyed -= HandleDestroyed;
            view.OnUpdate -= HandleViewUpdate;
            Input.OnMovementRequested -= HandleMovementInputPerformed;
        }
    }
}