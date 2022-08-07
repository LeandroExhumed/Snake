using LeandroExhumed.SnakeGame.Collectables;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface IBlockModel : ICollectableModel
    {
        event Action<Transform> OnAttached;

        int ID { get; }
        bool IsAttached { get; }

        void Attach (Transform owner);

        public class Factory : PlaceholderFactory<IBlockModel> { }
    }
}