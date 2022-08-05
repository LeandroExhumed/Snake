using System;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridModel<T> : IGridModel<T>
    {
        public event Action<T[,]> OnInitialized;

        public int Width { get; }
        public int Height { get; }

        private readonly T[,] array;

        public GridModel (int width, int height)
        {
            Width = width;
            Height = height;

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