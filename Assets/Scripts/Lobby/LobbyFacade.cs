﻿using System;
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
        private LobbyController controller;

        [Inject]
        public void Constructor (ILobbyModel model, LobbyController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        public void Initialize ()
        {
            model.Initialize();
            controller.Setup();
        }

        public void RecoverKeys (char leftKey, char rightKey) => model.RecoverKeys(leftKey, rightKey);

        public void ReturnKeys (char leftKey, char rightKey) => model.ReturnKeys(leftKey, rightKey);

        public void StartListeningToInput () => model.StartListeningToInput();

        private void OnDestroy ()
        {
            controller.Dispose();
        }
    }
}