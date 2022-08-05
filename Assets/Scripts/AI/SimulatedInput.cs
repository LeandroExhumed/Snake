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

        private float safeInputTime = 0.15f;
        private const float SAFE_INPUT_DELAY = 0.1F;

        List<PathNode> path;
        int nextNode = 0;
        private Vector2Int block;
        private bool reasonedAboutNewBlock = false;

        private readonly ISnakeModel snake;
        private readonly IGridModel<INode> grid;
        private readonly PathFinding pathFinding;
        private readonly MonoBehaviour monoBehaviour;

        private readonly MatchModel match;

        public SimulatedInput (ISnakeModel snake, IGridModel<INode> grid, PathFinding pathFinding, MonoBehaviour monoBehaviour, MatchModel match)
        {
            this.snake = snake;
            this.grid = grid;
            this.pathFinding = pathFinding;
            this.monoBehaviour = monoBehaviour;
            this.match = match;
        }

        public void Initialize ()
        {
            snake.OnPositionChanged += HandlePositionChanged;
            snake.OnTimeToMoveChanged += HandleTimeToMoveChanged;
            grid.OnNodeChanged += HandleGridNodeChanged;
            match.OnBlockGenerated += HandleBlockGenerated;
        }

        public void HandleGridNodeChanged (Vector2Int nodePosition)
        {
            if (Path == null)
            {
                return;
            }

            int pathindex = Path.FindIndex(x => x.Position == nodePosition);
            if (nodePosition != snake.Position && pathindex > nextNode)
            {
                Debug.Log("Changed block " + nodePosition);
                monoBehaviour.StartCoroutine(delayedFindPath());
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

            Vector2Int input = Path[nextNode + 1].Position - snake.Position;
            nextNode++;
            Debug.Log("Node: " + nextNode);

            if (snake.Direction == input)
            {
                return;
            }

            Debug.Log("From " + snake.Position + " to " + Path[nextNode].Position + "Input: " + input);
            monoBehaviour.StartCoroutine(delayedInput(input));
        }

        private void HandleTimeToMoveChanged (float value)
        {
            safeInputTime = value - SAFE_INPUT_DELAY;
        }

        private IEnumerator delayedInput (Vector2Int input)
        {
            yield return new WaitForSeconds(safeInputTime);
            OnMovementRequested?.Invoke(input);
        }

        private IEnumerator delayedFindPath ()
        {
            Path = null;
            yield return new WaitForSeconds(0.5f);

            reasonedAboutNewBlock = true;
        }

        private void HandleBlockGenerated (Vector2Int position)
        {
            block = position;
            reasonedAboutNewBlock = false;
            monoBehaviour.StartCoroutine(delayedFindPath());
        }

        private void FindPath (int endX, int endY)
        {
            Path = pathFinding.FindPath(snake.Position.x, snake.Position.y, endX, endY);
            if (Path != null)
            {
                for (int i = 0; i < Path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(Path[i].x, Path[i].y), new Vector3(Path[i + 1].x, Path[i + 1].y), color: Color.red);
                    Debug.Break();
                }
            }

            nextNode = 0;
        }
    }
}