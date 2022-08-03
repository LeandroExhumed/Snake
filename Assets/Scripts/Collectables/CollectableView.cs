using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class CollectableView : MonoBehaviour
    {
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void Destroy ()
        {
            Destroy(gameObject);
        }
    }
}