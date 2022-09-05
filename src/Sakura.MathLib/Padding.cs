
namespace Sakura.MathLib
{
	public struct Padding : IEquatable<Padding>
	{
		public int Left { get; }
		public int Right { get; }
		public int Top { get; }
		public int Bottom { get; }

		public int Horz => Left + Right;
		public int Vert => Top + Bottom;

		public Padding(int value)
			=> Left = Right = Top = Bottom = value;

		public Padding(int horz, int vert)
		{
			Left = Right = horz;
			Top = Bottom = vert;
		}

		public Padding(int left, int right, int top, int bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}

		public override bool Equals(object? obj)
			=> obj is Padding padding && Equals(padding);

		public bool Equals(Padding padding)
			=> Left == padding.Left
				&& Right == padding.Right
				&& Top == padding.Top
				&& Bottom == padding.Bottom;

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = 0;
				hashCode = hashCode * 65599 + Left;
				hashCode = hashCode * 65599 + Right;
				hashCode = hashCode * 65599 + Top;
				hashCode = hashCode * 65599 + Bottom;
				return hashCode;
			}
		}

		public static bool operator ==(Padding a, Padding b)
			=> a.Equals(b);

		public static bool operator !=(Padding a, Padding b)
			=> !a.Equals(b);

		public override string ToString()
			=> $"(left:{Left}, right:{Right}, top:{Top}, bottom:{Bottom})";
	}
}
