using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    [CreateAssetMenu(fileName = "Block", menuName = "Data/Block")]
    public class BlockData : ScriptableObject
    {
        public int ID => id;

        [SerializeField]
        private int id;
    }
}