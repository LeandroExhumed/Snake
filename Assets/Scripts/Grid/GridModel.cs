using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridModel<T> : IGridModel<T>
    {
        public event Action<T[,]> OnInitialized;
        public event Action<Vector2Int> OnNodeChanged;

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

        public T GetNode (Vector2Int position)
        {
            return array[position.x, position.y];
        }

        public void SetNode (Vector2Int position, T node)
        {
            array[position.x, position.y] = node;
            OnNodeChanged?.Invoke(position);
        }

        public void Clear ()
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    array[x, y] = default;
                }
            }
        }
    }
}