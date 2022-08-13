namespace LeandroExhumed.SnakeGame.Block
{
    public class BatteringRamBlockModel : BlockModel
    {
        public BatteringRamBlockModel (BlockData data) : base(data) { }

        public override void ApplyEffect ()
        {
            owner.CollectBatteringRam(this);
        }
    }
}