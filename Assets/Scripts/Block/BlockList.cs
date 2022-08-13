using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    [CreateAssetMenu(fileName = "BlockList", menuName = "Data/Block List")]
    public class BlockList : ScriptableObject
    {
        public BlockData[] Blocks => blocks;

        [SerializeField]
        private BlockData[] blocks;
    }
}