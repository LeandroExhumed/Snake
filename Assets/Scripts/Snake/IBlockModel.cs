using LeandroExhumed.SnakeGame.Grid;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface IBlockModel : INode
    {
        event Action<IBlockModel> OnCollected;
        event Action<Transform> OnAttached;
        event Action OnBenefitRemoved;
        event Action OnDestroyed;

        int ID { get; }
        bool IsAttached { get; }
        bool HasBenefit { get; }

        void Initialize (Vector2Int startPosition, bool hasBenefit, ICollector owner = null);
        void BeCollected ();
        void ApplyEffect ();
        void Attach (Transform owner);
        void RemoveBenefit ();
        void Destroy ();
        bool IsEqual (IBlockModel other);

        public class Factory : PlaceholderFactory<IBlockModel> { }
    }
}