using System.Threading;

namespace Sakura.Model
{
	public class Curve
	{
		public long Id { get; }
		public CurveKind Kind { get; }
		public Bounds Bounds { get; }
		public ImmutableArray<Node> Nodes { get; }
		public ImmutableArray<Edge> Edges { get; }

		private static long _idSource = 1;

		public Curve()
		{
			Id = 0;
			Bounds = Bounds.None;
			Nodes = ImmutableArray<Node>.Empty;
			Kind = default;
			Edges = ImmutableArray<Edge>.Empty;
		}

		public Curve(ImmutableArray<Node> nodes, Bounds? bounds = null, ImmutableArray<Edge>? edges = null, CurveKind kind = default, long id = -1)
		{
			Id = id >= 0 ? id : Interlocked.Increment(ref _idSource);

			Nodes = nodes;
			Bounds = bounds ?? CalcBounds(nodes);
			Kind = kind;

			if (edges.HasValue)
				Edges = edges.Value;
			else
			{
				int edgeCount = Nodes.Length >= 2
					? ((Kind & CurveKind.Closed) != 0 ? Nodes.Length : Nodes.Length - 1)
					: 0;
				Edge[] newEdges = new Edge[edgeCount];
				GenerateEdges(newEdges, 0);
				Edges = newEdges.ToImmutableArray();
			}
		}

		private void GenerateEdges(Span<Edge> edges, int index)
		{
			Node first = Nodes[0];
			Node prev = first;
			for (int i = 1; i < Nodes.Length - 1; i++)
			{
				Node next = Nodes[i];
				edges[index++] = new Edge(prev.Kind, prev, next);
				prev = next;
			}
			if ((Kind & CurveKind.Closed) != 0)
				edges[Nodes.Length - 1] = new Edge(prev.Kind, prev, first);
		}

		public Curve WithNodes(ImmutableArray<Node> nodes, Bounds? bounds = null, ImmutableArray<Edge>? edges = null)
			=> new Curve(nodes, bounds, edges, Kind, Id);
		public Curve WithKind(CurveKind kind)
			=> new Curve(Nodes, Bounds, Edges, kind, Id);

		public Curve AddNode(Node node)
			=> WithNodes(Nodes.Add(node));
		public Curve AddNodes(IEnumerable<Node> nodes)
			=> WithNodes(Nodes.AddRange(nodes));
		public Curve InsertNode(int index, Node node)
			=> WithNodes(Nodes.Insert(index, node));
		public Curve RemoveNode(int index)
			=> WithNodes(Nodes.RemoveAt(index));
		public Curve SetNode(int index, Node node)
			=> WithNodes(Nodes.SetItem(index, node),
				node.HasSamePoints(Nodes[index]) ? Bounds : null);

		public Curve Clone()
			=> new Curve(Nodes, Bounds, Edges, Kind, Id);
		public Curve CloneWithNewId()
			=> new Curve(Nodes, Bounds, Edges, Kind);

		public static Bounds CalcBounds(IEnumerable<Node> nodes)
		{
			Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

			foreach (Node node in nodes)
			{
				min = Vector2.Min(min, node.Point);
				min = Vector2.Min(min, node.AbsC1);
				min = Vector2.Min(min, node.AbsC2);

				max = Vector2.Max(max, node.Point);
				max = Vector2.Max(max, node.AbsC1);
				max = Vector2.Max(max, node.AbsC2);
			}

			return new Bounds(min, max);
		}
	}
}
