using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchStartup : MonoBehaviour
    {
        private IMatchModel match;

        [Inject]
        public void Constructor (IMatchModel match)
        {
            this.match = match;
        }

        private void Start ()
        {
            match.Initialize();
        }
    }
}