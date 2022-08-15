using LeandroExhumed.SnakeGame.Block;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public interface IMatchModel
    {
        event Action<IBlockModel> OnBlockGenerated;
        event Action<int, Vector2Int> OnSnakePositionChanged;
        event Action<Vector2Int?> OnSnakeHit;
        event Action<int, char, char> OnPlayerLeft;
        event Action<char, char> OnPlayerReturned;
        event Action<int> OnOver;
        event Action OnRestarted;

        bool IsRunning { get; }

        void Initialize ();
        void Begin ();
        void AddPlayer (char leftKey, char rightKey);
        void Rewind ();
        void Restart ();
    }
}