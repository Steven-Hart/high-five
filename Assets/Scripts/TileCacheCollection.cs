using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HighFive.Grid
{
    [Serializable]
    public class TileCacheCollection<T> : ICollection where T: Tile
    {
        [SerializeField]
        private List<T> _innerItems;
        public int Count => _innerItems.Count;

        private bool _isSynchronized;
        public bool IsSynchronized => _isSynchronized;
        
        private object _syncRoot;
        public object SyncRoot => _syncRoot;

        public TileCacheCollection()
        {
            _innerItems = new List<T>();
        }

        public T this[int index]
        {
            get => (T)_innerItems[index];
            set => _innerItems[index] = value;
        }


        public IEnumerator GetEnumerator()
        {
            return new TileCacheEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds to collection. Cannot add duplicate items
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentException("Item cannot be null");
            }
            if (Contains(item))
            {
                throw new ArgumentException("Item is already within collection");
            }
            _innerItems.Add(item);
        }

        public bool Contains(T item)
        {
            return Count != 0 && Enumerable.Contains(_innerItems, item);
        }

        /// <summary>
        /// Returns inner item at equality match
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool TryGetFromCoordinate(Coordinate coordinate, out Tile tile)
        {
            tile = null;
            foreach (var innerItem in _innerItems)
            {
                if (innerItem.CurrentPosition.Equals(coordinate))
                {
                    tile = innerItem;
                }
            }
            return tile != null;
        }

        /// <summary>
        /// Destroys all tile objects in collection
        /// </summary>
        public void DeleteAllTiles()
        {
            for (int i = 0; i < Count; i++)
            {
                Object.DestroyImmediate(_innerItems[i].gameObject);
            }
            _innerItems.Clear();
        }

        public void CopyTo(Array array, int index)
        {
            _innerItems.CopyTo(array as T[], index);
        }
    }

    [Serializable]
    public class TileCache : TileCacheCollection<Tile>
    {
        public void InitialiseTileCache(Transform parentTransform, GameObject tilePrefab, List<Coordinate> grid, int tileSize)
        {
            if (tilePrefab.GetComponent<Tile>() == null)
            {
                throw new ArgumentException("Tile prefab cannot have no Tile component");
            }
            foreach (var coordinate in grid)
            {
                var instance = Object.Instantiate(tilePrefab, parentTransform);
                var tileComponent = instance.GetComponent<Tile>();
                Add(tileComponent);
                tileComponent.SetCurrentPosition(coordinate, tileSize);
            }
        }

        public void RepositionTiles(int tileSize)
        {
            for (int i = 0; i < Count; i++)
            {
                var tile = this[i];
                tile.AdjustTransformToCoordinate(tileSize);
            }
        }
    }

    public class TileCacheEnumerator<T> : IEnumerator<T> where T : Tile
    {
        private TileCacheCollection<T> _collection;
        private int currentIndex;
        private T current;

        public TileCacheEnumerator(TileCacheCollection<T> collection)
        {
            _collection = collection;
            currentIndex = -1;
            current = default(T);
        }

        public bool MoveNext()
        {
            // Avoid going beyond collection bounds
            if(++currentIndex >= _collection.Count)
            {
                return false;
            }
            current = _collection[currentIndex];
            return true;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        public T Current => current;
        object IEnumerator.Current => Current;
        public void Dispose()
        {
        }
    }
}