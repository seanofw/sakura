using System.Globalization;

namespace Sakura.MathLib
{
	/// <summary>
	/// A Frac32 represents a number in [0, +1] (inclusive!), with a precision of 31 bits
	/// after the decimal place (better than float, worse than double).  It uses all-integer
	/// math, so it's fast, and unlike float, it *always* has exactly 31 bits of precision
	/// after the decimal place --- it doesn't lose precision just because the scale changes.
	/// 
	/// Any operations on a frac that exceed the range of [0, +1] will be clamped to [0, +1].
	/// 
	/// In effect, this gives about 9 decimal digits of precision after the decimal point
	/// compared to about 12 for a double and 6 for a float, but it does so in the same space
	/// as a float.
	/// </summary>
	public struct Frac32 : IEquatable<Frac32>
	{
		private const int Bits = 31;
		private const uint InternalMax = (1U << Bits);

		private uint _v;

		public double Value => (double)_v / InternalMax;

		public static Frac32 Zero => new Frac32(0, false);
		public static Frac32 One => new Frac32(InternalMax, false);
		public static Frac32 Half => new Frac32(InternalMax >> 1, false);

		/// <summary>
		/// Inverse = 1.0 - this.
		/// </summary>
		public Frac32 Inv => new Frac32(InternalMax - _v, false);

		/// <summary>
		/// Direct access to the raw bits, primarily for serialization.
		/// </summary>
		public uint RawBits
		{
			get => _v;
			set => _v = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public Frac32(double value)
		{
			if (value > 1)
				_v = InternalMax;
			else if (value < 0)
				_v = 0;
			else
				_v = (uint)((long)(value * (InternalMax * 2UL)) >> 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public Frac32(int value)
			=> _v = (uint)((value >> 31) & 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Frac32(uint v, bool _)
			=> _v = v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint ClampMax(long v)
			=> (uint)Math.Min(v, InternalMax);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint ClampMin(long v)
			=> (uint)Math.Max(v, 0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac32 operator +(Frac32 a, Frac32 b)
			=> new Frac32(ClampMax(a._v + (long)b._v), false);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac32 operator -(Frac32 a, Frac32 b)
			=> new Frac32(ClampMin(a._v - (long)b._v), false);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac32 operator *(Frac32 a, Frac32 b)
			=> new Frac32((uint)(((long)a._v * b._v) >> Bits), false);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac32 operator /(Frac32 a, Frac32 b)
			=> b._v > 0 ? new Frac32((uint)(((long)a._v << Bits) / b._v), false) : Zero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac32 operator *(Frac32 a, int b)
			=> new Frac32(ClampMax((long)a._v * b), false);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac32 operator /(Frac32 a, int b)
			=> b > 0 ? new Frac32(a._v / (uint)b, false) : Zero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Frac32 operator -(Frac32 f)	// Negative for a frac always clamps to zero.
			=> Zero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Frac32 f)
			=> _v == f._v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object? obj)
			=> obj is Frac32 f && _v == f._v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
			=> (int)_v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Frac32 a, Frac32 b)
			=> a._v == b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Frac32 a, Frac32 b)
			=> a._v != b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(Frac32 a, Frac32 b)
			=> a._v < b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(Frac32 a, Frac32 b)
			=> a._v > b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(Frac32 a, Frac32 b)
			=> a._v <= b._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(Frac32 a, Frac32 b)
			=> a._v >= b._v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Frac32 f, int i)
			=> f._v == ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Frac32 f, int i)
			=> f._v != ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(Frac32 f, int i)
			=> f._v < ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(Frac32 f, int i)
			=> f._v > ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(Frac32 f, int i)
			=> f._v <= ((long)i << Bits);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(Frac32 f, int i)
			=> f._v >= ((long)i << Bits);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(int i, Frac32 f)
			=> ((long)i << Bits) == f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(int i, Frac32 f)
			=> ((long)i << Bits) != f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(int i, Frac32 f)
			=> ((long)i << Bits) < f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(int i, Frac32 f)
			=> ((long)i << Bits) > f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(int i, Frac32 f)
			=> ((long)i << Bits) <= f._v;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(int i, Frac32 f)
			=> ((long)i << Bits) >= f._v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double(Frac32 f)
			=> f.Value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Frac32(double d)
			=> new Frac32(d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float(Frac32 f)
			=> (float)f.Value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Frac32(float f)
			=> new Frac32(f);

		public Frac32 Sqrt()
		{
			// Fast square root using Jim Ulery's method, unrolled.
			ulong n = (ulong)_v << 31;
			ulong g = 0x80000000;

			if (g * g > n) g ^= 0x80000000; g |= 0x40000000;
			if (g * g > n) g ^= 0x40000000; g |= 0x20000000;
			if (g * g > n) g ^= 0x20000000; g |= 0x10000000;
			if (g * g > n) g ^= 0x10000000; g |= 0x08000000;
			if (g * g > n) g ^= 0x08000000; g |= 0x04000000;
			if (g * g > n) g ^= 0x04000000; g |= 0x02000000;
			if (g * g > n) g ^= 0x02000000; g |= 0x01000000;
			if (g * g > n) g ^= 0x01000000; g |= 0x00800000;
			if (g * g > n) g ^= 0x00800000; g |= 0x00400000;
			if (g * g > n) g ^= 0x00400000; g |= 0x00200000;
			if (g * g > n) g ^= 0x00200000; g |= 0x00100000;
			if (g * g > n) g ^= 0x00100000; g |= 0x00080000;
			if (g * g > n) g ^= 0x00080000; g |= 0x00040000;
			if (g * g > n) g ^= 0x00040000; g |= 0x00020000;
			if (g * g > n) g ^= 0x00020000; g |= 0x00010000;
			if (g * g > n) g ^= 0x00010000; g |= 0x00008000;
			if (g * g > n) g ^= 0x00008000; g |= 0x00004000;
			if (g * g > n) g ^= 0x00004000; g |= 0x00002000;
			if (g * g > n) g ^= 0x00002000; g |= 0x00001000;
			if (g * g > n) g ^= 0x00001000; g |= 0x00000800;
			if (g * g > n) g ^= 0x00000800; g |= 0x00000400;
			if (g * g > n) g ^= 0x00000400; g |= 0x00000200;
			if (g * g > n) g ^= 0x00000200; g |= 0x00000100;
			if (g * g > n) g ^= 0x00000100; g |= 0x00000080;
			if (g * g > n) g ^= 0x00000080; g |= 0x00000040;
			if (g * g > n) g ^= 0x00000040; g |= 0x00000020;
			if (g * g > n) g ^= 0x00000020; g |= 0x00000010;
			if (g * g > n) g ^= 0x00000010; g |= 0x00000008;
			if (g * g > n) g ^= 0x00000008; g |= 0x00000004;
			if (g * g > n) g ^= 0x00000004; g |= 0x00000002;
			if (g * g > n) g ^= 0x00000002; g |= 0x00000001;
			if (g * g > n) g ^= 0x00000001;

			return new Frac32((uint)g, false);
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
