using System;
using System.Collections.Generic;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class Seed : IEquatable<Seed>
    {
        public Tile Tile { get; }

        public Seed(Tile tile)
        {
            Tile = tile;
        }

        public override string ToString()
        {
            return Tile.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Seed);
        }

        public bool Equals(Seed other)
        {
            return other != null &&
                EqualityComparer<Tile>.Default.Equals(Tile, other.Tile);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tile);
        }

        public static bool operator ==(Seed first, Seed second)
        {
            return first?.Tile == second?.Tile;
        }

        public static bool operator !=(Seed first, Seed second)
        {
            return !(first == second);
        }
    }
}
