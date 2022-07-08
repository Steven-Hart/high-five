using System;

namespace HighFive.Grid
{
    [Serializable]
    public struct Coordinate : IEquatable<Coordinate>
    {
        public int x;
        public int y;

        public Coordinate(int width, int height)
        {
            x = width;
            y = height;
        }
        
        #region Equatable
        public bool Equals(Coordinate other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}