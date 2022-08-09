using LeandroExhumed.SnakeGame.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Match
{
    public interface IMatchModel
    {
        event Action OnInitialized;
        event Action<Vector2Int> OnBlockGenerated;
        event Action<int, Vector2Int> OnSnakePositionChanged;
        event Action<int> OnPlayerLeft;
        event Action<int> OnOver;

        void Initialize ();
        void AddPlayer (InputAction inputAction);
        void Play (int selectedSnakeID, int playerNumber, IMovementRequester input);
        void GenerateBlock ();
        void End ();
    }
}