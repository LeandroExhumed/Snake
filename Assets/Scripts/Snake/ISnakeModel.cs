using LeandroExhumed.SnakeGame.Input;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ISnakeModel : ICollector
    {
        event Action<IMovementRequester> OnInitialized;
        event Action<ISnakeModel, Vector2Int> OnPositionChanged;
        event Action<IBlockModel> OnBlockAttached;
        event Action<ISnakeModel> OnHit;

        Vector2Int Position { get; }
        Vector2Int Direction { get; }
        float TimeToMove { get; }

        void Initialize (Vector2Int startPosition, Vector2Int startDirection, IMovementRequester input);
        void LookTo (int direction);
        void Grow (IBlockModel block);
        void Tick ();

        public class Factory : PlaceholderFactory<ISnakeModel> { }
    }
}