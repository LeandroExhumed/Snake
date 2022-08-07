using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class EnginePowerBlockModel : BlockModel
    {
        public EnginePowerBlockModel (BlockData data) : base(data) { }

        private readonly float speedAddition = 0.075f;

        public override void BeCollected (ICollector collector)
        {
            collector.CollectEnginePower(this, speedAddition);
            base.BeCollected(collector);
        }
    }
}