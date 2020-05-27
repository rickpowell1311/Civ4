using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class World
    {
        public IEnumerable<Tile> LandTiles => _continents.SelectMany(x => x.LandTiles).ToList();

        public IEnumerable<Tile> IceTiles => _polarRegions.SelectMany(x => x.IceTiles).ToList();

        private readonly IEnumerable<ContinentalRegion> _continents;
        private readonly IEnumerable<PolarRegion> _polarRegions;

        private World(IEnumerable<ContinentalRegion> continents, IEnumerable<PolarRegion> polarRegions)
        {
            _continents = continents;
            _polarRegions = polarRegions;
        }

        public static World Continents(Dimensions dimensions, int number)
        {
            var worldBoundary = Boundary.FromDimensions(dimensions, 0, 0);

            var northPoleBoundary = worldBoundary
                .CutHorizontally(worldBoundary.MinY + worldBoundary.Height - PolarRegion.PolarHeight)
                .Top;
            var southPoleBoundary = worldBoundary
                .CutHorizontally(worldBoundary.MinY + PolarRegion.PolarHeight)
                .Bottom;

            var poles = new[]
            {
                PolarRegion.GenerateNorthPole(northPoleBoundary),
                PolarRegion.GenerateSouthPole(southPoleBoundary)
            };

            if (number == 2)
            {
                var westContinentalBoundary = worldBoundary
                    .TrimTop(PolarRegion.PolarHeight + ContinentalRegion.PolarGap)
                    .TrimBottom(PolarRegion.PolarHeight + ContinentalRegion.PolarGap)
                    .BisectVertically()
                    .Left
                    .TrimLeft(ContinentalRegion.ContinentalOceanGap / 2)
                    .TrimRight(ContinentalRegion.ContinentalOceanGap / 2);

                var eastContinentalBoundary = worldBoundary
                    .TrimTop(PolarRegion.PolarHeight + ContinentalRegion.PolarGap)
                    .TrimBottom(PolarRegion.PolarHeight + ContinentalRegion.PolarGap)
                    .BisectVertically()
                    .Right
                    .TrimLeft(ContinentalRegion.ContinentalOceanGap / 2)
                    .TrimRight(ContinentalRegion.ContinentalOceanGap / 2);

                var continents = new[]
                {
                    ContinentalRegion.Generate(westContinentalBoundary),
                    ContinentalRegion.Generate(eastContinentalBoundary)
                };

                return new World(continents, poles);
            }

            throw new NotImplementedException();
        }
    }

    public static partial class BoundaryExtensions
    {
        public static (Boundary Top, Boundary Bottom) BisectHorizontally(this Boundary boundary)
        {
            var halfHeight = (boundary.Height) / 2;

            return boundary.CutHorizontally(boundary.MinY + halfHeight);
        }

        public static (Boundary Left, Boundary Right) BisectVertically(this Boundary boundary)
        {
            var halfWidth = (boundary.Width) / 2;

            return boundary.CutVertically(boundary.MinX + halfWidth);
        }
    }
}
