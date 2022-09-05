
namespace Sakura.MathLib
{
	public struct Paddingd : IEquatable<Paddingd>
	{
		public double Left { get; }
		public double Right { get; }
		public double Top { get; }
		public double Bottom { get; }

		public double Horz => Left + Right;
		public double Vert => Top + Bottom;

		public Paddingd(double value)
			=> Left = Right = Top = Bottom = value;

		public Paddingd(double horz, double vert)
		{
			Left = Right = horz;
			Top = Bottom = vert;
		}

		public Paddingd(double left, double right, double top, double bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}

		public override bool Equals(object? obj)
			=> obj is Paddingd padding && Equals(padding);

		public bool Equals(Paddingd padding)
			=> Left == padding.Left
				&& Right == padding.Right
				&& Top == padding.Top
				&& Bottom == padding.Bottom;

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = 0;
				hashCode = hashCode * 65599 + Left.GetHashCode();
				hashCode = hashCode * 65599 + Right.GetHashCode();
				hashCode = hashCode * 65599 + Top.GetHashCode();
				hashCode = hashCode * 65599 + Bottom.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Paddingd a, Paddingd b)
			=> a.Equals(b);

		public static bool operator !=(Paddingd a, Paddingd b)
			=> !a.Equals(b);

		public override string ToString()
			=> $"(left:{Left}, right:{Right}, top:{Top}, bottom:{Bottom})";
	}
}
