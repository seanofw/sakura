using System.Drawing;

namespace Sakura.BetterControls.Extensions
{
	public static class ColorExtensions
	{
		public static Color Mix(this Color a, Color b)
			=> Color.FromArgb(
				(a.R + b.R) >> 1,
				(a.G + b.G) >> 1,
				(a.B + b.B) >> 1,
				(a.A + b.A) >> 1
			);

		public static Color MixF(this Color a, Color b, double amount)
		{
			if (amount < 0.0) amount = 0.0;
			if (amount > 1.0) amount = 1.0;

			int m = (int)(amount * 65536.0 + 0.5);
			int im = 65536 - m;

			return Color.FromArgb(
				(a.R * m + b.R * im) >> 16,
				(a.G * m + b.G * im) >> 16,
				(a.B * m + b.B * im) >> 16,
				(a.A * m + b.A * im) >> 16
			);
		}

		public static Color Mix(this Color a, Color b, int amount)
		{
			if (amount < 0) amount = 0;
			if (amount > 256) amount = 256;

			int m = amount;
			int im = 256 - m;

			return Color.FromArgb(
				(a.R * m + b.R * im) >> 8,
				(a.G * m + b.G * im) >> 8,
				(a.B * m + b.B * im) >> 8,
				(a.A * m + b.A * im) >> 8
			);
		}
	}
}
