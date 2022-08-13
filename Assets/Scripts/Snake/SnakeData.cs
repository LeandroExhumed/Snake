using LeandroExhumed.SnakeGame.Block;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    [CreateAssetMenu(fileName = "Snake", menuName = "Data/Snake")]
    public class SnakeData : ScriptableObject
    {
        public int ID => id;
        public BlockData[] StartingBlocks => startingBlocks;
        public float BaseMoveRate => baseMoveRate;

        [SerializeField]
        private int id;
        [SerializeField]
        private BlockData[] startingBlocks;
        [SerializeField]
        [Tooltip("Move rate without considering any block benefit.")]
        private float baseMoveRate = 0.25f;
    }
}