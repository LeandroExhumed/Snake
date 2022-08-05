using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ISnakeModel : ICollector
    {
        event Action<ISnakeModel, Vector2Int> OnPositionChanged;
        event Action OnHit;

        Vector2Int Position { get; }
        Vector2Int Direction { get; }
        float TimeToMove { get; }

        void Initialize (Vector2Int startPosition, Vector2Int startDirection);
        void LookTo (Vector2Int direction);
        void Grow ();
        void Tick ();

        public class Factory : PlaceholderFactory<ISnakeModel> { }
    }
}