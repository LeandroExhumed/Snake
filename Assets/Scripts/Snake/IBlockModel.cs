using LeandroExhumed.SnakeGame.Collectables;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface IBlockModel : ICollectableModel
    {
        event Action<Transform> OnAttached;
        event Action OnBenefitRemoved;

        int ID { get; }
        bool IsAttached { get; }
        bool HasBenefit { get; }

        void Attach (Transform owner);
        void RemoveBenefit ();

        public class Factory : PlaceholderFactory<IBlockModel> { }
    }
}