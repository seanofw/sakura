namespace Sakura.Model
{
	[Flags]
	public enum NodeKind : uint
	{
		Cusp = (0 << 0),
		Smooth = (1 << 0),
		Symmetric = (2 << 0),

		StraightEdge = (0 << 4),
		CurveEdge = (1 << 4),

		NoEdge = (0 << 8),
		SolidEdge = (1 << 8),
		DashedEdge = (2 << 8),
	}
}
