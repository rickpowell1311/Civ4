using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class Boundary : IEquatable<Boundary>
    {
        public int MaxX { get; }

        public int MaxY { get; }

        public int MinX { get; }
        
        public int MinY { get; }

        private HashSet<Coordinate> ContainedCoordinates
        {
            get
            {
                var coordinates = new HashSet<Coordinate>();

                for (int i = MinX; i <= MaxX; i++)
                {
                    for (int j = MinY; j <= MaxY; j++)
                    {
                        coordinates.Add(new Coordinate(i, j));
                    }
                }

                return coordinates;
            }
        }

        private Boundary(int maxX, int maxY, int minX, int minY)
        {
            MaxX = maxX;
            MaxY = maxY;
            MinX = minX;
            MinY = minY;
        }

        public static Boundary FromDimensions(Dimensions dimensions, int minX, int minY)
        {
            return new Boundary(minX + dimensions.Width - 1, minY + dimensions.Height - 1, minX, minY);
        }

        public IEnumerable<SeedGroup> GenerateSeedGroups(int number)
        {
            var allSeedOptions = ContainedCoordinates
                .Select(x => new Seed(new Tile(x)))
                .ToList();

            var picker = new Random();
            var pickedSeeds = new HashSet<Seed>();
            for (int i = 0; i < number; i++)
            {
                var availableSeedOptions = allSeedOptions
                    .Except(pickedSeeds)
                    .ToArray();

                var pick = availableSeedOptions[picker.Next(0, availableSeedOptions.Length)];
                pickedSeeds.Add(pick);
            }

            return pickedSeeds
                .Select(x => new SeedGroup(x, this))
                .ToList();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Boundary);
        }

        public bool Equals(Boundary other)
        {
            return other != null &&
                EqualityComparer<int>.Default.Equals(MaxX, other.MaxX) &&
                EqualityComparer<int>.Default.Equals(MaxY, other.MaxY) &&
                EqualityComparer<int>.Default.Equals(MinX, other.MinX) &&
                EqualityComparer<int>.Default.Equals(MinY, other.MinY);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MaxX, MaxY, MinX, MinY);
        }

        public static bool operator ==(Boundary first, Boundary second)
        {
            return first?.MaxY == second?.MaxY
                && first?.MaxX == second?.MaxX
                && first?.MinY == second?.MinY
                && first?.MinX == second?.MinX;
        }

        public static bool operator !=(Boundary first, Boundary second)
        {
            return !(first == second);
        }
    }

    public static class TileExtensions
    {
        public static bool IsWithinBoundary(this Tile tile, Boundary boundary)
        {
            return tile.Location.X >= boundary.MinX
                && tile.Location.X <= boundary.MaxX
                && tile.Location.Y >= boundary.MinY
                && tile.Location.Y <= boundary.MaxY;
        }

        public static bool IsAtMinWidth(this Tile tile, Boundary boundary)
        {
            return tile.Location.X == boundary.MinX;
        }

        public static bool IsAtMaxWidth(this Tile tile, Boundary boundary)
        {
            return tile.Location.X == boundary.MaxX;
        }

        public static bool IsAtMinHeight(this Tile tile, Boundary boundary)
        {
            return tile.Location.Y == boundary.MinY;
        }

        public static bool IsAtMaxHeight(this Tile tile, Boundary boundary)
        {
            return tile.Location.Y == boundary.MaxY;
        }

        public static bool IsAlongBoundary(this Tile tile, Boundary boundary)
        {
            return tile.IsAtMinWidth(boundary)
                || tile.IsAtMinHeight(boundary)
                || tile.IsAtMaxWidth(boundary)
                || tile.IsAtMaxHeight(boundary);
        }
    }
}
