using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class BatteringRamBlockModel : BlockModel
    {
        public BatteringRamBlockModel (BlockData data) : base(data) { }

        public override void BeCollected (ICollector collector)
        {
            collector.CollectBatteringRam(this);
            base.BeCollected(collector);
        }
    }
}