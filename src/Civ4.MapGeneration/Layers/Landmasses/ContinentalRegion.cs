using System;
using System.Collections.Generic;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class ContinentalRegion
    {
        public const int ContinentalOceanGap = 8;

        public IEnumerable<Tile> LandTiles { get; }

        private ContinentalRegion(IEnumerable<Tile> landTiles)
        {
            LandTiles = landTiles;
        }

        public static ContinentalRegion Generate(Boundary boundary)
        {
            return new ContinentalRegion(Landmass.Generate(boundary).LandTiles);
        }
    }
}
