using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Input
{
    public class InputFacade
    {
        public InputAction Movement => input.Gameplay.Movement;

        private readonly PlayerInput input;

        public InputFacade (PlayerInput input)
        {
            this.input = input;

            input.Enable();
        }
    }
}