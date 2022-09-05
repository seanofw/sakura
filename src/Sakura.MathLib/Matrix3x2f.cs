using System.Globalization;

namespace Sakura.MathLib
{
	public struct Matrix3x2f : IEquatable<Matrix3x2f>
	{
		// Row-major order, like a mathematician would expect, and not column-major like OpenGL/DirectX.
		public float M11, M12, M13;
		public float M21, M22, M23;

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
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => M11 * M22 - M12 * M21;
		}

		public bool HasInverse
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Determinant != 0;
		}

		public static Matrix3x2f Identity => new Matrix3x2f(1, 0, 0, 0, 1, 0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Matrix3x2f(float m11, float m12, float m13, float m21, float m22, float m23)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M21 = m21;
			M22 = m22;
			M23 = m23;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Matrix3x2f(Matrix2x2f m, float m13, float m23)
		{
			M11 = m.M11;
			M12 = m.M12;
			M13 = m13;
			M21 = m.M21;
			M22 = m.M22;
			M23 = m23;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Matrix3x2f(Matrix2x2f m, Vector2f v = default)
		{
			M11 = m.M11;
			M12 = m.M12;
			M13 = v.X;
			M21 = m.M21;
			M22 = m.M22;
			M23 = v.Y;
		}

		public Matrix2x2f Extract2x2()
			=> new Matrix2x2f(M11, M12, M21, M22);

		public static explicit operator Matrix3x2f(Matrix3x2d m)
			=> new Matrix3x2f(
				(float)m.M11, (float)m.M12, (float)m.M13,
				(float)m.M21, (float)m.M22, (float)m.M23);
		public static implicit operator Matrix3x2(Matrix3x2f m)
			=> new Matrix3x2(
				m.M11, m.M21,
				m.M12, m.M22,
				m.M13, m.M23);
		public static implicit operator Matrix3x2f(Matrix3x2 m)
			=> new Matrix3x2f(
				m.M11, m.M21, m.M31,
				m.M12, m.M22, m.M32);

		public bool IsIdentity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => M11 == 1 && M12 == 0 && M13 == 0 && M21 == 0 && M22 == 1 && M23 == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2f operator +(Matrix3x2f a, Matrix3x2f b)
			=> new Matrix3x2f(
				a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13,
				a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2f operator -(Matrix3x2f a, Matrix3x2f b)
			=> new Matrix3x2f(
				a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
				a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2f operator -(Matrix3x2f m)
			=> new Matrix3x2f(-m.M11, -m.M12, -m.M13, -m.M21, -m.M22, -m.M23);

		public static Matrix3x2f operator *(Matrix3x2f a, Matrix3x2f b)
			=> new Matrix3x2f(
				a.M11 * b.M11 + a.M12 * b.M21,
				a.M11 * b.M12 + a.M12 * b.M22,
				a.M11 * b.M13 + a.M12 * b.M23 + a.M13,

				a.M21 * b.M11 + a.M22 * b.M21,
				a.M21 * b.M12 + a.M22 * b.M22,
				a.M21 * b.M13 + a.M22 * b.M23 + a.M23);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2f operator *(Matrix3x2f m, float scalar)
			=> new Matrix3x2f(
				m.M11 * scalar, m.M12 * scalar, m.M13 * scalar,
				m.M21 * scalar, m.M22 * scalar, m.M23 * scalar);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2f operator *(float scalar, Matrix3x2f m)
			=> new Matrix3x2f(
				scalar * m.M11, scalar * m.M12, scalar * m.M13,
				scalar * m.M21, scalar * m.M22, scalar * m.M23);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2f operator *(Matrix3x2f m, Vector2f v)
			=> new Vector2f(
				m.M11 * v.X + m.M12 * v.Y + m.M13,
				m.M21 * v.X + m.M22 * v.Y + m.M23);

		public static Matrix3x2f operator /(Matrix3x2f m, float scalar)
		{
			float oos = 1.0f / scalar;
			return new Matrix3x2f(
				m.M11 * oos, m.M12 * oos, m.M13 * oos,
				m.M21 * oos, m.M22 * oos, m.M23 * oos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2f Translate(float x, float y)
			=> new Matrix3x2f(1, 0, x, 0, 1, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2f Scale(float x, float y)
			=> new Matrix3x2f(x, 0, 0, 0, y, 0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2f Rotate(float angleInRadians)
		{
			float s = MathF.Sin(angleInRadians);
			float c = MathF.Cos(angleInRadians);
			return new Matrix3x2f(c, -s, 0, s, c, 0);
		}

		public Matrix3x2f Invert()
		{
			float det = Determinant;
			float inv = det != 0 ? 1.0f / det
				: throw new InvalidOperationException("This Matrix3x2f is not invertible.");

			return new Matrix3x2f(
				 M22 * inv, -M12 * inv, (M12 * M23 - M13 * M22) * inv,
				-M21 * inv,  M11 * inv, (M21 * M13 - M11 * M23) * inv
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object? obj)
			=> obj is Matrix3x2f other && Equals(other);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Matrix3x2f other)
			=> M11 == other.M11 && M12 == other.M12 && M13 == other.M13
			&& M21 == other.M21 && M22 == other.M22 && M23 == other.M23;

		public bool NearlyEquals(Matrix3x2f other, float epsilon = 0.00001f)
			=> Math.Abs(M11 - other.M11) < epsilon
				&& Math.Abs(M12 - other.M12) < epsilon
				&& Math.Abs(M13 - other.M13) < epsilon
				&& Math.Abs(M21 - other.M21) < epsilon
				&& Math.Abs(M22 - other.M22) < epsilon
				&& Math.Abs(M23 - other.M23) < epsilon;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Matrix3x2f a, Matrix3x2f b)
			=> a.Equals(b);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Matrix3x2f a, Matrix3x2f b)
			=> !a.Equals(b);

		public override int GetHashCode()
		{
			int hashCode = M11.GetHashCode();
			hashCode = (hashCode * 65599) + M12.GetHashCode();
			hashCode = (hashCode * 65599) + M13.GetHashCode();
			hashCode = (hashCode * 65599) + M21.GetHashCode();
			hashCode = (hashCode * 65599) + M22.GetHashCode();
			hashCode = (hashCode * 65599) + M23.GetHashCode();
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
			stringBuilder.Append(']');

			return stringBuilder.ToString();
		}

		#endregion
	}
}
