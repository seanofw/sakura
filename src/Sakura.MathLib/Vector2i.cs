using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector2i : IEquatable<Vector2i>, IFormattable
    {
		#region Fields

		public int X;
        public int Y;

		#endregion

		#region Properties

		public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)Math.Sqrt((double)X * X + (double)Y * Y);
        }

        public int Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y;
        }

        public float Length2f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)((double)X * X + (double)Y * Y);
        }

		#endregion

		#region Static instances

		public static Vector2i Zero => new Vector2i();
        public static Vector2i One => new Vector2i(1, 1);
        public static Vector2i UnitX => new Vector2i(1, 0);
        public static Vector2i UnitY => new Vector2i(0, 1);

		#endregion

		#region Construction and conversion

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2i RoundFrom(Vector2f v)
            => new Vector2i((int)(v.X + 0.5f), (int)(v.Y + 0.5f));
        public static Vector2i RoundFrom(Vector2d v)
            => new Vector2i((int)(v.X + 0.5), (int)(v.Y + 0.5));
        public static Vector2i RoundFrom(Vector2 v)
            => new Vector2i((int)(v.X + 0.5f), (int)(v.Y + 0.5f));

        public static explicit operator Vector2i(Vector2f v)
            => new Vector2i((int)v.X, (int)v.Y);
        public static explicit operator Vector2i(Vector2d v)
            => new Vector2i((int)v.X, (int)v.Y);

        public static explicit operator Vector2i(Vector2 v)
            => new Vector2i((int)v.X, (int)v.Y);
        public static implicit operator Vector2(Vector2i v)
            => new Vector2(v.X, v.Y);

		#endregion

		#region Equality and hash codes

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2i other)
            => X == other.X && Y == other.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector2i other && X == other.X && Y == other.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked(Y * 65599 + X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NearlyEquals(Vector2i other, int epsilon = 1)
            => Math.Abs(X - other.X) <= epsilon
                && Math.Abs(Y - other.Y) <= epsilon;

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector2i v)
            => X * v.X + Y * v.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2i Min(Vector2i v)
            => new Vector2i(Math.Min(X, v.X), Math.Min(Y, v.Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2i Max(Vector2i v)
            => new Vector2i(Math.Max(X, v.X), Math.Max(Y, v.Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2i Abs()
            => new Vector2i(Math.Abs(X), Math.Abs(Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance(Vector2i v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance2(Vector2i v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;

            return (float)(dx * dx + dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2i Clamp(Vector2i min, Vector2i max)
        {
            int x = X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            int y = Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            return new Vector2i(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2i Lerp(Vector2i v, float amount)
            => new Vector2i(
                (int)(X + (v.X - X) * amount + 0.5f),
                (int)(Y + (v.Y - Y) * amount + 0.5f));

		#endregion

		#region Operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator +(Vector2i a, Vector2i b)
            => new Vector2i(a.X + b.X, a.Y + b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator -(Vector2i a, Vector2i b)
            => new Vector2i(a.X - b.X, a.Y - b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator *(Vector2i a, Vector2i b)
            => new Vector2i(a.X * b.X, a.Y * b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator *(Vector2i v, int scalar)
            => new Vector2i(v.X * scalar, v.Y * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator *(int scalar, Vector2i v)
            => new Vector2i(scalar * v.X, scalar * v.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator /(Vector2i a, Vector2i b)
            => new Vector2i(a.X / b.X, a.Y / b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator /(Vector2i v, int scalar)
            => new Vector2i(v.X / scalar, v.Y / scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator -(Vector2i v)
            => new Vector2i(-v.X, -v.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2i a, Vector2i b)
            => a.X == b.X && a.Y == b.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2i a, Vector2i b)
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
