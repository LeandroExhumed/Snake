using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Input
{
    public class InputFacade : IMovementRequester
    {
        public event Action<Vector2Int> OnMovementRequested;

        public InputAction Movement => input.Gameplay.Movement;

        private readonly PlayerInput input;

        public InputFacade (PlayerInput input)
        {
            this.input = input;
        }

        public void Initialize ()
        {
            input.Enable();
            input.Gameplay.Movement.performed += HandleMovementInputperformed;
        }

        private void HandleMovementInputperformed (InputAction.CallbackContext obj)
        {
            OnMovementRequested?.Invoke(Vector2Int.RoundToInt(obj.ReadValue<Vector2>()));
        }
    }
}