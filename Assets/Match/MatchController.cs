using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchController : IDisposable
    {
        private readonly IMatchModel model;
        private readonly MatchView view;

        private readonly LobbyModel lobby;

        public MatchController (IMatchModel model, MatchView view, LobbyModel lobby)
        {
            this.model = model;
            this.view = view;
            this.lobby = lobby;
        }

        public void Setup ()
        {
            model.OnInitialized += HandleInitialized;
            model.OnSnakePositionChanged += HandleSnakePositionChanged;
            lobby.OnNewPlayerJoined += HandleNewPlayerJoined;
        }

        private void HandleSnakePositionChanged (int player, Vector2Int position)
        {
            view.SyncGuidePosition(player, position);
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
            model.OnInitialized -= HandleInitialized;
            model.OnSnakePositionChanged -= HandleSnakePositionChanged;
            lobby.OnNewPlayerJoined -= HandleNewPlayerJoined;
        }
    }
}