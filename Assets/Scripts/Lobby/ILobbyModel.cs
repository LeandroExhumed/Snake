using System;

namespace LeandroExhumed.SnakeGame.Match
{
    public interface ILobbyModel
    {
        event Action<int> OnInitialized;
        event Action<char, char> OnNewPlayerJoined;

        void Initialize ();
        void StartListeningToInput ();
        void RecoverKeys (char leftKey, char rightKey);
        void ReturnKeys (char leftKey, char rightKey);
    }
}