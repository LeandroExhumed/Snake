using System;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Input
{
    public class PlayerInputModel : IPlayerInputModel
    {
        public event Action<int> OnMovementRequested;

        public char LeftKey { get; private set; }
        public char RightKey { get; private set; }

        private readonly InputAction action;

        public PlayerInputModel (char leftKey, char rightKey)
        {
            LeftKey = leftKey;
            RightKey = rightKey;

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