using System.Globalization;

namespace Sakura.MathLib
{
	public struct Matrix3x3f : IEquatable<Matrix3x3f>
	{
		// Row-major order, like a mathematician would expect, and not column-major like OpenGL/DirectX.
		public float M11, M12, M13;
		public float M21, M22, M23;
		public float M31, M32, M33;

		// Access by simple index.
		public float this[int index]
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
		public float this[int row, int col]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this[row * 3 + col];

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => this[row * 3 + col] = value;
		}

		public float Determinant
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

		public static Matrix3x3f Identity => new Matrix3x3f(1, 0, 0, 0, 1, 0, 0, 0, 1);

		public Matrix3x3f(float m11, float m12, float m13,
			float m21, float m22, float m23,
			float m31, float m32, float m33)
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

		public Matrix3x3f(Matrix3x2f m, float m31, float m32, float m33)
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

		public Matrix3x3f(Matrix3x2f m, Vector3f v = default)
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

		public Matrix3x3f(Matrix2x2f m,
			float m13, float m23,				// Last column
			float m31, float m32, float m33		// Last row
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

		public Matrix2x2f TopLeft2x2()
			=> new Matrix2x2f(M11, M12, M21, M22);
		public Matrix2x2f TopRight2x2()
			=> new Matrix2x2f(M12, M13, M22, M23);
		public Matrix2x2f BottomLeft2x2()
			=> new Matrix2x2f(M21, M22, M31, M32);
		public Matrix2x2f BottomRight2x2()
			=> new Matrix2x2f(M22, M23, M32, M33);

		public Matrix3x2f Extract3x2()
			=> new Matrix3x2f(M11, M12, M13, M21, M22, M23);

		public static explicit operator Matrix3x3f(in Matrix3x3d m)
			=> new Matrix3x3f(
				(float)m.M11, (float)m.M12, (float)m.M13,
				(float)m.M21, (float)m.M22, (float)m.M23,
				(float)m.M31, (float)m.M32, (float)m.M33);

		public bool IsIdentity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => M11 == 1 && M12 == 0 && M13 == 0
				&& M21 == 0 && M22 == 1 && M23 == 0
				&& M31 == 0 && M32 == 0 && M33 == 1;
		}

		public static Matrix3x3f operator +(in Matrix3x3f a, in Matrix3x3f b)
			=> new Matrix3x3f(
				a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13,
				a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23,
				a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33);

		public static Matrix3x3f operator -(in Matrix3x3f a, in Matrix3x3f b)
			=> new Matrix3x3f(
				a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
				a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23,
				a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33);

		public static Matrix3x3f operator -(in Matrix3x3f m)
			=> new Matrix3x3f(
				-m.M11, -m.M12, -m.M13,
				-m.M21, -m.M22, -m.M23,
				-m.M31, -m.M32, -m.M33);

		public static Matrix3x3f operator *(in Matrix3x3f m1, in Matrix3x3f m2)
		{
			float a = m1.M11, b = m1.M12, c = m1.M13;
			float d = m1.M21, e = m1.M22, f = m1.M23;
			float g = m1.M31, h = m1.M32, i = m1.M33;

			float j = m2.M11, k = m2.M12, l = m2.M13;
			float m = m2.M21, n = m2.M22, o = m2.M23;
			float p = m2.M31, q = m2.M32, r = m2.M33;

			return new Matrix3x3f(
				a*j + b*m + c*p, a*k + b*n + c*q, a*l + b*o + c*r,
				d*j + e*m + f*p, d*k + e*n + f*q, d*l + e*o + f*r,
				g*j + h*m + i*p, g*k + h*n + i*q, g*l + h*o + i*r
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3f operator *(in Matrix3x3f m, float scalar)
			=> new Matrix3x3f(
				m.M11 * scalar, m.M12 * scalar, m.M13 * scalar,
				m.M21 * scalar, m.M22 * scalar, m.M23 * scalar,
				m.M31 * scalar, m.M32 * scalar, m.M33 * scalar);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3f operator *(float scalar, in Matrix3x3f m)
			=> new Matrix3x3f(
				scalar * m.M11, scalar * m.M12, scalar * m.M13,
				scalar * m.M21, scalar * m.M22, scalar * m.M23,
				scalar * m.M31, scalar * m.M32, scalar * m.M33);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3f operator *(in Matrix3x3f m, Vector3f v)
			=> new Vector3f(
				m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
				m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
				m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2f operator *(in Matrix3x3f m, Vector2f v)
			=> new Vector2f(
				m.M11 * v.X + m.M12 * v.Y + m.M13,
				m.M21 * v.X + m.M22 * v.Y + m.M23);

		public static Matrix3x3f operator /(in Matrix3x3f m, float scalar)
		{
			float oos = 1.0f / scalar;
			return new Matrix3x3f(
				m.M11 * oos, m.M12 * oos, m.M13 * oos,
				m.M21 * oos, m.M22 * oos, m.M23 * oos,
				m.M31 * oos, m.M32 * oos, m.M33 * oos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3f Translate(float x, float y)
			=> new Matrix3x3f(1, 0, x, 0, 1, y, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3f FlipHorz()
			=> new Matrix3x3f(-1, 0, 0, 0, 1, 0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3f FlipVert()
			=> new Matrix3x3f(1, 0, 0, 0, -1, 0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3f Scale(float x, float y, float z)
			=> new Matrix3x3f(x, 0, 0, 0, y, 0, 0, 0, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3f Shear(float x, float y)
			=> new Matrix3x3f(1, x, 0, y, 1, 0, 0, 0, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x3f Rotate(float angleInRadians)
		{
			float s = MathF.Sin(angleInRadians);
			float c = MathF.Cos(angleInRadians);
			return new Matrix3x3f(c, -s, 0, s, c, 0, 0, 0, 1);
		}

		public Matrix3x3f Invert()
		{
			float det = Determinant;
			float inv = det != 0 ? 1.0f / det
				: throw new InvalidOperationException("This Matrix3x3f is not invertible.");

			// Find the determinants of the minor matrices.
			float d11 = M22 * M33 - M23 * M32;
			float d12 = M21 * M33 - M23 * M31;
			float d13 = M21 * M32 - M22 * M31;

			float d21 = M12 * M33 - M13 * M32;
			float d22 = M11 * M33 - M13 * M31;
			float d23 = M11 * M32 - M12 * M31;

			float d31 = M12 * M23 - M13 * M22;
			float d32 = M11 * M23 - M13 * M21;
			float d33 = M11 * M22 - M12 * M21;

			// Construct the inverse from the determinant multiplied by the adjugate matrix.
			return new Matrix3x3f(
				 d11 * inv, -d21 * inv,  d31 * inv,
				-d12 * inv,  d22 * inv, -d32 * inv,
				 d13 * inv, -d23 * inv,  d33 * inv
			);
		}

		public Matrix3x3f Transpose()
			=> new Matrix3x3f(
				M11, M21, M31,
				M12, M22, M32,
				M13, M23, M33);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object? obj)
			=> obj is Matrix3x3f other && Equals(other);

		bool IEquatable<Matrix3x3f>.Equals(Matrix3x3f other)
			=> M11 == other.M11 && M12 == other.M12 && M13 == other.M13
			&& M21 == other.M21 && M22 == other.M22 && M23 == other.M23
			&& M31 == other.M31 && M32 == other.M32 && M33 == other.M33;

		public bool Equals(in Matrix3x3f other)
			=> M11 == other.M11 && M12 == other.M12 && M13 == other.M13
			&& M21 == other.M21 && M22 == other.M22 && M23 == other.M23
			&& M31 == other.M31 && M32 == other.M32 && M33 == other.M33;

		public bool NearlyEquals(in Matrix3x3f other, float epsilon = 0.00001f)
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
		public static bool operator ==(in Matrix3x3f a, in Matrix3x3f b)
			=> a.Equals(b);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in Matrix3x3f a, in Matrix3x3f b)
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
