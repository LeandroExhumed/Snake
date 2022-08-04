using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeView : MonoBehaviour
    {
        public event Action OnUpdate;

        private void Update ()
        {
            OnUpdate?.Invoke();
        }
    }
}