using LeandroExhumed.SnakeGame.Grid;
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

        private float reasoningTime = 0.5f;

        private float safeInputTime = 0.2f;
        private const float SAFE_INPUT_DELAY = 0.05F;

        List<PathNode> path;
        int targetNode = 0;
        private Vector2Int block;
        private bool reasonedAboutNewBlock = false;

        private readonly ISnakeModel snake;
        private readonly PathFinding pathFinding;
        private readonly MonoBehaviour monoBehaviour;

        private readonly MatchModel match;

        public SimulatedInput (ISnakeModel snake, PathFinding pathFinding, MonoBehaviour monoBehaviour, MatchModel match)
        {
            this.snake = snake;
            this.pathFinding = pathFinding;
            this.monoBehaviour = monoBehaviour;
            this.match = match;
        }

        public void Initialize ()
        {
            snake.OnPositionChanged += HandlePositionChanged;
            snake.OnTimeToMoveChanged += HandleTimeToMoveChanged;
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
                monoBehaviour.StartCoroutine(ThinkAboutPathObstructed());
            }
        }

        private void HandlePositionChanged (ISnakeModel arg1, Vector2Int arg2)
        {
            if (Path == null)
            {
                if (reasonedAboutNewBlock)
                {
                    FindPath(block.x, block.y);
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

        private void HandleTimeToMoveChanged (float value)
        {
            safeInputTime = value - SAFE_INPUT_DELAY;
        }

        private IEnumerator RequestMovementInput (Vector2Int input)
        {
            yield return new WaitForSeconds(safeInputTime);
            OnMovementRequested?.Invoke(input);
        }

        private IEnumerator ThinkAboutPathObstructed ()
        {
            Path = null;
            yield return new WaitForSeconds(reasoningTime);
            reasonedAboutNewBlock = true;
        }

        private void HandleBlockGenerated (Vector2Int position)
        {
            block = position;
            reasonedAboutNewBlock = false;
            monoBehaviour.StartCoroutine(ThinkAboutPathObstructed());
        }

        private void FindPath (int endX, int endY)
        {
            Path = pathFinding.FindPath(snake.Position.x, snake.Position.y, endX, endY);
            targetNode = 0;
        }
    }
}