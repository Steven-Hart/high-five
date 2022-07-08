using UnityEditor;

namespace HighFive.Grid
{
    using System.Collections.Generic;
    using UnityEngine;

    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GridManager gridManager = (GridManager)target;

            if (GUILayout.Button("Grid Reset"))
            {
                gridManager.ResetGrid();
            }

            if (GUILayout.Button("Tile Reset"))
            {
                gridManager.ResetTileCache();
            }

            if (GUILayout.Button("Update Tile Size"))
            {
                gridManager.UpdateTileSize();
            }
        }
    }

    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject tilePrefab;
        
        // Define the bounds of the grid
        [SerializeField]
        private int xMin = 0;
        [SerializeField]
        private int xMax = 100;

        [SerializeField]
        private int yMin = 0;
        [SerializeField]
        private int yMax = 100;

        [SerializeField]
        private int tileSize = 1;
        
        [SerializeField]
        private List<Coordinate> grid;
        [SerializeField]
        private TileCache tileCache;
        
        // Fill grid to bounds
        public void ResetGrid()
        {
            grid = new List<Coordinate>();
            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    grid.Add(new Coordinate(x,y));
                }
            }
        }

        public void ResetTileCache()
        {
            ResetGrid();
            tileCache.DeleteAllTiles();
            tileCache.InitialiseTileCache(gameObject.transform, tilePrefab, grid, tileSize);
        }

        public void UpdateTileSize()
        {
            tileCache.RepositionTiles(tileSize);
        }
    }
}

