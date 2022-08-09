using LeandroExhumed.SnakeGame.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchFacade : MonoBehaviour, IMatchModel
    {
        private IMatchModel model;
        private MatchController controller;

        [Inject]
        public void Constructor (IMatchModel model, MatchController controller)
        {
            this.model = model;
            this.controller = controller;

            controller.Setup();
        }

        public event Action OnInitialized
        {
            add => model.OnInitialized += value;
            remove => model.OnInitialized -= value;
        }
        public event Action<Vector2Int> OnBlockGenerated
        {
            add => model.OnBlockGenerated += value;
            remove => model.OnBlockGenerated -= value;
        }

        public void Initialize () => model.Initialize();

        public void AddPlayer (InputAction inputAction) => model.AddPlayer(inputAction);

        public void Play (int selectedSnakeID, IMovementRequester input) => model.Play(selectedSnakeID, input);

        public void GenerateBlock () => model.GenerateBlock();

        public void End () => model.End();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}