using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public interface IPathFindingModel
    {
        List<IPathNodeModel> FindPath (Vector2Int start, Vector2Int end);
    }
}