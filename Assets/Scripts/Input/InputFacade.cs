using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Input
{
    public class InputFacade : IMovementRequester
    {
        public event Action<int> OnMovementRequested;

        private readonly InputAction action;

        public InputFacade (InputAction action)
        {
            this.action = action;
        }

        public void Initialize ()
        {
            action.performed += HandleMovementInputperformed;
        }

        private void HandleMovementInputperformed (InputAction.CallbackContext callback)
        {
            OnMovementRequested?.Invoke((int)(callback.ReadValue<float>()));
        }
    }
}