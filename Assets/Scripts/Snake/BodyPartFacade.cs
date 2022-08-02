using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BodyPartFacade : MonoBehaviour, IBodyPartModel
    {
        public Vector2Int Position { get => model.Position; set => model.Position = value; }

        public event Action<Vector2Int> OnPositionChanged
        {
            add => model.OnPositionChanged += value;
            remove => model.OnPositionChanged -= value;
        }

        private IBodyPartModel model;
        private BodyPartController controller;

        public void Constructor (IBodyPartModel model, BodyPartController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        public void Initialize ()
        {
            controller.Setup();
        }

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}