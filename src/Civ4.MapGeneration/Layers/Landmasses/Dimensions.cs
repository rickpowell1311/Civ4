using System;
using System.Collections;

namespace Civ4.MapGeneration.Layers.Landmasses
{
    public class Dimensions : IEquatable<Dimensions>
    {
        public int Width { get; }

        public int Height { get; }

        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Dimensions);
        }

        public bool Equals(Dimensions other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public static bool operator ==(Dimensions first, Dimensions second)
        {
            return first?.Width == second?.Width && first?.Height == second?.Height;
        }

        public static bool operator !=(Dimensions first, Dimensions second)
        {
            return !(first == second);
        }
    }
}
