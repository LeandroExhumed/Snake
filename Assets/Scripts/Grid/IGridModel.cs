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

        T GetNode (Vector2Int position);
        void Initialize ();
        void SetNode (Vector2Int position, T node);
        void Clear ();
    }
}