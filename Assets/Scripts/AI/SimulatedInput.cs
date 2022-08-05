using LeandroExhumed.SnakeGame.Match;
using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class SimulatedInput : IMovementRequester
    {
        public event Action<Vector2Int> OnMovementRequested;

        private readonly Vector2Int[] directions = {
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.down
        };

        private float lastReasonTime;
        private float reasoningTime = 0.5f;

        List<PathNode> path;
        int nextNode = 0;
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
            match.OnBlockGenerated += HandleBlockGenerated;
            //monoBehaviour.StartCoroutine(TickRoutine());
        }

        private void HandlePositionChanged (ISnakeModel arg1, Vector2Int arg2)
        {
            if (!reasonedAboutNewBlock && (Time.time - lastReasonTime < reasoningTime))
            {
                FindPath(block.x, block.y);
                nextNode = 0;

                reasonedAboutNewBlock = true;
            }

            nextNode++;
            Vector2Int input = path[nextNode].Position - snake.Position;
            Debug.Log("Node: " + nextNode);
            
            if (snake.Direction == input)
            {
                return;
            }

            
            Debug.Log("From " + snake.Position + " to " + path[nextNode].Position + "Input: " + input);
            monoBehaviour.StartCoroutine(delayedInput(input));
        }

        IEnumerator delayedInput (Vector2Int input)
        {
            yield return new WaitForSeconds(0.15f);
            OnMovementRequested?.Invoke(input);
        }

        private void HandleBlockGenerated (Vector2Int position)
        {
            block = position;
            reasonedAboutNewBlock = false;
            lastReasonTime = Time.time;
        }

        private void FindPath (int endX, int endY)
        {
            path = pathFinding.FindPath(snake.Position.x, snake.Position.y, endX, endY);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y), new Vector3(path[i + 1].x, path[i + 1].y), color: Color.red);
                    Debug.Break();
                }
            }
        }

        private IEnumerator TickRoutine ()
        {
            while (true)
            {
                int directionIndex = UnityEngine.Random.Range(0, directions.Length);
                OnMovementRequested?.Invoke(directions[directionIndex]);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}