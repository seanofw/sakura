namespace Sakura.Model
{
	public class Shape : Item
	{
		public ImmutableList<Curve> Curves { get; }
		public Fill? Fill { get; }
		public Stroke? Stroke { get; }

		public Shape()
			: base(Bounds.None)
		{
			Curves = ImmutableList<Curve>.Empty;
			Fill = null;
			Stroke = null;
		}

		public Shape(ImmutableList<Curve> curves, Bounds? bounds = null,
			Fill? fill = null, Stroke? stroke = null, ItemMetadata? metadata = null, long id = -1)
			: base(bounds ?? CalcBounds(curves), metadata, id)
		{
			Curves = curves;
			Fill = fill;
			Stroke = stroke;
		}

		public Shape WithCurves(ImmutableList<Curve> curves, Bounds? bounds = null)
			=> new Shape(curves, bounds, Fill, Stroke, Metadata, Id);
		public Shape WithFill(Fill? fill)
			=> new Shape(Curves, Bounds, fill, Stroke, Metadata, Id);
		public Shape WithStroke(Stroke? stroke)
			=> new Shape(Curves, Bounds, Fill, stroke, Metadata, Id);

		public override Shape WithMetadata(ItemMetadata? metadata)
			=> new Shape(Curves, Bounds, Fill, Stroke, metadata, Id);
		public override Shape Clone()
			=> new Shape(Curves, Bounds, Fill, Stroke, Metadata, Id);
		public override Shape CloneWithNewId()
			=> new Shape(Curves, Bounds, Fill, Stroke, Metadata);

		private static Bounds CalcBounds(IEnumerable<Curve> curves)
		{
			Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

			foreach (Curve curve in curves)
			{
				min = Vector2.Min(min, curve.Bounds.Min);
				max = Vector2.Max(max, curve.Bounds.Max);
			}

			return new Bounds(min, max);
		}
	}
}
