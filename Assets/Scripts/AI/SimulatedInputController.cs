using LeandroExhumed.SnakeGame.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class SimulatedInputController : IDisposable
    {
        private readonly ISimulatedInput model;
        private readonly SimulatedInputView view;

        private readonly IGridModel<INode> levelGrid;

        public SimulatedInputController (ISimulatedInput model, SimulatedInputView view, IGridModel<INode> levelGrid)
        {
            this.model = model;
            this.view = view;
            this.levelGrid = levelGrid;
        }

        public void Setup ()
        {
            model.OnPathChanged += HandlePathChanged;
            levelGrid.OnNodeChanged += HandleLevelGridNodeChanged;
        }

        private void HandlePathChanged (List<PathNode> path)
        {
            view.SetPath(path?.ToArray());
        }

        private void HandleLevelGridNodeChanged (Vector2Int nodePosition)
        {
            model.HandleGridNodeChanged(nodePosition);
        }

        public void Dispose ()
        {
            model.OnPathChanged -= HandlePathChanged;
            levelGrid.OnNodeChanged -= HandleLevelGridNodeChanged;
        }
    }
}