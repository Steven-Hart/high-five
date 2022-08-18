using System;
using System.Collections.Generic;
using UnityEditor;

namespace HighFive.Grid
{
    using UnityEngine;

    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GridManager gridManager = (GridManager)target;

            if (GUILayout.Button("Initialise Grid"))
            {
                gridManager.Initialise();
            }
        }
    }

    public class GridManager : MonoBehaviour
    {

        public LayerMask unwalkableMask;

        public Vector2 gridBounds;
        public float nodeRadius;
        public Node[,] grid;

        public List<Node> path;

        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        private RectTransform rectTransform => transform as RectTransform;

        public void Awake()
        {
            Initialise();
        }

        public void Initialise()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = (int)Math.Round(gridBounds.x/nodeDiameter, MidpointRounding.ToEven);
            gridSizeY = (int)Math.Round(gridBounds.y/nodeDiameter, MidpointRounding.ToEven);
            CreateGrid();
        }

        private void CreateGrid()
        {
            grid = new Node[gridSizeX,gridSizeY];
            Vector2 worldBottomLeft = rectTransform.position 
                                      - (Vector3.right * gridBounds.x/2) // Move Left
                                      - (Vector3.up * gridBounds.y/2); // Move Down

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 nodeWorldPosition = worldBottomLeft + 
                                                Vector2.right * (x * nodeDiameter + nodeRadius) +
                                                Vector2.up * (y * nodeDiameter + nodeRadius);
                    grid[x, y] = new Node(nodeWorldPosition, x, y)
                    {
                        tileStatus = Node.CheckStatus(nodeWorldPosition, nodeRadius, unwalkableMask)
                    };
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if(x == 0 && y == 0) continue;
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if (checkX >= 0 && checkX < gridSizeX
                                    && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridBounds.x/2) / gridBounds.x;
            float percentY = (worldPosition.y + gridBounds.y/2) / gridBounds.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            
            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x, y];
        }

        // Visualize the grid
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridBounds.x, gridBounds.y, 1));
            if (grid != null)
            {
                foreach (var node in grid)
                {
                    Gizmos.color = (node.tileStatus == TileStatus.Walkable)? Color.white : Color.red;
                    if (path != null)
                    {
                        if (path.Contains(node))
                        {
                            Gizmos.color = Color.green;
                        }
                    }
                    Gizmos.DrawWireCube(new Vector3(
                        node.worldPosition.x,
                        node.worldPosition.y,
                        1), Vector3.one * (nodeDiameter - .1f));
                    
                }
            }
        }
    }
}