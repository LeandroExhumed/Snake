using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.AI
{
    public interface ISimulatedInput : IMovementRequester
    {
        event Action<List<PathNode>> OnPathChanged;

        void HandleGridNodeChanged (Vector2Int nodePosition);

        public class Factory : PlaceholderFactory<ISnakeModel, ISimulatedInput> { }
    }
}