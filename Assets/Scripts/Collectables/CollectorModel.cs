namespace LeandroExhumed.SnakeGame.Snake
{
    public class CollectorModel : ICollector
    {
        private readonly ISnakeModel snake;

        public CollectorModel (ISnakeModel snake)
        {
            this.snake = snake;
        }

        public void CollectBatteringRam ()
        {
            ApplyCommonEffect();
            //snake.ApplyBatteringRamEffect();
        }

        public void CollectEnginePower (float speedAddition)
        {
            ApplyCommonEffect();
            //snake.IncreaseSpeed(speedAddition);
        }

        private void ApplyCommonEffect ()
        {
            snake.Grow();
        }
    }
}