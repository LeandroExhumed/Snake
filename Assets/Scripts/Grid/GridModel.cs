using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridModel<T> : IGridModel<T>
    {
        public event Action<T[,]> OnInitialized;

        private readonly int width;
        private readonly int height;
        private T[,] array;

        public GridModel (int width, int height)
        {
            this.width = width;
            this.height = height;

            array = new T[width, height];
        }

        public void Initialize ()
        {
            OnInitialized?.Invoke(array);
        }

        public T GetNode (int x, int y)
        {
            return array[x, y];
        }

        public void SetNode (int x, int y, T node)
        {
            array[x, y] = node;
        }
    }
}