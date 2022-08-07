using LeandroExhumed.SnakeGame.Collectables;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BlockModel : CollectableModel, IBlockModel
    {
        public event Action<Transform> OnAttached;

        public int ID => data.ID;
        public bool IsAttached { get; private set; }

        private readonly BlockData data;

        public BlockModel (BlockData data)
        {
            this.data = data;
        }

        public void Attach (Transform owner)
        {
            IsAttached = true;
            OnAttached?.Invoke(owner);
        }
    }
}