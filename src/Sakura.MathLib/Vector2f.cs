using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector2f : IEquatable<Vector2f>, IFormattable
    {
		#region Fields

		public float X;
        public float Y;

		#endregion

		#region Properties

		public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)Math.Sqrt((double)X * X + (double)Y * Y);
        }

        public float Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y;
        }

		#endregion

		#region Static instances

		public static Vector2f Zero => new Vector2f();
        public static Vector2f One => new Vector2f(1, 1);
        public static Vector2f UnitX => new Vector2f(1, 0);
        public static Vector2f UnitY => new Vector2f(0, 1);

        #endregion

        #region Construction and conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2f(Vector2i v)
            => new Vector2f(v.X, v.Y);
        public static explicit operator Vector2f(Vector2d v)
            => new Vector2f((float)v.X, (float)v.Y);

        public static implicit operator Vector2f(Vector2 v)
            => new Vector2f(v.X, v.Y);
        public static implicit operator Vector2(Vector2f v)
            => new Vector2(v.X, v.Y);

		#endregion

		#region Equality and hash codes

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2f other)
            => X == other.X && Y == other.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector2f other && X == other.X && Y == other.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked(Y.GetHashCode() * 65599 + X.GetHashCode());

		#endregion

		#region Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector2f v)
            => X * v.X + Y * v.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Normalized()
        {
            float ool = 1.0f / Length;
            return new Vector2f(X * ool, Y * ool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            float ool = 1.0f / Length;
            X *= ool;
            Y *= ool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Min(Vector2f v)
            => new Vector2f(Math.Min(X, v.X), Math.Min(Y, v.Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Max(Vector2f v)
            => new Vector2f(Math.Max(X, v.X), Math.Max(Y, v.Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Abs()
            => new Vector2f(Math.Abs(X), Math.Abs(Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance(Vector2f v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance2(Vector2f v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;

            return (float)(dx * dx + dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Clamp(Vector2f min, Vector2f max)
        {
            float x = X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            float y = Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            return new Vector2f(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Lerp(Vector2f v, float amount)
            => new Vector2f(
                X + (v.X - X) * amount,
                Y + (v.Y - Y) * amount);

        public Vector2f Towards(Vector2f target, float stepSize)
        {
            Vector2f delta = target - this;
            float distance = delta.Length;
            return distance <= stepSize || distance == 0.0f
                ? target
                : this + delta * stepSize / distance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Floor()
            => new Vector2f(MathF.Floor(X), MathF.Floor(Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Ceiling()
            => new Vector2f(MathF.Ceiling(X), MathF.Ceiling(Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Round()
            => new Vector2f(MathF.Floor(X + 0.5f), MathF.Floor(Y + 0.5f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f Truncate()
            => new Vector2f(MathF.Truncate(X), MathF.Truncate(Y));

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator +(Vector2f a, Vector2f b)
            => new Vector2f(a.X + b.X, a.Y + b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(Vector2f a, Vector2f b)
            => new Vector2f(a.X - b.X, a.Y - b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(Vector2f a, Vector2f b)
            => new Vector2f(a.X * b.X, a.Y * b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(Vector2f v, float scalar)
            => new Vector2f(v.X * scalar, v.Y * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(float scalar, Vector2f v)
            => new Vector2f(scalar * v.X, scalar * v.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator /(Vector2f a, Vector2f b)
            => new Vector2f(a.X / b.X, a.Y / b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator /(Vector2f v, float scalar)
            => new Vector2f(v.X / scalar, v.Y / scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(Vector2f value)
            => new Vector2f(-value.X, -value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2f a, Vector2f b)
            => a.X == b.X && a.Y == b.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2f a, Vector2f b)
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
