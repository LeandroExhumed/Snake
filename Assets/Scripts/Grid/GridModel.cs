﻿using System;
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
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    Debug.Log(array[x, y]);
                }
            }

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