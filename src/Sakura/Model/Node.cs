using System.Threading;

namespace Sakura.Model
{
	public class Node
	{
		public long Id { get; }

		public NodeKind Kind { get; }
		public Vector2 Point { get; }
		public Vector2 C1 { get; }			// Forward control point
		public Vector2 C2 { get; }			// Reverse control point

		public Vector2 AbsC1 => Point + C1;
		public Vector2 AbsC2 => Point + C2;

		private static long _idSource = 1;

		public Node(NodeKind kind, Vector2 point, Vector2 c1 = default, Vector2 c2 = default, long id = -1)
		{
			Id = id >= 0 ? id : Interlocked.Increment(ref _idSource);
			Kind = kind;
			Point = point;
			C1 = c1;
			C2 = c2;
		}

		public Node WithKind(NodeKind kind)
			=> new Node(kind, Point, C1, C2, Id);
		public Node WithPoint(Vector2 point)
			=> new Node(Kind, point, C1, C2, Id);
		public Node WithC1(Vector2 c1)
			=> new Node(Kind, Point, c1, C2, Id);
		public Node WithC2(Vector2 c2)
			=> new Node(Kind, Point, C1, c2, Id);

		public Node Clone()
			=> new Node(Kind, Point, C1, C2, Id);
		public Node CloneWithNewId()
			=> new Node(Kind, Point, C1, C2);

		public bool HasSamePoints(Node other)
			=> Point == other.Point && C1 == other.C1 && C2 == other.C2;
	}
}
