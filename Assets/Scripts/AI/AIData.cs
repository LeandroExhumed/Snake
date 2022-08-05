using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    [CreateAssetMenu(fileName = "AI", menuName = "Data/AI")]
    public class AIData : ScriptableObject
    {
        public float MinReactionSpeedRegardingNewBlockGenerated => reasoningTimeToPlanPathToBlock.Min;
        public float MaxReactionSpeedRegardingNewBlockGenerated => reasoningTimeToPlanPathToBlock.Max;
        public float MinReactionSpeedRegardingPathObstructed => reasoningTimeToPlanPathAfterObstruction.Min;
        public float MaxReactionSpeedRegardingPathObstructed => reasoningTimeToPlanPathAfterObstruction.Max;

        [SerializeField]
        [Tooltip("How fast(in seconds) the AI will plan a path to the new block.")]
        private MinMaxField reasoningTimeToPlanPathToBlock;
        [SerializeField]
        [Tooltip("How fast(in seconds) the AI will plan a new path when the current was obstructed.")]
        private MinMaxField reasoningTimeToPlanPathAfterObstruction;

        private void OnValidate ()
        {
            ValidateFields(reasoningTimeToPlanPathToBlock);
            ValidateFields(reasoningTimeToPlanPathAfterObstruction);
        }

        private void ValidateFields (MinMaxField field)
        {
            field.Min = Mathf.Clamp(field.Min, 0, field.Max);
            field.Max = Mathf.Max(field.Min, field.Max);
        }
    }

    [Serializable]
    public class MinMaxField
    {
        private const float RECOMMENDED_MIN = 0.1F;
        private const float RECOMMENDED_MAX = 0.5F;

        public float Min = RECOMMENDED_MIN;
        public float Max = RECOMMENDED_MAX;
    }
}