using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    [CreateAssetMenu(fileName = "SnakeList", menuName = "Data/Snake List")]
    public class SnakeList : ScriptableObject
    {
        public SnakeData[] Snakes => snakes;

        [SerializeField]
        private SnakeData[] snakes;
    }
}