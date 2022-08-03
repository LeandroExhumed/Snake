using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchStartup : MonoBehaviour
    {
        private MatchModel match;

        [Inject]
        public void Constructor (MatchModel match)
        {
            this.match = match;
        }

        private void Start ()
        {
            match.Initialize();
        }
    }
}