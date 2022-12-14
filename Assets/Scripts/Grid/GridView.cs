using UnityEngine;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField]
        private Color primaryColor = Color.black;
        [SerializeField]
        private Color secondaryColor = Color.gray;

        private const float GRID_HALF_SIZE = 0.5F;

        public void Initialize (INodeModel[,] array)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    SpriteRenderer entry = Instantiate(sprite, new Vector3(x, y, 0), Quaternion.identity, transform);
                    if ((x + y) % 2 == 0)
                    {
                        entry.color = primaryColor;
                    }
                    else
                    {
                        entry.color = secondaryColor;
                    }
                }
            }

            CenterizeCamera(array);
        }

        private void CenterizeCamera (INodeModel[,] array)
        {
            float xCenter = GetCellCenter(array.GetLength(0));
            float yCenter = GetCellCenter(array.GetLength(1));
            Camera.main.transform.position = new Vector3(xCenter, yCenter, Camera.main.transform.position.z);
        }

        private float GetCellCenter (int dimension)
        {
            return (dimension / 2) - GRID_HALF_SIZE;
        }
    }
}