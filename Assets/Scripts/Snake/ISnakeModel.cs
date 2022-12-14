using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Input;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ISnakeModel : ICollectorModel
    {
        event Action<IGameInputModel> OnInitialized;
        event Action<ISnakeModel, Vector2Int> OnPositionChanged;
        event Action<IBlockModel> OnBlockAttached;
        event Action<ISnakeModel, IBlockModel> OnHit;
        event Action OnDestroyed;

        int ID { get; }
        Vector2Int Position { get; }
        Vector2Int Direction { get; }
        float TimeToMove { get; }

        void Initialize (Vector2Int startPosition, Vector2Int startDirection, IGameInputModel input);
        void Initialize (SnakePersistentData persistentData, IGameInputModel input);
        void LookTo (int direction);
        void Tick ();
        void Save (SnakePersistentData persistentData);
        void Destroy ();

        public class Factory : PlaceholderFactory<ISnakeModel> { }
    }
}