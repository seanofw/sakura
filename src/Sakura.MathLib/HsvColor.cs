
namespace Sakura.MathLib
{
	public struct HsvColor : IEquatable<HsvColor>
	{
		public ushort H;	// Hue angle, 0-359
		public byte S;		// Saturation, 0-255
		public byte V;		// Value, 0-255
		public byte A;      // Alpha, 0-255

		#region Properties that simulate floats

		/// <summary>
		/// The hue component, as a floating-point value, in the range of [0, 2*pi).
		/// </summary>
		public float Hf
		{
			get => H * (MathF.PI / 180);
			set => H = (ushort)Math.Min(Math.Max((int)(value * (180 / MathF.PI) + 0.5f), 0), 359);
		}

		/// <summary>
		/// The saturation component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Sf
		{
			get => S * (1f / 255);
			set => S = (byte)Math.Min(Math.Max((int)(value * 255 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The value component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Vf
		{
			get => V * (1f / 255);
			set => V = (byte)Math.Min(Math.Max((int)(value * 255 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The alpha component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Af
		{
			get => A * (1f / 255);
			set => A = (byte)Math.Min(Math.Max((int)(value * 255 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The color represented as a 4-component vector, where X=H, Y=S, Z=V, and W=A.
		/// </summary>
		public Vector4d Vector4d
		{
			get => new Vector4d(Hf, S * (1.0 / 255), V * (1.0 / 255), A * (1.0 / 255));
			set
			{
				Hf = (float)value.X;
				S = (byte)Math.Min(Math.Max((int)(value.Y * 255 + 0.5), 0), 255);
				V = (byte)Math.Min(Math.Max((int)(value.Z * 255 + 0.5), 0), 255);
				A = (byte)Math.Min(Math.Max((int)(value.W * 255 + 0.5), 0), 255);
			}
		}

		/// <summary>
		/// The color represented as a 4-component vector, where X=H, Y=S, Z=V, and W=A.
		/// </summary>
		public Vector4f Vector4f
		{
			get => new Vector4f(Hf, S * (1.0f / 255), V * (1.0f / 255), A * (1.0f / 255));
			set
			{
				Hf = value.X;
				S = (byte)Math.Min(Math.Max((int)(value.Y * 255 + 0.5f), 0), 255);
				V = (byte)Math.Min(Math.Max((int)(value.Z * 255 + 0.5f), 0), 255);
				A = (byte)Math.Min(Math.Max((int)(value.W * 255 + 0.5f), 0), 255);
			}
		}

		#endregion

		#region Construction

		/// <summary>
		/// Construct a color from four floating-point values, H, S, V, and A, in the range
		/// of [0.0f, 1.0f] each, except for H, which is in the range of [0, 2*pi).
		/// </summary>
		/// <remarks>
		/// Values outside the allowed ranges but still in the range of [-2^23, 2^23] will be clamped
		/// to the allowed ranges.  Values larger than 2^23 or smaller than -2^23 will produce garbage
		/// and have undefined results.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public HsvColor(float h, float s, float v, float a = 1.0f)
			: this((int)(h * (180 / MathF.PI) + 0.5f), (int)(s * 255 + 0.5f), (int)(v * 255 + 0.5f), (int)(a * 255 + 0.5f))
		{
		}

		/// <summary>
		/// Construct a color from a Vector4f representing X=H, Y=S, Z=V, and W=A, in the range
		/// of [0.0f, 1.0f] each, except for H, which is in the range of [0, 2*pi).
		/// </summary>
		/// <remarks>
		/// Values outside the allowed ranges but still in the range of [-2^23, 2^23] will be clamped
		/// to the allowed ranges.  Values larger than 2^23 or smaller than -2^23 will produce garbage
		/// and have undefined results.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public HsvColor(Vector4f v)
			: this((int)(v.X * (180 / MathF.PI) + 0.5f), (int)(v.Y * 255 + 0.5f), (int)(v.Z * 255 + 0.5f), (int)(v.W * 255 + 0.5f))
		{
		}

		/// <summary>
		/// Construct a color from a Vector4d representing X=H, Y=S, Z=V, and W=A, in the range
		/// of [0.0f, 1.0f] each, except for H, which is in the range of [0, 2*pi).
		/// </summary>
		/// <remarks>
		/// Values outside the allowed ranges but still in the range of [-2^23, 2^23] will be clamped
		/// to the allowed ranges.  Values larger than 2^23 or smaller than -2^23 will produce garbage
		/// and have undefined results.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public HsvColor(Vector4d v)
			: this((int)(v.X * (180 / Math.PI) + 0.5), (int)(v.Y * 255 + 0.5), (int)(v.Z * 255 + 0.5), (int)(v.W * 255 + 0.5))
		{
		}

		/// <summary>
		/// Construct a color from four integer values, H, S, V, and A, in the range of 0-255
		/// each, except for H, which is in the range of 0 to 359.  For S, V, and A, values
		/// outside 0-255 will be clamped to that range; H will be computed modulo 360.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public HsvColor(int h, int s, int v, int a = 255)
		{
			// Compute the modulus of h if it's out of range, using an unsigned test
			// to compare both ends.
			if ((uint)h >= 360)
			{
				h += 360;		// Quick win for negative numbers near 0.
				if ((uint)h >= 360)
				{
					h %= 360;	// General case.
					if (h < 0) h += 360;
				}
			}

			// We only need to clamp if there are illegal bits.
			if (((s | v | a) & 0xFFFFFF00) != 0)
			{
				H = (ushort)h;
				S = (byte)Math.Min(Math.Max(s, 0), 255);
				V = (byte)Math.Min(Math.Max(v, 0), 255);
				A = (byte)Math.Min(Math.Max(a, 0), 255);
			}
			else
			{
				H = (ushort)h;
				S = (byte)s;
				V = (byte)v;
				A = (byte)a;
			}
		}

		public HsvColor(Color c)
		{
			int max, delta, diff, offset;
			int r = c.R, g = c.G, b = c.B;

			int v;
			if (r > g) {
				if (r > b) { v = max = r; offset =     0; diff = g - b; }
				else       { v = max = b; offset = 2*120; diff = r - g; }
				delta = max - ((g < b) ? g : b);
			}
			else {
				if (g > b) { v = max = g; offset =   120; diff = b - r; }
				else       { v = max = b; offset = 2*120; diff = r - g; }
				delta = max - ((r < b) ? r : b);
			}

			V = (byte)v;

			if (max != 0)
				S = (byte)((delta * 255) / max);
			else
				S = 0;

			if (S == 0)
				H = 0;
			else
			{
				int h = (offset + diff * 120 / (delta << 1)) % 360;
				if (h < 0)
					h += 360;
				H = (ushort)h;
			}

			A = c.A;
		}

		public Color ToRgbColor()
		{
			if (S == 0)
				return new Color(V, V, V, A);

			int v = V;
			int hue = H * 6;
			int vs = (v * S) / 255;
			int vsf = (vs * (hue % 360)) / 360;

			int r, g, b;
			switch (hue / 360)
			{
				case 0: r = v; g = v - vs + vsf; b = v - vs; break;
				case 1: r = v - vsf; g = v; b = v - vs; break;
				case 2: r = v - vs; g = v; b = v - vs + vsf; break;
				case 3: r = v - vs; g = v - vsf; b = v; break;
				case 4: r = v - vs + vsf; g = v - vs; b = v; break;
				case 5: r = v; g = v - vs; b = v - vsf; break;
				default: r = g = b = 0; break;	// Should never get here.
			}

			return new Color((byte)r, (byte)g, (byte)b, A);
		}

		#endregion

		#region Conversions

		public static explicit operator Color(HsvColor c)
			=> c.ToRgbColor();
		public static explicit operator HsvColor(Color c)
			=> new HsvColor(c);

		#endregion

		#region Equality and hash codes

		/// <summary>
		/// Compare this color to another object on the heap for equality.
		/// </summary>
		public override bool Equals(object? obj)
			=> obj is HsvColor color && this == color;

		/// <summary>
		/// Compare this color to another for equality.
		/// </summary>
		public bool Equals(HsvColor other)
			=> this == other;

		/// <summary>
		/// Compare this color to another color for equality.
		/// </summary>
		public static bool operator ==(HsvColor a, HsvColor b)
			=> a.H == b.H && a.S == b.S && a.V == b.V && a.A == b.A;

		/// <summary>
		/// Compare this color to another color for inequality.
		/// </summary>
		public static bool operator !=(HsvColor a, HsvColor b)
			=> a.H != b.H || a.S != b.S || a.V != b.V || a.A != b.A;

		/// <summary>
		/// Calculate a hash code for this color so that it can be used in
		/// hash tables and dictionaries efficiently.
		/// </summary>
		public override int GetHashCode()
			=> unchecked(((A * 65599 + V) * 65599 + S) * 65599 + H);

		#endregion

		#region Deconstruction

		/// <summary>
		/// Deconstruct the individual color components (a method to support modern
		/// C#'s deconstruction syntax).
		/// </summary>
		/// <param name="h">The resulting hue.</param>
		/// <param name="s">The resulting saturation.</param>
		/// <param name="v">The resulting value.</param>
		public void Deconstruct(out int h, out int s, out int v)
		{
			h = H;
			s = S;
			v = V;
		}

		/// <summary>
		/// Deconstruct the individual color components (a method to support modern
		/// C#'s deconstruction syntax).
		/// </summary>
		/// <param name="h">The resulting hue.</param>
		/// <param name="s">The resulting saturation.</param>
		/// <param name="v">The resulting value.</param>
		/// <param name="a">The resulting alpha.</param>
		public void Deconstruct(out int h, out int s, out int v, out int a)
		{
			h = H;
			s = S;
			v = V;
			a = A;
		}

		#endregion

		#region Operators

		/// <summary>
		/// Calculate the "inverse" of the given color, treating each value as
		/// though it is in the range of [0, 1] and then subtracting it from 1.
		/// The alpha will be left unchanged.  Note that the result of this
		/// operation is usually only meaningful for non-premultiplied colors.
		/// </summary>
		/// <param name="c">The original color.</param>
		/// <returns>The "inverse" of that color.</returns>
		public static HsvColor operator -(HsvColor c)
			=> new HsvColor(c.H < 180 ? c.H + 180 : c.H - 180, (byte)(255 - c.S), (byte)(255 - c.V), c.A);

		#endregion

		#region Stringification

		/// <summary>
		/// Convert this to a string of the form 'hsv(H,S,V,A)', omitting the 'A' if it's 255.
		/// </summary>
		/// <returns>A string form of this color.</returns>
		public override string ToString()
			=> A < 255 ? $"hsv({H},{S},{V},{A})" : $"hsv({H},{S},{V})";

		#endregion
	}
}
