using System;
using UnityEngine;

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
            model.OnRewind += HandleRewind;
            model.OnPlayerReturned += HandlePlayerReturned;
            model.OnOver += HandleOver;
            view.OnRewindEffectOver += HandleRewindEffectOver;
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

        private void HandlePlayerLeft (int playerNumber, char leftKey, char rightKey)
        {
            lobby.ReturnKeys(leftKey, rightKey);
            view.RemoveGuide(playerNumber);
        }

        private void HandleRewind ()
        {
            //Time.timeScale = 0;
            view.PlayRewindEffect();
        }

        private void HandlePlayerReturned (char leftKey, char rightKey)
        {
            lobby.RecoverKeys(leftKey, rightKey);
        }

        private void HandleRewindEffectOver ()
        {
            Time.timeScale = 1;
            model.Rewind();
        }

        private void HandleNewPlayerJoined (char leftKey, char rightKey)
        {
            model.AddPlayer(leftKey, rightKey);
        }

        public void Dispose ()
        {
            model.OnInitialized -= HandleInitialized;
            model.OnSnakePositionChanged -= HandleSnakePositionChanged;
            model.OnPlayerLeft -= HandlePlayerLeft;
            model.OnRewind -= HandleRewind;
            model.OnPlayerReturned -= HandlePlayerReturned;
            model.OnOver -= HandleOver;
            view.OnRewindEffectOver -= HandleRewindEffectOver;
            lobby.OnNewPlayerJoined -= HandleNewPlayerJoined;
        }
    }
}