using LeandroExhumed.SnakeGame.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeController : IDisposable
    {
        private readonly ISnakeModel model;

        private readonly InputFacade input;

        public SnakeController (ISnakeModel model, InputFacade input)
        {
            this.model = model;
            this.input = input;
        }

        public void Setup ()
        {
            input.Movement.performed += HandleMovementInputPerformed;
        }

        private void HandleMovementInputPerformed (InputAction.CallbackContext obj)
        {

            model.LookTo(Vector2Int.RoundToInt(obj.ReadValue<Vector2>()));
        }

        public void Dispose ()
        {
            input.Movement.performed -= HandleMovementInputPerformed;
        }
    }
}