using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class Landmass
    {
        public IEnumerable<Tile> Tiles { get; }

        private Landmass(IEnumerable<SeedGroup> seedGroups)
        {
            Tiles = seedGroups
                .SelectMany(x => x.Tiles)
                .Distinct();
        }

        public static Landmass Build(Dimensions dimensions, int? numberOfSeedGroups = default)
        {
            // Create a boundary for the landmass to be built within - build boundary with positive coordinates for readability
            var boundary = Boundary.FromDimensions(dimensions, 0, 0);

            // Place 1 seed in every 6 tiles if not specified
            var numberOfSeeds = numberOfSeedGroups ?? dimensions.Area / 6 + 1;
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
