using LeandroExhumed.SnakeGame.Input;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeFacade : MonoBehaviour, ISnakeModel
    {
        public event Action<IMovementRequester> OnInitialized
        {
            add => model.OnInitialized += value;
            remove => model.OnInitialized -= value;
        }
        public event Action<ISnakeModel, Vector2Int> OnPositionChanged
        {
            add => model.OnPositionChanged += value;
            remove => model.OnPositionChanged -= value;
        }
        public event Action<IBlockModel> OnBlockAttached
        {
            add => model.OnBlockAttached += value;
            remove => model.OnBlockAttached -= value;
        }
        public event Action<ISnakeModel, IBlockModel> OnHit
        {
            add => model.OnHit += value;
            remove => model.OnHit -= value;
        }
        public event Action OnDestroyed
        {
            add => model.OnDestroyed += value;
            remove => model.OnDestroyed -= value;
        }

        public int ID => model.ID;
        public Vector2Int Position => model.Position;
        public Vector2Int Direction => model.Direction;
        public float TimeToMove => model.TimeToMove;

        private ISnakeModel model;
        private SnakeController controller;

        [Inject]
        public void Constructor (ISnakeModel model, SnakeController controller)
        {
            this.model = model;
            this.controller = controller;

            controller.Setup();
        }

        public void Initialize (Vector2Int startPosition, Vector2Int startDirection, IMovementRequester input)
            => model.Initialize(startPosition, startDirection, input);

        public void Initialize (SnakePersistentData persistentData, IMovementRequester input)
            => model.Initialize(persistentData, input);

        public void LookTo (int direction) => model.LookTo(direction);

        public void Grow (IBlockModel block) => model.Grow(block);

        public void CollectEnginePower (IBlockModel block, float speedAddition) => model.CollectEnginePower(block, speedAddition);

        public void CollectBatteringRam (IBlockModel block) => model.CollectBatteringRam(block);

        public void CollectTimeTravel (IBlockModel block) => model.CollectTimeTravel(block);

        public void Tick () => model.Tick();

        public void Save (SnakePersistentData persistentData) => model.Save(persistentData);

        public void Destroy () => model.Destroy();

        public override bool Equals (object other)
        {
            return model != null ? model.Equals(other) : base.Equals(other);
        }

        public override int GetHashCode ()
        {
            return model != null ? model.GetHashCode() : base.GetHashCode();
        }

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}