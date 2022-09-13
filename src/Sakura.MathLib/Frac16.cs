using System.Globalization;

namespace Sakura.MathLib
{
	/// <summary>
	/// A Frac16 represents a number in [0, +1] (inclusive!), with a precision of 15 bits
	/// after the decimal place (better than half, worse than float).  It uses all-integer
	/// math, so it's fast, and unlike float, it *always* has exactly 15 bits of precision
	/// after the decimal place --- it doesn't lose precision just because the scale changes.
	/// 
	/// Any operations on a frac that exceed the range of [0, +1] will be clamped to [0, +1].
	/// 
	/// In effect, this gives about 4.5 decimal digits of precision after the decimal point
	/// compared to about 6 for a float, but it does so in half the space of a float.
	/// </summary>
	public struct Frac16 : IEquatable<Frac16>
	{
		private const int Bits = 15;
		private const ushort InternalMax = (1 << Bits);

		private ushort _v;

		public double Value => (double)_v / InternalMax;

		public static Frac16 Zero => new Frac16(0, false);
		public static Frac16 One => new Frac16(InternalMax, false);
		public static Frac16 Half => new Frac16(InternalMax >> 1, false);

		/// <summary>
		/// Inverse = 1.0 - this.
		/// </summary>
		public Frac16 Inv => new Frac16((ushort)(InternalMax - _v), false);

		/// <summary>
		/// Direct access to the raw bits, primarily for serialization.
		/// </summary>
		public ushort RawBits
		{
			get => _v;
			set => _v = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public Frac16(double value)
		{
			if (value > 1)
				_v = InternalMax;
			else if (value < 0)
				_v = 0;
			else
				_v = (ushort)((int)(value * (InternalMax * 2UL)) >> 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public Frac16(int value)
			=> _v = (ushort)((value >> 31) & 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Frac16(ushort v, bool _)
			=> _v = v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ushort ClampMax(int v)
			=> (ushort)Math.Min(v, InternalMax);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ushort ClampMin(int v)
			=> (ushort)Math.Max(v, 0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac16 operator +(Frac16 a, Frac16 b)
			=> new Frac16(ClampMax(a._v + (int)b._v), false);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac16 operator -(Frac16 a, Frac16 b)
			=> new Frac16(ClampMin(a._v - (int)b._v), false);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac16 operator *(Frac16 a, Frac16 b)
			=> new Frac16((ushort)(((int)a._v * b._v) >> Bits), false);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac16 operator /(Frac16 a, Frac16 b)
			=> b._v > 0 ? new Frac16((ushort)(((int)a._v << Bits) / b._v), false) : Zero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac16 operator *(Frac16 a, int b)
			=> new Frac16(ClampMax((int)a._v * b), false);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac16 operator /(Frac16 a, int b)
			=> b > 0 ? new Frac16((ushort)(a._v / (uint)b), false) : Zero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac16 operator -(Frac16 f)	// Negative for a frac always clamps to zero.
			=> Zero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Frac16 f)
			=> _v == f._v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object? obj)
			=> obj is Frac16 f && _v == f._v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
			=> (int)_v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Frac16 a, Frac16 b)
			=> a._v == b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Frac16 a, Frac16 b)
			=> a._v != b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(Frac16 a, Frac16 b)
			=> a._v < b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(Frac16 a, Frac16 b)
			=> a._v > b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(Frac16 a, Frac16 b)
			=> a._v <= b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(Frac16 a, Frac16 b)
			=> a._v >= b._v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Frac16 f, int i)
			=> f._v == ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Frac16 f, int i)
			=> f._v != ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(Frac16 f, int i)
			=> f._v < ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(Frac16 f, int i)
			=> f._v > ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(Frac16 f, int i)
			=> f._v <= ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(Frac16 f, int i)
			=> f._v >= ((long)i << Bits);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(int i, Frac16 f)
			=> ((long)i << Bits) == f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(int i, Frac16 f)
			=> ((long)i << Bits) != f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(int i, Frac16 f)
			=> ((long)i << Bits) < f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(int i, Frac16 f)
			=> ((long)i << Bits) > f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(int i, Frac16 f)
			=> ((long)i << Bits) <= f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(int i, Frac16 f)
			=> ((long)i << Bits) >= f._v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double(Frac16 f)
			=> f.Value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Frac16(double d)
			=> new Frac16(d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float(Frac16 f)
			=> (float)f.Value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Frac16(float f)
			=> new Frac16(f);

		public Frac16 Sqrt()
		{
			// Fast square root using Jim Ulery's method, unrolled.
			uint n = (uint)_v << 15;
			uint g = 0x8000;

			if (g * g > n) g ^= 0x8000; g |= 0x4000;
			if (g * g > n) g ^= 0x4000; g |= 0x2000;
			if (g * g > n) g ^= 0x2000; g |= 0x1000;
			if (g * g > n) g ^= 0x1000; g |= 0x0800;
			if (g * g > n) g ^= 0x0800; g |= 0x0400;
			if (g * g > n) g ^= 0x0400; g |= 0x0200;
			if (g * g > n) g ^= 0x0200; g |= 0x0100;
			if (g * g > n) g ^= 0x0100; g |= 0x0080;
			if (g * g > n) g ^= 0x0080; g |= 0x0040;
			if (g * g > n) g ^= 0x0040; g |= 0x0020;
			if (g * g > n) g ^= 0x0020; g |= 0x0010;
			if (g * g > n) g ^= 0x0010; g |= 0x0008;
			if (g * g > n) g ^= 0x0008; g |= 0x0004;
			if (g * g > n) g ^= 0x0004; g |= 0x0002;
			if (g * g > n) g ^= 0x0002; g |= 0x0001;
			if (g * g > n) g ^= 0x0001;

			return new Frac16((ushort)g, false);
		}

		/// <summary>
		/// Scale an integer by this fractional value.  This is the same
		/// as '(int)(i * (double)f)', but it's much faster to compute.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Scale(int i)
			=> (int)(((long)i * _v) >> Bits);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
			=> ToString("G", CultureInfo.CurrentCulture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
			=> ToString(format, CultureInfo.CurrentCulture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string? format, IFormatProvider? formatProvider)
			=> Value.ToString(format, formatProvider);
	}
}
