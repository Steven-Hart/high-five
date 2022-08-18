using System;
using System.Collections.Generic;
using System.Linq;
using HighFive.Grid;
using UnityEngine;

namespace HighFive.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        private GridManager grid;

        public Transform StartTransform;
        public Transform TargetTransform;

        public void Awake()
        {
            grid = GetComponent<GridManager>();
            
        }

        public void Update()
        {
            FindPath(StartTransform.position, TargetTransform.position);
        }

        public void FindPath(Vector3 start, Vector3 target)
        {
            Node startNode = grid.NodeFromWorldPoint(start);
            Node targetNode = grid.NodeFromWorldPoint(target);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost ||
                        openSet[i].fCost == currentNode.fCost
                        && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (var neighbour in grid.GetNeighbours(currentNode))
                {
                    if(neighbour.tileStatus != TileStatus.Walkable 
                       || closedSet.Contains(neighbour)) continue;
                    int newMoveCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMoveCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMoveCost;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        public void RetracePath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node currentNode = end;

            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            grid.path = path;
        }

        private int GetDistance(Node first, Node second)
        {
            int distanceX = Mathf.Abs(first.gridX - second.gridX);
            int distanceY = Mathf.Abs(first.gridY - second.gridY);

            if (distanceX > distanceY)
            {
                return 14*distanceY + 10*(distanceX - distanceY);
            }
            return 14*distanceX + 10*(distanceY - distanceX);
        }
    }
}

