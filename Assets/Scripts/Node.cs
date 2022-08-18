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
    public class Node
    {
        public Vector2 worldPosition;
        public TileStatus tileStatus;

        public Node(Vector2 position, TileStatus status = TileStatus.Walkable)
        {
            worldPosition = position;
            tileStatus = status;
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