using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections;
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

        private readonly MonoBehaviour monoBehaviour;

        public SimulatedInput (MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour;
        }

        public void Initialize ()
        {
            monoBehaviour.StartCoroutine(TickRoutine());
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