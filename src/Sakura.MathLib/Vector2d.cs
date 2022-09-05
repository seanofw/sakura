using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector2d : IEquatable<Vector2d>, IFormattable
    {
		#region Fields

		public double X;
        public double Y;

		#endregion

		#region Properties

		public double Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Sqrt(X * X + Y * Y);
        }

        public double Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y;
        }

		#endregion

		#region Static instances

		public static Vector2d Zero => new Vector2d();
        public static Vector2d One => new Vector2d(1, 1);
        public static Vector2d UnitX => new Vector2d(1, 0);
        public static Vector2d UnitY => new Vector2d(0, 1);

		#endregion

		#region Construction and conversion

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2d(Vector2i v)
            => new Vector2d(v.X, v.Y);
        public static implicit operator Vector2d(Vector2f v)
            => new Vector2d(v.X, v.Y);

        public static implicit operator Vector2d(Vector2 v)
            => new Vector2d(v.X, v.Y);
        public static explicit operator Vector2(Vector2d v)
            => new Vector2((float)v.X, (float)v.Y);

		#endregion

		#region Equality and hash codes

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2d other)
            => X == other.X && Y == other.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector2d other && X == other.X && Y == other.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked(Y.GetHashCode() * 65599 + X.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NearlyEquals(Vector2d other, double epsilon = 0.00001)
            => Math.Abs(X - other.X) < epsilon
                && Math.Abs(Y - other.Y) < epsilon;

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Dot(Vector2d v)
            => X * v.X + Y * v.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Normalized()
        {
            double ool = 1.0 / Length;
            return new Vector2d(X * ool, Y * ool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            double ool = 1.0d / Length;
            X *= ool;
            Y *= ool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Min(Vector2d v)
            => new Vector2d(Math.Min(X, v.X), Math.Min(Y, v.Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Max(Vector2d v)
            => new Vector2d(Math.Max(X, v.X), Math.Max(Y, v.Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Abs()
            => new Vector2d(Math.Abs(X), Math.Abs(Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Distance(Vector2d v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Distance2(Vector2d v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;

            return dx * dx + dy * dy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Clamp(Vector2d min, Vector2d max)
        {
            double x = X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            double y = Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            return new Vector2d(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Lerp(Vector2d v, double amount)
            => new Vector2d(
                X + (v.X - X) * amount,
                Y + (v.Y - Y) * amount);

        public Vector2d Towards(Vector2d target, double stepSize)
        {
            Vector2d delta = target - this;
            double distance = delta.Length;
            return distance <= stepSize || distance == 0.0
                ? target
                : this + delta * stepSize / distance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Floor()
            => new Vector2d(Math.Floor(X), Math.Floor(Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Ceiling()
            => new Vector2d(Math.Ceiling(X), Math.Ceiling(Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Round()
            => new Vector2d(Math.Floor(X + 0.5), Math.Floor(Y + 0.5));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Truncate()
            => new Vector2d(Math.Truncate(X), Math.Truncate(Y));

		#endregion

		#region Operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator +(Vector2d a, Vector2d b)
            => new Vector2d(a.X + b.X, a.Y + b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator -(Vector2d a, Vector2d b)
            => new Vector2d(a.X - b.X, a.Y - b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator *(Vector2d a, Vector2d b)
            => new Vector2d(a.X * b.X, a.Y * b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator *(Vector2d v, double scalar)
            => new Vector2d(v.X * scalar, v.Y * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator *(double scalar, Vector2d v)
            => new Vector2d(scalar * v.X, scalar * v.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator /(Vector2d a, Vector2d b)
            => new Vector2d(a.X / b.X, a.Y / b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator /(Vector2d v, double scalar)
        {
            double oos = 1.0 / scalar;
            return new Vector2d(v.X * oos, v.Y * oos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator -(Vector2d v)
            => new Vector2d(-v.X, -v.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2d a, Vector2d b)
            => a.X == b.X && a.Y == b.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2d a, Vector2d b)
            => a.X != b.X || a.Y != b.Y;

		#endregion

		#region Stringification

		public override string ToString()
            => ToString("G", CultureInfo.CurrentCulture);

        public string ToString(string format)
            => ToString(format, CultureInfo.CurrentCulture);

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append('(');
            stringBuilder.Append(X.ToString(format, formatProvider));
            stringBuilder.Append(separator);
            stringBuilder.Append(' ');
            stringBuilder.Append(Y.ToString(format, formatProvider));
            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }

        #endregion
    }
}
