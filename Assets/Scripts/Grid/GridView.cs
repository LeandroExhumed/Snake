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

        public void Initialize (INode[,] array)
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
        }
    }
}