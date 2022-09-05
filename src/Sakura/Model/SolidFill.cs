using Sakura.MathLib;

namespace Sakura.Model
{
	public class SolidFill : Fill
	{
		public Color Color { get; }

		public SolidFill(Color color)
		{
			Color = color;
		}
	}
}
