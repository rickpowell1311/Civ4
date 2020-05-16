﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class SeedGroup : IEquatable<SeedGroup>
    {
        private readonly HashSet<Tile> _chosenTiles;
        private readonly Boundary _boundary;

        public Seed Seed { get; }

        public IEnumerable<Tile> Tiles => _chosenTiles;

        public SeedGroup(Seed seed, Boundary boundary)
        {
            Seed = seed;
            _boundary = boundary;
            _chosenTiles = new HashSet<Tile> { seed.Tile };
        }

        public void Grow()
        {
            var growth = new HashSet<Tile>();

            foreach (var tile in _chosenTiles)
            {
                if (!TryPickNextTile(tile, out var next))
                {
                    continue;
                }

                growth.Add(next);
            }

            foreach (var chosenTile in growth)
            {
                _chosenTiles.Add(chosenTile);
            }
        }

        public bool Overlaps(SeedGroup other)
        {
            foreach (var chosenTile in _chosenTiles)
            {
                if (other._chosenTiles.Contains(chosenTile))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryPickNextTile(Tile tile, out Tile next)
        {
            var neighbours = tile.Neighbours.ToHashSet();

            var availableTiles = neighbours
                .Where(x => !_chosenTiles.Contains(x) && x.IsWithinBoundary(_boundary))
                .ToHashSet();

            if (availableTiles.Count == 0)
            {
                next = null;
                return false;
            }

            next = availableTiles
                .Select(x => new TileChoice(x, _boundary, _chosenTiles))
                .ChooseOne()
                .Tile;

            return true;
        }

        public override string ToString()
        {
            return $"SeedGroup with Seed {Seed}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SeedGroup);
        }

        public bool Equals(SeedGroup other)
        {
            return other != null &&
                EqualityComparer<Seed>.Default.Equals(Seed, other.Seed);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Seed);
        }

        public static bool operator ==(SeedGroup first, SeedGroup second)
        {
            return first?.Seed == second?.Seed;
        }

        public static bool operator !=(SeedGroup first, SeedGroup second)
        {
            return !(first == second);
        }
    }

    public static class SeedGroupExtensions
    {
        public static bool AreAllOverlapping(this IEnumerable<SeedGroup> seedGroups)
        {
            if (seedGroups.Count() == 1)
            {
                return true;
            }

            foreach (var seedGroup in seedGroups)
            {
                var otherSeedGroups = seedGroups.Except(new[] { seedGroup });

                if (!otherSeedGroups.Any(x => x.Overlaps(seedGroup)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
