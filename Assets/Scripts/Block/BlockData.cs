using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    [CreateAssetMenu(fileName = "Block", menuName = "Data/Block")]
    public class BlockData : ScriptableObject
    {
        public int ID => id;
        public float MoveCost => moveCost;

        [SerializeField]
        private int id;
        [SerializeField]
        [Tooltip("How much (in seconds) is added to the snake's movement rate")]
        private float moveCost = 0.05f;
    }
}