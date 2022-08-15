using LeandroExhumed.SnakeGame.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class AIInputController : IController
    {
        private readonly IAIInputModel model;
        private readonly AIInputView view;

        private readonly IGridModel<INodeModel> levelGrid;

        public AIInputController (IAIInputModel model, AIInputView view, IGridModel<INodeModel> levelGrid)
        {
            this.model = model;
            this.view = view;
            this.levelGrid = levelGrid;
        }

        public void Setup ()
        {
            model.OnPathChanged += HandlePathChanged;
            levelGrid.OnNodeChanged += HandleLevelGridNodeChanged;
            model.OnDestroyed += HandleDestroyed;
        }

        private void HandlePathChanged (List<IPathNodeModel> path)
        {
            view.SetPath(path?.ToArray());
        }

        private void HandleDestroyed ()
        {
            view.Destroy();
        }

        private void HandleLevelGridNodeChanged (Vector2Int nodePosition)
        {
            model.HandleGridNodeChanged(nodePosition);
        }

        public void Dispose ()
        {
            model.OnPathChanged -= HandlePathChanged;
            levelGrid.OnNodeChanged -= HandleLevelGridNodeChanged;
            model.OnDestroyed -= HandleDestroyed;
        }
    }
}