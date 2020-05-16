using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class Tile : IEquatable<Tile>
    {
        public Coordinate Location { get; }

        public IEnumerable<Tile> DiagonalNeighbours
        {
            get
            {
                return new[]
                {
                    new Tile(new Coordinate(Location.X + 1, Location.Y + 1)),
                    new Tile(new Coordinate(Location.X + 1, Location.Y - 1)),
                    new Tile(new Coordinate(Location.X - 1, Location.Y + 1)),
                    new Tile(new Coordinate(Location.X - 1, Location.Y - 1))
                };
            }
        }

        public IEnumerable<Tile> AdjacentNeighbours
        {
            get
            {
                return new[]
                {
                    new Tile(new Coordinate(Location.X + 1, Location.Y)),
                    new Tile(new Coordinate(Location.X, Location.Y + 1)),
                    new Tile(new Coordinate(Location.X - 1, Location.Y)),
                    new Tile(new Coordinate(Location.X, Location.Y - 1)),
                };
            }
        }

        public IEnumerable<Tile> Neighbours => DiagonalNeighbours.Concat(AdjacentNeighbours);

        public Tile(Coordinate location)
        {
            Location = location;
        }

        public bool IsAdjecentTo(Tile tile)
        {
            return AdjacentNeighbours.Contains(tile);
        }

        public override string ToString()
        {
            return Location.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tile);
        }

        public bool Equals(Tile other)
        {
            return other != null &&
                EqualityComparer<Coordinate>.Default.Equals(Location, other.Location);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location);
        }

        public static bool operator ==(Tile first, Tile second)
        {
            return first?.Location == second?.Location;
        }

        public static bool operator !=(Tile first, Tile second)
        {
            return !(first == second);
        }
    }
}
