using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class PathFinding
    {
        private const int MOVE_STRAIGHT_COST = 10;

        private List<PathNode> openList;
        private List<PathNode> closedList;

        private readonly AIData data;

        private readonly IGridModel<INode> levelGrid;
        private readonly GridModel<PathNode> grid;

        public PathFinding (AIData data, IGridModel<INode> levelGrid)
        {
            this.data = data;
            this.levelGrid = levelGrid;

            grid = new GridModel<PathNode>(levelGrid.Width, levelGrid.Height);
        }

        public List<PathNode> FindPath (Vector2Int start, Vector2Int end)
        {
            UpdateGridBasedOnLevel(levelGrid);

            PathNode startNode = grid.GetNode(start);
            PathNode endNode = grid.GetNode(end);

            openList = new List<PathNode>() { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    PathNode node = grid.GetNode(new(x, y));
                    node.GCost = int.MaxValue;
                    node.CalculateFCost();
                    node.CameFromNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                List<PathNode> neighbourList = GetNeighbourList(currentNode);
                for (int i = 0; i < neighbourList.Count; i++)
                {
                    if (closedList.Contains(neighbourList[i]))
                    {
                        continue;
                    }
                    if (!neighbourList[i].IsWalkable)
                    {
                        closedList.Add(neighbourList[i]);
                        
                        continue;
                    }

                    int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourList[i]);
                    if (tentativeGCost < neighbourList[i].GCost)
                    {
                        neighbourList[i].CameFromNode = currentNode;
                        neighbourList[i].GCost = tentativeGCost;
                        neighbourList[i].HCost = CalculateDistanceCost(neighbourList[i], endNode);
                        neighbourList[i].CalculateFCost();

                        if (!openList.Contains(neighbourList[i]))
                        {
                            openList.Add(neighbourList[i]);
                        }
                    }
                }
            }

            Debug.LogError("No path to use");

            return null;
        }

        private void UpdateGridBasedOnLevel (IGridModel<INode> levelGrid)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Vector2Int position = new(x, y);
                    bool isWalkable = true;
                    if (levelGrid.GetNode(position) is IBlockModel block)
                    {
                        isWalkable = !block.IsAttached;
                    }
                    grid.SetNode(position, new PathNode(new(x, y), isWalkable));
                }
            }
        }

        private int CalculateDistanceCost (PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.Position.x - b.Position.x);
            int yDistance = Mathf.Abs(a.Position.y - b.Position.y);

            int shortestDistance = Mathf.Min(xDistance, yDistance);
            if (Random.value >= data.BestPathFindingRate)
            {
                return shortestDistance;
            }
            else
            {
                if (Random.value >= data.Luck)
                {
                    return shortestDistance;
                }
                else
                {
                    return Mathf.Max(xDistance, yDistance);
                }
            }            
        }

        private PathNode GetLowestFCostNode (List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }

            return lowestFCostNode;
        }

        private List<PathNode> CalculatePath (PathNode endNode)
        {
            List<PathNode> path = new();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.CameFromNode != null)
            {
                path.Add(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }
            path.Reverse();

            return path;
        }

        private List<PathNode> GetNeighbourList (PathNode currentNode)
        {
            List<PathNode> neighbourList = new();

            if (currentNode.Position.x -1 >= 0)
            {
                Vector2Int leftNeighbour = new(currentNode.Position.x - 1, currentNode.Position.y);
                neighbourList.Add(GetNode(leftNeighbour));
            }
            if (currentNode.Position.x + 1 < grid.Width)
            {
                Vector2Int rightNeighbour = new(currentNode.Position.x + 1, currentNode.Position.y);
                neighbourList.Add(GetNode(rightNeighbour));
            }
            Vector2Int downNeighbour = new(currentNode.Position.x, currentNode.Position.y - 1);
            if (currentNode.Position.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(downNeighbour));
            }
            Vector2Int upNeighbour = new(currentNode.Position.x, currentNode.Position.y + 1);
            if (currentNode.Position.y + 1 < grid.Height)
            {
                neighbourList.Add(GetNode(upNeighbour));
            }

            return neighbourList;
        }

        private PathNode GetNode (Vector2Int position)
        {
            return grid.GetNode(position);
        }
    }
}