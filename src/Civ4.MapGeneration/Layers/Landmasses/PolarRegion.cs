using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class PolarRegion
    {
        public const int PolarHeight = 3;

        public static int GeneratedRows = 1;

        public static int FilledRows => PolarHeight - GeneratedRows;

        public IEnumerable<Tile> IceTiles { get; }

        private PolarRegion(IEnumerable<Tile> iceTiles)
        {
            IceTiles = iceTiles;
        }

        public static PolarRegion GenerateNorthPole(Boundary boundary)
        {
            var (top, bottom) = boundary.CutHorizontally(boundary.MinY + boundary.Height - FilledRows);

            var iceTiles = Landmass.Fill(top).LandTiles
                .Union(Landmass.Generate(bottom, bottom.Area / 4).LandTiles);

            return new PolarRegion(iceTiles);
        }

        public static PolarRegion GenerateSouthPole(Boundary boundary)
        {
            var (top, bottom) = boundary.CutHorizontally(boundary.MinY + FilledRows);

            var iceTiles = Landmass.Fill(bottom).LandTiles
                .Union(Landmass.Generate(top, top.Area / 4).LandTiles);

            return new PolarRegion(iceTiles);
        }
    }
}
