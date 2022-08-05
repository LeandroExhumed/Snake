using LeandroExhumed.SnakeGame.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.AI
{
    public class PathFinding
    {
        private const int MOVE_STRAIGHT_COST = 10;

        private List<PathNode> openList;
        private List<PathNode> closedList;

        private readonly GridModel<PathNode> grid;

        public PathFinding ()
        {
            grid = new GridModel<PathNode>(30, 30);
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    grid.SetNode(x, y, new PathNode(x, y));
                }
            }
        }

        public List<PathNode> FindPath (int startX, int startY, int endX, int endY)
        {
            PathNode startNode = grid.GetNode(startX, startY);
            PathNode endNode = grid.GetNode(endX, endY);

            openList = new List<PathNode>() { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    PathNode node = grid.GetNode(x, y);
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
                    Debug.Log("Final node reached!");
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

        private List<PathNode> GetNeighbourList (PathNode currentNode)
        {
            List<PathNode> neighbourList = new();

            if (currentNode.x -1 >= 0)
            {
                // Left
                neighbourList.Add(GetNode(currentNode.x -1, currentNode.y));
            }
            if (currentNode.x + 1 < grid.Width)
            {
                // Right
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            }
            // Down
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
            }
            // Up
            if (currentNode.y + 1 < grid.Height)
            {
                neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
            }

            return neighbourList;
        }

        private PathNode GetNode (int x, int y)
        {
            return grid.GetNode(x, y);
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

        private int CalculateDistanceCost (PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);

            return Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
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
    }
}