using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    [CreateAssetMenu(fileName = "Match", menuName = "Data/Match")]
    public class MatchData : ScriptableObject
    {
        public Vector2Int BoardSize => boardSize;

        [SerializeField]
        private Vector2Int boardSize;

        private const int MIN_BOARD_DIMENSION = 30;

        private void OnValidate ()
        {
            boardSize = Vector2Int.Max(Vector2Int.one * MIN_BOARD_DIMENSION, boardSize);
        }
    }
}