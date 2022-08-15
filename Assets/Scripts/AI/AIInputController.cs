using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Match;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class AIInputController : IController
    {
        private readonly IAIInputModel model;
        private readonly AIInputView view;

        private readonly IGridModel<INodeModel> levelGrid;
        private readonly IMatchModel match;

        public AIInputController (IAIInputModel model, AIInputView view, IGridModel<INodeModel> levelGrid, IMatchModel match)
        {
            this.model = model;
            this.view = view;
            this.levelGrid = levelGrid;
            this.match = match;
        }

        public void Setup ()
        {
            model.OnPathChanged += HandlePathChanged;
            model.OnDestroyed += HandleDestroyed;
            levelGrid.OnNodeChanged += HandleLevelGridNodeChanged;
            match.OnBlockGenerated += HandleBlockGenerated;
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

        private void HandleBlockGenerated (IBlockModel block)
        {
            model.HandleBlockGenerated(block);
        }

        public void Dispose ()
        {
            model.OnPathChanged -= HandlePathChanged;
            model.OnDestroyed -= HandleDestroyed;
            levelGrid.OnNodeChanged -= HandleLevelGridNodeChanged;
            match.OnBlockGenerated -= HandleBlockGenerated;
        }
    }
}