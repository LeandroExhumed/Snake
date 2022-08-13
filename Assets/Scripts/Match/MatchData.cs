using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    [CreateAssetMenu(fileName = "Match", menuName = "Data/Match")]
    public class MatchData : ScriptableObject
    {
        public Vector2Int BoardSize => boardSize;
        public int AISnakesPerPlayer => aiSnakesPerPlayer;

        [SerializeField]
        private Vector2Int boardSize;
        [SerializeField]
        private int aiSnakesPerPlayer = 2;

        private const int MIN_BOARD_DIMENSION = 30;
        private const int MIN_AI_SNAKES_PER_PLAYER = 1;

        private void OnValidate ()
        {
            boardSize = Vector2Int.Max(Vector2Int.one * MIN_BOARD_DIMENSION, boardSize);
            aiSnakesPerPlayer = Mathf.Max(MIN_AI_SNAKES_PER_PLAYER, aiSnakesPerPlayer);
        }
    }
}