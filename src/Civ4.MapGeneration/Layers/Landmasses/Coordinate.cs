using System;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; }

        public int Y { get; }

        public double DistanceFromOrigin => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinate RelativeLocationTo(Coordinate other)
        {
            return new Coordinate(other.X - X, other.Y - Y);
        }

        public double DistanceFrom(Coordinate other)
        {
            return RelativeLocationTo(other).DistanceFromOrigin;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate other)
        {
            return this == other;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Coordinate first, Coordinate second)
        {
            return first?.X == second?.X && first?.Y == second?.Y;
        }

        public static bool operator !=(Coordinate first, Coordinate second)
        {
            return !(first == second);
        }

        public static Coordinate operator -(Coordinate first, Coordinate second)
        {
            return new Coordinate(first.X - second.X, first.Y - second.Y);
        }

        public static Coordinate operator +(Coordinate first, Coordinate second)
        {
            return new Coordinate(first.X + second.X, first.Y + second.Y);
        }
    }
}
