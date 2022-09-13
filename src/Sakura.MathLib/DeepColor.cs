
namespace Sakura.MathLib
{
	/// <summary>
	/// An RGBA color, with lots of operations to manipulate it, where each
	/// color component is represented by 16 bits.
	/// </summary>
	public struct DeepColor : IEquatable<DeepColor>
	{
		#region Core storage: Exactly four bytes in sequence.

		/// <summary>
		/// The red component of the color.
		/// </summary>
		public ushort R;

		/// <summary>
		/// The green component of the color.
		/// </summary>
		public ushort G;

		/// <summary>
		/// The blue component of the color.
		/// </summary>
		public ushort B;

		/// <summary>
		/// The alpha component of the color.
		/// </summary>
		public ushort A;

		#endregion

		#region Properties that simulate floats

		/// <summary>
		/// The red component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Rf
		{
			get => R * (1f / 65535);
			set => R = (byte)Math.Min(Math.Max((int)(value * 65535 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The green component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Gf
		{
			get => G * (1f / 65535);
			set => G = (byte)Math.Min(Math.Max((int)(value * 65535 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The blue component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Bf
		{
			get => B * (1f / 65535);
			set => B = (byte)Math.Min(Math.Max((int)(value * 65535 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The alpha component, as a floating-point value, clamped to [0.0f, 1.0f].
		/// </summary>
		public float Af
		{
			get => A * (1f / 65535);
			set => A = (byte)Math.Min(Math.Max((int)(value * 65535 + 0.5f), 0), 255);
		}

		/// <summary>
		/// The color represented as a 4-component vector, where X=R, Y=G, Z=B, and W=A.
		/// </summary>
		public Vector4d Vector4d
		{
			get => new Vector4d(R * (1.0 / 65535), G * (1.0 / 65535), B * (1.0 / 65535), A * (1.0 / 65535));
			set
			{
				R = (byte)Math.Min(Math.Max((int)(value.X * 65535 + 0.5), 0), 255);
				G = (byte)Math.Min(Math.Max((int)(value.Y * 65535 + 0.5), 0), 255);
				B = (byte)Math.Min(Math.Max((int)(value.Z * 65535 + 0.5), 0), 255);
				A = (byte)Math.Min(Math.Max((int)(value.W * 65535 + 0.5), 0), 255);
			}
		}

		/// <summary>
		/// The color represented as a 4-component vector, where X=R, Y=G, Z=B, and W=A.
		/// </summary>
		public Vector4f Vector4f
		{
			get => new Vector4f(R * (1.0f / 65535), G * (1.0f / 65535), B * (1.0f / 65535), A * (1.0f / 65535));
			set
			{
				R = (byte)Math.Min(Math.Max((int)(value.X * 65535 + 0.5f), 0), 255);
				G = (byte)Math.Min(Math.Max((int)(value.Y * 65535 + 0.5f), 0), 255);
				B = (byte)Math.Min(Math.Max((int)(value.Z * 65535 + 0.5f), 0), 255);
				A = (byte)Math.Min(Math.Max((int)(value.W * 65535 + 0.5f), 0), 255);
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
		public DeepColor(float r, float g, float b, float a = 1.0f)
			: this((int)(r * 65535 + 0.5f), (int)(g * 65535 + 0.5f), (int)(b * 65535 + 0.5f), (int)(a * 65535 + 0.5f))
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
		public DeepColor(Vector4f v)
			: this((int)(v.X * 65535 + 0.5f), (int)(v.Y * 65535 + 0.5f), (int)(v.Z * 65535 + 0.5f), (int)(v.W * 65535 + 0.5f))
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
		public DeepColor(Vector4d v)
			: this((int)(v.X * 65535 + 0.5), (int)(v.Y * 65535 + 0.5), (int)(v.Z * 65535 + 0.5), (int)(v.W * 65535 + 0.5))
		{
		}

		/// <summary>
		/// Construct a color from four integer values, R, G, B, and A, in the range of 0-255
		/// each.  Values outside 0-65535 will be clamped to that range.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public DeepColor(int r, int g, int b, int a = 65535)
		{
			if (((r | g | b | a) & 0xFFFF0000) == 0)
			{
				R = (byte)r;
				G = (byte)g;
				B = (byte)b;
				A = (byte)a;
			}
			else
			{
				R = (byte)Math.Min(Math.Max(r, 0), 65535);
				G = (byte)Math.Min(Math.Max(g, 0), 65535);
				B = (byte)Math.Min(Math.Max(b, 0), 65535);
				A = (byte)Math.Min(Math.Max(a, 0), 65535);
			}
		}

		/// <summary>
		/// Construct a color from four byte values, R, G, B, and A, in the range of 0-65535
		/// each.  This overload sets the bytes directly, so it may be faster than the other
		/// constructors.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public DeepColor(ushort r, ushort g, ushort b, ushort a = 65535)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		/// <summary>
		/// Construct a DeepColor from an ordinary Color.
		/// </summary>
		public DeepColor(Color c)
		{
			R = (ushort)(c.R * 256 + c.R);
			G = (ushort)(c.G * 256 + c.G);
			B = (ushort)(c.B * 256 + c.B);
			A = (ushort)(c.A * 256 + c.A);
		}

		public static implicit operator DeepColor(Color c)
			=> new DeepColor(c);
		public static explicit operator Color(DeepColor c)
			=> new Color((byte)c.R >> 8, (byte)c.G >> 8, (byte)c.B >> 8, (byte)c.A >> 8);

		#endregion

		#region Equality and hash codes

		/// <summary>
		/// Compare this color to another object on the heap for equality.
		/// </summary>
		public override bool Equals(object? obj)
			=> obj is DeepColor color && this == color;

		/// <summary>
		/// Compare this color to another for equality.
		/// </summary>
		public bool Equals(DeepColor other)
			=> this == other;

		/// <summary>
		/// Compare this color to another color for equality.
		/// </summary>
		public static bool operator ==(DeepColor a, DeepColor b)
			=> a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;

		/// <summary>
		/// Compare this color to another color for inequality.
		/// </summary>
		public static bool operator !=(DeepColor a, DeepColor b)
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
				WriteByte(buffer, 1, (byte)(R >> 8));
				WriteByte(buffer, 3, (byte)(G >> 8));
				WriteByte(buffer, 5, (byte)(B >> 8));
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
				WriteByte(buffer, 1, (byte)(R >> 8));
				WriteByte(buffer, 3, (byte)(G >> 8));
				WriteByte(buffer, 5, (byte)(B >> 8));
				WriteByte(buffer, 7, (byte)(A >> 8));
				return new string(buffer);
			}
		}

		/// <summary>
		/// Get a string form of this color, as a 12-digit hex code, preceded by a
		/// sharp sign, like in CSS:  #RRRRGGGGBBBB
		/// </summary>
		public string Hex12
		{
			get
			{
				Span<char> buffer = stackalloc char[13];
				buffer[0] = '#';
				WriteUShort(buffer, 1, R);
				WriteUShort(buffer, 5, G);
				WriteUShort(buffer, 9, B);
				return new string(buffer);
			}
		}

		/// <summary>
		/// Get a string form of this color, as an 16-digit hex code, preceded by a
		/// sharp sign, like extended CSS:  #RRRRGGGGBBBBAAAA
		/// </summary>
		public string Hex16
		{
			get
			{
				Span<char> buffer = stackalloc char[17];
				buffer[0] = '#';
				WriteUShort(buffer, 1, R);
				WriteUShort(buffer, 5, G);
				WriteUShort(buffer, 9, B);
				WriteUShort(buffer, 13, A);
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
		/// Write a single hex ushort out to the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer to write to.</param>
		/// <param name="position">The position in that buffer to write.</param>
		/// <param name="value">The byte to write.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void WriteUShort(Span<char> buffer, int position, ushort value)
		{
			WriteNybble(buffer, position, (byte)((value >> 12) & 0xF));
			WriteNybble(buffer, position + 1, (byte)((value >> 8) & 0xF));
			WriteNybble(buffer, position + 2, (byte)((value >> 4) & 0xF));
			WriteNybble(buffer, position + 3, (byte)(value & 0xF));
		}

		/// <summary>
		/// Convert this color to its most natural string representation.
		/// </summary>
		/// <returns>A "natural" string representation, which will be the color's name if
		/// it has one, or an #RRGGBB(AA) form if it doesn't have a name.</returns>
		public override string ToString()
			=> NamesByColor.TryGetValue(this, out string? name) ? name : ToHexString();

		/// <summary>
		/// Convert this to a string of the form rgb(R,G,B,A), omitting the 'A' if it's 65535, and
		/// converting 'A' to a value in the range of [0, 1].
		/// </summary>
		/// <returns>A string form of this color, as a CSS rgb() three-valued or four-valued vector.</returns>
		public string ToRgbString()
			=> A < 65535 ? $"rgba({R},{G},{B},{A * (1.0 / 255)})" : $"rgb({R},{G},{B})";

		/// <summary>
		/// Convert this to a string of the form (R,G,B,A), omitting the 'A' if it's 255.
		/// </summary>
		/// <returns>A string form of this color, as a three-valued or four-valued vector.</returns>
		public string ToVectorString()
			=> A < 65535 ? $"({R},{G},{B},{A})" : $"({R},{G},{B})";

		/// <summary>
		/// Convert this to a string of the form #RRRRGGGGBBBBAAAA, omitting the AAAA if 'A' is 65535,
		/// and truncating to a shorter form if it just happens to be 8-bit data.
		/// </summary>
		/// <returns>This color converted to a hex string.</returns>
		public string ToHexString()
		{
			if (IsEffectively8Bit)
				return A < 65535 ? Hex8 : Hex6;
			else
				return A < 65535 ? Hex16 : Hex12;
		}

		/// <summary>
		/// Answer whether this color is effectively 8 bits per channel that just happens
		/// to be stored in a 16-bit form.
		/// </summary>
		public bool IsEffectively8Bit
			=> (R >> 8) == (R & 0xFF)
				&& (G >> 8) == (G & 0xFF)
				&& (B >> 8) == (B & 0xFF)
				&& (A >> 8) == (A & 0xFF);

		#endregion

		#region DeepColor mixing

		/// <summary>
		/// Combine two colors, with equal amounts of each.
		/// </summary>
		/// <param name="other">The other color to merge with this color.</param>
		/// <returns>The new, fused color.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public DeepColor Merge(DeepColor other)
			=> new DeepColor(
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
		public DeepColor Mix(DeepColor other, float amount)
		{
			long am = (long)(Math.Min(Math.Max(amount, 0), 1) * 0x100000000L);
			long ia = 0x100000000L - am;

			ushort r = (ushort)((R * ia + other.R * am + 0x80000000L) >> 32);
			ushort g = (ushort)((G * ia + other.G * am + 0x80000000L) >> 32);
			ushort b = (ushort)((B * ia + other.B * am + 0x80000000L) >> 32);
			ushort a = (ushort)((A * ia + other.A * am + 0x80000000L) >> 32);

			return new DeepColor(r, g, b, a);
		}

		/// <summary>
		/// Perform linear interpolation between this and another color.  'amount'
		/// describes how much of the other color is included in the result, on a
		/// scale of [0, 65536].
		/// </summary>
		/// <param name="other">The other color to mix with this color.</param>
		/// <param name="amount">How much of the other color to mix with this color,
		/// on a range of 0 (entirely this color) to 1 (entirely the other color).
		/// Exactly 32768 is equivalent to calling the Merge() method instead.</param>
		/// <remarks>Merge() runs faster if you need exactly 50% of each color.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public DeepColor Mix(DeepColor other, int amount)
		{
			uint am = (uint)(Math.Min(Math.Max(amount, 0), 0x10000));
			uint ia = 0x10000U - am;

			ushort r = (ushort)((R * ia + other.R * am + 0x8000U) >> 16);
			ushort g = (ushort)((G * ia + other.G * am + 0x8000U) >> 16);
			ushort b = (ushort)((B * ia + other.B * am + 0x8000U) >> 16);
			ushort a = (ushort)((A * ia + other.A * am + 0x8000U) >> 16);

			return new DeepColor(r, g, b, a);
		}

		/// <summary>
		/// Perform additive blend of this color over top of another "surface" color
		/// (i.e., this is a "compositing" operation, not just mixing colors).
		/// Neither color should use premultiplied alpha.
		/// </summary>
		/// <param name="surface">The surface color to apply this color on top of.
		/// This surface color may have any valid alpha value.</param>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public DeepColor Blend(DeepColor surface)
		{
			const long divisor = 65535L * 65535L * 2;
			const long half = divisor / 2;

			long oa = A * 65535L * 2;
			long sa = (65535L - oa) * surface.A * 2;

			ushort r = (ushort)((R * oa + surface.R * sa + half) / divisor);
			ushort g = (ushort)((G * oa + surface.G * sa + half) / divisor);
			ushort b = (ushort)((B * oa + surface.B * sa + half) / divisor);
			ushort a = (ushort)((oa + sa + 65535L) / (65535L * 2));

			return new DeepColor(r, g, b, a);
		}

		/// <summary>
		/// Transform a color that uses non-premultipled alpha to one that contains premultiplied alpha.
		/// Note that this is a lossy transformation:  Unpremultiply(Premultiply(x)) will result in a
		/// *similar* color, but not the *same* color as the original.
		/// </summary>
		/// <param name="r">The red value, which should be from 0 to 65535.</param>
		/// <param name="g">The green value, which should be from 0 to 65535.</param>
		/// <param name="b">The blue value, which should be from 0 to 65535.</param>
		/// <param name="a">The alpha value, which should be from 0 to 65535.</param>
		public static DeepColor Premultiply(int r, int g, int b, int a)
			=> a >= 65535
				? new DeepColor(r, g, b, 65535)
				: new DeepColor(
					(int)(((uint)r * a) / 65535),
					(int)(((uint)g * a) / 65535),
					(int)(((uint)b * a) / 65535), a);

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
		public static DeepColor Unpremultiply(int r, int g, int b, int a)
		{
			if (a == 65535) return new DeepColor(r, g, b);
			if (a == 0) return new DeepColor((byte)0, (byte)0, (byte)0, (byte)0);

			long ooa = (0xFFFF * 0x100000000L) / a;
			return new DeepColor((int)((r * ooa) >> 32), (int)((g * ooa) >> 32), (int)((b * ooa) >> 32), a);
		}

		/// <summary>
		/// Transform a color that uses non-premultipled alpha to one that contains premultiplied alpha.
		/// Note that this is a lossy transformation:  Unpremultiply(Premultiply(x)) will result in a
		/// *similar* color, but not the *same* color as the original.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public DeepColor Premultiply()
			=> Premultiply(R, G, B, A);

		/// <summary>
		/// Transform a color that uses premultiplied color components into one that contains
		/// non-premultiplied color components.  Note that premultiplication is a lossy transformation:
		/// Unpremultiply(Premultiply(x)) will result in a *similar* color, but not the *same* color
		/// as the original.
		/// </summary>
		/// <returns>The un-premultiplied color.</returns>
		public DeepColor Unpremultiply()
			=> Unpremultiply(R, G, B, A);

		/// <summary>
		/// Generate a weighted grayscale color, taking into account the relative apparent
		/// brightness of each color component:  0.299 * R + 0.587 * G + 0.114 * B
		/// </summary>
		public DeepColor Grayscale()
		{
			ushort g = (ushort)((R * 19595 + G * 38469 + B * 7471) >> 16);
			return new DeepColor(g, g, g, A);
		}

		/// <summary>
		/// Generate an unweighted grayscale color, giving each color component equal value.
		/// You usually don't want this, unless you do.
		/// </summary>
		public DeepColor UnweightedGrayscale()
		{
			ushort g = (ushort)((R * 2 + G * 2 + B * 2 + 3) / 6);
			return new DeepColor(g, g, g, A);
		}

		#endregion

		#region Deconstruction and tuple-conversion

		/// <summary>
		/// Convert this color to a tuple of floating-point values.
		/// </summary>
		/// <returns>A four-valued tuple of floating-point values in the range of [0, 1], in the
		/// form (r, g, b, a).</returns>
		public (float r, float g, float b, float a) ToFloats()
			=> (R * (1 / 65535f), G * (1 / 65535f), B * (1 / 65535f), A * (1 / 65535f));

		/// <summary>
		/// Convert this to a tuple of integer values.
		/// </summary>
		/// <returns>A four-valued tuple of integer values in the range of [0, 65535], in the
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
		public void Deconstruct(out ushort r, out ushort g, out ushort b)
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
		public void Deconstruct(out ushort r, out ushort g, out ushort b, out ushort a)
		{
			r = R;
			g = G;
			b = B;
			a = A;
		}

		#endregion

		#region DeepColor "arithmetic" operators

		/// <summary>
		/// Perform componentwise addition on the R, G, and B values of the given colors.
		/// The alpha of the result will be assigned to the larger alpha of either color.
		/// Note that the result of this operation is usually only meaningful for
		/// non-premultiplied colors.
		/// </summary>
		/// <param name="x">The first color to add.</param>
		/// <param name="y">The second color to add.</param>
		/// <returns>The "sum" of those colors.</returns>
		public static DeepColor operator +(DeepColor x, DeepColor y)
			=> new DeepColor(x.R + y.R, x.G + y.G, x.B + y.B, Math.Max(x.A, y.A));

		/// <summary>
		/// Perform componentwise subtraction on the R, G, and B values of the given colors.
		/// The alpha of the result will be assigned to the larger alpha of either color.
		/// Note that the result of this operation is usually only meaningful for
		/// non-premultiplied colors.
		/// </summary>
		/// <param name="x">The source color.</param>
		/// <param name="y">The color to subtract from the source.</param>
		/// <returns>The "difference" of those colors.</returns>
		public static DeepColor operator -(DeepColor x, DeepColor y)
			=> new DeepColor(x.R - y.R, x.G - y.G, x.B - y.B, Math.Max(x.A, y.A));

		/// <summary>
		/// Calculate the "inverse" of the given color, treating each value as
		/// though it is in the range of [0, 1] and then subtracting it from 1.
		/// The alpha will be left unchanged.  Note that the result of this
		/// operation is usually only meaningful for non-premultiplied colors.
		/// </summary>
		/// <param name="c">The original color.</param>
		/// <returns>The "inverse" of that color.</returns>
		public static DeepColor operator -(DeepColor c)
			=> new DeepColor((ushort)(65535 - c.R), (ushort)(65535 - c.G), (ushort)(65535 - c.B), c.A);

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
		public static DeepColor operator *(DeepColor x, DeepColor y)
			=> new DeepColor(
				(ushort)(((uint)x.R * y.R) / 65535),
				(ushort)(((uint)x.G * y.G) / 65535),
				(ushort)(((uint)x.B * y.B) / 65535), Math.Max(x.A, y.A));

		#endregion

		#region Static colors (CSS colors)

		public static readonly DeepColor Transparent = new DeepColor((ushort)0, (ushort)0, (ushort)0, (ushort)0);

		public static readonly DeepColor AntiqueWhite = new DeepColor((ushort)64250, (ushort)60395, (ushort)55255, (ushort)65535);
		public static readonly DeepColor Aqua = new DeepColor((ushort)0, (ushort)65535, (ushort)65535, (ushort)65535);
		public static readonly DeepColor Aquamarine = new DeepColor((ushort)32639, (ushort)65535, (ushort)54484, (ushort)65535);
		public static readonly DeepColor Azure = new DeepColor((ushort)61680, (ushort)65535, (ushort)65535, (ushort)65535);
		public static readonly DeepColor Beige = new DeepColor((ushort)62965, (ushort)62965, (ushort)56540, (ushort)65535);
		public static readonly DeepColor Bisque = new DeepColor((ushort)65535, (ushort)58596, (ushort)50372, (ushort)65535);
		public static readonly DeepColor Black = new DeepColor((ushort)0, (ushort)0, (ushort)0, (ushort)65535);
		public static readonly DeepColor BlanchedAlmond = new DeepColor((ushort)65535, (ushort)60395, (ushort)52685, (ushort)65535);
		public static readonly DeepColor Blue = new DeepColor((ushort)0, (ushort)0, (ushort)65535, (ushort)65535);
		public static readonly DeepColor BlueViolet = new DeepColor((ushort)35466, (ushort)11051, (ushort)58082, (ushort)65535);
		public static readonly DeepColor Brown = new DeepColor((ushort)42405, (ushort)10794, (ushort)10794, (ushort)65535);
		public static readonly DeepColor Burlywood = new DeepColor((ushort)57054, (ushort)47288, (ushort)34695, (ushort)65535);
		public static readonly DeepColor CadetBlue = new DeepColor((ushort)24415, (ushort)40606, (ushort)41120, (ushort)65535);
		public static readonly DeepColor Chartreuse = new DeepColor((ushort)32639, (ushort)65535, (ushort)0, (ushort)65535);
		public static readonly DeepColor Chocolate = new DeepColor((ushort)53970, (ushort)26985, (ushort)7710, (ushort)65535);
		public static readonly DeepColor Coral = new DeepColor((ushort)65535, (ushort)32639, (ushort)20560, (ushort)65535);
		public static readonly DeepColor CornflowerBlue = new DeepColor((ushort)25700, (ushort)38293, (ushort)60909, (ushort)65535);
		public static readonly DeepColor Cornsilk = new DeepColor((ushort)65535, (ushort)63736, (ushort)56540, (ushort)65535);
		public static readonly DeepColor Crimson = new DeepColor((ushort)56540, (ushort)5140, (ushort)15420, (ushort)65535);
		public static readonly DeepColor Cyan = new DeepColor((ushort)0, (ushort)65535, (ushort)65535, (ushort)65535);
		public static readonly DeepColor DarkBlue = new DeepColor((ushort)0, (ushort)0, (ushort)35723, (ushort)65535);
		public static readonly DeepColor DarkCyan = new DeepColor((ushort)0, (ushort)35723, (ushort)35723, (ushort)65535);
		public static readonly DeepColor DarkGoldenrod = new DeepColor((ushort)47288, (ushort)34438, (ushort)2827, (ushort)65535);
		public static readonly DeepColor DarkGray = new DeepColor((ushort)43433, (ushort)43433, (ushort)43433, (ushort)65535);
		public static readonly DeepColor DarkGreen = new DeepColor((ushort)0, (ushort)25700, (ushort)0, (ushort)65535);
		public static readonly DeepColor DarkGrey = new DeepColor((ushort)43433, (ushort)43433, (ushort)43433, (ushort)65535);
		public static readonly DeepColor DarkKhaki = new DeepColor((ushort)48573, (ushort)47031, (ushort)27499, (ushort)65535);
		public static readonly DeepColor DarkMagenta = new DeepColor((ushort)35723, (ushort)0, (ushort)35723, (ushort)65535);
		public static readonly DeepColor DarkOliveGreen = new DeepColor((ushort)21845, (ushort)27499, (ushort)12079, (ushort)65535);
		public static readonly DeepColor DarkOrange = new DeepColor((ushort)65535, (ushort)35980, (ushort)0, (ushort)65535);
		public static readonly DeepColor DarkOrchid = new DeepColor((ushort)39321, (ushort)12850, (ushort)52428, (ushort)65535);
		public static readonly DeepColor DarkRed = new DeepColor((ushort)35723, (ushort)0, (ushort)0, (ushort)65535);
		public static readonly DeepColor DarkSalmon = new DeepColor((ushort)59881, (ushort)38550, (ushort)31354, (ushort)65535);
		public static readonly DeepColor DarkSeaGreen = new DeepColor((ushort)36751, (ushort)48316, (ushort)36751, (ushort)65535);
		public static readonly DeepColor DarkSlateBlue = new DeepColor((ushort)18504, (ushort)15677, (ushort)35723, (ushort)65535);
		public static readonly DeepColor DarkSlateGray = new DeepColor((ushort)12079, (ushort)20303, (ushort)20303, (ushort)65535);
		public static readonly DeepColor DarkSlateGrey = new DeepColor((ushort)12079, (ushort)20303, (ushort)20303, (ushort)65535);
		public static readonly DeepColor DarkTurquoise = new DeepColor((ushort)0, (ushort)52942, (ushort)53713, (ushort)65535);
		public static readonly DeepColor DarkViolet = new DeepColor((ushort)38036, (ushort)0, (ushort)54227, (ushort)65535);
		public static readonly DeepColor DeepPink = new DeepColor((ushort)65535, (ushort)5140, (ushort)37779, (ushort)65535);
		public static readonly DeepColor DeepSkyBlue = new DeepColor((ushort)0, (ushort)49087, (ushort)65535, (ushort)65535);
		public static readonly DeepColor DimGray = new DeepColor((ushort)26985, (ushort)26985, (ushort)26985, (ushort)65535);
		public static readonly DeepColor DimGrey = new DeepColor((ushort)26985, (ushort)26985, (ushort)26985, (ushort)65535);
		public static readonly DeepColor DodgerBlue = new DeepColor((ushort)7710, (ushort)37008, (ushort)65535, (ushort)65535);
		public static readonly DeepColor FireBrick = new DeepColor((ushort)45746, (ushort)8738, (ushort)8738, (ushort)65535);
		public static readonly DeepColor FloralWhite = new DeepColor((ushort)65535, (ushort)64250, (ushort)61680, (ushort)65535);
		public static readonly DeepColor ForestGreen = new DeepColor((ushort)8738, (ushort)35723, (ushort)8738, (ushort)65535);
		public static readonly DeepColor Fuchsia = new DeepColor((ushort)65535, (ushort)0, (ushort)65535, (ushort)65535);
		public static readonly DeepColor Gainsboro = new DeepColor((ushort)56540, (ushort)56540, (ushort)56540, (ushort)65535);
		public static readonly DeepColor GhostWhite = new DeepColor((ushort)63736, (ushort)63736, (ushort)65535, (ushort)65535);
		public static readonly DeepColor Gold = new DeepColor((ushort)65535, (ushort)55255, (ushort)0, (ushort)65535);
		public static readonly DeepColor Goldenrod = new DeepColor((ushort)56026, (ushort)42405, (ushort)8224, (ushort)65535);
		public static readonly DeepColor Gray = new DeepColor((ushort)32896, (ushort)32896, (ushort)32896, (ushort)65535);
		public static readonly DeepColor Green = new DeepColor((ushort)0, (ushort)32896, (ushort)0, (ushort)65535);
		public static readonly DeepColor GreenYellow = new DeepColor((ushort)44461, (ushort)65535, (ushort)12079, (ushort)65535);
		public static readonly DeepColor Grey = new DeepColor((ushort)32896, (ushort)32896, (ushort)32896, (ushort)65535);
		public static readonly DeepColor Honeydew = new DeepColor((ushort)61680, (ushort)65535, (ushort)61680, (ushort)65535);
		public static readonly DeepColor HotPink = new DeepColor((ushort)65535, (ushort)26985, (ushort)46260, (ushort)65535);
		public static readonly DeepColor IndianRed = new DeepColor((ushort)52685, (ushort)23644, (ushort)23644, (ushort)65535);
		public static readonly DeepColor Indigo = new DeepColor((ushort)19275, (ushort)0, (ushort)33410, (ushort)65535);
		public static readonly DeepColor Ivory = new DeepColor((ushort)65535, (ushort)65535, (ushort)61680, (ushort)65535);
		public static readonly DeepColor Khaki = new DeepColor((ushort)61680, (ushort)59110, (ushort)35980, (ushort)65535);
		public static readonly DeepColor Lavender = new DeepColor((ushort)59110, (ushort)59110, (ushort)64250, (ushort)65535);
		public static readonly DeepColor LavenderBlush = new DeepColor((ushort)65535, (ushort)61680, (ushort)62965, (ushort)65535);
		public static readonly DeepColor LawnGreen = new DeepColor((ushort)31868, (ushort)64764, (ushort)0, (ushort)65535);
		public static readonly DeepColor LemonChiffon = new DeepColor((ushort)65535, (ushort)64250, (ushort)52685, (ushort)65535);
		public static readonly DeepColor LightBlue = new DeepColor((ushort)44461, (ushort)55512, (ushort)59110, (ushort)65535);
		public static readonly DeepColor LightCoral = new DeepColor((ushort)61680, (ushort)32896, (ushort)32896, (ushort)65535);
		public static readonly DeepColor LightCyan = new DeepColor((ushort)57568, (ushort)65535, (ushort)65535, (ushort)65535);
		public static readonly DeepColor LightGoldenrodYellow = new DeepColor((ushort)64250, (ushort)64250, (ushort)53970, (ushort)65535);
		public static readonly DeepColor LightGray = new DeepColor((ushort)54227, (ushort)54227, (ushort)54227, (ushort)65535);
		public static readonly DeepColor LightGreen = new DeepColor((ushort)37008, (ushort)61166, (ushort)37008, (ushort)65535);
		public static readonly DeepColor LightGrey = new DeepColor((ushort)54227, (ushort)54227, (ushort)54227, (ushort)65535);
		public static readonly DeepColor LightPink = new DeepColor((ushort)65535, (ushort)46774, (ushort)49601, (ushort)65535);
		public static readonly DeepColor LightSalmon = new DeepColor((ushort)65535, (ushort)41120, (ushort)31354, (ushort)65535);
		public static readonly DeepColor LightSeaGreen = new DeepColor((ushort)8224, (ushort)45746, (ushort)43690, (ushort)65535);
		public static readonly DeepColor LightSkyBlue = new DeepColor((ushort)34695, (ushort)52942, (ushort)64250, (ushort)65535);
		public static readonly DeepColor LightSlateGray = new DeepColor((ushort)30583, (ushort)34952, (ushort)39321, (ushort)65535);
		public static readonly DeepColor LightSlateGrey = new DeepColor((ushort)30583, (ushort)34952, (ushort)39321, (ushort)65535);
		public static readonly DeepColor LightSteelBlue = new DeepColor((ushort)45232, (ushort)50372, (ushort)57054, (ushort)65535);
		public static readonly DeepColor LightYellow = new DeepColor((ushort)65535, (ushort)65535, (ushort)57568, (ushort)65535);
		public static readonly DeepColor Lime = new DeepColor((ushort)0, (ushort)65535, (ushort)0, (ushort)65535);
		public static readonly DeepColor LimeGreen = new DeepColor((ushort)12850, (ushort)52685, (ushort)12850, (ushort)65535);
		public static readonly DeepColor Linen = new DeepColor((ushort)64250, (ushort)61680, (ushort)59110, (ushort)65535);
		public static readonly DeepColor Magenta = new DeepColor((ushort)65535, (ushort)0, (ushort)65535, (ushort)65535);
		public static readonly DeepColor Maroon = new DeepColor((ushort)32896, (ushort)0, (ushort)0, (ushort)65535);
		public static readonly DeepColor MediumAquamarine = new DeepColor((ushort)26214, (ushort)52685, (ushort)43690, (ushort)65535);
		public static readonly DeepColor MediumBlue = new DeepColor((ushort)0, (ushort)0, (ushort)52685, (ushort)65535);
		public static readonly DeepColor MediumOrchid = new DeepColor((ushort)47802, (ushort)21845, (ushort)54227, (ushort)65535);
		public static readonly DeepColor MediumPurple = new DeepColor((ushort)37779, (ushort)28784, (ushort)56283, (ushort)65535);
		public static readonly DeepColor MediumSeaGreen = new DeepColor((ushort)15420, (ushort)46003, (ushort)29041, (ushort)65535);
		public static readonly DeepColor MediumSlateBlue = new DeepColor((ushort)31611, (ushort)26728, (ushort)61166, (ushort)65535);
		public static readonly DeepColor MediumSpringGreen = new DeepColor((ushort)0, (ushort)64250, (ushort)39578, (ushort)65535);
		public static readonly DeepColor MediumTurquoise = new DeepColor((ushort)18504, (ushort)53713, (ushort)52428, (ushort)65535);
		public static readonly DeepColor MediumVioletRed = new DeepColor((ushort)51143, (ushort)5397, (ushort)34181, (ushort)65535);
		public static readonly DeepColor MidnightBlue = new DeepColor((ushort)6425, (ushort)6425, (ushort)28784, (ushort)65535);
		public static readonly DeepColor MintCream = new DeepColor((ushort)62965, (ushort)65535, (ushort)64250, (ushort)65535);
		public static readonly DeepColor MistyRose = new DeepColor((ushort)65535, (ushort)58596, (ushort)57825, (ushort)65535);
		public static readonly DeepColor Moccasin = new DeepColor((ushort)65535, (ushort)58596, (ushort)46517, (ushort)65535);
		public static readonly DeepColor NavajoWhite = new DeepColor((ushort)65535, (ushort)57054, (ushort)44461, (ushort)65535);
		public static readonly DeepColor Navy = new DeepColor((ushort)0, (ushort)0, (ushort)32896, (ushort)65535);
		public static readonly DeepColor OldLace = new DeepColor((ushort)65021, (ushort)62965, (ushort)59110, (ushort)65535);
		public static readonly DeepColor Olive = new DeepColor((ushort)32896, (ushort)32896, (ushort)0, (ushort)65535);
		public static readonly DeepColor OliveDrab = new DeepColor((ushort)27499, (ushort)36494, (ushort)8995, (ushort)65535);
		public static readonly DeepColor Orange = new DeepColor((ushort)65535, (ushort)42405, (ushort)0, (ushort)65535);
		public static readonly DeepColor Orangered = new DeepColor((ushort)65535, (ushort)17733, (ushort)0, (ushort)65535);
		public static readonly DeepColor Orchid = new DeepColor((ushort)56026, (ushort)28784, (ushort)54998, (ushort)65535);
		public static readonly DeepColor PaleGoldenrod = new DeepColor((ushort)61166, (ushort)59624, (ushort)43690, (ushort)65535);
		public static readonly DeepColor PaleGreen = new DeepColor((ushort)39064, (ushort)64507, (ushort)39064, (ushort)65535);
		public static readonly DeepColor PaleTurquoise = new DeepColor((ushort)44975, (ushort)61166, (ushort)61166, (ushort)65535);
		public static readonly DeepColor PaleVioletRed = new DeepColor((ushort)56283, (ushort)28784, (ushort)37779, (ushort)65535);
		public static readonly DeepColor PapayaWhip = new DeepColor((ushort)65535, (ushort)61423, (ushort)54741, (ushort)65535);
		public static readonly DeepColor Peach = new DeepColor((ushort)65535, (ushort)49344, (ushort)32896, (ushort)65535);
		public static readonly DeepColor PeachPuff = new DeepColor((ushort)65535, (ushort)56026, (ushort)47545, (ushort)65535);
		public static readonly DeepColor Peru = new DeepColor((ushort)52685, (ushort)34181, (ushort)16191, (ushort)65535);
		public static readonly DeepColor Pink = new DeepColor((ushort)65535, (ushort)49344, (ushort)52171, (ushort)65535);
		public static readonly DeepColor Plum = new DeepColor((ushort)56797, (ushort)41120, (ushort)56797, (ushort)65535);
		public static readonly DeepColor PowderBlue = new DeepColor((ushort)45232, (ushort)57568, (ushort)59110, (ushort)65535);
		public static readonly DeepColor Purple = new DeepColor((ushort)32896, (ushort)0, (ushort)32896, (ushort)65535);
		public static readonly DeepColor RebeccaPurple = new DeepColor((ushort)26214, (ushort)13107, (ushort)39321, (ushort)65535);
		public static readonly DeepColor Red = new DeepColor((ushort)65535, (ushort)0, (ushort)0, (ushort)65535);
		public static readonly DeepColor RosyBrown = new DeepColor((ushort)48316, (ushort)36751, (ushort)36751, (ushort)65535);
		public static readonly DeepColor RoyalBlue = new DeepColor((ushort)16705, (ushort)26985, (ushort)57825, (ushort)65535);
		public static readonly DeepColor SaddleBrown = new DeepColor((ushort)35723, (ushort)17733, (ushort)4883, (ushort)65535);
		public static readonly DeepColor Salmon = new DeepColor((ushort)64250, (ushort)32896, (ushort)29298, (ushort)65535);
		public static readonly DeepColor SandyBrown = new DeepColor((ushort)62708, (ushort)42148, (ushort)24672, (ushort)65535);
		public static readonly DeepColor SeaGreen = new DeepColor((ushort)11822, (ushort)35723, (ushort)22359, (ushort)65535);
		public static readonly DeepColor Seashell = new DeepColor((ushort)65535, (ushort)62965, (ushort)61166, (ushort)65535);
		public static readonly DeepColor Sienna = new DeepColor((ushort)41120, (ushort)21074, (ushort)11565, (ushort)65535);
		public static readonly DeepColor Silver = new DeepColor((ushort)49344, (ushort)49344, (ushort)49344, (ushort)65535);
		public static readonly DeepColor SkyBlue = new DeepColor((ushort)34695, (ushort)52942, (ushort)60395, (ushort)65535);
		public static readonly DeepColor SlateBlue = new DeepColor((ushort)27242, (ushort)23130, (ushort)52685, (ushort)65535);
		public static readonly DeepColor SlateGray = new DeepColor((ushort)28784, (ushort)32896, (ushort)37008, (ushort)65535);
		public static readonly DeepColor Snow = new DeepColor((ushort)65535, (ushort)64250, (ushort)64250, (ushort)65535);
		public static readonly DeepColor SpringGreen = new DeepColor((ushort)0, (ushort)65535, (ushort)32639, (ushort)65535);
		public static readonly DeepColor SteelBlue = new DeepColor((ushort)17990, (ushort)33410, (ushort)46260, (ushort)65535);
		public static readonly DeepColor Tan = new DeepColor((ushort)53970, (ushort)46260, (ushort)35980, (ushort)65535);
		public static readonly DeepColor Tea = new DeepColor((ushort)0, (ushort)32896, (ushort)32896, (ushort)65535);
		public static readonly DeepColor Thistle = new DeepColor((ushort)55512, (ushort)49087, (ushort)55512, (ushort)65535);
		public static readonly DeepColor Tomato = new DeepColor((ushort)65535, (ushort)25443, (ushort)18247, (ushort)65535);
		public static readonly DeepColor Turquoise = new DeepColor((ushort)16448, (ushort)57568, (ushort)53456, (ushort)65535);
		public static readonly DeepColor Violet = new DeepColor((ushort)61166, (ushort)33410, (ushort)61166, (ushort)65535);
		public static readonly DeepColor Wheat = new DeepColor((ushort)62965, (ushort)57054, (ushort)46003, (ushort)65535);
		public static readonly DeepColor White = new DeepColor((ushort)65535, (ushort)65535, (ushort)65535, (ushort)65535);
		public static readonly DeepColor WhiteSmoke = new DeepColor((ushort)62965, (ushort)62965, (ushort)62965, (ushort)65535);
		public static readonly DeepColor Yellow = new DeepColor((ushort)65535, (ushort)65535, (ushort)0, (ushort)65535);
		public static readonly DeepColor YellowGreen = new DeepColor((ushort)39578, (ushort)52685, (ushort)12850, (ushort)65535);

		#endregion

		#region DeepColor names

		private static readonly (string Name, DeepColor DeepColor)[] _colorList = new (string Name, DeepColor DeepColor)[]
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
			("seashell", Seashell),
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

		static DeepColor()
		{
			Dictionary<string, DeepColor> colorsByName = new Dictionary<string, DeepColor>();
			foreach ((string Name, DeepColor DeepColor) pair in _colorList)
			{
				colorsByName.Add(pair.Name, pair.DeepColor);
			}
			ColorsByName = colorsByName;

			Dictionary<DeepColor, string> namesByColor = new Dictionary<DeepColor, string>();
			foreach ((string Name, DeepColor DeepColor) pair in _colorList)
			{
				if (!namesByColor.ContainsKey(pair.DeepColor))
					namesByColor.Add(pair.DeepColor, pair.Name);
			}
			NamesByColor = namesByColor;
		}

		/// <summary>
		/// Retrieve the full list of defined colors, in their definition order.
		/// </summary>
		public static IReadOnlyList<(string Name, DeepColor DeepColor)> ColorList => _colorList;

		/// <summary>
		/// A dictionary that maps color values to their matching names, in lowercase.
		/// </summary>
		public static IReadOnlyDictionary<DeepColor, string> NamesByColor { get; }

		/// <summary>
		/// A dictionary that maps color names to their matching color values.
		/// </summary>
		public static IReadOnlyDictionary<string, DeepColor> ColorsByName { get; }

		#endregion

		#region DeepColor parsing

		/// <summary>
		/// Parse a 64-bit RGBA color (16 bits per channel) in one of several commmon CSS-style formats:
		///    - "#RGB"
		///    - "#RGBA"
		///    - "#RRGGBB"
		///    - "#RRGGBBAA"
		///    - "#RRRRGGGGBBBB"
		///    - "#RRRRGGGGBBBBAAAA"
		///    - "rgb(12323, 4545, 6767)"
		///    - "rgba(12323, 4545, 6767, 0.5)"
		///    - "name"   (standard web color names)
		/// </summary>
		/// <param name="value">The color value to parse.</param>
		/// <param name="color">The resulting actual color.</param>
		/// <exception cref="ArgumentException">Thrown if the color string cannot be parsed in one
		/// of the known formats.</exception>
		public static DeepColor Parse(string value)
		{
			if (!TryParse(value, out DeepColor color))
				throw new ArgumentException($"Invalid color value '{value}'.");
			return color;
		}

		/// <summary>
		/// Attempt to parse a 64-bit RGBA color (16 bits per channel) in one of several commmon CSS-style formats:
		///    - "#RGB"
		///    - "#RGBA"
		///    - "#RRGGBB"
		///    - "#RRGGBBAA"
		///    - "#RRRRGGGGBBBB"
		///    - "#RRRRGGGGBBBBAAAA"
		///    - "rgb(12323, 4545, 6767)"
		///    - "rgba(12323, 4545, 6767, 0.5)"
		///    - "name"   (standard web color names)
		/// </summary>
		/// <param name="value">The color value to parse.</param>
		/// <param name="color">The resulting actual color; if the string cannot be parsed,
		/// this will be set to DeepColor.Transparent.</param>
		/// <returns>True if the color could be parsed, false if it could not.</returns>
		public static bool TryParse(string value, out DeepColor color)
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

			if (ColorsByName.TryGetValue(new string(lowerName), out DeepColor c))
			{
				color = c;
				return true;
			}

			color = Transparent;
			return false;
		}

		public static bool TryParseHexColor(ReadOnlySpan<char> hex, out DeepColor color)
		{
			switch (hex.Length)
			{
				case 1:
					int n = ParseHexDigit(hex[0]);
					n = (n << 4) | n;
					color = new DeepColor(n, n, n, 255);
					return true;

				case 2:
					n = ParseHexPair(hex[0], hex[1]);
					color = new DeepColor(n, n, n, 255);
					return true;

				case 3:
					int r = ParseHexDigit(hex[0]);
					int g = ParseHexDigit(hex[1]);
					int b = ParseHexDigit(hex[2]);
					r = (r << 4) | r;
					g = (g << 4) | g;
					b = (b << 4) | b;
					color = new DeepColor(r, g, b, 255);
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
					color = new DeepColor(r, g, b, a);
					return true;

				case 6:
					r = ParseHexPair(hex[0], hex[1]);
					g = ParseHexPair(hex[2], hex[3]);
					b = ParseHexPair(hex[4], hex[5]);
					color = new DeepColor(r, g, b, 255);
					return true;

				case 8:
					r = ParseHexPair(hex[0], hex[1]);
					g = ParseHexPair(hex[2], hex[3]);
					b = ParseHexPair(hex[4], hex[5]);
					a = ParseHexPair(hex[6], hex[7]);
					color = new DeepColor(r, g, b, a);
					return true;

				case 12:
					r = ParseHexFour(hex[0], hex[1], hex[2], hex[3]);
					g = ParseHexFour(hex[4], hex[5], hex[6], hex[7]);
					b = ParseHexFour(hex[8], hex[9], hex[10], hex[11]);
					color = new DeepColor(r, g, b, 65535);
					return true;

				case 16:
					r = ParseHexFour(hex[0], hex[1], hex[2], hex[3]);
					g = ParseHexFour(hex[4], hex[5], hex[6], hex[7]);
					b = ParseHexFour(hex[8], hex[9], hex[10], hex[11]);
					a = ParseHexFour(hex[12], hex[13], hex[14], hex[15]);
					color = new DeepColor(r, g, b, a);
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
		private static int ParseHexFour(char v1, char v2, char v3, char v4)
			=> (ParseHexDigit(v1) << 12) | (ParseHexDigit(v2) << 8) | (ParseHexDigit(v3) << 4) | ParseHexDigit(v4);

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

		private static bool TryLexRgba(ReadOnlySpan<char> input, out DeepColor color)
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

			int alpha = 65535;
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
			color = new DeepColor(red, green, blue, alpha);
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

			// We now have a value from 0 to 0x1000000 instead of the desired value of 0 to 65535.
			// So use multiplication and shifting to turn it into the range of 0 to 65535.
			alpha = (int)(((long)(alpha >> 1) * 65535) >> 23);

			// For 0.xxx, we use the fractional value, but anything bigger is clamped to 1.
			value = prefix != 0 ? 65535 : alpha;
			return ptr > start;
		}

		#endregion
	}
}
