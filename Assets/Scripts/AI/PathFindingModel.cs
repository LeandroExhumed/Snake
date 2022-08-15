using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class PathFindingModel : IPathFindingModel
    {
        private List<IPathNodeModel> openList;
        private List<IPathNodeModel> closedList;

        private readonly AIData data;

        private readonly IGridModel<INodeModel> levelGrid;
        private readonly GridModel<IPathNodeModel> grid;

        public PathFindingModel (AIData data, IGridModel<INodeModel> levelGrid)
        {
            this.data = data;
            this.levelGrid = levelGrid;

            grid = new GridModel<IPathNodeModel>(levelGrid.Width, levelGrid.Height);
        }

        public List<IPathNodeModel> FindPath (Vector2Int start, Vector2Int end)
        {
            UpdateGridBasedOnLevel(levelGrid);

            IPathNodeModel startNode = grid.GetNode(start);
            IPathNodeModel endNode = grid.GetNode(end);

            openList = new List<IPathNodeModel>() { startNode };
            closedList = new List<IPathNodeModel>();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    IPathNodeModel node = grid.GetNode(new Vector2Int(x, y));
                    node.GCost = int.MaxValue;
                    node.CameFrom = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);

            while (openList.Count > 0)
            {
                IPathNodeModel currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                List<IPathNodeModel> neighbourList = GetNeighbourList(currentNode);
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
                        neighbourList[i].CameFrom = currentNode;
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
                    Vector2Int position = new Vector2Int(x, y);
                    bool isWalkable = true;
                    if (levelGrid.GetNode(position) is IBlockModel block)
                    {
                        isWalkable = !block.IsAttached;
                    }
                    grid.SetNode(position, new PathNodeModel(new Vector2Int(x, y), isWalkable));
                }
            }
        }

        private int CalculateDistanceCost (IPathNodeModel a, IPathNodeModel b)
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

        private IPathNodeModel GetLowestFCostNode (List<IPathNodeModel> pathNodeList)
        {
            IPathNodeModel lowestFCostNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }

            return lowestFCostNode;
        }

        private List<IPathNodeModel> CalculatePath (IPathNodeModel endNode)
        {
            List<IPathNodeModel> path = new List<IPathNodeModel>();
            path.Add(endNode);
            IPathNodeModel currentNode = endNode;
            while (currentNode.CameFrom != null)
            {
                path.Add(currentNode.CameFrom);
                currentNode = currentNode.CameFrom;
            }
            path.Reverse();

            return path;
        }

        private List<IPathNodeModel> GetNeighbourList (IPathNodeModel currentNode)
        {
            List<IPathNodeModel> neighbourList = new List<IPathNodeModel>();

            if (currentNode.Position.x - 1 >= 0)
            {
                Vector2Int leftNeighbour = new Vector2Int(currentNode.Position.x - 1, currentNode.Position.y);
                neighbourList.Add(GetNode(leftNeighbour));
            }
            if (currentNode.Position.x + 1 < grid.Width)
            {
                Vector2Int rightNeighbour = new Vector2Int(currentNode.Position.x + 1, currentNode.Position.y);
                neighbourList.Add(GetNode(rightNeighbour));
            }
            Vector2Int downNeighbour = new Vector2Int(currentNode.Position.x, currentNode.Position.y - 1);
            if (currentNode.Position.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(downNeighbour));
            }
            Vector2Int upNeighbour = new Vector2Int(currentNode.Position.x, currentNode.Position.y + 1);
            if (currentNode.Position.y + 1 < grid.Height)
            {
                neighbourList.Add(GetNode(upNeighbour));
            }

            return neighbourList;
        }

        private IPathNodeModel GetNode (Vector2Int position)
        {
            return grid.GetNode(position);
        }
    }
}