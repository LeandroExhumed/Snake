using System;

namespace LeandroExhumed.SnakeGame.Grid
{
    public interface IGridModel
    {
        event Action<INode[,]> OnInitialized;

        INode GetNode (int x, int y);
        void Initialize ();
        void SetNode (int x, int y, INode node);
    }
}