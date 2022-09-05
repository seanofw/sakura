namespace Sakura.MathLib
{
	public enum Alignment
	{
		Default = 0,

		Left = 1 << 0,
		Right = 2 << 0,
		HorzCenter = 3 << 0,
		HorzMask = 3 << 0,

		Top = 1 << 2,
		Bottom = 2 << 2,
		VertCenter = 3 << 2,
		VertMask = 3 << 2,

		Center = HorzCenter | VertCenter,

		LeftCenter = Left | VertCenter,
		RightCenter = Right | VertCenter,
		TopCenter = Top | HorzCenter,
		BottomCenter = Bottom | HorzCenter,

		TopLeft = Top | Left,
		TopRight = Top | Right,
		BottomLeft = Bottom | Left,
		BottomRight = Bottom | Right,
	}
}
