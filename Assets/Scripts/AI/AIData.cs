using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    [CreateAssetMenu(fileName = "AI", menuName = "Data/AI")]
    public class AIData : ScriptableObject
    {
        public float BestPathFindingRate => bestPathFindingRate;
        public float Luck => luck;
        public float MinReasoningTimeToPlanPathToBlock => reasoningTimeToPlanPathToBlock.Min;
        public float MaxReasoningTimeToPlanPathToBlock => reasoningTimeToPlanPathToBlock.Max;
        public float MinReasoningTimeToPlanPathAfterObstruction => reasoningTimeToPlanPathAfterObstruction.Min;
        public float MaxReasoningTimeToPlanPathAfterObstruction => reasoningTimeToPlanPathAfterObstruction.Max;

        [Header("Path finding")]
        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Probability(0 - 1) of finding the most optimized path to the target.")]
        private float bestPathFindingRate = 0.5f;
        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Probability(0 - 1) of choosing the most optimized path to the target after miscalculating.")]
        private float luck = 0.5f;

        [Header("Reasoning time")]
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