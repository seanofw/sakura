
namespace Sakura.Model
{
	public struct Edge
	{
		public readonly NodeKind Kind;
		public readonly Node Start;
		public readonly Node End;

		public Vector2 P1 => Start.Point;
		public Vector2 P2 => Start.AbsC1;
		public Vector2 P3 => End.AbsC2;
		public Vector2 P4 => End.Point;

		public Edge(NodeKind kind, Node start, Node end)
		{
			Kind = kind;
			Start = start;
			End = end;
		}
	}
}
