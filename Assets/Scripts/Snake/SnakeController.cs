using LeadroExhumed.SnakeGame.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeController : IDisposable
    {
        private readonly ISnakeModel model;

        private readonly InputFacade input;

        private bool usingMovementAxis = false;

        public SnakeController (ISnakeModel model, InputFacade input)
        {
            this.model = model;
            this.input = input;
        }

        public void Setup ()
        {
            input.Movement.performed += HandleMovementInputPerformed;
            input.Movement.canceled += HandleMovementInputCanceled;
        }

        private void HandleMovementInputPerformed (InputAction.CallbackContext obj)
        {
            if (usingMovementAxis)
            {
                return;
            }

            model.LookTo(Vector2Int.RoundToInt(obj.ReadValue<Vector2>()));
            usingMovementAxis = true;
        }

        private void HandleMovementInputCanceled (InputAction.CallbackContext obj)
        {
            usingMovementAxis = false;
            model.LookTo(Vector2Int.RoundToInt(obj.ReadValue<Vector2>()));
        }

        public void Dispose ()
        {
            input.Movement.performed -= HandleMovementInputPerformed;
            input.Movement.canceled -= HandleMovementInputCanceled;
        }
    }
}