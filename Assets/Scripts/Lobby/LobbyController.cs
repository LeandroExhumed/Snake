using System;

namespace LeandroExhumed.SnakeGame.Match
{
    public class LobbyController : IDisposable
    {
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
        }

        private void HandleInitialized (int holdDuration)
        {
            view.SetJoinMatchMessageText(holdDuration);
        }

        private void HandleIntroOver ()
        {
            model.StartListeningToInput();
            match.Initialize();
            view.SetJoinMatchMessageActive(true);
        }

        private void HandlePlayerLeft (int playerNumber, char leftKey, char rightKey)
        {
            model.ReturnKeys(leftKey, rightKey);
        }

        private void HandlePlayerReturned (char leftKey, char rightKey)
        {
            model.RecoverKeys(leftKey, rightKey);
        }

        public void Dispose ()
        {
            model.OnInitialized -= HandleInitialized;
            view.OnIntroOver -= HandleIntroOver;
            match.OnPlayerLeft -= HandlePlayerLeft;
            match.OnPlayerReturned -= HandlePlayerReturned;
        }
    }
}