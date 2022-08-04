using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class EnginePowerBlockModel : CollectableModel
    {
        private readonly float speedAddition = 0.075f;

        public override void BeCollected (ICollector collector)
        {
            collector.CollectEnginePower(speedAddition);
            base.BeCollected(collector);
        }
    }
}