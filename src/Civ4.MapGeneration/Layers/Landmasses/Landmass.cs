using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class Landmass
    {
        public IEnumerable<Tile> LandTiles { get; }

        private Landmass(IEnumerable<SeedGroup> seedGroups)
            : this(seedGroups.SelectMany(x => x.Tiles))
        {
        }

        private Landmass(IEnumerable<Tile> tiles)
        {
            LandTiles = tiles;
        }

        public static Landmass Generate(Boundary boundary, int? numberOfSeedGroups = default)
        {
            // Place 1 seed in every 24 tiles if not specified
            var numberOfSeeds = numberOfSeedGroups ?? boundary.Area / 24 + 1;
            var seedGroups = boundary
                .GenerateSeedGroups(numberOfSeeds)
                .ToHashSet();

            // Continue to grow the seed groups until a landmass can be built
            while (!seedGroups.AreAllOverlapping())
            {
                foreach (var seedGroup in seedGroups)
                {
                    seedGroup.Grow();
                }

                // Centralize the seed groups to avoid truncating the seed group shapes at the boundary edges
                seedGroups = seedGroups
                    .Select(x => x.MoveAwayFromBoundary())
                    .ToHashSet();
            }

            return new Landmass(seedGroups);
        }

        public static Landmass Fill(Boundary boundary)
        {
            var landTiles = new HashSet<Tile>();

            for (int i = boundary.MinX; i <= boundary.MaxX; i++)
            {
                for (int j = boundary.MinY; j <= boundary.MaxY; j++)
                {
                    var tile = new Tile(new Coordinate(i, j));

                    landTiles.Add(tile);
                }
            }

            return new Landmass(landTiles);
        }
    }
}
