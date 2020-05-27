using Civ4.MapGeneration.Layers.Landmasses;
using Xunit;

namespace Civ4.MapGeneration.Tests.Layers.Landmasses
{
    public class LandmassTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(4, 4)]
        [InlineData(8, 8)]
        [InlineData(16, 16)]
        [InlineData(32, 32)]
        [InlineData(64, 64)]
        public void Generate_WithSpecifiedDimensions_ReturnsLandmass(int width, int height)
        {
            var boundary = Boundary.FromDimensions(new Dimensions(width, height), 0, 0);

            _ = Landmass.Generate(boundary);
        }
    }
}
