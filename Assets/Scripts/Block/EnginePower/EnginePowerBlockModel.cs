using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    public class EnginePowerBlockModel : BlockModel
    {
        public EnginePowerBlockModel (BlockData data) : base(data) { }

        public override void ApplyEffect ()
        {
            if (data is MoveRateBuffData buff)
            {
                owner.CollectEnginePower(this, buff.MoveRateAddition);
                RemoveBenefit();
            }
            else
            {
                Debug.LogError("This data doesn't contain any move rate buff.");
            }            
        }
    }
}