using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class Landmass
    {
        public IEnumerable<Tile> LandTiles { get; }

        private Landmass(IEnumerable<SeedGroup> seedGroups)
        {
            LandTiles = seedGroups
                .SelectMany(x => x.Tiles)
                .Distinct();
        }

        public static Landmass Build(Boundary boundary, int? numberOfSeedGroups = default)
        {
            // Place 1 seed in every 6 tiles if not specified
            var numberOfSeeds = numberOfSeedGroups ?? boundary.Area / 6 + 1;
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
    }
}
