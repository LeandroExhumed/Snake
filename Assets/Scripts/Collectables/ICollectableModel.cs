using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public interface ICollectableModel : INode
    {
        event Action<Vector2Int> OnPositionChanged;
        event Action OnCollected;

        void Initialize (Vector2Int startPosition);
        void BeCollected (ICollector collector);

        public class Factory : PlaceholderFactory<ICollectableModel> { }
    }
}