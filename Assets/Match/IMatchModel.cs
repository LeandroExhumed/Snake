using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public interface IMatchModel
    {
        event Action OnInitialized;
        event Action<IBlockModel> OnBlockGenerated;
        event Action<int, Vector2Int> OnSnakePositionChanged;
        event Action<int> OnPlayerLeft;
        event Action OnRewind;
        event Action<int> OnOver;

        void Initialize ();
        void AddPlayer (char leftKey, char rightKey);
        void Play (int selectedSnakeID, int playerNumber, IPlayerInput input);
        void Rewind ();
    }
}