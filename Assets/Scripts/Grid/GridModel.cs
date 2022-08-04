using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridModel : IGridModel
    {
        public event Action<INode[,]> OnInitialized;

        private readonly int width;
        private readonly int height;
        private INode[,] array;

        public GridModel (int width, int height)
        {
            this.width = width;
            this.height = height;

            array = new INode[width, height];
        }

        public void Initialize ()
        {
            OnInitialized?.Invoke(array);
        }

        public INode GetNode (int x, int y)
        {
            return array[x, y];
        }

        public void SetNode (int x, int y, INode node)
        {
            array[x, y] = node;
        }
    }
}