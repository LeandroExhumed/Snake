using LeandroExhumed.SnakeGame.Block;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchFacade : MonoBehaviour, IMatchModel
    {
        public event Action OnRestarted
        {
            add => model.OnRestarted += value;
            remove => model.OnRestarted -= value;
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
        public event Action<Vector2Int?> OnSnakeHit
        {
            add => model.OnSnakeHit += value;
            remove => model.OnSnakeHit -= value;
    }
        public event Action<int, char, char> OnPlayerLeft
        {
            add => model.OnPlayerLeft += value;
            remove => model.OnPlayerLeft -= value;
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

        public bool IsRunning => model.IsRunning;

        private IMatchModel model;
        private IController controller;

        [Inject]
        public void Constructor (IMatchModel model, IController controller)
        {
            this.model = model;
            this.controller = controller;

            controller.Setup();
        }

        public void Initialize () => model.Initialize();

        public void Begin () => model.Begin();

        public void AddPlayer (char leftKey, char rightKey) => model.AddPlayer(leftKey, rightKey);

        public void Rewind () => model.Rewind();

        public void Restart () => model.Restart();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}