using System;

namespace HighFive.Grid
{
    using UnityEngine;
    
    public enum TileStatus
    {
        Walkable = 0,
        Unwalkable = 1,
        Unreachable = 2,
    }

    [Serializable]
    public class Node : IHeapItem<Node>
    {
        public Vector2 worldPosition;
        public TileStatus tileStatus;

        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;

        public int gridX;
        public int gridY;

        public Node parent;

        private int heapIndex;

        public Node(Vector2 position, int gridX, int gridY ,TileStatus status = TileStatus.Walkable)
        {
            worldPosition = position;
            tileStatus = status;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(Node otherNode)
        {
            int compare = fCost.CompareTo(otherNode.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(otherNode.hCost);
            }
            return -compare;
        }
        
        public static TileStatus CheckStatus(Vector3 position, float radius, LayerMask layerMask)
        {
            if (Physics2D.OverlapBox(position, new Vector2(radius,radius), 0,layerMask))
            {
                return TileStatus.Unwalkable;
            }
            return TileStatus.Walkable;
        }
    }
}