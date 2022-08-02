using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeFacade : MonoBehaviour, ISnakeModel
    {
        public event Action<Vector2Int> OnPositionChanged
        {
            add => model.OnPositionChanged += value;
            remove => model.OnPositionChanged -= value;
        }

        public Vector2Int Position => model.Position;

        [SerializeField]
        private Vector2Int initialPosition;

        private ISnakeModel model;
        private SnakeController controller;

        [Inject]
        public void Constructor (ISnakeModel model, SnakeController controller)
        {
            this.model = model;
            this.controller = controller;

            controller.Setup();
        }

        private void Start ()
        {
            Initialize(initialPosition);
        }

        private void Update ()
        {
            model.Tick();
        }

        public void Initialize (Vector2Int initialPosition) => model.Initialize(initialPosition);

        public void LookTo (Vector2Int direction) => model.LookTo(direction);

        public void Grow () => model.Grow();

        public void IncreaseSpeed (float speedAddition) => model.IncreaseSpeed(speedAddition);

        public void ApplyBatteringRamEffect () => model.ApplyBatteringRamEffect();

        public void Tick () => model.Tick();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}