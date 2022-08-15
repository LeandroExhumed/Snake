using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class PathFinding
    {
        private List<PathNodeModel> openList;
        private List<PathNodeModel> closedList;

        private readonly AIData data;

        private readonly IGridModel<INodeModel> levelGrid;
        private readonly GridModel<PathNodeModel> grid;

        public PathFinding (AIData data, IGridModel<INodeModel> levelGrid)
        {
            this.data = data;
            this.levelGrid = levelGrid;

            grid = new GridModel<PathNodeModel>(levelGrid.Width, levelGrid.Height);
        }

        public List<PathNodeModel> FindPath (Vector2Int start, Vector2Int end)
        {
            UpdateGridBasedOnLevel(levelGrid);

            PathNodeModel startNode = grid.GetNode(start);
            PathNodeModel endNode = grid.GetNode(end);

            openList = new List<PathNodeModel>() { startNode };
            closedList = new List<PathNodeModel>();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    PathNodeModel node = grid.GetNode(new(x, y));
                    node.GCost = int.MaxValue;
                    node.CameFromNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);

            while (openList.Count > 0)
            {
                PathNodeModel currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                List<PathNodeModel> neighbourList = GetNeighbourList(currentNode);
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

        private void UpdateGridBasedOnLevel (IGridModel<INodeModel> levelGrid)
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
                    grid.SetNode(position, new PathNodeModel(new(x, y), isWalkable));
                }
            }
        }

        private int CalculateDistanceCost (PathNodeModel a, PathNodeModel b)
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

        private PathNodeModel GetLowestFCostNode (List<PathNodeModel> pathNodeList)
        {
            PathNodeModel lowestFCostNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }

            return lowestFCostNode;
        }

        private List<PathNodeModel> CalculatePath (PathNodeModel endNode)
        {
            List<PathNodeModel> path = new();
            path.Add(endNode);
            PathNodeModel currentNode = endNode;
            while (currentNode.CameFromNode != null)
            {
                path.Add(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }
            path.Reverse();

            return path;
        }

        private List<PathNodeModel> GetNeighbourList (PathNodeModel currentNode)
        {
            List<PathNodeModel> neighbourList = new();

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

        private PathNodeModel GetNode (Vector2Int position)
        {
            return grid.GetNode(position);
        }
    }
}