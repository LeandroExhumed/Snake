using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.AI
{
    public interface IAIInputModel : IGameInputModel, IDisposable
    {
        event Action<List<IPathNodeModel>> OnPathChanged;
        event Action OnDestroyed;

        void Initialize (ISnakeModel snake);
        void HandleGridNodeChanged (Vector2Int nodePosition);
        void HandleBlockCollected (IBlockModel block);
        void Destroy ();

        public class Factory : PlaceholderFactory<IAIInputModel> { }
    }
}