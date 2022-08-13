using LeandroExhumed.SnakeGame.Block;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    [CreateAssetMenu(fileName = "Snake", menuName = "Data/Snake")]
    public class SnakeData : ScriptableObject
    {
        public int ID => id;
        public BlockData[] StartingBlocks => startingBlocks;
        public float Speed => speed;

        [SerializeField]
        private int id;
        [SerializeField]
        private BlockData[] startingBlocks;
        [SerializeField]
        private float speed;
    }
}