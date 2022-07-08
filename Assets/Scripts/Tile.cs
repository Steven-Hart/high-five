using System;

namespace HighFive.Grid
{
    using UnityEngine;

    [Serializable]
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        private Coordinate currentPosition;

        public Coordinate CurrentPosition
        {
            get => currentPosition;
        }

        public void AdjustTransformToCoordinate(int tileSize)
        {
            int newX = CurrentPosition.x * tileSize;
            int newY = CurrentPosition.y * tileSize;

            var rectTransform = transform as RectTransform;
            rectTransform.anchoredPosition = new Vector2(newX, newY);
        }

        public void AdjustTileSize(int tileSize)
        {
            var rectTransform = transform as RectTransform;
            rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
        }

        public void SetCurrentPosition(Coordinate newPosition, int tileSize)
        {
            currentPosition = newPosition;
            AdjustTileSize(tileSize);
            AdjustTransformToCoordinate(tileSize);
            RenameObject();
        }

        private void RenameObject()
        {
            gameObject.name = $"({currentPosition.x},{currentPosition.y}) Tile";
        }
    }
}