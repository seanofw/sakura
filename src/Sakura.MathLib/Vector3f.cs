using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector3f : IEquatable<Vector3f>, IFormattable
    {
        #region Fields

        public float X;
        public float Y;
        public float Z;

        #endregion

        #region Properties

        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)Math.Sqrt((double)X * X + (double)Y * Y + (double)Z * Z);
        }

        public float Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y + Z * Z;
        }

        public Vector2f XY
        {
            get => new Vector2f(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2f YX
        {
            get => new Vector2f(Y, X);
            set
            {
                Y = value.X;
                X = value.Y;
            }
        }

        public Vector2f YZ
        {
            get => new Vector2f(Y, Z);
            set
            {
                Y = value.X;
                Z = value.Y;
            }
        }

        public Vector2f ZY
        {
            get => new Vector2f(Z, Y);
            set
            {
                Z = value.X;
                Y = value.Y;
            }
        }

        public Vector2f XZ
        {
            get => new Vector2f(X, Z);
            set
            {
                X = value.X;
                Z = value.Y;
            }
        }

        public Vector2f ZX
        {
            get => new Vector2f(Z, X);
            set
            {
                Z = value.X;
                X = value.Y;
            }
        }

        #endregion

        #region Static instances

        public static Vector3f Zero => new Vector3f();
        public static Vector3f One => new Vector3f(1, 1, 1);
        public static Vector3f UnitX => new Vector3f(1, 0, 0);
        public static Vector3f UnitY => new Vector3f(0, 1, 0);
        public static Vector3f UnitZ => new Vector3f(0, 0, 1);

        #endregion

        #region Construction and conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f(Vector2f v, float z)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
        }

        public static implicit operator Vector3f(Vector3i v)
            => new Vector3f(v.X, v.Y, v.Z);
        public static explicit operator Vector3f(Vector3d v)
            => new Vector3f((float)v.X, (float)v.Y, (float)v.Z);

        public static implicit operator Vector3f(Vector3 v)
            => new Vector3f(v.X, v.Y, v.Z);
        public static implicit operator Vector3(Vector3f v)
            => new Vector3(v.X, v.Y, v.Z);

        #endregion

        #region Equality and hash codes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3f other)
            => X == other.X && Y == other.Y && Z == other.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector3f other && X == other.X && Y == other.Y && Z == other.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked((Z.GetHashCode() * 65599 + Y.GetHashCode()) * 65599 + X.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NearlyEquals(Vector3f other, float epsilon = 0.00001f)
            => Math.Abs(X - other.X) < epsilon
                && Math.Abs(Y - other.Y) < epsilon
                && Math.Abs(Z - other.Z) < epsilon;

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector3f v)
            => X * v.X + Y * v.Y + Z * v.Z;

        public Vector3f Cross(Vector3f v)
            => new Vector3f(
                Y * v.Z - Z * v.Y,
                Z * v.X - X * v.Z,
                X * v.Y - Y * v.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Normalized()
        {
            float ool = 1.0f / Length;
            return new Vector3f(X * ool, Y * ool, Z * ool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            float ool = 1.0f / Length;
            X *= ool;
            Y *= ool;
            Z *= ool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Min(Vector3f v)
            => new Vector3f(Math.Min(X, v.X), Math.Min(Y, v.Y), Math.Min(Z, v.Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Max(Vector3f v)
            => new Vector3f(Math.Max(X, v.X), Math.Max(Y, v.Y), Math.Max(Z, v.Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Abs()
            => new Vector3f(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance(Vector3f v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance2(Vector3f v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;

            return (float)(dx * dx + dy * dy + dz * dz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Clamp(Vector3f min, Vector3f max)
        {
            float x = X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            float y = Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            float z = Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            return new Vector3f(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Lerp(Vector3f v, float amount)
            => new Vector3f(
                X + (v.X - X) * amount,
                Y + (v.Y - Y) * amount,
                Z + (v.Z - Z) * amount);

        public Vector3f Towards(Vector3f target, float stepSize)
        {
            Vector3f delta = target - this;
            float distance = delta.Length;
            return distance <= stepSize || distance == 0.0f
                ? target
                : this + delta * stepSize / distance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Floor()
            => new Vector3f(MathF.Floor(X), MathF.Floor(Y), MathF.Floor(Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Ceiling()
            => new Vector3f(MathF.Ceiling(X), MathF.Ceiling(Y), MathF.Ceiling(Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Round()
            => new Vector3f(MathF.Floor(X + 0.5f), MathF.Floor(Y + 0.5f), MathF.Floor(Z + 0.5f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Truncate()
            => new Vector3f(MathF.Truncate(X), MathF.Truncate(Y), MathF.Truncate(Z));

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator +(Vector3f a, Vector3f b)
            => new Vector3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator -(Vector3f a, Vector3f b)
            => new Vector3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator *(Vector3f a, Vector3f b)
            => new Vector3f(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator *(Vector3f v, float scalar)
            => new Vector3f(v.X * scalar, v.Y * scalar, v.Z * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator *(float scalar, Vector3f v)
            => new Vector3f(scalar * v.X, scalar * v.Y, scalar * v.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator /(Vector3f a, Vector3f b)
            => new Vector3f(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator /(Vector3f v, float scalar)
        {
            double oos = 1.0 / scalar;
            return new Vector3f((float)(v.X * oos), (float)(v.Y * oos), (float)(v.Z * oos));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator -(Vector3f v)
            => new Vector3f(-v.X, -v.Y, -v.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3f a, Vector3f b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3f a, Vector3f b)
            => a.X != b.X || a.Y != b.Y || a.Z != b.Z;

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
            stringBuilder.Append(separator);
            stringBuilder.Append(' ');
            stringBuilder.Append(Z.ToString(format, formatProvider));
            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }

        #endregion
    }
}
