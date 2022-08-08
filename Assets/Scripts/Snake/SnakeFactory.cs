namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeFactory
    {
        private readonly ISnakeModel.Factory[] factories;

        public SnakeFactory (ISnakeModel.Factory[] factories)
        {
            this.factories = factories;
        }

        public ISnakeModel Create (int id)
        {
            return factories[id - 1].Create();
        }
    }
}