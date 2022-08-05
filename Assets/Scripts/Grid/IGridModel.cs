using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Grid
{
    public interface IGridModel<T>
    {
        event Action<T[,]> OnInitialized;
        event Action<Vector2Int> OnNodeChanged;

        public int Width { get; }
        public int Height { get; }

        T GetNode (int x, int y);
        void Initialize ();
        void SetNode (int x, int y, T node);
    }
}