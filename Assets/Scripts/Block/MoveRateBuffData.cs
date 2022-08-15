using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    [CreateAssetMenu(fileName = "MoveRateBuff", menuName = "Data/Blocks/MoveRateBuff")]
    public class MoveRateBuffData : BlockData
    {
        public float MoveRateAddition => moveRateAddition;

        [SerializeField]
        private float moveRateAddition = 0.075f;

        private void OnValidate ()
        {
            moveRateAddition = Mathf.Max(MoveCost, moveRateAddition);
        }
    }
}