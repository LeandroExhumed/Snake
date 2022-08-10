using System;

namespace LeandroExhumed.SnakeGame.Match
{
    [Serializable]
    public class LobbyPersistentData
    {
        public char[] UnavailableKeys { get; private set; }

        public LobbyPersistentData (char[] unavailableKeys)
        {
            UnavailableKeys = unavailableKeys;
        }
    }
}