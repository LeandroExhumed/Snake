using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridFacade : MonoBehaviour, IGridModel<INode>
    {
        public event Action<INode[,]> OnInitialized
        {
            add => model.OnInitialized += value;
            remove => model.OnInitialized -= value;
        }
        public event Action<Vector2Int> OnNodeChanged
        {
            add => model.OnNodeChanged += value;
            remove => model.OnNodeChanged -= value;
        }

        public int Width => model.Width;
        public int Height => model.Height;

        private IGridModel<INode> model;
        private GridController controller;

        [Inject]
        public void Constructor (IGridModel<INode> model, GridController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        public void Initialize ()
        {
            controller.Setup();
            model.Initialize();
        }

        public INode GetNode (Vector2Int position) => model.GetNode(position);

        public void SetNode (Vector2Int position, INode node) => model.SetNode(position, node);

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}