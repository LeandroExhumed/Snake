using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class AIInputView : MonoBehaviour
    {
        [SerializeField]
        private bool debug;

        private PathNodeModel[] path;

        public void SetPath (PathNodeModel[] path)
        {
            this.path = path;
        }

        public void Destroy () => Destroy(gameObject);

        private void OnDrawGizmos ()
        {
            if (!debug || path == null)
            {
                return;
            }

            for (int i = 0; i < path.Length - 1; i++)
            {
                Gizmos.DrawLine(
                    new Vector3(path[i].Position.x, path[i].Position.y),
                    new Vector3(path[i + 1].Position.x, path[i + 1].Position.y));
            }
        }
    }
}