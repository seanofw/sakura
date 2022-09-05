using System.Drawing;

namespace Sakura.BetterControls.Scrollbar
{
	public struct Mark : IEquatable<Mark>
	{
		public readonly Color Color;
		public readonly MarkStyle MarkStyle;
		public readonly int Thickness;

		public Mark(Color color, MarkStyle markStyle = MarkStyle.Cross, int thickness = 2)
		{
			Color = color;
			MarkStyle = markStyle;
			Thickness = thickness;
		}

		public Mark WithColor(Color color)
			=> new Mark(color: color, markStyle: MarkStyle, thickness: Thickness);

		public Mark WithStyle(MarkStyle markStyle)
			=> new Mark(color: Color, markStyle: markStyle, thickness: Thickness);

		public Mark WithThickness(int thickness)
			=> new Mark(color: Color, markStyle: MarkStyle, thickness: thickness);

		public override bool Equals(object? other)
			=> (other is Mark mark) && Equals(mark);

		public bool Equals(Mark other)
			=> Color == other.Color
				&& MarkStyle == other.MarkStyle
				&& Thickness == other.Thickness;

		public override int GetHashCode()
			=> (((Color.GetHashCode()) * 29
				+ MarkStyle.GetHashCode()) * 29
				+ Thickness.GetHashCode());

		public static bool operator ==(Mark a, Mark b)
			=> a.Equals(b);
		public static bool operator !=(Mark a, Mark b)
			=> !a.Equals(b);

		public override string ToString()
			=> $"(+{Thickness}): {Color}, {MarkStyle}";
	}
}