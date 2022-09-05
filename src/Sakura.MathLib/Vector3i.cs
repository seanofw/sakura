using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector3i : IEquatable<Vector3i>, IFormattable
    {
        #region Fields

        public int X;
        public int Y;
        public int Z;

        #endregion

        #region Properties

        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)Math.Sqrt((double)X * X + (double)Y * Y + (double)Z * Z);
        }

        public int Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y + Z * Z;
        }

        public float Length2f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)((double)X * X + (double)Y * Y + (double)Z * Z);
        }

        #endregion

        #region Static instances

        public static Vector3i Zero => new Vector3i();
        public static Vector3i One => new Vector3i(1, 1, 1);
        public static Vector3i UnitX => new Vector3i(1, 0, 0);
        public static Vector3i UnitY => new Vector3i(0, 1, 0);
        public static Vector3i UnitZ => new Vector3i(0, 0, 1);

        #endregion

        #region Construction and conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3i RoundFrom(Vector3f v)
            => new Vector3i((int)(v.X + 0.5f), (int)(v.Y + 0.5f), (int)(v.Z + 0.5f));
        public static Vector3i RoundFrom(Vector3d v)
            => new Vector3i((int)(v.X + 0.5), (int)(v.Y + 0.5), (int)(v.Z + 0.5));
        public static Vector3i RoundFrom(Vector3 v)
            => new Vector3i((int)(v.X + 0.5f), (int)(v.Y + 0.5f), (int)(v.Z + 0.5f));

        public static explicit operator Vector3i(Vector3f v)
            => new Vector3i((int)v.X, (int)v.Y, (int)v.Z);
        public static explicit operator Vector3i(Vector3d v)
            => new Vector3i((int)v.X, (int)v.Y, (int)v.Z);

        public static explicit operator Vector3i(Vector3 v)
            => new Vector3i((int)v.X, (int)v.Y, (int)v.Z);
        public static implicit operator Vector3(Vector3i v)
            => new Vector3(v.X, v.Y, v.Z);

        #endregion

        #region Equality and hash codes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3i other)
            => X == other.X && Y == other.Y && Z == other.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector3i other && X == other.X && Y == other.Y && Z == other.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked((Z * 65599 + Y) * 65599 + X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NearlyEquals(Vector3i other, int epsilon)
            => Math.Abs(X - other.X) <= epsilon
                && Math.Abs(Y - other.Y) <= epsilon
                && Math.Abs(Z - other.Z) <= epsilon;

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector3i v)
            => X * v.X + Y * v.Y + Z * v.Z;

        public Vector3i Cross(Vector3i v)
            => new Vector3i(
                Y * v.Z - Z * v.Y,
                Z * v.X - X * v.Z,
                X * v.Y - Y * v.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i Min(Vector3i v)
            => new Vector3i(Math.Min(X, v.X), Math.Min(Y, v.Y), Math.Min(Z, v.Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i Max(Vector3i v)
            => new Vector3i(Math.Max(X, v.X), Math.Max(Y, v.Y), Math.Max(Z, v.Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i Abs()
            => new Vector3i(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance(Vector3i v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance2(Vector3i v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;

            return (float)(dx * dx + dy * dy + dz * dz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i Clamp(Vector3i min, Vector3i max)
        {
            int x = X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            int y = Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            int z = Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            return new Vector3i(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i Lerp(Vector3i v, float amount)
            => new Vector3i(
                (int)(X + (v.X - X) * amount + 0.5f),
                (int)(Y + (v.Y - Y) * amount + 0.5f),
                (int)(Z + (v.Z - Z) * amount + 0.5f));

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator +(Vector3i a, Vector3i b)
            => new Vector3i(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator -(Vector3i a, Vector3i b)
            => new Vector3i(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator *(Vector3i a, Vector3i b)
            => new Vector3i(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator *(Vector3i v, int scalar)
            => new Vector3i(v.X * scalar, v.Y * scalar, v.Z * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator *(int scalar, Vector3i v)
            => new Vector3i(scalar * v.X, scalar * v.Y, scalar * v.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator /(Vector3i a, Vector3i b)
            => new Vector3i(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator /(Vector3i v, int scalar)
            => new Vector3i(v.X / scalar, v.Y / scalar, v.Z / scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator -(Vector3i v)
            => new Vector3i(-v.X, -v.Y, -v.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3i a, Vector3i b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3i a, Vector3i b)
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

