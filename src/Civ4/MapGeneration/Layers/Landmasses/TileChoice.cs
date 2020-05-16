using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class TileChoice : IEquatable<TileChoice>
    {
        public Tile Tile { get; }

        public bool IsAdjacentToTileGroup { get; }

        public bool IsAlongBoundary { get; }

        public TileChoice(Tile tile, Boundary boundary, HashSet<Tile> existingTiles)
        {
            Tile = tile;
            IsAdjacentToTileGroup = existingTiles.Any(x => x.IsAdjecentTo(Tile));
            IsAlongBoundary = tile.IsAlongBoundary(boundary);
        }

        public double GetProbability(IEnumerable<TileChoice> otherTileChoices)
        {
            var thisProbabilityFactor = GetProbabilityWeighting();
            var totalChoiceWeighting = thisProbabilityFactor + otherTileChoices.Sum(x => x.GetProbabilityWeighting());

            return thisProbabilityFactor / totalChoiceWeighting;
        }

        private double GetProbabilityWeighting()
        {
            return IsAlongBoundary ? 1d
                : IsAdjacentToTileGroup ? 4d 
                : 2d;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TileChoice);
        }

        public bool Equals(TileChoice other)
        {
            return other != null &&
                EqualityComparer<Tile>.Default.Equals(Tile, other.Tile);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tile);
        }

        public static bool operator ==(TileChoice first, TileChoice second)
        {
            return first?.Tile == second?.Tile;
        }

        public static bool operator !=(TileChoice first, TileChoice second)
        {
            return !(first == second);
        }
    }

    public static class TileChoiceExtensions
    {
        private static readonly Random _picker;

        static TileChoiceExtensions()
        {
            _picker = new Random();
        }

        public static TileChoice ChooseOne(this IEnumerable<TileChoice> choices)
        {
            var assignedProbability = 0d;
            var pickBoundaries = new List<(double Start, double End, TileChoice TileChoice)>();

            foreach (var choice in choices)
            {
                var otherChoices = choices.Except(new[] { choice });

                var pickBoundaryStart = Math.Round(assignedProbability, 5);
                var pickBoundaryEnd = Math.Round(assignedProbability + choice.GetProbability(otherChoices), 5);

                pickBoundaries.Add((pickBoundaryStart, pickBoundaryEnd, choice));
                assignedProbability += (pickBoundaryEnd - pickBoundaryStart);
            }

            var picked = _picker.NextDouble();

            return pickBoundaries
                .Single(x => x.Start <= picked && picked < x.End)
                .TileChoice;
        }
    }
}
