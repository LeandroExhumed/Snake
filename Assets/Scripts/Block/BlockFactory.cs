using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    public class BlockFactory
    {
        private readonly IBlockModel.Factory[] factories;

        public BlockFactory (IBlockModel.Factory[] factories)
        {
            this.factories = factories;
        }

        public IBlockModel Create (int id)
        {
            return factories[id - 1].Create();
        }

        public IBlockModel CreateRandomly (int startIndex = 0)
        {
            return factories[Random.Range(startIndex, factories.Length)].Create();
        }
    }
}