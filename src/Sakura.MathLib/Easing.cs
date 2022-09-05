
namespace Sakura.MathLib
{
	public static class Easing
	{
		private const float C1 = 1.70158f;
		private const float C2 = C1 * 1.525f;
		private const float C3 = C1 + 1;
		private const float C4 = 2.0f * 3.141592653f / 3.0f;
		private const float N1 = 7.5625f;
		private const float D1 = 2.75f;
		private const float P2 = 3.141592653f / 2.0f;

		public static float EaseLinear(float value)
			=> value;

		public static float EaseInQuad(float value)
			=> value * value;

		public static float EaseOutQuad(float value)
			=> 1 - (1 - value) * (1 - value);

		public static float EaseInOutQuad(float value)
			=> value < 0.5f ? 2 * value * value
				: 1 - MathF.Pow(-2 * value + 2, 2) * 0.5f;

		public static float EaseInCubic(float value)
			=> value * value * value;

		public static float EaseOutCubic(float value)
			=> 1.0f - MathF.Pow(1 - value, 3);

		public static float EaseInOutCubic(float value)
			=> value < 0.5f ? 4 * value * value * value
				: 1.0f - MathF.Pow(-2 * value + 2, 3) * 0.5f;

		public static float EaseInQuart(float value)
			=> value * value * value * value;

		public static float EaseOutQuart(float value)
			=> 1.0f - MathF.Pow(1 - value, 4);

		public static float EaseInOutQuart(float value)
			=> value < 0.5f ? 8 * value * value * value * value
				: 1.0f - MathF.Pow(-2 * value + 2, 4) * 0.5f;

		public static float EaseInQuint(float value)
			=> value * value * value * value * value;

		public static float EaseOutQuint(float value)
			=> 1.0f - MathF.Pow(1 - value, 5);

		public static float EaseInOutQuint(float value)
			=> value < 0.5f ? 16 * value * value * value * value * value
				: 1.0f - MathF.Pow(-2 * value + 2, 5) * 0.5f;

		public static float EaseInExpo(float value)
			=> value <= 0 ? 0 : MathF.Pow(2, 10 * value - 10);

		public static float EaseOutExpo(float value)
			=> value >= 1 ? 1 : 1 - MathF.Pow(2, -10 * value);

		public static float EaseInOutExpo(float value)
			=> value <= 0 ? 0
				: value >= 1 ? 1
				: value < 0.5f ? MathF.Pow(2, 20 * value - 10) * 0.5f
				: (2 - MathF.Pow(2, -20 * value + 10)) * 0.5f;

		public static float EaseInCirc(float value)
			=> 1 - MathF.Sqrt(1 - MathF.Pow(value, 2));

		public static float EaseOutCirc(float value)
			=> MathF.Sqrt(1 - MathF.Pow(value - 1, 2));

		public static float EaseInOutCirc(float value)
			=> value < 0.5f
				? (1 - MathF.Sqrt(1 - MathF.Pow(2 * value, 2))) * 0.5f
				: (MathF.Sqrt(1 - MathF.Pow(-2 * value + 2, 2)) + 1) * 0.5f;

		public static float EaseInSine(float value)
			=> 1 - MathF.Cos(value * P2);

		public static float EaseOutSine(float value)
			=> MathF.Sin(value * P2);

		public static float EaseInOutSine(float value)
			=> -0.5f * (MathF.Cos(MathF.PI * value) - 1);

		public static float EaseInElastic(float value)
			=> value == 0 ? 0.0f
				: value == 1 ? 1.0f
				: (-MathF.Pow(2, 10 * value - 10) * MathF.Sin((value * 10 - 10.75f) * C4));

		public static float EaseOutElastic(float value)
			=> value == 0 ? 0.0f
				: value == 1 ? 1.0f
				: (MathF.Pow(2, -10 * value) * MathF.Sin((value * 10 - 0.75f) * C4) + 1);

		public static float EaseInBack(float value)
			=> C3 * value * value * value - C1 * value * value;

		public static float EaseOutBack(float value)
			=> 1 + C3 * MathF.Pow(value - 1, 3) + C1 * MathF.Pow(value - 1, 2);

		public static float EaseInOutBack(float value)
			=> value < 0.5f
				? MathF.Pow(2 * value, 2) * ((C2 + 1) * 2 * value - C2) * 0.5f
				: (MathF.Pow(2 * value - 2, 2) * ((C2 + 1) * (value * 2 - 2) + C2) + 2) * 0.5f;

		public static float EaseInBounce(float value)
			=> 1.0f - EaseOutBounce(1.0f - value);

		public static float EaseOutBounce(float value)
			=> value < 1 / D1 ? N1 * value * value
				: value < 2 / D1 ? N1 * (value -= 1.5f / D1) * value + 0.75f
				: value < 2.5f / D1 ? N1 * (value -= 2.25f / D1) * value + 0.9375f
				: N1 * (value -= 2.625f / D1) * value + 0.984375f;

		public static float EaseInOutBounce(float value)
			=> value < 0.5f
				? (1 - EaseOutBounce(1 - 2 * value)) * 0.5f
				: (1 + EaseOutBounce(2 * value - 1)) * 0.5f;
	}
}
