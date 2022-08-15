using LeandroExhumed.SnakeGame.Block;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ICollectorModel
    {
        void CollectEnginePower (IBlockModel block, float speedAddition);
        void CollectBatteringRam (IBlockModel block);
        void CollectTimeTravel (IBlockModel block);
    }
}