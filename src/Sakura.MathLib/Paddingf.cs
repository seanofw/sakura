
namespace Sakura.MathLib
{
	public struct Paddingf : IEquatable<Paddingf>
	{
		public float Left { get; }
		public float Right { get; }
		public float Top { get; }
		public float Bottom { get; }

		public float Horz => Left + Right;
		public float Vert => Top + Bottom;

		public Paddingf(float value)
			=> Left = Right = Top = Bottom = value;

		public Paddingf(float horz, float vert)
		{
			Left = Right = horz;
			Top = Bottom = vert;
		}

		public Paddingf(float left, float right, float top, float bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}

		public override bool Equals(object? obj)
			=> obj is Paddingf padding && Equals(padding);

		public bool Equals(Paddingf padding)
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

		public static bool operator ==(Paddingf a, Paddingf b)
			=> a.Equals(b);

		public static bool operator !=(Paddingf a, Paddingf b)
			=> !a.Equals(b);

		public override string ToString()
			=> $"(left:{Left}, right:{Right}, top:{Top}, bottom:{Bottom})";
	}
}
