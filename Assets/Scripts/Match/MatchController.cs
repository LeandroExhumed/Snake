using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchController : IDisposable
    {
        private const string AI_NAME = "AI";

        private readonly IMatchModel model;
        private readonly MatchView view;

        private readonly ILobbyModel lobby;

        public MatchController (IMatchModel model, MatchView view, ILobbyModel lobby)
        {
            this.model = model;
            this.view = view;
            this.lobby = lobby;
        }

        public void Setup ()
        {
            model.OnSnakePositionChanged += HandleSnakePositionChanged;
            model.OnSnakeHit += HandleSnakeHit;
            model.OnPlayerLeft += HandlePlayerLeft;
            model.OnOver += HandleOver;
            model.OnRestarted += HandleRestarted;
            view.OnBlockFocusOver += HandleBlockFocusOver;
            view.OnRewindEffectOver += HandleRewindEffectOver;
            view.OnRestarted += HandleRestartButtonClicked;
            lobby.OnNewPlayerJoined += HandleNewPlayerJoined;
        }

        private void HandleSnakePositionChanged (int player, Vector2Int position)
        {
            view.SyncGuidePosition(player, position);
        }

        private void HandleSnakeHit (Vector2Int? timeTravelBlockPosition)
        {
            view.PlayDeathEffect();
            if (timeTravelBlockPosition != null)
            {
                view.FocusBlockHit(timeTravelBlockPosition.Value);
            }
            else
            {
                view.ShakeCamera();
            }
        }

        private void HandlePlayerLeft (int playerNumber, char leftKey, char rightKey)
        {
            view.RemoveGuide(playerNumber);
        }

        private void HandleOver (int playerNumber)
        {
            view.SetWinnerMessageActive(true);
            string name = AI_NAME;
            if (playerNumber != 0)
            {
                name = $"P{playerNumber}";
                view.RemoveGuide(playerNumber);
            }

            view.SetWinnerMessage(name);
        }

        private void HandleRestarted ()
        {
            view.SetWinnerMessageActive(false);
        }

        private void HandleBlockFocusOver ()
        {
            view.PlayRewindEffect();
        }

        private void HandleRewindEffectOver ()
        {
            Time.timeScale = 1;
            view.LeaveFocus();
            model.Rewind();
        }

        private void HandleRestartButtonClicked ()
        {
            if (model.IsRunning)
            {
                return;
            }

            model.Restart();
        }

        private void HandleNewPlayerJoined (char leftKey, char rightKey)
        {
            model.AddPlayer(leftKey, rightKey);
        }

        public void Dispose ()
        {
            model.OnSnakePositionChanged -= HandleSnakePositionChanged;
            model.OnSnakeHit -= HandleSnakeHit;
            model.OnPlayerLeft -= HandlePlayerLeft;
            model.OnOver -= HandleOver;
            model.OnRestarted -= HandleRestarted;
            view.OnBlockFocusOver -= HandleBlockFocusOver;
            view.OnRewindEffectOver -= HandleRewindEffectOver;
            view.OnRestarted -= HandleRestartButtonClicked;
            lobby.OnNewPlayerJoined -= HandleNewPlayerJoined;
        }
    }
}