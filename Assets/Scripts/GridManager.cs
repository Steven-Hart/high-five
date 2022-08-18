using System;
using UnityEditor;
using UnityEngine.Tilemaps;

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

        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        private RectTransform rectTransform => transform as RectTransform;

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
                    grid[x, y] = new Node(nodeWorldPosition)
                    {
                        tileStatus = Node.CheckStatus(nodeWorldPosition, nodeRadius, unwalkableMask)
                    };
                }
            }
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
                    Gizmos.DrawWireCube(new Vector3(
                        node.worldPosition.x,
                        node.worldPosition.y,
                        1), Vector3.one * (nodeDiameter - .1f));
                    
                }
            }
        }
    }
}