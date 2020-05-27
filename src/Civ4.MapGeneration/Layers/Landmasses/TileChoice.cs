using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class TileChoice : IEquatable<TileChoice>
    {
        public Tile Tile { get; }

        public bool IsAdjacentToTileGroup { get; }

        public bool IsAlongBoundary { get; }

        public TileChoice(Tile tile, bool isAlongBoundary, bool isAdjacentToTileGroup)
        {
            Tile = tile;
            IsAdjacentToTileGroup = isAdjacentToTileGroup;
            IsAlongBoundary = isAlongBoundary;
        }

        public double GetProbabilityWeighting()
        {
            return IsAlongBoundary ? 1d
                : IsAdjacentToTileGroup ? 4d 
                : 2d;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TileChoice);
        }

        public bool Equals(TileChoice other)
        {
            return other != null &&
                EqualityComparer<Tile>.Default.Equals(Tile, other.Tile);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tile);
        }

        public static bool operator ==(TileChoice first, TileChoice second)
        {
            return first?.Tile == second?.Tile;
        }

        public static bool operator !=(TileChoice first, TileChoice second)
        {
            return !(first == second);
        }
    }
}
