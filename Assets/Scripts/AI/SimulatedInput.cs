using LeandroExhumed.SnakeGame.Match;
using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class SimulatedInput : ISimulatedInput
    {
        public event Action<Vector2Int> OnMovementRequested;
        public event Action<List<PathNode>> OnPathChanged;

        private List<PathNode> Path
        {
            get => path;
            set
            {
                path = value;
                OnPathChanged?.Invoke(value);
            }
        }

        private const float MIN_INPUT_SPEED = 0.05F;
        private const float MAX_INPUT_SPEED = 0.2F;

        private List<PathNode> path;
        private int targetNode = 0;
        private Vector2Int block;
        private bool reasonedAboutNewBlock = false;

        private readonly AIData data;

        private readonly ISnakeModel snake;
        private readonly PathFinding pathFinding;
        private readonly MonoBehaviour monoBehaviour;

        private readonly MatchModel match;

        public SimulatedInput (AIData data, ISnakeModel snake, PathFinding pathFinding, MonoBehaviour monoBehaviour, MatchModel match)
        {
            this.data = data;
            this.snake = snake;
            this.pathFinding = pathFinding;
            this.monoBehaviour = monoBehaviour;
            this.match = match;
        }

        public void Initialize ()
        {
            snake.OnPositionChanged += HandlePositionChanged;
            match.OnBlockGenerated += HandleBlockGenerated;
        }

        public void HandleGridNodeChanged (Vector2Int nodePosition)
        {
            if (Path == null)
            {
                return;
            }

            int pathindex = Path.FindIndex(x => x.Position == nodePosition);
            if (nodePosition != snake.Position && pathindex > targetNode)
            {
                float reasoningTime = UnityEngine.Random.Range(
                    data.MinReactionSpeedRegardingPathObstructed,
                    data.MaxReactionSpeedRegardingPathObstructed);
                monoBehaviour.StartCoroutine(ThinkAboutNewPath(reasoningTime));
            }
        }

        private void FindPath (Vector2Int end)
        {
            Path = pathFinding.FindPath(snake.Position, end);
            targetNode = 0;
        }

        private IEnumerator RequestMovementInput (Vector2Int input)
        {
            float reasoningTime = UnityEngine.Random.Range(MIN_INPUT_SPEED, MAX_INPUT_SPEED);
            yield return new WaitForSeconds(snake.TimeToMove - reasoningTime);
            OnMovementRequested?.Invoke(input);
        }

        private IEnumerator ThinkAboutNewPath (float reasoningSpeed)
        {
            Path = null;
            yield return new WaitForSeconds(reasoningSpeed);
            reasonedAboutNewBlock = true;
        }

        private void HandlePositionChanged (ISnakeModel arg1, Vector2Int arg2)
        {
            if (Path == null)
            {
                if (reasonedAboutNewBlock)
                {
                    FindPath(block);
                }
                else
                {
                    return;
                }
            }

            Vector2Int input = Path[targetNode + 1].Position - snake.Position;
            targetNode++;

            if (snake.Direction == input)
            {
                return;
            }

            monoBehaviour.StartCoroutine(RequestMovementInput(input));
        }

        private void HandleBlockGenerated (Vector2Int position)
        {
            block = position;
            reasonedAboutNewBlock = false;
            float reasoningTime = UnityEngine.Random.Range(
                data.MinReactionSpeedRegardingNewBlockGenerated,
                data.MaxReactionSpeedRegardingNewBlockGenerated);
            monoBehaviour.StartCoroutine(ThinkAboutNewPath(reasoningTime));
        }
    }
}