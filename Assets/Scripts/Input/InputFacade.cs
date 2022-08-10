using System;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Input
{
    public class InputFacade : IMovementRequester
    {
        public event Action<int> OnMovementRequested;

        private readonly InputAction action;

        public InputFacade (char leftKey, char rightKey)
        {
            action = new("Move");
            action.AddCompositeBinding("1DAxis")
                .With("Negative", $"<Keyboard>/{leftKey}")
                .With("Positive", $"<Keyboard>/{rightKey}");
            action.Enable();
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