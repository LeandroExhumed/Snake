using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Block
{
    public interface IBlockModel : INodeModel
    {
        event Action<IBlockModel> OnCollected;
        event Action<Transform> OnAttached;
        event Action OnBenefitRemoved;
        event Action OnHit;
        event Action OnDestroyed;

        int ID { get; }
        float MoveCost { get; }
        bool IsAttached { get; }
        bool HasBenefit { get; }

        void Initialize (Vector2Int startPosition, bool hasBenefit, ICollectorModel owner = null);
        void BeCollected ();
        void ApplyEffect ();
        void Attach (Transform owner);
        void RemoveBenefit ();
        void GetHit ();
        void Destroy ();
        bool IsEqual (IBlockModel other);

        public class Factory : PlaceholderFactory<IBlockModel> { }
    }
}