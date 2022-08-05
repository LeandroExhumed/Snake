using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeFacade : MonoBehaviour, ISnakeModel
    {
        public event Action<ISnakeModel, Vector2Int> OnPositionChanged
        {
            add => model.OnPositionChanged += value;
            remove => model.OnPositionChanged -= value;
        }
        public event Action OnHit
        {
            add => model.OnHit += value;
            remove => model.OnHit -= value;
        }

        public Vector2Int Position => model.Position;
        public Vector2Int Direction => model.Direction;

        private ISnakeModel model;
        private SnakeController controller;
        private IMovementRequester movementRequester;

        [Inject]
        public void Constructor (ISnakeModel model, SnakeController controller, IMovementRequester movementRequester)
        {
            this.model = model;
            this.controller = controller;
            this.movementRequester = movementRequester;

            controller.Setup();
        }

        public void Initialize (Vector2Int startPosition, Vector2Int startDirection)
        {
            model.Initialize(startPosition, startDirection);
            movementRequester.Initialize();
        }

        public void LookTo (Vector2Int direction) => model.LookTo(direction);

        public void Grow () => model.Grow();

        public void CollectEnginePower (float speedAddition) => model.CollectEnginePower(speedAddition);

        public void CollectBatteringRam () => model.CollectBatteringRam();

        public void Tick () => model.Tick();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}