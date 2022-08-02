using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BodyPartController : IDisposable
    {
        private readonly IBodyPartModel model;
        private readonly BodyPartView view;

        public BodyPartController (IBodyPartModel model, BodyPartView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Setup ()
        {
            model.OnPositionChanged += HandlePositionChanged;
        }

        private void HandlePositionChanged (Vector2Int value)
        {
            view.Position = value;
        }

        public void Dispose ()
        {
            model.OnPositionChanged -= HandlePositionChanged;
        }
    }
}