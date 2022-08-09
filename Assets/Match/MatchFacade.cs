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
        public event Action<int, Vector2Int> OnSnakePositionChanged
        {
            add => model.OnSnakePositionChanged += value;
            remove => model.OnSnakePositionChanged -= value;
        }
        public event Action<int> OnPlayerLeft
        {
            add => model.OnPlayerLeft += value;
            remove => model.OnPlayerLeft -= value;
        }
        public event Action<int> OnOver
        {
            add => model.OnOver += value;
            remove => model.OnOver -= value;
        }

        public void Initialize () => model.Initialize();

        public void AddPlayer (InputAction inputAction) => model.AddPlayer(inputAction);

        public void Play (int selectedSnakeID, int playerNumber, IMovementRequester input)
            => model.Play(selectedSnakeID, playerNumber, input);

        public void GenerateBlock () => model.GenerateBlock();

        public void End () => model.End();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}