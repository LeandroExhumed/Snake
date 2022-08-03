using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class BatteringRamBlockModel : CollectableModel
    {
        public override void BeCollected (ICollector collector)
        {
            collector.CollectBatteringRam();
            base.BeCollected(collector);
        }
    }
}