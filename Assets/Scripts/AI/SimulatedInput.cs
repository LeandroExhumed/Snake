using LeandroExhumed.SnakeGame.Block;
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
        public event Action<int> OnMovementRequested;
        public event Action<List<PathNode>> OnPathChanged;
        public event Action OnDestroyed;

        private List<PathNode> Path
        {
            get => path;
            set
            {
                path = value;
                OnPathChanged?.Invoke(value);
            }
        }

        private const float MIN_INPUT_SPEED = 0.5F;
        private const float MAX_INPUT_SPEED = 2F;

        private List<PathNode> path;
        private int targetNode = 0;
        private IBlockModel targetBlock;
        private bool reasonedAboutNewBlock = false;

        private readonly AIData data;
        private ISnakeModel snake;

        private readonly PathFinding pathFinding;
        private readonly MonoBehaviour monoBehaviour;

        private readonly IMatchModel match;

        public SimulatedInput (AIData data, PathFinding pathFinding, MonoBehaviour monoBehaviour, IMatchModel match)
        {
            this.data = data;            
            this.pathFinding = pathFinding;
            this.monoBehaviour = monoBehaviour;
            this.match = match;
        }

        public void Initialize (ISnakeModel snake)
        {
            this.snake = snake;
            Initialize();
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
                    data.MinReasoningTimeToPlanPathAfterObstruction,
                    data.MaxReasoningTimeToPlanPathAfterObstruction);
                monoBehaviour.StartCoroutine(ThinkAboutNewPath(reasoningTime));
            }
        }

        public void HandleBlockCollected (IBlockModel block)
        {
            if (targetBlock.IsEqual(block))
            {
                targetBlock = null;
            }
        }

        public void Destroy ()
        {
            OnDestroyed?.Invoke();
        }

        private void FindPath (Vector2Int end)
        {
            Path = pathFinding.FindPath(snake.Position, end);
            targetNode = 0;
        }

        private IEnumerator RequestMovementInput (int input)
        {
            float reasoningTime = UnityEngine.Random.Range(MIN_INPUT_SPEED, MAX_INPUT_SPEED);
            yield return new WaitForSeconds(snake.TimeToMove - reasoningTime);
            OnMovementRequested?.Invoke(input);
        }

        private IEnumerator ThinkAboutNewPath (float reasoningSpeed)
        {
            if (reasonedAboutNewBlock)
            {
                yield return null;
            }

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
                    FindPath(targetBlock.Position);
                }
                else
                {
                    return;
                }
            }

            Vector2Int targetDirection = Path[targetNode + 1].Position - snake.Position;
            targetNode++;

            if (snake.Direction == targetDirection)
            {
                return;
            }

            // TODO: Improve this
            int input = 0;
            if (targetDirection.y > 0)
            {
                input = snake.Direction.x > 0 ? -1 : 1;
            }
            else if (targetDirection.y < 0)
            {
                input = snake.Direction.x > 0 ? 1 : -1;
            }
            if (targetDirection.x > 0)
            {
                input = snake.Direction.y > 0 ? 1 : -1;
            }
            else if (targetDirection.x < 0)
            {
                input = snake.Direction.y > 0 ? -1 : 1;
            }
            monoBehaviour.StartCoroutine(RequestMovementInput(input));
        }

        private void HandleBlockGenerated (IBlockModel block)
        {
            if (targetBlock != null)
            {
                float currentDistance = Vector2Int.Distance(snake.Position, targetBlock.Position);
                if (currentDistance < Vector2Int.Distance(snake.Position, block.Position))
                {
                    return;
                }
            }

            targetBlock = block;
            reasonedAboutNewBlock = false;
            float reasoningTime = UnityEngine.Random.Range(
                data.MinReasoningTimeToPlanPathToBlock,
                data.MaxReasoningTimeToPlanPathToBlock);
            monoBehaviour.StartCoroutine(ThinkAboutNewPath(reasoningTime));
        }

        public void Dispose ()
        {
            snake.OnPositionChanged -= HandlePositionChanged;
            match.OnBlockGenerated -= HandleBlockGenerated;
        }
    }
}