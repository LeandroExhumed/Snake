using System;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchController : IDisposable
    {
        private readonly IMatchModel model;

        private readonly LobbyModel lobby;

        public MatchController (IMatchModel model, LobbyModel lobby)
        {
            this.model = model;
            this.lobby = lobby;
        }

        public void Setup ()
        {
            model.OnInitialized += HandleInitialized;
            lobby.OnNewPlayerJoined += HandleNewPlayerJoined;
        }

        private void HandleInitialized ()
        {
            lobby.Initialize();
        }

        private void HandleNewPlayerJoined (InputAction inputAction)
        {
            model.AddPlayer(inputAction);
        }

        public void Dispose ()
        {
            lobby.OnNewPlayerJoined -= HandleNewPlayerJoined;
        }
    }
}