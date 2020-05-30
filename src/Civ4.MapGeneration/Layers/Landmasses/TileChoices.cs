using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class TileChoices
    {
        private readonly Tile _currentTile;
        private readonly IEnumerable<Tile> _availableTiles;
        private readonly Boundary _boundary;
        private readonly SeedGroup _seedGroup;

        public TileChoices(Tile currentTile, IEnumerable<Tile> availableTiles, Boundary boundary, SeedGroup seedGroup)
        {
            _currentTile = currentTile;
            _availableTiles = availableTiles;
            _boundary = boundary;
            _seedGroup = seedGroup;
        }

        public Tile ChooseOne()
        {
            var choices = _availableTiles
                .Select(x =>
                {
                    var isAlongBoundary = x.IsAlongBoundary(_boundary);
                    var isAdjacentToTileGroup = _seedGroup.Tiles.Any(y => y.IsAdjecentTo(x));
                    var isTowardsBoundaryCenterPoint =
                        _boundary.Center.DistanceFrom(x.Location) < _boundary.Center.DistanceFrom(_currentTile.Location);

                    return new TileChoice(x, isAlongBoundary, isAdjacentToTileGroup, isTowardsBoundaryCenterPoint);
                })
                .ToHashSet();

            var choicePicker = new ChoicePicker<Tile>();

            foreach (var choice in choices)
            {
                choicePicker.AddChoice(choice.Tile, choice.GetProbabilityWeighting());
            }

            return choicePicker.Choose();
        }

        public class ChoicePicker<T>
            where T : IEquatable<T>
        {
            private readonly static Random _picker;

            static ChoicePicker()
            {
                _picker = new Random();
            }

            public Dictionary<T, double> _choiceProbabilityWeightings;

            public ChoicePicker()
            {
                _choiceProbabilityWeightings = new Dictionary<T, double>();
            }

            public void AddChoice(T choice, double probabilityWeighting)
            {
                _choiceProbabilityWeightings[choice] = probabilityWeighting;
            }

            public T Choose()
            {
                var totalProbabilityWeighting = _choiceProbabilityWeightings.Values.Sum();

                var normalizedTileProbabilityWeightings = _choiceProbabilityWeightings
                    .ToDictionary(x => x.Key, x => x.Value / totalProbabilityWeighting);

                var val = _picker.NextDouble();
                var lowerProbabilityBoundary = 0d;
                foreach (var choiceProbabilityWeighting in normalizedTileProbabilityWeightings)
                {
                    var upperProbabilityBoundary = lowerProbabilityBoundary + choiceProbabilityWeighting.Value;

                    if (val > lowerProbabilityBoundary && val < upperProbabilityBoundary)
                    {
                        return choiceProbabilityWeighting.Key;
                    }

                    lowerProbabilityBoundary += choiceProbabilityWeighting.Value;
                }

                return default;
            }
        }
    }
}
