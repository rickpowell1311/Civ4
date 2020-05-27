using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class TileChoices
    {
        private readonly IEnumerable<Tile> _tiles;
        private readonly Boundary _boundary;
        private readonly SeedGroup _seedGroup;

        public TileChoices(IEnumerable<Tile> tiles, Boundary boundary, SeedGroup seedGroup)
        {
            _tiles = tiles;
            _boundary = boundary;
            _seedGroup = seedGroup;
        }

        public Tile ChooseOne()
        {
            var choices = _tiles
                .Select(x => new TileChoice(
                    x,
                    x.IsAlongBoundary(_boundary),
                    _seedGroup.Tiles.Any(y => y.IsAdjecentTo(x))))
                .ToHashSet();

            var choicePicker = new ChoicePicker<Tile>();

            foreach (var choice in choices)
            {
                choicePicker.AddChoice(choice.Tile, choice.GetProbabilityWeighting());
            }

            return choicePicker.Choose();
        }

        private class ChoicePicker<T>
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
                var totalProbabilityWeighting = Math.Round(_choiceProbabilityWeightings.Values.Sum());

                var normalizedTileProbabilityWeightings = _choiceProbabilityWeightings
                    .ToDictionary(x => x.Key, x => x.Value / totalProbabilityWeighting);

                var val = _picker.NextDouble();
                var accum = 0d;
                foreach (var choiceProbabilityWeighting in normalizedTileProbabilityWeightings)
                {
                    var min = accum;
                    var max = accum + choiceProbabilityWeighting.Value;

                    if (val > min && val < max)
                    {
                        return choiceProbabilityWeighting.Key;
                    }

                    accum += choiceProbabilityWeighting.Value;
                }

                return default;
            }
        }
    }
}
