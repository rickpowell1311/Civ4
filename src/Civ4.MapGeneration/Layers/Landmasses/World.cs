using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class World
    {
        public IEnumerable<Tile> LandTiles => _landmasses.SelectMany(x => x.LandTiles).ToList();

        private readonly IEnumerable<Landmass> _landmasses;

        private World(IEnumerable<Landmass> landmasses)
        {
            _landmasses = landmasses;
        }

        public static World Continents(Dimensions dimensions, int number)
        {
            var worldBoundary = Boundary.FromDimensions(dimensions, 0, 0);
            var continentalBoundaries = new List<Boundary>();

            if (number == 2)
            {
                var splitContinentalBoundaries = worldBoundary
                    .BisectVertically()
                    .Select(x =>
                    {
                        return x
                            .AddHorizontalMargin(4) // TODO: Tidy this. This is to ensure ocean exists between continents
                            .AddVerticalMargin(3); // TODO: Tidy this. This is to leave this space for polar regions
                    }); 

                return new World(splitContinentalBoundaries.Select(x => Landmass.Build(x)));
            }

            throw new NotImplementedException();
        }
    }

    public static class BoundaryExtensions
    {
        public static IEnumerable<Boundary> BisectHorizontally(this Boundary boundary)
        {
            var halfHeight = (boundary.Height) / 2;
            var dimensions = new Dimensions(boundary.Width, halfHeight);

            var first = Boundary.FromDimensions(
                dimensions,
                boundary.MinX,
                boundary.MinY);

            var second = Boundary.FromDimensions(
                dimensions,
                boundary.MinX,
                boundary.MinY + halfHeight);

            return new[] { first, second };
        }

        public static IEnumerable<Boundary> BisectVertically(this Boundary boundary)
        {
            var halfWidth = (boundary.Width) / 2;
            var dimensions = new Dimensions(halfWidth, boundary.Height);

            var first = Boundary.FromDimensions(
                dimensions,
                boundary.MinX,
                boundary.MinY);

            var second = Boundary.FromDimensions(
                dimensions,
                boundary.MinX + halfWidth,
                boundary.MinY);

            return new[] { first, second };
        }

        public static Boundary AddMargin(this Boundary boundary, int size)
        {
            return boundary
                .AddVerticalMargin(size)
                .AddHorizontalMargin(size);
        }

        public static Boundary AddVerticalMargin(this Boundary boundary, int size)
        {
            var dimensions = new Dimensions(boundary.Width, boundary.Height - size * 2);

            return Boundary.FromDimensions(dimensions, boundary.MinX, boundary.MinY + size);
        }

        public static Boundary AddHorizontalMargin(this Boundary boundary, int size)
        {
            var dimensions = new Dimensions(boundary.Width - size * 2, boundary.Height);

            return Boundary.FromDimensions(dimensions, boundary.MinX + size, boundary.MinY);
        }
    }
}
