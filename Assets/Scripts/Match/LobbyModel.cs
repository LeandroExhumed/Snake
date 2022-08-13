﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Match
{
    public class LobbyModel
    {
        public event Action<char, char> OnNewPlayerJoined;

        private readonly List<char> unavailableKeys = new();
        private readonly List<char> currentHeldKeys = new();

        private readonly MatchData data;

        public LobbyModel (MatchData data)
        {
            this.data = data;
        }

        public void Initialize ()
        {
            char[] qwertyKeys =
            {
                '1','2','3','4','5','6','7','8','9','0',
                'q','w','e','r','t','y','u','i','o','p',
                'a','s','d','f','g','h','j','k','l',
                'z','x','c','v','b','n','m'
            };

            InputAction[] actions = new InputAction[qwertyKeys.Length];
            for (int i = 0; i < qwertyKeys.Length; i++)
            {
                actions[i] = new InputAction();
                actions[i].AddBinding($"<Keyboard>/{qwertyKeys[i]}")
                    .WithInteraction($"hold(duration={data.KeyHoldDuration})");
                actions[i].Enable();

                actions[i].performed += HandleAnyKeyHeld;
                actions[i].canceled += HandleAnyKeyReleased;
            }
        }

        public void ReturnKeys (char leftKey, char rightKey)
        {
            unavailableKeys.Remove(leftKey);
            unavailableKeys.Remove(rightKey);
        }

        public void RecoverKeys (char leftKey, char rightKey)
        {
            unavailableKeys.Add(leftKey);
            unavailableKeys.Add(rightKey);
        }

        private void HandleAnyKeyHeld (InputAction.CallbackContext obj)
        {
            if (unavailableKeys.Contains(GetKey(obj)))
            {
                return;
            }

            currentHeldKeys.Add(GetKey(obj));
            if (currentHeldKeys.Count >= 2)
            {
                char left = currentHeldKeys[0];
                char right = currentHeldKeys[1];
                unavailableKeys.Add(left);
                unavailableKeys.Add(right);
                currentHeldKeys.Clear();

                OnNewPlayerJoined?.Invoke(left, right);
            }
        }

        private void HandleAnyKeyReleased (InputAction.CallbackContext callback)
        {
            if (unavailableKeys.Contains(GetKey(callback)))
            {
                return;
            }

            currentHeldKeys.Remove(GetKey(callback));
        }

        private char GetKey (InputAction.CallbackContext callback)
        {
            return char.Parse(callback.control.name);
        }
    }
}