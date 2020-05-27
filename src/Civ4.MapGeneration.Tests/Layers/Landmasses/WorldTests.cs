using Civ4.MapGeneration.Layers.Landmasses;
using Xunit;

namespace Civ4.MapGeneration.Tests.Layers.Landmasses
{
    public class WorldTests
    {
        [Fact]
        public void Continents_WhenTwoContinents_ReturnsWorld()
        {
            var dimensions = new Dimensions(60, 40);

            var world = World.Continents(dimensions, 2);
        }
    }
}
