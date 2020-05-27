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

        public int Width => Math.Abs(MaxX) - Math.Abs(MinX) + 1;

        public int Height => Math.Abs(MaxY) - Math.Abs(MinY) + 1;

        public int Area => Width * Height;

        public Coordinate Center => new Coordinate(MaxX - (Width / 2), MaxY - (Height / 2));

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

        public (Boundary Top, Boundary Bottom) CutHorizontally(int y)
        {
            var bottomDimensions = new Dimensions(Width, y - MinY);
            var topDimensions = new Dimensions(Width, Height - bottomDimensions.Height);

            var bottom = FromDimensions(
                bottomDimensions,
                MinX,
                MinY);

            var top = FromDimensions(
                topDimensions,
                MinX,
                MinY + bottomDimensions.Height);

            return (top, bottom);
        }

        public (Boundary Left, Boundary Right) CutVertically(int x)
        {
            var leftDimensions = new Dimensions(x - MinX, Height);
            var rightDimensions = new Dimensions(Width - leftDimensions.Width, Height);

            var left = FromDimensions(
                leftDimensions,
                MinX,
                MinY);

            var right = FromDimensions(
                rightDimensions,
                MinX + leftDimensions.Width,
                MinY);

            return (left, right);
        }

        public Boundary TrimRight(int width)
        {
            return CutVertically(MinX + Width - width).Left;
        }

        public Boundary TrimLeft(int width)
        {
            return CutVertically(MinX + width).Right;
        }

        public Boundary TrimTop(int height)
        {
            return CutHorizontally(MinY + Height - height).Bottom;
        }

        public Boundary TrimBottom(int height)
        {
            return CutHorizontally(height).Top;
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
}
