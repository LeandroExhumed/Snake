using LeandroExhumed.SnakeGame.Grid;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface IBodyPartModel : INode
    {
        event Action<IBodyPartModel, Vector2Int> OnPositionChanged;

        void Initialize (Vector2Int initialPosition);

        public class Factory : PlaceholderFactory<IBodyPartModel> { }
    }
}