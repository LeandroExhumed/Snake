using LeandroExhumed.SnakeGame.Grid;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BlockController : IDisposable
    {
        private readonly IBlockModel model;
        private readonly BlockView view;

        public BlockController (IBlockModel model, BlockView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Setup ()
        {
            model.OnPositionChanged += HandlePositionChanged;
            model.OnAttached += HandleAttached;
        }

        private void HandleAttached (Transform owner)
        {
            view.SetParent(owner);
        }

        private void HandlePositionChanged (INode model, Vector2Int value)
        {
            view.Position = value;
        }

        public void Dispose ()
        {
            model.OnPositionChanged -= HandlePositionChanged;
        }
    }
}