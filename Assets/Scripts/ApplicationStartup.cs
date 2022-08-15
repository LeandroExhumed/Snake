using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class ApplicationStartup : MonoBehaviour
    {
        private ILobbyModel lobby;

        [Inject]
        public void Constructor (ILobbyModel lobby)
        {
            this.lobby = lobby;
        }

        private void Awake ()
        {
            lobby.Initialize();
        }
    }
}