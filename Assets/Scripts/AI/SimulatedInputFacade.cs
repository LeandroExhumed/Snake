﻿using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.AI
{
    public class SimulatedInputFacade : MonoBehaviour, ISimulatedInput
    {
        private ISimulatedInput model;
        private SimulatedInputController controller;

        [Inject]
        public void Constructor (ISimulatedInput model, SimulatedInputController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        public event Action<List<PathNode>> OnPathChanged
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

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}