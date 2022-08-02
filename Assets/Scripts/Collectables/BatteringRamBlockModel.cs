using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class BatteringRamBlockModel : CollectableModel
    {
        protected override void BeCollected (ICollector collector)
        {
            collector.CollectBatteringRam();
        }
    }
}