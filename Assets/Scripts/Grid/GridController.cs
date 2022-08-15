using System;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridController : IDisposable
    {
        private readonly IGridModel<INodeModel> model;
        private readonly GridView view;

        public GridController (IGridModel<INodeModel> model, GridView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Setup ()
        {
            model.OnInitialized += HandleInitialized;
        }

        private void HandleInitialized (INodeModel[,] obj)
        {
            view.Initialize(obj);
        }

        public void Dispose ()
        {
            model.OnInitialized -= HandleInitialized;
        }
    }
}