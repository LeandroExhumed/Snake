using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
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