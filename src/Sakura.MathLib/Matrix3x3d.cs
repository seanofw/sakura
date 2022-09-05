using System.Globalization;

namespace Sakura.MathLib
{
	public struct Matrix3x3d : IEquatable<Matrix3x3d>
	{
		// Row-major order, like a mathematician would expect, and not column-major like OpenGL/DirectX.
		public double M11, M12, M13;
		public double M21, M22, M23;
		public double M31, M32, M33;

		// Access by simple index.
		public double this[int index]
		{
			get => index switch
			{
				0 => M11,
				1 => M12,
				2 => M13,
				3 => M21,
				4 => M22,
				5 => M23,
				6 => M31,
				7 => M32,
				8 => M33,
				_ => throw new ArgumentOutOfRangeException(),
			};
			set
			{
				switch (index)
				{
					case 0: M11 = value; break;
					case 1: M12 = value; break;
					case 2: M13 = value; break;
					case 3: M21 = value; break;
					case 4: M22 = value; break;
					case 5: M23 = value; break;
					case 6: M31 = value; break;
					case 7: M32 = value; break;
					case 8: M33 = value; break;
					default: throw new ArgumentOutOfRangeException();
				};
			}
		}

		// Access by row and column (zero-based!).
		public double this[int row, int col]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this[row * 3 + col];

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => this[row * 3 + col] = value;
		}

		public double Determinant
		{
			get => M11 * (M22 * M33 - M23 * M32)
				 + M21 * (M21 * M33 - M23 * M31)
				 + M31 * (M21 * M32 - M22 * M31);
		}

		public bool HasInverse
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Determinant != 0;
		}

		public static Matrix3x3d Identity => new Matrix3x3d(1, 0, 0, 0, 1, 0, 0, 0, 1);

		public Matrix3x3d(double m11, double m12, double m13,
			double m21, double m22, double m23,
			double m31, double m32, double m33)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M21 = m21;
			M22 = m22;
			M23 = m23;
			M31 = m31;
			M32 = m32;
			M33 = m33;
		}

		public Matrix3x3d(Matrix3x2f m, double m31, double m32, double m33)
		{
			M11 = m.M11;
			M12 = m.M12;
			M13 = m.M13;
			M21 = m.M21;
			M22 = m.M22;
			M23 = m.M23;
			M31 = m31;
			M32 = m32;
			M33 = m33;
		}

		public Matrix3x3d(Matrix3x2d m, Vector3d v = default)
		{
			M11 = m.M11;
			M12 = m.M12;
			M13 = m.M13;
			M21 = m.M21;
			M22 = m.M22;
			M23 = m.M23;
			M31 = v.X;
			M32 = v.Y;
			M33 = v.Z;
		}

		public Matrix3x3d(Matrix2x2d m,
			double m13, double m23,					// Last column
			double m31, double m32, double m33		// Last row
			)
		{
			M11 = m.M11;
			M12 = m.M12;
			M13 = m13;
			M21 = m.M21;
			M22 = m.M22;
			M23 = m23;
			M31 = m31;
			M32 = m32;
			M33 = m33;
		}

		public Matrix2x2d TopLeft2x2()
			=> new Matrix2x2d(M11, M12, M21, M22);
		public Matrix2x2d TopRight2x2()
			=> new Matrix2x2d(M12, M13, M22, M23);
		public Matrix2x2d BottomLeft2x2()
			=> new Matrix2x2d(M21, M22, M31, M32);
		public Matrix2x2d BottomRight2x2()
			=> new Matrix2x2d(M22, M23, M32, M33);

		public Matrix3x2d Extract3x2()
			=> new Matrix3x2d(M11, M12, M13, M21, M22, M23);

		public static implicit operator Matrix3x3d(in Matrix3x3f m)
			=> new Matrix3x3d(
				m.M11, m.M12, m.M13,
				m.M21, m.M22, m.M23,
				m.M31, m.M32, m.M33);

		public bool IsIdentity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => M11 == 1 && M12 == 0 && M13 == 0
				&& M21 == 0 && M22 == 1 && M23 == 0
				&& M31 == 0 && M32 == 0 && M33 == 1;
		}

		public static Matrix3x3d operator +(in Matrix3x3d a, in Matrix3x3d b)
			=> new Matrix3x3d(
				a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13,
				a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23,
				a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33);

		public static Matrix3x3d operator -(in Matrix3x3d a, in Matrix3x3d b)
			=> new Matrix3x3d(
				a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
				a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23,
				a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33);

		public static Matrix3x3d operator -(in Matrix3x3d m)
			=> new Matrix3x3d(
				-m.M11, -m.M12, -m.M13,
				-m.M21, -m.M22, -m.M23,
				-m.M31, -m.M32, -m.M33);

		public static Matrix3x3d operator *(in Matrix3x3d m1, in Matrix3x3d m2)
		{
			double a = m1.M11, b = m1.M12, c = m1.M13;
			double d = m1.M21, e = m1.M22, f = m1.M23;
			double g = m1.M31, h = m1.M32, i = m1.M33;

			double j = m2.M11, k = m2.M12, l = m2.M13;
			double m = m2.M21, n = m2.M22, o = m2.M23;
			double p = m2.M31, q = m2.M32, r = m2.M33;

			return new Matrix3x3d(
				a*j + b*m + c*p, a*k + b*n + c*q, a*l + b*o + c*r,
				d*j + e*m + f*p, d*k + e*n + f*q, d*l + e*o + f*r,
				g*j + h*m + i*p, g*k + h*n + i*q, g*l + h*o + i*r
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3d operator *(in Matrix3x3d m, double scalar)
			=> new Matrix3x3d(
				m.M11 * scalar, m.M12 * scalar, m.M13 * scalar,
				m.M21 * scalar, m.M22 * scalar, m.M23 * scalar,
				m.M31 * scalar, m.M32 * scalar, m.M33 * scalar);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3d operator *(double scalar, in Matrix3x3d m)
			=> new Matrix3x3d(
				scalar * m.M11, scalar * m.M12, scalar * m.M13,
				scalar * m.M21, scalar * m.M22, scalar * m.M23,
				scalar * m.M31, scalar * m.M32, scalar * m.M33);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator *(in Matrix3x3d m, Vector3d v)
			=> new Vector3d(
				m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
				m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
				m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z);

		public static Matrix3x3d operator /(in Matrix3x3d m, double scalar)
		{
			double oos = 1.0f / scalar;
			return new Matrix3x3d(
				m.M11 * oos, m.M12 * oos, m.M13 * oos,
				m.M21 * oos, m.M22 * oos, m.M23 * oos,
				m.M31 * oos, m.M32 * oos, m.M33 * oos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3d Translate(double x, double y)
			=> new Matrix3x3d(1, 0, x, 0, 1, y, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3d FlipHorz()
			=> new Matrix3x3d(-1, 0, 0, 0, 1, 0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3d FlipVert()
			=> new Matrix3x3d(1, 0, 0, 0, -1, 0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3d Scale(double x, double y, double z)
			=> new Matrix3x3d(x, 0, 0, 0, y, 0, 0, 0, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3d Shear(double x, double y)
			=> new Matrix3x3d(1, x, 0, y, 1, 0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3d Rotate(double angleInRadians)
		{
			double s = Math.Sin(angleInRadians);
			double c = Math.Cos(angleInRadians);
			return new Matrix3x3d(c, -s, 0, s, c, 0, 0, 0, 1);
		}

		public Matrix3x3d Invert()
		{
			double det = Determinant;
			double inv = det != 0 ? 1.0 / det
				: throw new InvalidOperationException("This Matrix3x3d is not invertible.");

			// Find the determinants of the minor matrices.
			double d11 = M22 * M33 - M23 * M32;
			double d12 = M21 * M33 - M23 * M31;
			double d13 = M21 * M32 - M22 * M31;

			double d21 = M12 * M33 - M13 * M32;
			double d22 = M11 * M33 - M13 * M31;
			double d23 = M11 * M32 - M12 * M31;

			double d31 = M12 * M23 - M13 * M22;
			double d32 = M11 * M23 - M13 * M21;
			double d33 = M11 * M22 - M12 * M21;

			// Construct the inverse from the determinant multiplied by the adjugate matrix.
			return new Matrix3x3d(
				 d11 * inv, -d21 * inv,  d31 * inv,
				-d12 * inv,  d22 * inv, -d32 * inv,
				 d13 * inv, -d23 * inv,  d33 * inv
			);
		}

		public Matrix3x3d Transpose()
			=> new Matrix3x3d(
				M11, M21, M31,
				M12, M22, M32,
				M13, M23, M33);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object? obj)
			=> obj is Matrix3x3d other && Equals(other);

		bool IEquatable<Matrix3x3d>.Equals(Matrix3x3d other)
			=> M11 == other.M11 && M12 == other.M12 && M13 == other.M13
			&& M21 == other.M21 && M22 == other.M22 && M23 == other.M23
			&& M31 == other.M31 && M32 == other.M32 && M33 == other.M33;

		public bool Equals(in Matrix3x3d other)
			=> M11 == other.M11 && M12 == other.M12 && M13 == other.M13
			&& M21 == other.M21 && M22 == other.M22 && M23 == other.M23
			&& M31 == other.M31 && M32 == other.M32 && M33 == other.M33;

		public bool NearlyEquals(in Matrix3x3d other, double epsilon = 0.00001)
			=> Math.Abs(M11 - other.M11) < epsilon
				&& Math.Abs(M12 - other.M12) < epsilon
				&& Math.Abs(M13 - other.M13) < epsilon
				&& Math.Abs(M21 - other.M21) < epsilon
				&& Math.Abs(M22 - other.M22) < epsilon
				&& Math.Abs(M23 - other.M23) < epsilon
				&& Math.Abs(M31 - other.M31) < epsilon
				&& Math.Abs(M32 - other.M32) < epsilon
				&& Math.Abs(M33 - other.M33) < epsilon;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in Matrix3x3d a, in Matrix3x3d b)
			=> a.Equals(b);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in Matrix3x3d a, in Matrix3x3d b)
			=> !a.Equals(b);

		public override int GetHashCode()
		{
			int hashCode =                      M11.GetHashCode();
			    hashCode = (hashCode * 65599) + M12.GetHashCode();
			    hashCode = (hashCode * 65599) + M13.GetHashCode();
			    hashCode = (hashCode * 65599) + M21.GetHashCode();
			    hashCode = (hashCode * 65599) + M22.GetHashCode();
			    hashCode = (hashCode * 65599) + M23.GetHashCode();
			    hashCode = (hashCode * 65599) + M31.GetHashCode();
			    hashCode = (hashCode * 65599) + M32.GetHashCode();
			    hashCode = (hashCode * 65599) + M33.GetHashCode();
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
			stringBuilder.Append('\n');
			stringBuilder.Append(' ');
			stringBuilder.Append(M31.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M32.ToString(format, formatProvider));
			stringBuilder.Append(separator);
			stringBuilder.Append(' ');
			stringBuilder.Append(M33.ToString(format, formatProvider));
			stringBuilder.Append(']');

			return stringBuilder.ToString();
		}

		#endregion
	}
}
