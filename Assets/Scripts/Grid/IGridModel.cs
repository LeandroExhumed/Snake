using System;

namespace LeandroExhumed.SnakeGame.Grid
{
    public interface IGridModel<T>
    {
        event Action<T[,]> OnInitialized;

        T GetNode (int x, int y);
        void Initialize ();
        void SetNode (int x, int y, T node);
    }
}