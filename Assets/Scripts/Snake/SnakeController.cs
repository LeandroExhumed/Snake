using System;
using UnityEngine;

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
            view.OnUpdate += HandleViewUpdate;
            input.OnMovementRequested += HandleMovementInputPerformed;
        }

        private void HandleViewUpdate ()
        {
            model.Tick();
        }

        private void HandleMovementInputPerformed (Vector2Int value)
        {

            model.LookTo(value);
        }

        public void Dispose ()
        {
            view.OnUpdate -= HandleViewUpdate;
            input.OnMovementRequested -= HandleMovementInputPerformed;
        }
    }
}