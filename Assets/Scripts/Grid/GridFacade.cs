using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridFacade : MonoBehaviour, IGridModel<INodeModel>
    {
        public event Action<INodeModel[,]> OnInitialized
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

        private IGridModel<INodeModel> model;
        private GridController controller;

        [Inject]
        public void Constructor (IGridModel<INodeModel> model, GridController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        public void Initialize ()
        {
            controller.Setup();
            model.Initialize();
        }

        public INodeModel GetNode (Vector2Int position) => model.GetNode(position);

        public void SetNode (Vector2Int position, INodeModel node) => model.SetNode(position, node);

        public void Clear () => model.Clear();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}