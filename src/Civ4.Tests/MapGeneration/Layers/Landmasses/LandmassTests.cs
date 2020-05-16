using Civ4.MapGeneration.Layers.Landmasses;
using Xunit;

namespace Civ4.Tests.MapGeneration.Layers.Landmasses
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
        public void Build_WithSpecifiedDimensions_ReturnsLandmass(int width, int height)
        {
            _ = Landmass.Build(new Dimensions(width, height));
        }
    }
}
