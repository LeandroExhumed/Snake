using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static LeandroExhumed.SnakeGame.Grid.GridModel;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite;

        public void Initialize (INode[,] array)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    SpriteRenderer entry = Instantiate(sprite, new Vector3(x, y, 1), Quaternion.identity);
                    if ((x + y) % 2 == 0)
                    {
                        entry.color = Color.green;
                    }
                }
            }
        }
    }
}
