﻿using LeandroExhumed.SnakeGame.Grid;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class CollectableController : IDisposable
    {
        private readonly ICollectableModel model;
        private readonly CollectableView view;

        public CollectableController (ICollectableModel model, CollectableView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Setup ()
        {
            model.OnPositionChanged += HandlePositionChanged;
            model.OnCollected += HandleCollected;
        }

        private void HandlePositionChanged (INode _, Vector2Int value)
        {
            view.Position = value;
        }

        private void HandleCollected (ICollectableModel model)
        {
            view.Destroy();
        }

        public void Dispose ()
        {
            model.OnCollected -= HandleCollected;
        }
    }
}