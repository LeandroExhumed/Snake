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
        public event Action<Collider> OnCollision;

        public void Destroy ()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter (Collider other)
        {
            OnCollision?.Invoke(other);
        }
    }
}