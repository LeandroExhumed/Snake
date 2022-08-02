using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface IBodyPartModel
    {
        event Action<Vector2Int> OnPositionChanged;

        Vector2Int Position { get; set; }

        void Initialize (Vector2Int initialPosition);

        public class Factory : PlaceholderFactory<IBodyPartModel> { }
    }
}