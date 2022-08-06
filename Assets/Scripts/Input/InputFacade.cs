using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Input
{
    public class InputFacade : IMovementRequester
    {
        public event Action<int> OnMovementRequested;

        private readonly PlayerInput input;

        public InputFacade (PlayerInput input)
        {
            this.input = input;
        }

        public void Initialize ()
        {
            input.Enable();
            input.Gameplay.Move.performed += HandleMovementInputperformed;
        }

        private void HandleMovementInputperformed (InputAction.CallbackContext obj)
        {
            OnMovementRequested?.Invoke((int)(obj.ReadValue<float>()));
        }
    }
}