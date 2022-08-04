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

        private readonly PathFinding pathFinding;
        private readonly MonoBehaviour monoBehaviour;

        public SimulatedInput (PathFinding pathFinding, MonoBehaviour monoBehaviour)
        {
            this.pathFinding = pathFinding;
            this.monoBehaviour = monoBehaviour;
        }

        public void Initialize ()
        {
            List<PathNode> path = pathFinding.FindPath(0, 0, 15, 15);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y), new Vector3(path[i + 1].x, path[i + 1].y));
                    Debug.Break();
                }
            }
            //monoBehaviour.StartCoroutine(TickRoutine());
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