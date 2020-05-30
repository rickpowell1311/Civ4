using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class TileChoice : IEquatable<TileChoice>
    {
        public Tile Tile { get; }

        public bool IsAdjacentToTileGroup { get; }

        public bool IsTowardsBoundaryCenterPoint { get; }

        public bool IsAlongBoundary { get; }

        public TileChoice(Tile tile, bool isAlongBoundary, bool isAdjacentToTileGroup, bool isTowardsBoundaryCenterPoint)
        {
            Tile = tile;
            IsAdjacentToTileGroup = isAdjacentToTileGroup;
            IsTowardsBoundaryCenterPoint = isTowardsBoundaryCenterPoint;
            IsAlongBoundary = isAlongBoundary;
        }

        public double GetProbabilityWeighting()
        {
            var @default = 2d;

            if (IsTowardsBoundaryCenterPoint)
            {
                return 12d;
            }

            if (IsAdjacentToTileGroup)
            {
                return 3d;
            }

            if (IsAlongBoundary)
            {
                return 1d;
            }

            return @default;
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
