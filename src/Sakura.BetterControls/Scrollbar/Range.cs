
namespace Sakura.BetterControls.Scrollbar
{
	public struct Range : IEquatable<Range>
	{
		public readonly int Start;
		public readonly int Length;

		public Range(int start, int length)
		{
			Start = start;
			Length = length;
		}

		public bool Contains(int point)
			=> point >= Start && point < Start + Length;

		public Range WithStart(int start)
			=> new Range(start: start, length: Length);

		public Range WithLength(int length)
			=> new Range(start: Start, length: length);

		public override bool Equals(object? obj)
			=> (obj is Range range) && Equals(range);

		public bool Equals(Range other)
			=> Start == other.Start && Length == other.Length;

		public static bool operator ==(Range a, Range b)
			=> a.Equals(b);
		public static bool operator !=(Range a, Range b)
			=> !a.Equals(b);

		public override int GetHashCode()
			=> Start * 29 + Length;

		public override string ToString()
			=> $"{Start} {Length:+0;-#}";
	}
}
