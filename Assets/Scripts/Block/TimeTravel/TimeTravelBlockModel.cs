namespace LeandroExhumed.SnakeGame.Block
{
    public class TimeTravelBlockModel : BlockModel
    {
        public TimeTravelBlockModel (BlockData data) : base(data) { }

        public override void ApplyEffect ()
        {
            owner.CollectTimeTravel(this);
        }
    }
}