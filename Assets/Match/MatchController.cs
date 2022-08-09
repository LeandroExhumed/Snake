using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchController : IDisposable
    {
        private const string AI_NAME = "AI";

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
            model.OnPlayerLeft += HandlePlayerLeft;
            model.OnOver += HandleOver;
            lobby.OnNewPlayerJoined += HandleNewPlayerJoined;
        }

        private void HandleOver (int playerNumber)
        {
            view.SetWinnerMessageActive(true);
            view.SetWinnerMessage(playerNumber != 0 ? $"P{playerNumber}" : AI_NAME);
        }

        private void HandleSnakePositionChanged (int player, Vector2Int position)
        {
            view.SyncGuidePosition(player, position);
        }

        private void HandleInitialized ()
        {
            lobby.Initialize();
        }

        private void HandlePlayerLeft (int playerNumber)
        {
            view.RemoveGuide(playerNumber);
        }

        private void HandleNewPlayerJoined (InputAction inputAction)
        {
            model.AddPlayer(inputAction);
        }

        public void Dispose ()
        {
            model.OnInitialized -= HandleInitialized;
            model.OnSnakePositionChanged -= HandleSnakePositionChanged;
            model.OnPlayerLeft -= HandlePlayerLeft;
            lobby.OnNewPlayerJoined -= HandleNewPlayerJoined;
        }
    }
}