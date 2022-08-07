using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
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
            int index = Random.Range(startIndex, factories.Length);
            Debug.Log("Index: " + index);
            return factories[index].Create();
        }
    }
}