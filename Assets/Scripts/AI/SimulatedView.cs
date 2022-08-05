﻿using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class SimulatedView : MonoBehaviour
    {
        [SerializeField]
        private bool debug;

        private PathNode[] path;

        public void SetPath (PathNode[] path)
        {
            this.path = path;
        }

        private void OnDrawGizmos ()
        {
            if (!debug || path == null)
            {
                return;
            }

            for (int i = 0; i < path.Length - 1; i++)
            {
                Gizmos.DrawLine(
                    new Vector3(path[i].x, path[i].y), new Vector3(path[i + 1].x, path[i + 1].y));
            }
        }
    }
}