using System;

namespace LeandroExhumed.SnakeGame.Match
{
    public class LobbyController : IDisposable
    {
        private int holdDuration;
        private const string JOIN_MATCH_MESSAGE_TEXT = "Hold any letter or number for {0} seconds to join the match.";
        private const string RESTART_MATCH_MESSAGE_TEXT = "Press enter to restart match.";

        private readonly ILobbyModel model;
        private readonly LobbyView view;

        private readonly IMatchModel match;

        public LobbyController (ILobbyModel model, LobbyView view, IMatchModel match)
        {
            this.model = model;
            this.view = view;
            this.match = match;
        }

        public void Setup ()
        {
            model.OnInitialized += HandleInitialized;
            view.OnIntroOver += HandleIntroOver;
            match.OnPlayerLeft += HandlePlayerLeft;
            match.OnPlayerReturned += HandlePlayerReturned;
            match.OnOver += HandleMatchOver;
            match.OnRestarted += HandleRestarted;
        }

        private void HandleInitialized (int holdDuration)
        {
            this.holdDuration = holdDuration;
            
            match.Initialize();
        }

        private void HandleIntroOver ()
        {
            SetupMatchStartup();
            view.ShowLobbyMessage();
            match.Begin();
        }

        private void HandlePlayerLeft (int playerNumber, char leftKey, char rightKey)
        {
            model.ReturnKeys(leftKey, rightKey);
        }

        private void HandlePlayerReturned (char leftKey, char rightKey)
        {
            model.RecoverKeys(leftKey, rightKey);
        }

        private void HandleMatchOver (int obj)
        {
            model.SetInputListeningActive(false);
            model.FreeAllKeys();
            view.SetLobbyMessageText(RESTART_MATCH_MESSAGE_TEXT);
        }

        private void HandleRestarted ()
        {
            SetupMatchStartup();
        }

        private void SetupMatchStartup ()
        {
            model.SetInputListeningActive(true);
            view.SetLobbyMessageText(string.Format(JOIN_MATCH_MESSAGE_TEXT, holdDuration));
        }

        public void Dispose ()
        {
            model.OnInitialized -= HandleInitialized;
            view.OnIntroOver -= HandleIntroOver;
            match.OnPlayerLeft -= HandlePlayerLeft;
            match.OnPlayerReturned -= HandlePlayerReturned;
            match.OnOver -= HandleMatchOver;
        }
    }
}