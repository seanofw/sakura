
namespace Sakura.MathLib
{
	/// <summary>
	/// An RGBA color, in 32-bit truecolor, with lots of operations to
	/// manipulate it.
	/// </summary>
	public struct Color : IEquatable<Color>
	{
		#region Core storage: Exactly four bytes in sequence.

		/// <summary>
		/// The red component of the color.
		/// </summary>
		public byte R;

		/// <summary>
		/// The green component of the color.
		/// </summary>
		public byte G;

		/// <summary>
		/// The blue component of the color.
		/// </summary>
		public byte B;

		/// <summary>
		/// The alpha component of the color.
		/// </summary>
		public byte A;

		#endregion

		#region Properties that simulate floats

		/// <summary>
		/// The red component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Rf
		{
			get => R * (1f / 255);
			set => R = (byte)Math.Min(Math.Max((int)(value * 255 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The green component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Gf
		{
			get => G * (1f / 255);
			set => G = (byte)Math.Min(Math.Max((int)(value * 255 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The blue component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Bf
		{
			get => B * (1f / 255);
			set => B = (byte)Math.Min(Math.Max((int)(value * 255 + 0.5f), 0), 255);
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
		/// The color represented as a 4-component vector, where X=R, Y=G, Z=B, and W=A.
		/// </summary>
		public Vector4d Vector4d
		{
			get => new Vector4d(R * (1.0 / 255), G * (1.0 / 255), B * (1.0 / 255), A * (1.0 / 255));
			set
			{
				R = (byte)Math.Min(Math.Max((int)(value.X * 255 + 0.5), 0), 255);
				G = (byte)Math.Min(Math.Max((int)(value.Y * 255 + 0.5), 0), 255);
				B = (byte)Math.Min(Math.Max((int)(value.Z * 255 + 0.5), 0), 255);
				A = (byte)Math.Min(Math.Max((int)(value.W * 255 + 0.5), 0), 255);
			}
		}

		/// <summary>
		/// The color represented as a 4-component vector, where X=R, Y=G, Z=B, and W=A.
		/// </summary>
		public Vector4f Vector4f
		{
			get => new Vector4f(R * (1.0f / 255), G * (1.0f / 255), B * (1.0f / 255), A * (1.0f / 255));
			set
			{
				R = (byte)Math.Min(Math.Max((int)(value.X * 255 + 0.5f), 0), 255);
				G = (byte)Math.Min(Math.Max((int)(value.Y * 255 + 0.5f), 0), 255);
				B = (byte)Math.Min(Math.Max((int)(value.Z * 255 + 0.5f), 0), 255);
				A = (byte)Math.Min(Math.Max((int)(value.W * 255 + 0.5f), 0), 255);
			}
		}

		#endregion

		#region Construction

		/// <summary>
		/// Construct a color from four floating-point values, R, G, B, and A, in the range
		/// of [0.0f, 1.0f] each.
		/// </summary>
		/// <remarks>
		/// Values outside [0.0f, 1.0f] but still in the range of [-2^23, 2^23] will be clamped
		/// to [0.0f, 1.0f].  Values larger than 2^23 or smaller than -2^23 will produce garbage
		/// and have undefined results.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Color(float r, float g, float b, float a = 1.0f)
			: this((int)(r * 255 + 0.5f), (int)(g * 255 + 0.5f), (int)(b * 255 + 0.5f), (int)(a * 255 + 0.5f))
		{
		}

		/// <summary>
		/// Construct a color from a Vector4f representing X=R, Y=G, Z=B, and W=A, in the range
		/// of [0.0f, 1.0f] each.
		/// </summary>
		/// <remarks>
		/// Values outside [0.0f, 1.0f] but still in the range of [-2^23, 2^23] will be clamped
		/// to [0.0f, 1.0f].  Values larger than 2^23 or smaller than -2^23 will produce garbage
		/// and have undefined results.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Color(Vector4f v)
			: this((int)(v.X * 255 + 0.5f), (int)(v.Y * 255 + 0.5f), (int)(v.Z * 255 + 0.5f), (int)(v.W * 255 + 0.5f))
		{
		}

		/// <summary>
		/// Construct a color from a Vector4d representing X=R, Y=G, Z=B, and W=A, in the range
		/// of [0.0f, 1.0f] each.
		/// </summary>
		/// <remarks>
		/// Values outside [0.0f, 1.0f] but still in the range of [-2^23, 2^23] will be clamped
		/// to [0.0f, 1.0f].  Values larger than 2^23 or smaller than -2^23 will produce garbage
		/// and have undefined results.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Color(Vector4d v)
			: this((int)(v.X * 255 + 0.5), (int)(v.Y * 255 + 0.5), (int)(v.Z * 255 + 0.5), (int)(v.W * 255 + 0.5))
		{
		}

		/// <summary>
		/// Construct a color from four integer values, R, G, B, and A, in the range of 0-255
		/// each.  Values outside 0-255 will be clamped to that range.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Color(int r, int g, int b, int a = 255)
		{
			if (((r | g | b | a) & 0xFFFFFF00) == 0)
			{
				R = (byte)r;
				G = (byte)g;
				B = (byte)b;
				A = (byte)a;
			}
			else
			{
				R = (byte)Math.Min(Math.Max(r, 0), 255);
				G = (byte)Math.Min(Math.Max(g, 0), 255);
				B = (byte)Math.Min(Math.Max(b, 0), 255);
				A = (byte)Math.Min(Math.Max(a, 0), 255);
			}
		}

		/// <summary>
		/// Construct a color from four byte values, R, G, B, and A, in the range of 0-255
		/// each.  This overload sets the bytes directly, so it may be faster than the other
		/// constructors.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Color(byte r, byte g, byte b, byte a = 255)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		#endregion

		#region Equality and hash codes

		/// <summary>
		/// Compare this color to another object on the heap for equality.
		/// </summary>
		public override bool Equals(object? obj)
			=> obj is Color color && this == color;

		/// <summary>
		/// Compare this color to another for equality.
		/// </summary>
		public bool Equals(Color other)
			=> this == other;

		/// <summary>
		/// Compare this color to another color for equality.
		/// </summary>
		public static bool operator ==(Color a, Color b)
			=> a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;

		/// <summary>
		/// Compare this color to another color for inequality.
		/// </summary>
		public static bool operator !=(Color a, Color b)
			=> a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;

		/// <summary>
		/// Calculate a hash code for this color so that it can be used in
		/// hash tables and dictionaries efficiently.
		/// </summary>
		public override int GetHashCode()
			=> unchecked(((A * 65599 + B) * 65599 + G) * 65599 + R);

		#endregion

		#region Stringification

		/// <summary>
		/// Get a string form of this color, as a 6-digit hex code, preceded by a
		/// sharp sign, like in CSS:  #RRGGBB
		/// </summary>
		public string Hex6
		{
			get
			{
				Span<char> buffer = stackalloc char[7];
				buffer[0] = '#';
				WriteByte(buffer, 1, R);
				WriteByte(buffer, 3, G);
				WriteByte(buffer, 5, B);
				return new string(buffer);
			}
		}

		/// <summary>
		/// Get a string form of this color, as an 8-digit hex code, preceded by a
		/// sharp sign, like extended CSS:  #RRGGBBAA
		/// </summary>
		public string Hex8
		{
			get
			{
				Span<char> buffer = stackalloc char[9];
				buffer[0] = '#';
				WriteByte(buffer, 1, R);
				WriteByte(buffer, 3, G);
				WriteByte(buffer, 5, B);
				WriteByte(buffer, 7, A);
				return new string(buffer);
			}
		}

		/// <summary>
		/// Write a single hex nybble out to the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer to write to.</param>
		/// <param name="position">The position in that buffer to write.</param>
		/// <param name="value">The nybble to write, which must be in the range of [0, 15].</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void WriteNybble(Span<char> buffer, int position, byte value)
			=> buffer[position] = value < 10
				? (char)(value + '0')
				: (char)(value - 10 + 'A');

		/// <summary>
		/// Write a single hex byte out to the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer to write to.</param>
		/// <param name="position">The position in that buffer to write.</param>
		/// <param name="value">The byte to write.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void WriteByte(Span<char> buffer, int position, byte value)
		{
			WriteNybble(buffer, position, (byte)(value >> 4));
			WriteNybble(buffer, position + 1, (byte)(value & 0xF));
		}

		/// <summary>
		/// Convert this color to its most natural string representation.
		/// </summary>
		/// <returns>A "natural" string representation, which will be the color's name if
		/// it has one, or an #RRGGBB(AA) form if it doesn't have a name.</returns>
		public override string ToString()
			=> NamesByColor.TryGetValue(this, out string? name) ? name : ToHexString();

		/// <summary>
		/// Convert this to a string of the form rgb(R,G,B,A), omitting the 'A' if it's 255, and
		/// converting 'A' to a value in the range of [0, 1].
		/// </summary>
		/// <returns>A string form of this color, as a CSS rgb() three-valued or four-valued vector.</returns>
		public string ToRgbString()
			=> A < 255 ? $"rgba({R},{G},{B},{A * (1.0 / 255)})" : $"rgb({R},{G},{B})";

		/// <summary>
		/// Convert this to a string of the form (R,G,B,A), omitting the 'A' if it's 255.
		/// </summary>
		/// <returns>A string form of this color, as a three-valued or four-valued vector.</returns>
		public string ToVectorString()
			=> A < 255 ? $"({R},{G},{B},{A})" : $"({R},{G},{B})";

		/// <summary>
		/// Convert this to a string of the form #RRGGBBAA, omitting the AA if 'A' is 255.
		/// </summary>
		/// <returns>This color converted to a hex string.</returns>
		public string ToHexString()
			=> A < 255 ? Hex8 : Hex6;

		#endregion

		#region Color mixing

		/// <summary>
		/// Combine two colors, with equal amounts of each.
		/// </summary>
		/// <param name="other">The other color to merge with this color.</param>
		/// <returns>The new, fused color.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Color Merge(Color other)
			=> new Color(
				(byte)((R + other.R) >> 1),
				(byte)((G + other.G) >> 1),
				(byte)((B + other.B) >> 1),
				(byte)((A + other.A) >> 1)
			);

		/// <summary>
		/// Perform linear interpolation between this and another color.  'amount'
		/// describes how much of the other color is included in the result, on a
		/// scale of [0.0f, 1.0f].
		/// </summary>
		/// <param name="other">The other color to mix with this color.</param>
		/// <param name="amount">How much of the other color to mix with this color,
		/// on a range of 0 (entirely this color) to 1 (entirely the other color).
		/// Exactly 0.5 is equivalent to calling the Merge() method instead.</param>
		/// <remarks>Merge() runs faster if you need exactly 50% of each color.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public Color Mix(Color other, float amount)
		{
			int am = (int)(Math.Min(Math.Max(amount, 0), 1) * 65536);
			int ia = 65536 - am;

			byte r = (byte)((R * ia + other.R * am + 32768) >> 16);
			byte g = (byte)((G * ia + other.G * am + 32768) >> 16);
			byte b = (byte)((B * ia + other.B * am + 32768) >> 16);
			byte a = (byte)((A * ia + other.A * am + 32768) >> 16);

			return new Color(r, g, b, a);
		}

		/// <summary>
		/// Transform a color that uses non-premultipled alpha to one that contains premultiplied alpha.
		/// Note that this is a lossy transformation:  Unpremultiply(Premultiply(x)) will result in a
		/// *similar* color, but not the *same* color as the original.
		/// </summary>
		/// <param name="r">The red value, which should be from 0 to 255.</param>
		/// <param name="g">The green value, which should be from 0 to 255.</param>
		/// <param name="b">The blue value, which should be from 0 to 255.</param>
		/// <param name="a">The alpha value, which should be from 0 to 255.</param>
		public static Color Premultiply(int r, int g, int b, int a)
			=> a >= 255 ? new Color(r, g, b, 255)
				: new Color(Div255(r * a), Div255(g * a), Div255(b * a), a);

		/// <summary>
		/// Transform a color that uses premultiplied color components into one that contains
		/// non-premultiplied color components.  Note that premultiplication is a lossy transformation:
		/// Unpremultiply(Premultiply(x)) will result in a *similar* color, but not the *same* color
		/// as the original.
		/// </summary>
		/// <param name="r">The red value, which should be from 0 to 255.</param>
		/// <param name="g">The green value, which should be from 0 to 255.</param>
		/// <param name="b">The blue value, which should be from 0 to 255.</param>
		/// <param name="a">The alpha value, which should be from 0 to 255.</param>
		/// <returns>The un-premultiplied color.</returns>
		public static Color Unpremultiply(int r, int g, int b, int a)
		{
			if (a == 255) return new Color(r, g, b);
			if (a == 0) return new Color((byte)0, (byte)0, (byte)0, (byte)0);

			long ooa = (255 * 0x100000000L) / a;
			return new Color((int)((r * ooa) >> 32), (int)((g * ooa) >> 32), (int)((b * ooa) >> 32), a);
		}

		/// <summary>
		/// Transform a color that uses non-premultipled alpha to one that contains premultiplied alpha.
		/// Note that this is a lossy transformation:  Unpremultiply(Premultiply(x)) will result in a
		/// *similar* color, but not the *same* color as the original.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Color Premultiply()
			=> Premultiply(R, G, B, A);

		/// <summary>
		/// Transform a color that uses premultiplied color components into one that contains
		/// non-premultiplied color components.  Note that premultiplication is a lossy transformation:
		/// Unpremultiply(Premultiply(x)) will result in a *similar* color, but not the *same* color
		/// as the original.
		/// </summary>
		/// <returns>The un-premultiplied color.</returns>
		public Color Unpremultiply()
			=> Unpremultiply(R, G, B, A);

		/// <summary>
		/// Divide the given value by 255, faster than '/' can divide on most CPUs.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static int Div255(int x)
			=> (x + 1 + (x >> 8)) >> 8;

		#endregion

		#region Deconstruction and tuple-conversion

		/// <summary>
		/// Convert this color to a tuple of floating-point values.
		/// </summary>
		/// <returns>A four-valued tuple of floating-point values in the range of [0, 1], in the
		/// form (r, g, b, a).</returns>
		public (float r, float g, float b, float a) ToFloats()
			=> (R * (1 / 255f), G * (1 / 255f), B * (1 / 255f), A * (1 / 255f));

		/// <summary>
		/// Convert this to a tuple of integer values.
		/// </summary>
		/// <returns>A four-valued tuple of integer values in the range of [0, 255], in the
		/// form (r, g, b, a).</returns>
		public (int r, int g, int b, int a) ToInts()
			=> (R, G, B, A);

		/// <summary>
		/// Deconstruct the individual color components (a method to support modern
		/// C#'s deconstruction syntax).
		/// </summary>
		/// <param name="r">The resulting red value.</param>
		/// <param name="g">The resulting green value.</param>
		/// <param name="b">The resulting blue value.</param>
		public void Deconstruct(out byte r, out byte g, out byte b)
		{
			r = R;
			g = G;
			b = B;
		}

		/// <summary>
		/// Deconstruct the individual color components (a method to support modern
		/// C#'s deconstruction syntax).
		/// </summary>
		/// <param name="r">The resulting red value.</param>
		/// <param name="g">The resulting green value.</param>
		/// <param name="b">The resulting blue value.</param>
		/// <param name="a">The resulting alpha value.</param>
		public void Deconstruct(out byte r, out byte g, out byte b, out byte a)
		{
			r = R;
			g = G;
			b = B;
			a = A;
		}

		#endregion

		#region Color "arithmetic" operators

		/// <summary>
		/// Perform componentwise addition on the R, G, and B values of the given colors.
		/// The alpha of the result will be assigned to the larger alpha of either color.
		/// Note that the result of this operation is usually only meaningful for
		/// non-premultiplied colors.
		/// </summary>
		/// <param name="x">The first color to add.</param>
		/// <param name="y">The second color to add.</param>
		/// <returns>The "sum" of those colors.</returns>
		public static Color operator +(Color x, Color y)
			=> new Color(x.R + y.R, x.G + y.G, x.B + y.B, Math.Max(x.A, y.A));

		/// <summary>
		/// Perform componentwise subtraction on the R, G, and B values of the given colors.
		/// The alpha of the result will be assigned to the larger alpha of either color.
		/// Note that the result of this operation is usually only meaningful for
		/// non-premultiplied colors.
		/// </summary>
		/// <param name="x">The source color.</param>
		/// <param name="y">The color to subtract from the source.</param>
		/// <returns>The "difference" of those colors.</returns>
		public static Color operator -(Color x, Color y)
			=> new Color(x.R - y.R, x.G - y.G, x.B - y.B, Math.Max(x.A, y.A));

		/// <summary>
		/// Calculate the "inverse" of the given color, treating each value as
		/// though it is in the range of [0, 1] and then subtracting it from 1.
		/// The alpha will be left unchanged.  Note that the result of this
		/// operation is usually only meaningful for non-premultiplied colors.
		/// </summary>
		/// <param name="c">The original color.</param>
		/// <returns>The "inverse" of that color.</returns>
		public static Color operator -(Color c)
			=> new Color((byte)(255 - c.R), (byte)(255 - c.G), (byte)(255 - c.B), c.A);

		/// <summary>
		/// Perform componentwise multiplication on the R, G, and B values of the given
		/// colors, treating each as though it is a fractional value in the range of [0, 1].
		/// The alpha of the result will be assigned to the larger alpha of either color.
		/// Note that the result of this operation is usually only meaningful for
		/// non-premultiplied colors.
		/// </summary>
		/// <param name="x">The first color to multiply.</param>
		/// <param name="y">The second color to multiply.</param>
		/// <returns>The "product" of those colors.</returns>
		public static Color operator *(Color x, Color y)
			=> new Color(Div255(x.R * y.R), Div255(x.G * y.G), Div255(x.B * y.B), Math.Max(x.A, y.A));

		#endregion

		#region Static colors (CSS colors)

		public static readonly Color Transparent = new Color((byte)0, (byte)0, (byte)0, (byte)0);

		public static readonly Color AntiqueWhite = new Color((byte)250, (byte)235, (byte)215, (byte)255);
		public static readonly Color Aqua = new Color((byte)0, (byte)255, (byte)255, (byte)255);
		public static readonly Color Aquamarine = new Color((byte)127, (byte)255, (byte)212, (byte)255);
		public static readonly Color Azure = new Color((byte)240, (byte)255, (byte)255, (byte)255);
		public static readonly Color Beige = new Color((byte)245, (byte)245, (byte)220, (byte)255);
		public static readonly Color Bisque = new Color((byte)255, (byte)228, (byte)196, (byte)255);
		public static readonly Color Black = new Color((byte)0, (byte)0, (byte)0, (byte)255);
		public static readonly Color BlanchedAlmond = new Color((byte)255, (byte)235, (byte)205, (byte)255);
		public static readonly Color Blue = new Color((byte)0, (byte)0, (byte)255, (byte)255);
		public static readonly Color BlueViolet = new Color((byte)138, (byte)43, (byte)226, (byte)255);
		public static readonly Color Brown = new Color((byte)165, (byte)42, (byte)42, (byte)255);
		public static readonly Color Burlywood = new Color((byte)222, (byte)184, (byte)135, (byte)255);
		public static readonly Color CadetBlue = new Color((byte)95, (byte)158, (byte)160, (byte)255);
		public static readonly Color Chartreuse = new Color((byte)127, (byte)255, (byte)0, (byte)255);
		public static readonly Color Chocolate = new Color((byte)210, (byte)105, (byte)30, (byte)255);
		public static readonly Color Coral = new Color((byte)255, (byte)127, (byte)80, (byte)255);
		public static readonly Color CornflowerBlue = new Color((byte)100, (byte)149, (byte)237, (byte)255);
		public static readonly Color Cornsilk = new Color((byte)255, (byte)248, (byte)220, (byte)255);
		public static readonly Color Crimson = new Color((byte)220, (byte)20, (byte)60, (byte)255);
		public static readonly Color Cyan = new Color((byte)0, (byte)255, (byte)255, (byte)255);
		public static readonly Color DarkBlue = new Color((byte)0, (byte)0, (byte)139, (byte)255);
		public static readonly Color DarkCyan = new Color((byte)0, (byte)139, (byte)139, (byte)255);
		public static readonly Color DarkGoldenrod = new Color((byte)184, (byte)134, (byte)11, (byte)255);
		public static readonly Color DarkGray = new Color((byte)169, (byte)169, (byte)169, (byte)255);
		public static readonly Color DarkGreen = new Color((byte)0, (byte)100, (byte)0, (byte)255);
		public static readonly Color DarkGrey = new Color((byte)169, (byte)169, (byte)169, (byte)255);
		public static readonly Color DarkKhaki = new Color((byte)189, (byte)183, (byte)107, (byte)255);
		public static readonly Color DarkMagenta = new Color((byte)139, (byte)0, (byte)139, (byte)255);
		public static readonly Color DarkOliveGreen = new Color((byte)85, (byte)107, (byte)47, (byte)255);
		public static readonly Color DarkOrange = new Color((byte)255, (byte)140, (byte)0, (byte)255);
		public static readonly Color DarkOrchid = new Color((byte)153, (byte)50, (byte)204, (byte)255);
		public static readonly Color DarkRed = new Color((byte)139, (byte)0, (byte)0, (byte)255);
		public static readonly Color DarkSalmon = new Color((byte)233, (byte)150, (byte)122, (byte)255);
		public static readonly Color DarkSeaGreen = new Color((byte)143, (byte)188, (byte)143, (byte)255);
		public static readonly Color DarkSlateBlue = new Color((byte)72, (byte)61, (byte)139, (byte)255);
		public static readonly Color DarkSlateGray = new Color((byte)47, (byte)79, (byte)79, (byte)255);
		public static readonly Color DarkSlateGrey = new Color((byte)47, (byte)79, (byte)79, (byte)255);
		public static readonly Color DarkTurquoise = new Color((byte)0, (byte)206, (byte)209, (byte)255);
		public static readonly Color DarkViolet = new Color((byte)148, (byte)0, (byte)211, (byte)255);
		public static readonly Color DeepPink = new Color((byte)255, (byte)20, (byte)147, (byte)255);
		public static readonly Color DeepSkyBlue = new Color((byte)0, (byte)191, (byte)255, (byte)255);
		public static readonly Color DimGray = new Color((byte)105, (byte)105, (byte)105, (byte)255);
		public static readonly Color DimGrey = new Color((byte)105, (byte)105, (byte)105, (byte)255);
		public static readonly Color DodgerBlue = new Color((byte)30, (byte)144, (byte)255, (byte)255);
		public static readonly Color FireBrick = new Color((byte)178, (byte)34, (byte)34, (byte)255);
		public static readonly Color FloralWhite = new Color((byte)255, (byte)250, (byte)240, (byte)255);
		public static readonly Color ForestGreen = new Color((byte)34, (byte)139, (byte)34, (byte)255);
		public static readonly Color Fuchsia = new Color((byte)255, (byte)0, (byte)255, (byte)255);
		public static readonly Color Gainsboro = new Color((byte)220, (byte)220, (byte)220, (byte)255);
		public static readonly Color GhostWhite = new Color((byte)248, (byte)248, (byte)255, (byte)255);
		public static readonly Color Gold = new Color((byte)255, (byte)215, (byte)0, (byte)255);
		public static readonly Color Goldenrod = new Color((byte)218, (byte)165, (byte)32, (byte)255);
		public static readonly Color Gray = new Color((byte)128, (byte)128, (byte)128, (byte)255);
		public static readonly Color Green = new Color((byte)0, (byte)128, (byte)0, (byte)255);
		public static readonly Color GreenYellow = new Color((byte)173, (byte)255, (byte)47, (byte)255);
		public static readonly Color Grey = new Color((byte)128, (byte)128, (byte)128, (byte)255);
		public static readonly Color Honeydew = new Color((byte)240, (byte)255, (byte)240, (byte)255);
		public static readonly Color HotPink = new Color((byte)255, (byte)105, (byte)180, (byte)255);
		public static readonly Color IndianRed = new Color((byte)205, (byte)92, (byte)92, (byte)255);
		public static readonly Color Indigo = new Color((byte)75, (byte)0, (byte)130, (byte)255);
		public static readonly Color Ivory = new Color((byte)255, (byte)255, (byte)240, (byte)255);
		public static readonly Color Khaki = new Color((byte)240, (byte)230, (byte)140, (byte)255);
		public static readonly Color Lavender = new Color((byte)230, (byte)230, (byte)250, (byte)255);
		public static readonly Color LavenderBlush = new Color((byte)255, (byte)240, (byte)245, (byte)255);
		public static readonly Color LawnGreen = new Color((byte)124, (byte)252, (byte)0, (byte)255);
		public static readonly Color LemonChiffon = new Color((byte)255, (byte)250, (byte)205, (byte)255);
		public static readonly Color LightBlue = new Color((byte)173, (byte)216, (byte)230, (byte)255);
		public static readonly Color LightCoral = new Color((byte)240, (byte)128, (byte)128, (byte)255);
		public static readonly Color LightCyan = new Color((byte)224, (byte)255, (byte)255, (byte)255);
		public static readonly Color LightGoldenrodYellow = new Color((byte)250, (byte)250, (byte)210, (byte)255);
		public static readonly Color LightGray = new Color((byte)211, (byte)211, (byte)211, (byte)255);
		public static readonly Color LightGreen = new Color((byte)144, (byte)238, (byte)144, (byte)255);
		public static readonly Color LightGrey = new Color((byte)211, (byte)211, (byte)211, (byte)255);
		public static readonly Color LightPink = new Color((byte)255, (byte)182, (byte)193, (byte)255);
		public static readonly Color LightSalmon = new Color((byte)255, (byte)160, (byte)122, (byte)255);
		public static readonly Color LightSeaGreen = new Color((byte)32, (byte)178, (byte)170, (byte)255);
		public static readonly Color LightSkyBlue = new Color((byte)135, (byte)206, (byte)250, (byte)255);
		public static readonly Color LightSlateGray = new Color((byte)119, (byte)136, (byte)153, (byte)255);
		public static readonly Color LightSlateGrey = new Color((byte)119, (byte)136, (byte)153, (byte)255);
		public static readonly Color LightSteelBlue = new Color((byte)176, (byte)196, (byte)222, (byte)255);
		public static readonly Color LightYellow = new Color((byte)255, (byte)255, (byte)224, (byte)255);
		public static readonly Color Lime = new Color((byte)0, (byte)255, (byte)0, (byte)255);
		public static readonly Color LimeGreen = new Color((byte)50, (byte)205, (byte)50, (byte)255);
		public static readonly Color Linen = new Color((byte)250, (byte)240, (byte)230, (byte)255);
		public static readonly Color Magenta = new Color((byte)255, (byte)0, (byte)255, (byte)255);
		public static readonly Color Maroon = new Color((byte)128, (byte)0, (byte)0, (byte)255);
		public static readonly Color MediumAquamarine = new Color((byte)102, (byte)205, (byte)170, (byte)255);
		public static readonly Color MediumBlue = new Color((byte)0, (byte)0, (byte)205, (byte)255);
		public static readonly Color MediumOrchid = new Color((byte)186, (byte)85, (byte)211, (byte)255);
		public static readonly Color MediumPurple = new Color((byte)147, (byte)112, (byte)219, (byte)255);
		public static readonly Color MediumSeaGreen = new Color((byte)60, (byte)179, (byte)113, (byte)255);
		public static readonly Color MediumSlateBlue = new Color((byte)123, (byte)104, (byte)238, (byte)255);
		public static readonly Color MediumSpringGreen = new Color((byte)0, (byte)250, (byte)154, (byte)255);
		public static readonly Color MediumTurquoise = new Color((byte)72, (byte)209, (byte)204, (byte)255);
		public static readonly Color MediumVioletRed = new Color((byte)199, (byte)21, (byte)133, (byte)255);
		public static readonly Color MidnightBlue = new Color((byte)25, (byte)25, (byte)112, (byte)255);
		public static readonly Color MintCream = new Color((byte)245, (byte)255, (byte)250, (byte)255);
		public static readonly Color MistyRose = new Color((byte)255, (byte)228, (byte)225, (byte)255);
		public static readonly Color Moccasin = new Color((byte)255, (byte)228, (byte)181, (byte)255);
		public static readonly Color NavajoWhite = new Color((byte)255, (byte)222, (byte)173, (byte)255);
		public static readonly Color Navy = new Color((byte)0, (byte)0, (byte)128, (byte)255);
		public static readonly Color OldLace = new Color((byte)253, (byte)245, (byte)230, (byte)255);
		public static readonly Color Olive = new Color((byte)128, (byte)128, (byte)0, (byte)255);
		public static readonly Color OliveDrab = new Color((byte)107, (byte)142, (byte)35, (byte)255);
		public static readonly Color Orange = new Color((byte)255, (byte)165, (byte)0, (byte)255);
		public static readonly Color Orangered = new Color((byte)255, (byte)69, (byte)0, (byte)255);
		public static readonly Color Orchid = new Color((byte)218, (byte)112, (byte)214, (byte)255);
		public static readonly Color PaleGoldenrod = new Color((byte)238, (byte)232, (byte)170, (byte)255);
		public static readonly Color PaleGreen = new Color((byte)152, (byte)251, (byte)152, (byte)255);
		public static readonly Color PaleTurquoise = new Color((byte)175, (byte)238, (byte)238, (byte)255);
		public static readonly Color PaleVioletRed = new Color((byte)219, (byte)112, (byte)147, (byte)255);
		public static readonly Color PapayaWhip = new Color((byte)255, (byte)239, (byte)213, (byte)255);
		public static readonly Color Peach = new Color((byte)255, (byte)192, (byte)128, (byte)255);
		public static readonly Color PeachPuff = new Color((byte)255, (byte)218, (byte)185, (byte)255);
		public static readonly Color Peru = new Color((byte)205, (byte)133, (byte)63, (byte)255);
		public static readonly Color Pink = new Color((byte)255, (byte)192, (byte)203, (byte)255);
		public static readonly Color Plum = new Color((byte)221, (byte)160, (byte)221, (byte)255);
		public static readonly Color PowderBlue = new Color((byte)176, (byte)224, (byte)230, (byte)255);
		public static readonly Color Purple = new Color((byte)128, (byte)0, (byte)128, (byte)255);
		public static readonly Color RebeccaPurple = new Color((byte)102, (byte)51, (byte)153, (byte)255);
		public static readonly Color Red = new Color((byte)255, (byte)0, (byte)0, (byte)255);
		public static readonly Color RosyBrown = new Color((byte)188, (byte)143, (byte)143, (byte)255);
		public static readonly Color RoyalBlue = new Color((byte)65, (byte)105, (byte)225, (byte)255);
		public static readonly Color SaddleBrown = new Color((byte)139, (byte)69, (byte)19, (byte)255);
		public static readonly Color Salmon = new Color((byte)250, (byte)128, (byte)114, (byte)255);
		public static readonly Color SandyBrown = new Color((byte)244, (byte)164, (byte)96, (byte)255);
		public static readonly Color SeaGreen = new Color((byte)46, (byte)139, (byte)87, (byte)255);
		public static readonly Color Seashel = new Color((byte)255, (byte)245, (byte)238, (byte)255);
		public static readonly Color Sienna = new Color((byte)160, (byte)82, (byte)45, (byte)255);
		public static readonly Color Silver = new Color((byte)192, (byte)192, (byte)192, (byte)255);
		public static readonly Color SkyBlue = new Color((byte)135, (byte)206, (byte)235, (byte)255);
		public static readonly Color SlateBlue = new Color((byte)106, (byte)90, (byte)205, (byte)255);
		public static readonly Color SlateGray = new Color((byte)112, (byte)128, (byte)144, (byte)255);
		public static readonly Color Snow = new Color((byte)255, (byte)250, (byte)250, (byte)255);
		public static readonly Color SpringGreen = new Color((byte)0, (byte)255, (byte)127, (byte)255);
		public static readonly Color SteelBlue = new Color((byte)70, (byte)130, (byte)180, (byte)255);
		public static readonly Color Tan = new Color((byte)210, (byte)180, (byte)140, (byte)255);
		public static readonly Color Tea = new Color((byte)0, (byte)128, (byte)128, (byte)255);
		public static readonly Color Thistle = new Color((byte)216, (byte)191, (byte)216, (byte)255);
		public static readonly Color Tomato = new Color((byte)255, (byte)99, (byte)71, (byte)255);
		public static readonly Color Turquoise = new Color((byte)64, (byte)224, (byte)208, (byte)255);
		public static readonly Color Violet = new Color((byte)238, (byte)130, (byte)238, (byte)255);
		public static readonly Color Wheat = new Color((byte)245, (byte)222, (byte)179, (byte)255);
		public static readonly Color White = new Color((byte)255, (byte)255, (byte)255, (byte)255);
		public static readonly Color WhiteSmoke = new Color((byte)245, (byte)245, (byte)245, (byte)255);
		public static readonly Color Yellow = new Color((byte)255, (byte)255, (byte)0, (byte)255);
		public static readonly Color YellowGreen = new Color((byte)154, (byte)205, (byte)50, (byte)255);

		#endregion

		#region Color names

		private static readonly (string Name, Color Color)[] _colorList = new (string Name, Color Color)[]
		{
			("transparent", Transparent),

			("antiquewhite", AntiqueWhite),
			("aqua", Aqua),
			("aquamarine", Aquamarine),
			("azure", Azure),
			("beige", Beige),
			("bisque", Bisque),
			("black", Black),
			("blanchedalmond", BlanchedAlmond),
			("blue", Blue),
			("blueviolet", BlueViolet),
			("brown", Brown),
			("burlywood", Burlywood),
			("cadetblue", CadetBlue),
			("chartreuse", Chartreuse),
			("chocolate", Chocolate),
			("coral", Coral),
			("cornflowerblue", CornflowerBlue),
			("cornsilk", Cornsilk),
			("crimson", Crimson),
			("cyan", Cyan),
			("darkblue", DarkBlue),
			("darkcyan", DarkCyan),
			("darkgoldenrod", DarkGoldenrod),
			("darkgray", DarkGray),
			("darkgreen", DarkGreen),
			("darkgrey", DarkGrey),
			("darkkhaki", DarkKhaki),
			("darkmagenta", DarkMagenta),
			("darkolivegreen", DarkOliveGreen),
			("darkorange", DarkOrange),
			("darkorchid", DarkOrchid),
			("darkred", DarkRed),
			("darksalmon", DarkSalmon),
			("darkseagreen", DarkSeaGreen),
			("darkslateblue", DarkSlateBlue),
			("darkslategray", DarkSlateGray),
			("darkslategrey", DarkSlateGrey),
			("darkturquoise", DarkTurquoise),
			("darkviolet", DarkViolet),
			("deeppink", DeepPink),
			("deepskyblue", DeepSkyBlue),
			("dimgray", DimGray),
			("dimgrey", DimGrey),
			("dodgerblue", DodgerBlue),
			("firebrick", FireBrick),
			("floralwhite", FloralWhite),
			("forestgreen", ForestGreen),
			("fuchsia", Fuchsia),
			("gainsboro", Gainsboro),
			("ghostwhite", GhostWhite),
			("gold", Gold),
			("goldenrod", Goldenrod),
			("gray", Gray),
			("green", Green),
			("greenyellow", GreenYellow),
			("grey", Grey),
			("honeydew", Honeydew),
			("hotpink", HotPink),
			("indianred", IndianRed),
			("indigo", Indigo),
			("ivory", Ivory),
			("khaki", Khaki),
			("lavender", Lavender),
			("lavenderblush", LavenderBlush),
			("lawngreen", LawnGreen),
			("lemonchiffon", LemonChiffon),
			("lightblue", LightBlue),
			("lightcoral", LightCoral),
			("lightcyan", LightCyan),
			("lightgoldenrodyellow", LightGoldenrodYellow),
			("lightgray", LightGray),
			("lightgreen", LightGreen),
			("lightgrey", LightGrey),
			("lightpink", LightPink),
			("lightsalmon", LightSalmon),
			("lightseagreen", LightSeaGreen),
			("lightskyblue", LightSkyBlue),
			("lightslategray", LightSlateGray),
			("lightslategrey", LightSlateGrey),
			("lightsteelblue", LightSteelBlue),
			("lightyellow", LightYellow),
			("lime", Lime),
			("limegreen", LimeGreen),
			("linen", Linen),
			("magenta", Magenta),
			("maroon", Maroon),
			("mediumaquamarine", MediumAquamarine),
			("mediumblue", MediumBlue),
			("mediumorchid", MediumOrchid),
			("mediumpurple", MediumPurple),
			("mediumseagreen", MediumSeaGreen),
			("mediumslateblue", MediumSlateBlue),
			("mediumspringgreen", MediumSpringGreen),
			("mediumturquoise", MediumTurquoise),
			("mediumvioletred", MediumVioletRed),
			("midnightblue", MidnightBlue),
			("mintcream", MintCream),
			("mistyrose", MistyRose),
			("moccasin", Moccasin),
			("navajowhite", NavajoWhite),
			("navy", Navy),
			("oldlace", OldLace),
			("olive", Olive),
			("olivedrab", OliveDrab),
			("orange", Orange),
			("orangered", Orangered),
			("orchid", Orchid),
			("palegoldenrod", PaleGoldenrod),
			("palegreen", PaleGreen),
			("paleturquoise", PaleTurquoise),
			("palevioletred", PaleVioletRed),
			("papayawhip", PapayaWhip),
			("peach", Peach),
			("peachpuff", PeachPuff),
			("peru", Peru),
			("pink", Pink),
			("plum", Plum),
			("powderblue", PowderBlue),
			("purple", Purple),
			("rebeccapurple", RebeccaPurple),
			("red", Red),
			("rosybrown", RosyBrown),
			("royalblue", RoyalBlue),
			("saddlebrown", SaddleBrown),
			("salmon", Salmon),
			("sandybrown", SandyBrown),
			("seagreen", SeaGreen),
			("seashel", Seashel),
			("sienna", Sienna),
			("silver", Silver),
			("skyblue", SkyBlue),
			("slateblue", SlateBlue),
			("slategray", SlateGray),
			("snow", Snow),
			("springgreen", SpringGreen),
			("steelblue", SteelBlue),
			("tan", Tan),
			("tea", Tea),
			("thistle", Thistle),
			("tomato", Tomato),
			("turquoise", Turquoise),
			("violet", Violet),
			("wheat", Wheat),
			("white", White),
			("whitesmoke", WhiteSmoke),
			("yellow", Yellow),
			("yellowgreen", YellowGreen),
		};

		/// <summary>
		/// The list of colors above, turned into a dictionary keyed by name.
		/// </summary>
		private static readonly IReadOnlyDictionary<string, Color> _colorsByName = (
			(Func<IReadOnlyDictionary<string, Color>>)(() =>
			{
				// Fanciness to project the color list to a dictionary, without taking a dependency on Linq.
				Dictionary<string, Color> colorsByName = new Dictionary<string, Color>();
				foreach ((string Name, Color Color) pair in _colorList)
				{
					colorsByName.Add(pair.Name, pair.Color);
				}
				return colorsByName;
			}))();

		/// <summary>
		/// The list of colors above, turned into a dictionary keyed by color.
		/// </summary>
		private static readonly IReadOnlyDictionary<Color, string> _namesByColor = (
			(Func<IReadOnlyDictionary<Color, string>>)(() =>
			{
				// Fanciness to project the color list to a dictionary, without taking a dependency on Linq.
				Dictionary<Color, string> namesByColor = new Dictionary<Color, string>();
				foreach ((string Name, Color Color) pair in _colorList)
				{
					namesByColor.Add(pair.Color, pair.Name);
				}
				return namesByColor;
			}))();

		/// <summary>
		/// Retrieve the full list of defined colors, in their definition order.
		/// </summary>
		public static IReadOnlyList<(string Name, Color Color)> ColorList => _colorList;

		/// <summary>
		/// A dictionary that maps color values to their matching names, in lowercase.
		/// </summary>
		public static IReadOnlyDictionary<Color, string> NamesByColor => _namesByColor;

		/// <summary>
		/// A dictionary that maps color names to their matching color values.
		/// </summary>
		public static IReadOnlyDictionary<string, Color> ColorsByName => _colorsByName;

		#endregion

		#region Color parsing

		/// <summary>
		/// Parse a 32-bit RGBA color (8 bits per channel) in one of several commmon CSS-style formats:
		///    - "#RGB"
		///    - "#RGBA"
		///    - "#RRGGBB"
		///    - "#RRGGBBAA"
		///    - "rgb(123, 45, 67)"
		///    - "rgba(123, 45, 67, 0.5)"
		///    - "name"   (standard web color names)
		/// </summary>
		/// <param name="value">The color value to parse.</param>
		/// <param name="color">The resulting actual color.</param>
		/// <exception cref="ArgumentException">Thrown if the color string cannot be parsed in one
		/// of the known formats.</exception>
		public static Color Parse(string value)
		{
			if (!TryParse(value, out Color color))
				throw new ArgumentException($"Invalid color value '{value}'.");
			return color;
		}

		/// <summary>
		/// Attempt to parse a 32-bit RGBA color (8 bits per channel) in one of several commmon CSS-style formats:
		///    - "#RGB"
		///    - "#RGBA"
		///    - "#RRGGBB"
		///    - "#RRGGBBAA"
		///    - "rgb(123, 45, 67)"
		///    - "rgba(123, 45, 67, 0.5)"
		///    - "name"   (standard web color names)
		/// </summary>
		/// <param name="value">The color value to parse.</param>
		/// <param name="color">The resulting actual color; if the string cannot be parsed,
		/// this will be set to Color.Transparent.</param>
		/// <returns>True if the color could be parsed, false if it could not.</returns>
		public static bool TryParse(string value, out Color color)
		{
			ReadOnlySpan<char> input = value.AsSpan().Trim();

			if (input.Length >= 2 && input[0] == '#' && IsHexDigits(input.Slice(1)))
				return TryParseHexColor(input.Slice(1), out color);

			char ch;
			if (input.Length >= 4
				&& ((ch = input[0]) == 'r' || ch == 'R')
				&& ((ch = input[1]) == 'g' || ch == 'G')
				&& ((ch = input[2]) == 'b' || ch == 'B'))
			{
				if (!TryLexRgba(input.Slice(3), out color))
					return false;
				return true;
			}

			if (input.Length > 20)
			{
				// All of the color names are 20 characters or less.
				color = Transparent;
				return false;
			}

			Span<char> lowerName = stackalloc char[input.Length];
			for (int i = 0; i < input.Length; i++)
			{
				ch = input[i];
				if (ch >= 'A' && ch <= 'Z')
					ch = (char)(ch + 32);       // Faster than ToLowerInvariant() for ASCII.
				lowerName[i] = ch;
			}

			if (ColorsByName.TryGetValue(new string(lowerName), out Color c))
			{
				color = c;
				return true;
			}

			color = Transparent;
			return false;
		}

		public static bool TryParseHexColor(ReadOnlySpan<char> hex, out Color color)
		{
			switch (hex.Length)
			{
				case 1:
					int n = ParseHexDigit(hex[0]);
					n = (n << 4) | n;
					color = new Color(n, n, n, 255);
					return true;

				case 2:
					n = ParseHexPair(hex[0], hex[1]);
					color = new Color(n, n, n, 255);
					return true;

				case 3:
					int r = ParseHexDigit(hex[0]);
					int g = ParseHexDigit(hex[1]);
					int b = ParseHexDigit(hex[2]);
					r = (r << 4) | r;
					g = (g << 4) | g;
					b = (b << 4) | b;
					color = new Color(r, g, b, 255);
					return true;

				case 4:
					r = ParseHexDigit(hex[0]);
					g = ParseHexDigit(hex[1]);
					b = ParseHexDigit(hex[2]);
					int a = ParseHexDigit(hex[3]);
					r = (r << 4) | r;
					g = (g << 4) | g;
					b = (b << 4) | b;
					a = (a << 4) | a;
					color = new Color(r, g, b, a);
					return true;

				case 6:
					r = ParseHexPair(hex[0], hex[1]);
					g = ParseHexPair(hex[2], hex[3]);
					b = ParseHexPair(hex[4], hex[5]);
					color = new Color(r, g, b, 255);
					return true;

				case 8:
					r = ParseHexPair(hex[0], hex[1]);
					g = ParseHexPair(hex[2], hex[3]);
					b = ParseHexPair(hex[4], hex[5]);
					a = ParseHexPair(hex[6], hex[7]);
					color = new Color(r, g, b, a);
					return true;

				default:
					color = Transparent;
					return false;
			}
		}

		#endregion

		#region Internal number parsing

		private static readonly sbyte[] _hexDigitValues =
		{
			-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
			-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,

			-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
			-1, 1, 2, 3, 4, 5, 6, 7, 8, 9,-1,-1,-1,-1,-1,-1,

			-1,10,11,12,13,14,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,
			-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,

			-1,10,11,12,13,14,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,
			-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
		};

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint ParseHexUInt(string text)
		{
			uint value = 0;
			foreach (char ch in text)
			{
				value <<= 4;
				value |= (uint)ParseHexDigit(ch);
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int ParseHexPair(char v1, char v2)
			=> (ParseHexDigit(v1) << 4) | ParseHexDigit(v2);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int ParseHexDigit(char v)
			   => v < 128 ? _hexDigitValues[v] : 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static bool IsHexDigits(ReadOnlySpan<char> chars)
		{
			foreach (char ch in chars)
				if (ch >= 128 || _hexDigitValues[ch] < 0)
					return false;
			return true;
		}

		private static bool TryLexNumber(ReadOnlySpan<char> input, ref int ptr, out int value)
		{
			value = 0;
			int start = ptr;

			char ch;
			while (ptr < input.Length && (ch = input[ptr]) >= '0' && ch <= '9')
			{
				value *= 10;
				value += ch - '0';
				ptr++;
			}

			return start < ptr && ptr < start + 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static void SkipWhitespace(ReadOnlySpan<char> input, ref int ptr)
		{
			char ch;
			while (ptr < input.Length
				&& (ch = input[ptr]) >= '\x0' && ch <= '\x20')
				ptr++;
		}

		private static bool TryLexRgba(ReadOnlySpan<char> input, out Color color)
		{
			// Before entering this method, we assume that the 'rgb' letters have
			// been tested and matched.
			int ptr = 3;
			color = default;

			// Consume an optional 'a' after the 'rgb'.
			char ch;
			if ((ch = input[ptr]) == 'a' || ch == 'A')
				ptr++;

			SkipWhitespace(input, ref ptr);

			// Require a '(' next.
			if (!(ptr < input.Length && input[ptr++] == '('))
				return false;

			SkipWhitespace(input, ref ptr);

			// Read the red value.
			if (!TryLexNumber(input, ref ptr, out int red))
				return false;

			// Require a ',' next.
			if (!(ptr < input.Length && input[ptr++] == ','))
				return false;

			SkipWhitespace(input, ref ptr);

			// Read the green value.
			if (!TryLexNumber(input, ref ptr, out int green))
				return false;

			// Require a ',' next.
			if (!(ptr < input.Length && input[ptr++] == ','))
				return false;

			SkipWhitespace(input, ref ptr);

			// Read the blue value.
			if (!TryLexNumber(input, ref ptr, out int blue))
				return false;

			SkipWhitespace(input, ref ptr);

			int alpha = 255;
			if (ptr < input.Length && input[ptr] == ',')
			{
				// Got a comma, so prepare to read the alpha value as a fraction.
				ptr++;

				SkipWhitespace(input, ref ptr);

				if (!TryLexFractionFrom0To1(input, ref ptr, out alpha))
					return false;

				SkipWhitespace(input, ref ptr);
			}

			// Require a ')' next.
			if (!(ptr < input.Length && input[ptr++] == ')'))
				return false;

			SkipWhitespace(input, ref ptr);

			// Finally, we should be at the end of the string.
			if (ptr != input.Length)
				return false;

			// We got it, and the data parsed cleanly.  Return it!
			color = new Color(red, green, blue, alpha);
			return true;
		}

		/// <summary>
		/// Lex and parse a fractional decimal value, clamping (saturating) the
		/// result to [0.0, 1.0].
		/// </summary>
		/// <param name="input">The input character string.</param>
		/// <param name="ptr">The current source pointer in the input string, which will
		/// be updated by this method to point after the decimal value after it is read.</param>
		/// <param name="value">The resulting fractional value, represented as [0, 255].</param>
		/// <returns>True for a successful parse, false for a bad input.</returns>
		private static bool TryLexFractionFrom0To1(ReadOnlySpan<char> input, ref int ptr, out int value)
		{
			int start = ptr;

			// Read digits before the decimal point.
			int prefix = 0;
			char ch;
			while (ptr < input.Length && (ch = input[ptr]) >= '0' && ch <= '9')
			{
				prefix |= ch - '0';
				ptr++;
			}

			int alpha = 0;

			if (!(ptr < input.Length && input[ptr] == '.'))
			{
				// No fractional digits, so we're done.
				value = prefix != 0 ? 255 : 0;
				return ptr > start;
			}

			ptr++;

			// Read digits after the decimal point, with 24 bits of precision (same as a float,
			// but we do it in all-integer math, which is faster to compute).
			int multiplier = 0x1000000;
			while (ptr < input.Length && (ch = input[ptr]) >= '0' && ch <= '9')
			{
				ptr++;
				if (multiplier > 0)
				{
					// Divide the multiplier by 10, using fast multiplication.
					multiplier = (int)((multiplier * ((1L << 32) / 10)) >> 32);

					// Add in the fractional value.
					alpha += multiplier * (ch - '0');
				}
			}

			// We now have a value from 0 to 0x1000000 instead of the desired value of 0 to 255.
			// So use multiplication and shifting to turn it into the range of 0 to 255.
			alpha = ((alpha >> 1) * 255) >> 23;

			// For 0.xxx, we use the fractional value, but anything bigger is clamped to 1.
			value = prefix != 0 ? 255 : alpha;
			return ptr > start;
		}

		#endregion
	}
}
