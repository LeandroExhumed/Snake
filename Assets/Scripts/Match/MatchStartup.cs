using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchStartup : MonoBehaviour
    {
        private ILobbyModel lobby;

        [Inject]
        public void Constructor (ILobbyModel lobby)
        {
            this.lobby = lobby;
        }

        private void Start ()
        {
            lobby.Initialize();
        }
    }
}