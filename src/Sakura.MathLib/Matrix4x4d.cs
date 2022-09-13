using System.Globalization;

namespace Sakura.MathLib
{
	public struct Matrix4x4d : IEquatable<Matrix4x4d>
	{
		// Row-major order, like a mathematician would expect, and not column-major like OpenGL/DirectX.
		public double M11, M12, M13, M14;
		public double M21, M22, M23, M24;
		public double M31, M32, M33, M34;
		public double M41, M42, M43, M44;

		// Access by simple index.
		public double this[int index]
		{
			get => index switch
			{
				0 => M11,
				1 => M12,
				2 => M13,
				3 => M14,
				4 => M21,
				5 => M22,
				6 => M23,
				7 => M24,
				8 => M31,
				9 => M32,
				10 => M33,
				11 => M34,
				12 => M41,
				13 => M42,
				14 => M43,
				15 => M44,
				_ => throw new ArgumentOutOfRangeException(),
			};
			set
			{
				switch (index)
				{
					case 0: M11 = value; break;
					case 1: M12 = value; break;
					case 2: M13 = value; break;
					case 3: M14 = value; break;
					case 4: M21 = value; break;
					case 5: M22 = value; break;
					case 6: M23 = value; break;
					case 7: M24 = value; break;
					case 8: M31 = value; break;
					case 9: M32 = value; break;
					case 10: M33 = value; break;
					case 11: M34 = value; break;
					case 12: M41 = value; break;
					case 13: M42 = value; break;
					case 14: M43 = value; break;
					case 15: M44 = value; break;
					default: throw new ArgumentOutOfRangeException();
				};
			}
		}

		// Access by row and column (zero-based!).
		public double this[int row, int col]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this[row * 4 + col];

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => this[row * 4 + col] = value;
		}

		public double Determinant
			=>    (M11 * M22 * M33 * M44) - (M11 * M22 * M34 * M43) + (M11 * M23 * M34 * M42) - (M11 * M23 * M32 * M44) + (M11 * M24 * M32 * M43) - (M11 * M24 * M33 * M42)
				- (M12 * M23 * M34 * M41) + (M12 * M23 * M31 * M44) - (M12 * M24 * M31 * M43) + (M12 * M24 * M33 * M41) - (M12 * M21 * M33 * M44) + (M12 * M21 * M34 * M43)
				+ (M13 * M24 * M31 * M42) - (M13 * M24 * M32 * M41) + (M13 * M21 * M32 * M44) - (M13 * M21 * M34 * M42) + (M13 * M22 * M34 * M41) - (M13 * M22 * M31 * M44)
				- (M14 * M21 * M32 * M43) + (M14 * M21 * M33 * M42) - (M14 * M22 * M33 * M41) + (M14 * M22 * M31 * M43) - (M14 * M23 * M31 * M42) + (M14 * M23 * M32 * M41);

		public bool HasInverse
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Determinant != 0;
		}

		public static Matrix4x4d Identity => new Matrix4x4d(1, 0, 0, 0,  0, 1, 0, 0,  0, 0, 1, 0,  0, 0, 0, 1);

		public Matrix4x4d(double m11, double m12, double m13, double m14,
			double m21, double m22, double m23, double m24,
			double m31, double m32, double m33, double m34,
			double m41, double m42, double m43, double m44)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M14 = m14;
			M21 = m21;
			M22 = m22;
			M23 = m23;
			M24 = m24;
			M31 = m31;
			M32 = m32;
			M33 = m33;
			M34 = m34;
			M41 = m41;
			M42 = m42;
			M43 = m43;
			M44 = m44;
		}

		public Matrix4x4d(Matrix3x3d m,
			double m14, double m24, double m34,              // Last column
			double m41, double m42, double m43, double m44)  // Last row
		{
			M11 = m.M11;
			M12 = m.M12;
			M13 = m.M13;
			M14 = m14;
			M21 = m.M21;
			M22 = m.M22;
			M23 = m.M23;
			M24 = m24;
			M31 = m.M31;
			M32 = m.M32;
			M33 = m.M33;
			M34 = m34;
			M41 = m41;
			M42 = m42;
			M43 = m43;
			M44 = m44;
		}

		public Matrix3x3d TopLeft3x3()
			=> new Matrix3x3d(M11, M12, M13, M21, M22, M23, M31, M32, M33);
		public Matrix3x3d TopRight3x3()
			=> new Matrix3x3d(M12, M13, M14, M22, M23, M24, M31, M32, M34);
		public Matrix3x3d BottomLeft3x3()
			=> new Matrix3x3d(M21, M22, M23, M31, M32, M33, M41, M42, M43);
		public Matrix3x3d BottomRight3x3()
			=> new Matrix3x3d(M22, M23, M24, M32, M33, M34, M41, M42, M44);

		public static implicit operator Matrix4x4d(in Matrix4x4f m)
			=> new Matrix4x4d(
				m.M11, m.M12, m.M13, m.M14,
				m.M21, m.M22, m.M23, m.M24,
				m.M31, m.M32, m.M33, m.M34,
				m.M41, m.M42, m.M43, m.M44);

		public bool IsIdentity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => M11 == 1 && M22 == 1 && M33 == 1 && M44 == 1		// Main diagonal first.
				&& M12 == 0 && M13 == 0 && M14 == 0
				&& M21 == 0 && M23 == 0 && M24 == 0
				&& M31 == 0 && M32 == 0 && M34 == 0
				&& M41 == 0 && M42 == 0 && M43 == 0;
		}

		public static Matrix4x4d operator +(in Matrix4x4d a, in Matrix4x4d b)
			=> new Matrix4x4d(
				a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13, a.M14 + b.M14,
				a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23, a.M24 + b.M24,
				a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33, a.M34 + b.M34,
				a.M41 + b.M41, a.M42 + b.M42, a.M43 + b.M43, a.M44 + b.M44);

		public static Matrix4x4d operator -(in Matrix4x4d a, in Matrix4x4d b)
			=> new Matrix4x4d(
				a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13, a.M14 - b.M14,
				a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23, a.M24 - b.M24,
				a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33, a.M34 - b.M34,
				a.M41 - b.M41, a.M42 - b.M42, a.M43 - b.M43, a.M44 - b.M44);

		public static Matrix4x4d operator -(in Matrix4x4d m)
			=> new Matrix4x4d(
				-m.M11, -m.M12, -m.M13, -m.M14,
				-m.M21, -m.M22, -m.M23, -m.M24,
				-m.M31, -m.M32, -m.M33, -m.M34,
				-m.M41, -m.M42, -m.M43, -m.M44);

		public static Matrix4x4d operator *(in Matrix4x4d m1, in Matrix4x4d m2)
		{
			return new Matrix4x4d(
				m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41,
				m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42,
				m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43,
				m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44,

				m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41,
				m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42,
				m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43,
				m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44,

				m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M34 * m2.M41,
				m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M34 * m2.M42,
				m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M34 * m2.M43,
				m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M34 * m2.M44,

				m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41,
				m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42,
				m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43,
				m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix4x4d operator *(in Matrix4x4d m, double scalar)
			=> new Matrix4x4d(
				m.M11 * scalar, m.M12 * scalar, m.M13 * scalar, m.M14 * scalar,
				m.M21 * scalar, m.M22 * scalar, m.M23 * scalar, m.M24 * scalar,
				m.M31 * scalar, m.M32 * scalar, m.M33 * scalar, m.M34 * scalar,
				m.M41 * scalar, m.M42 * scalar, m.M43 * scalar, m.M44 * scalar);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix4x4d operator *(double scalar, in Matrix4x4d m)
			=> new Matrix4x4d(
				scalar * m.M11, scalar * m.M12, scalar * m.M13, scalar * m.M14,
				scalar * m.M21, scalar * m.M22, scalar * m.M23, scalar * m.M24,
				scalar * m.M31, scalar * m.M32, scalar * m.M33, scalar * m.M34,
				scalar * m.M41, scalar * m.M42, scalar * m.M43, scalar * m.M44);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator *(in Matrix4x4d m, Vector4d v)
			=> new Vector4d(
				m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14 * v.W,
				m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24 * v.W,
				m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34 * v.W,
				m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44 * v.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator *(in Matrix4x4d m, Vector3d v)
			=> new Vector3d(
				m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14,
				m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24,
				m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34);

		public static Matrix4x4d operator /(in Matrix4x4d m, double scalar)
		{
			double oos = 1.0f / scalar;
			return new Matrix4x4d(
				m.M11 * oos, m.M12 * oos, m.M13 * oos, m.M14 * oos,
				m.M21 * oos, m.M22 * oos, m.M23 * oos, m.M24 * oos,
				m.M31 * oos, m.M32 * oos, m.M33 * oos, m.M34 * oos,
				m.M41 * oos, m.M42 * oos, m.M43 * oos, m.M44 * oos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix4x4d Translate(double x, double y, double z)
			=> new Matrix4x4d(1, 0, 0, x,  0, 1, 0, y,  0, 0, 1, z,  0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix4x4d FlipX()
			=> new Matrix4x4d(-1, 0, 0, 0,  0, 1, 0, 0,  0, 0, 1, 0,  0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix4x4d FlipY()
			=> new Matrix4x4d(1, 0, 0, 0,  0, -1, 0, 0,  0, 0, 1, 0,  0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix4x4d FlipZ()
			=> new Matrix4x4d(1, 0, 0, 0,  0, 1, 0, 0,  0, 0, -1, 0,  0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix4x4d Scale(double x, double y, double z, double w = 1)
			=> new Matrix4x4d(x, 0, 0, 0,  0, y, 0, 0,  0, 0, z, 0,  0, 0, 0, w);

		public Matrix4x4d Invert()
		{
			// Derived from:
			//   https://github.com/opentk/opentk/blob/master/src/OpenTK.Mathematics/Matrix/Matrix4.cs
			// (MIT License)
			// and previously from:
			//    https://github.com/dotnet/runtime/blob/79ae74f5ca5c8a6fe3a48935e85bd7374959c570/src/libraries/System.Private.CoreLib/src/System/Numerics/Matrix4x4.cs#L1556

			Matrix4x4d result = default;

			double a = M11, b = M21, c = M31, d = M41;
			double e = M12, f = M22, g = M32, h = M42;
			double i = M13, j = M23, k = M33, l = M43;
			double m = M14, n = M24, o = M34, p = M44;

			double kp_lo = k * p - l * o;
			double jp_ln = j * p - l * n;
			double jo_kn = j * o - k * n;
			double ip_lm = i * p - l * m;
			double io_km = i * o - k * m;
			double in_jm = i * n - j * m;

			double a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
			double a12 = -(e * kp_lo - g * ip_lm + h * io_km);
			double a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
			double a14 = -(e * jo_kn - f * io_km + g * in_jm);

			double det = a * a11 + b * a12 + c * a13 + d * a14;

			double inv = det != 0 ? 1.0 / det
				: throw new InvalidOperationException("This Matrix4x4d is not invertible.");

			result.M11 = a11 * inv;
			result.M12 = a12 * inv;
			result.M13 = a13 * inv;
			result.M14 = a14 * inv;

			result.M21 = -(b * kp_lo - c * jp_ln + d * jo_kn) * inv;
			result.M22 = +(a * kp_lo - c * ip_lm + d * io_km) * inv;
			result.M23 = -(a * jp_ln - b * ip_lm + d * in_jm) * inv;
			result.M24 = +(a * jo_kn - b * io_km + c * in_jm) * inv;

			double gp_ho = g * p - h * o;
			double fp_hn = f * p - h * n;
			double fo_gn = f * o - g * n;
			double ep_hm = e * p - h * m;
			double eo_gm = e * o - g * m;
			double en_fm = e * n - f * m;

			result.M31 = +(b * gp_ho - c * fp_hn + d * fo_gn) * inv;
			result.M32 = -(a * gp_ho - c * ep_hm + d * eo_gm) * inv;
			result.M33 = +(a * fp_hn - b * ep_hm + d * en_fm) * inv;
			result.M34 = -(a * fo_gn - b * eo_gm + c * en_fm) * inv;

			double gl_hk = g * l - h * k;
			double fl_hj = f * l - h * j;
			double fk_gj = f * k - g * j;
			double el_hi = e * l - h * i;
			double ek_gi = e * k - g * i;
			double ej_fi = e * j - f * i;

			result.M41 = -(b * gl_hk - c * fl_hj + d * fk_gj) * inv;
			result.M42 = +(a * gl_hk - c * el_hi + d * ek_gi) * inv;
			result.M43 = -(a * fl_hj - b * el_hi + d * ej_fi) * inv;
			result.M44 = +(a * fk_gj - b * ek_gi + c * ej_fi) * inv;

			return result;
		}

		public Matrix4x4d Transpose()
			=> new Matrix4x4d(
				M11, M21, M31, M41,
				M12, M22, M32, M42,
				M13, M23, M33, M43,
				M14, M24, M34, M44);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object? obj)
			=> obj is Matrix4x4d other && Equals(other);

		bool IEquatable<Matrix4x4d>.Equals(Matrix4x4d other)
			=> M11 == other.M11 && M12 == other.M12 && M13 == other.M13 && M14 == other.M14
			&& M21 == other.M21 && M22 == other.M22 && M23 == other.M23 && M24 == other.M24
			&& M31 == other.M31 && M32 == other.M32 && M33 == other.M33 && M34 == other.M34
			&& M41 == other.M41 && M42 == other.M42 && M43 == other.M43 && M44 == other.M44;

		public bool Equals(in Matrix4x4d other)
			=> M11 == other.M11 && M12 == other.M12 && M13 == other.M13 && M14 == other.M14
			&& M21 == other.M21 && M22 == other.M22 && M23 == other.M23 && M24 == other.M24
			&& M31 == other.M31 && M32 == other.M32 && M33 == other.M33 && M34 == other.M34
			&& M41 == other.M41 && M42 == other.M42 && M43 == other.M43 && M44 == other.M44;

		public bool NearlyEquals(in Matrix4x4d other, double epsilon = 0.00001)
			=> Math.Abs(M11 - other.M11) < epsilon
				&& Math.Abs(M12 - other.M12) < epsilon
				&& Math.Abs(M13 - other.M13) < epsilon
				&& Math.Abs(M14 - other.M14) < epsilon
				&& Math.Abs(M21 - other.M21) < epsilon
				&& Math.Abs(M22 - other.M22) < epsilon
				&& Math.Abs(M23 - other.M23) < epsilon
				&& Math.Abs(M24 - other.M24) < epsilon
				&& Math.Abs(M31 - other.M31) < epsilon
				&& Math.Abs(M32 - other.M32) < epsilon
				&& Math.Abs(M33 - other.M33) < epsilon
				&& Math.Abs(M34 - other.M34) < epsilon
				&& Math.Abs(M41 - other.M41) < epsilon
				&& Math.Abs(M42 - other.M42) < epsilon
				&& Math.Abs(M43 - other.M43) < epsilon
				&& Math.Abs(M44 - other.M44) < epsilon;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in Matrix4x4d a, in Matrix4x4d b)
			=> a.Equals(b);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in Matrix4x4d a, in Matrix4x4d b)
			=> !a.Equals(b);

		public override int GetHashCode()
		{
			int hashCode = M11.GetHashCode();
			hashCode = (hashCode * 65599) + M12.GetHashCode();
			hashCode = (hashCode * 65599) + M13.GetHashCode();
			hashCode = (hashCode * 65599) + M14.GetHashCode();
			hashCode = (hashCode * 65599) + M21.GetHashCode();
			hashCode = (hashCode * 65599) + M22.GetHashCode();
			hashCode = (hashCode * 65599) + M23.GetHashCode();
			hashCode = (hashCode * 65599) + M24.GetHashCode();
			hashCode = (hashCode * 65599) + M31.GetHashCode();
			hashCode = (hashCode * 65599) + M32.GetHashCode();
			hashCode = (hashCode * 65599) + M33.GetHashCode();
			hashCode = (hashCode * 65599) + M34.GetHashCode();
			hashCode = (hashCode * 65599) + M41.GetHashCode();
			hashCode = (hashCode * 65599) + M42.GetHashCode();
			hashCode = (hashCode * 65599) + M43.GetHashCode();
			hashCode = (hashCode * 65599) + M44.GetHashCode();
			return hashCode;
		}

		#region Stringification

		public override string ToString()
			=> ToString("G", CultureInfo.CurrentCulture);

		public string ToString(string format)
			=> ToString(format, CultureInfo.CurrentCulture);

		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append('[');
			stringBuilder.Append(M11.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M12.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M13.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M14.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append('\n');
			stringBuilder.Append(' ');
			stringBuilder.Append(M21.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M22.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M23.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M24.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append('\n');
			stringBuilder.Append(' ');
			stringBuilder.Append(M31.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M32.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M33.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M34.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append('\n');
			stringBuilder.Append(' ');
			stringBuilder.Append(M41.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M42.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M43.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M44.ToString(format, formatProvider));
			stringBuilder.Append(']');

			return stringBuilder.ToString();
		}

		#endregion
	}
}
