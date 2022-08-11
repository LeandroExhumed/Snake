namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ICollector
    {
        void CollectEnginePower (IBlockModel block, float speedAddition);
        void CollectBatteringRam (IBlockModel block);
        void CollectTimeTravel (IBlockModel block);
    }
}