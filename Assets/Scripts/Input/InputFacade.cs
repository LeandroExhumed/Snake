using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Input
{
    public class InputFacade : IMovementRequester
    {
        public event Action<int> OnMovementRequested;

        private readonly PlayerInput input;

        private readonly List<char> UnavailableKeys = new();
        private readonly List<char> currentHeldKeys = new();

        public InputFacade (PlayerInput input)
        {
            this.input = input;
        }

        public void Initialize ()
        {
            char[] qwertyKeys =
            {
                '1','2','3','4','5',
                'q','w','e','r','t'
            };

            InputAction[] actions = new InputAction[qwertyKeys.Length];
            for (int i = 0; i < qwertyKeys.Length; i++)
            {
                actions[i] = new InputAction();
                actions[i].AddBinding($"<Keyboard>/{qwertyKeys[i]}").WithInteraction("hold(duration=2)");
                actions[i].Enable();

                actions[i].performed += HandleAnyKeyPerformed;
                actions[i].canceled += HandleAnyKeyCanceled;
            }

            InputAction move = new("Move");
            move.AddCompositeBinding("1DAxis")
                .With("Negative", "<Keyboard>/q")
                .With("Positive", "<Keyboard>/w");
            move.Enable();

            //input.Enable();
            move.performed += HandleMovementInputperformed;
        }

        private void HandleAnyKeyPerformed (InputAction.CallbackContext obj)
        {
            if (UnavailableKeys.Contains(GetKey(obj)))
            {
                return;
            }

            currentHeldKeys.Add(char.Parse(obj.control.name));
            if (currentHeldKeys.Count >= 2)
            {
                char left = currentHeldKeys[0];
                char right = currentHeldKeys[1];
                UnavailableKeys.Add(left);
                UnavailableKeys.Add(right);
                currentHeldKeys.Clear();
                Debug.Log("Left: " + left + ", right: " + right);
            }
        }

        private void HandleAnyKeyCanceled (InputAction.CallbackContext obj)
        {
            if (UnavailableKeys.Contains(GetKey(obj)))
            {
                return;
            }

            currentHeldKeys.Remove(char.Parse(obj.control.name));            
        }

        private void HandleMovementInputperformed (InputAction.CallbackContext obj)
        {
            OnMovementRequested?.Invoke((int)(obj.ReadValue<float>()));
        }

        private char GetKey (InputAction.CallbackContext callback)
        {
            return char.Parse(callback.control.name);
        }
    }
}