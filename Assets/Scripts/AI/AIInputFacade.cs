using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.AI
{
    public class AIInputFacade : MonoBehaviour, IAIInputModel
    {
        public event Action OnDestroyed
        {
            add => model.OnDestroyed += value;
            remove => model.OnDestroyed -= value;
        }

        private IAIInputModel model;
        private AIInputController controller;

        [Inject]
        public void Constructor (IAIInputModel model, AIInputController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        public event Action<List<PathNodeModel>> OnPathChanged
        {
            add => model.OnPathChanged += value;
            remove => model.OnPathChanged -= value;
        }

        public event Action<int> OnMovementRequested
        {
            add => model.OnMovementRequested += value;
            remove => model.OnMovementRequested -= value;
        }

        public void Initialize (ISnakeModel snake)
        {
            controller.Setup();
            model.Initialize(snake);
        }

        public void Initialize () => model.Initialize();

        public void HandleGridNodeChanged (Vector2Int nodePosition) => model.HandleGridNodeChanged(nodePosition);

        public void HandleBlockCollected (IBlockModel block) => model.HandleBlockCollected(block);

        public void Destroy () => model.Destroy();

        public void Dispose ()
        {
            model.Dispose();
        }

        private void OnDestroy ()
        {
            model.Dispose();
            controller.Dispose();
        }
    }
}