using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Input;
using System;
using UnityEngine;
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
        public event Action<IBlockModel> OnBlockGenerated
        {
            add => model.OnBlockGenerated += value;
            remove => model.OnBlockGenerated -= value;
        }
        public event Action<int, Vector2Int> OnSnakePositionChanged
        {
            add => model.OnSnakePositionChanged += value;
            remove => model.OnSnakePositionChanged -= value;
        }
        public event Action<int, char, char> OnPlayerLeft
        {
            add => model.OnPlayerLeft += value;
            remove => model.OnPlayerLeft -= value;
        }
        public event Action OnRewind
        {
            add => model.OnRewind += value;
            remove => model.OnRewind -= value;
        }
        public event Action<char, char> OnPlayerReturned
        {
            add => model.OnPlayerReturned += value;
            remove => model.OnPlayerReturned -= value;
        }
        public event Action<int> OnOver
        {
            add => model.OnOver += value;
            remove => model.OnOver -= value;
        }

        public void Initialize () => model.Initialize();

        public void AddPlayer (char leftKey, char rightKey) => model.AddPlayer(leftKey, rightKey);

        public void Play (int selectedSnakeID, int playerNumber, IPlayerInput input)
            => model.Play(selectedSnakeID, playerNumber, input);

        public void Rewind () => model.Rewind();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}