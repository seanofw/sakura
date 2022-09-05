using Sakura.MathLib;

namespace Sakura.Model
{
	public class Stroke
	{
		public Color Color { get; }
		public uint DashPattern { get; }
		public float Width { get; }

		public static Stroke BlackPen { get; } = new Stroke(Color.Black);
		public static Stroke WhitePen { get; } = new Stroke(Color.White);

		public Stroke(Color color, float width = 1, uint dashPattern = 0xFFFFFFFFU)
		{
			Color = color;
			Width = width;
			DashPattern = dashPattern;
		}
	}
}
