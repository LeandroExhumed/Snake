using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class LobbyFacade : MonoBehaviour, ILobbyModel
    {
        public event Action<int> OnInitialized
        {
            add => model.OnInitialized += value;
            remove => model.OnInitialized -= value;
        }
        public event Action<char, char> OnNewPlayerJoined
        {
            add => model.OnNewPlayerJoined += value;
            remove => model.OnNewPlayerJoined -= value;
        }

        private ILobbyModel model;
        private IController controller;

        [Inject]
        public void Constructor (ILobbyModel model, IController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        public void Initialize ()
        {
            controller.Setup();
            model.Initialize();
        }

        public void RecoverKeys (char leftKey, char rightKey) => model.RecoverKeys(leftKey, rightKey);

        public void ReturnKeys (char leftKey, char rightKey) => model.ReturnKeys(leftKey, rightKey);

        public void SetInputListeningActive (bool value) => model.SetInputListeningActive(value);

        public void FreeAllKeys () => model.FreeAllKeys();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}