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

        public Vector2Int Position { get => model.Position; set => model.Position = value; }

        [SerializeField]
        private Vector2Int initialPosition;
        [SerializeField]
        private float size;

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
            Initialize(initialPosition, size);
        }

        private void Update ()
        {
            model.Tick();
        }

        public void Initialize (Vector2Int initialPosition, float size) => model.Initialize(initialPosition, size);

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