
namespace Sakura.MathLib
{
	public struct Rect : IEquatable<Rect>
	{
		#region Public Fields

		public int X;

		public int Y;

		public int Width;

		public int Height;

		#endregion

		#region Public Properties

		public int Left
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				Width = Right - value;
				X = value;
			}
		}

		public int Right
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X + Width;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Width = value - X;
		}

		public int Top
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Y;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				Height = Bottom - value;
				Y = value;
			}
		}

		public int Bottom
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Y + Height;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Height = value - Y;
		}

		public Vector2i TopLeft
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new Vector2i(X, Y);
		}
		public Vector2i TopRight
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new Vector2i(X + Width, Y);
		}
		public Vector2i BottomLeft
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new Vector2i(X, Y + Height);
		}
		public Vector2i BottomRight
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new Vector2i(X + Width, Y + Height);
		} 

		public Vector2i Point
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new Vector2i(X, Y);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		public Vector2i Size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new Vector2i(Width, Height);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				Width = value.X;
				Height = value.Y;
			}
		}

		public Vector2i Center
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new Vector2i(X + Width / 2, Y + Height / 2);
		}

		#endregion

		#region Constructors

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect(Vector2i point, Vector2i size)
		{
			X = point.X;
			Y = point.Y;
			Width = size.X;
			Height = size.Y;
		}

		/// <summary>
		/// Create a new rectangle from edges, instead of X/Y/Width/Height.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rect FromEdges(int left, int top, int right, int bottom)
			=> new Rect(left, top, right - left, bottom - top);

		#endregion

		#region Operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Rect a, Rect b)
			=> a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Rect a, Rect b)
			=> !(a == b);

		#endregion

		#region Public Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(int x, int y)
			=> X <= x && x < X + Width && Y <= y && y < Y + Height;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(float x, float y)
			=> X <= x && x < X + Width && Y <= y && y < Y + Height;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Vector2i value)
			=> X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Vector2 value)
			=> X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Rect other)
			=> X <= other.X && other.X + other.Width <= X + Width
				&& Y <= other.Y && other.Y + other.Height <= Y + Height;

		public override bool Equals(object? obj)
			=> obj is Rect rect && this == rect;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Rect other)
			=> this == other;

		public override int GetHashCode()
			=> unchecked(((X * 65599 + Y) * 65599 + Width) * 65599 + Height);

		public Rect Grow(int horizontalAmount, int verticalAmount)
		{
			X -= horizontalAmount;
			Y -= verticalAmount;
			Width += horizontalAmount * 2;
			Height += verticalAmount * 2;
			return this;
		}

		public Rect Grow(float horizontalAmount, float verticalAmount)
		{
			X -= (int)horizontalAmount;
			Y -= (int)verticalAmount;
			Width += (int)horizontalAmount * 2;
			Height += (int)verticalAmount * 2;
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect Add(int dx, int dy, int dw, int dh)
			=> new Rect(X + dx, Y + dy, Width + dw, Height + dh);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect Sub(int dx, int dy, int dw, int dh)
			=> new Rect(X - dx, Y - dy, Width - dw, Height - dh);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rect operator *(Rect rect, int scalar)
			=> new Rect(rect.X * scalar, rect.Y * scalar, rect.Width * scalar, rect.Height * scalar);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rect operator /(Rect rect, int scalar)
			=> new Rect(rect.X / scalar, rect.Y / scalar, rect.Width / scalar, rect.Height / scalar);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Intersects(Rect value)
			=> value.Left < Right && Left < value.Right
				&& value.Top < Bottom && Top < value.Bottom;

		public Rect Clip(int maxWidth, int maxHeight)
		{
			int x = X, y = Y, width = Width, height = Height;

			if (width <= 0 || height <= 0 || x >= maxWidth || y >= maxHeight)
				return new Rect(0, 0, 0, 0);

			if (x < 0)
			{
				width += x;
				x = 0;
			}
			if (y < 0)
			{
				height += y;
				y = 0;
			}
			if (width > maxWidth - x)
				width = maxWidth - x;
			if (height > maxHeight - y)
				height = maxHeight - y;

			return new Rect(x, y, width, height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect Intersect(Rect other)
			=> TryIntersect(this, other, out Rect result) ? result : result;

		public static bool TryIntersect(Rect a, Rect b, out Rect result)
		{
			if (!a.Intersects(b))
			{
				result = new Rect(0, 0, 0, 0);
				return false;
			}

			int left = Math.Max(a.Left, b.Left);
			int top = Math.Max(a.Top, b.Top);
			int right = Math.Min(a.Right, b.Right);
			int bottom = Math.Min(a.Bottom, b.Bottom);

			result = FromEdges(left, top, right, bottom);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Offset(int offsetX, int offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Offset(float offsetX, float offsetY)
		{
			X += (int)offsetX;
			Y += (int)offsetY;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Offset(Vector2i amount)
		{
			X += amount.X;
			Y += amount.Y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Offset(Vector2 amount)
		{
			X += (int)amount.X;
			Y += (int)amount.Y;
		}

		public override string ToString() =>
			$"({X},{Y})+({Width},{Height})";

		public static Rect Union(Rect value1, Rect value2)
		{
			int x = Math.Min(value1.X, value2.X);
			int y = Math.Min(value1.Y, value2.Y);
			return new Rect(x, y,
				Math.Max(value1.Right, value2.Right) - x,
				Math.Max(value1.Bottom, value2.Bottom) - y);
		}

		public static void Union(ref Rect value1, ref Rect value2, out Rect result)
		{
			result.X = Math.Min(value1.X, value2.X);
			result.Y = Math.Min(value1.Y, value2.Y);
			result.Width = Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Deconstruct(out int x, out int y, out int width, out int height)
		{
			x = X;
			y = Y;
			width = Width;
			height = Height;
		}

		/// <summary>
		/// Transpose the rectangle by swapping its X/Y and Width/Height.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect Transposed()
			=> new Rect(Y, X, Height, Width);

		/// <summary>
		/// Move the given rectangle by the given delta, making a copy of it.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect Offseted(int dx, int dy)
			=> new Rect(X + dx, Y + dy, Width, Height);

		/// <summary>
		/// Move the given rectangle by the given delta, making a copy of it.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect Offseted(Vector2i delta)
			=> new Rect(X + delta.X, Y + delta.Y, Width, Height);

		/// <summary>
		/// Flip the rectangle horizontally over the given X coordinate.  The resulting
		/// rectangle will still be normalized in its new position.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect FlippedHorz(int axis = 0)
			=> FromEdges(axis + (axis - Right), Top, axis + (axis - Left), Bottom);

		/// <summary>
		/// Flip the rectangle vertically over the given Y coordinate.  The resulting
		/// rectangle will still be normalized in its new position.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect FlippedVert(int axis = 0)
			=> FromEdges(Left, axis + (axis - Bottom), Right, axis + (axis - Top));

		/// <summary>
		/// Scale the given rectangle by multiplying each of its components by the given amount.
		/// </summary>
		public Rect Scaled(double sx = 1, double sy = 1, double swidth = 1, double sheight = 1)
			=> new Rect((int)(X * sx + 0.5), (int)(Y * sy + 0.5),
				(int)(Width * swidth + 0.5), (int)(Height * sheight + 0.5));

		#endregion
	}
}
