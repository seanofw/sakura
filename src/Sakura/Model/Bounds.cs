namespace Sakura.Model
{
	public struct Bounds : IEquatable<Bounds>
	{
		public readonly Vector2 Min;
		public readonly Vector2 Max;

		public static Bounds None => new Bounds(
			new Vector2(float.PositiveInfinity, float.PositiveInfinity),
			new Vector2(float.NegativeInfinity, float.NegativeInfinity));

		public static Bounds Infinity => new Bounds(
			new Vector2(float.NegativeInfinity, float.NegativeInfinity),
			new Vector2(float.PositiveInfinity, float.PositiveInfinity));

		public Bounds(Vector2 min, Vector2 max)
		{
			Min = min;
			Max = max;
		}

		public override bool Equals(object? obj)
			=> obj is Bounds other && Min == other.Min && Max == other.Max;

		public override int GetHashCode()
			=> (Max.GetHashCode() * 65599) + Min.GetHashCode();

		public bool Equals(Bounds other)
			=> Min == other.Min && Max == other.Max;

		public static bool operator ==(Bounds a, Bounds b)
			=> a.Min == b.Min && a.Max == b.Max;

		public static bool operator !=(Bounds a, Bounds b)
			=> a.Min == b.Min && a.Max == b.Max;
	}
}
