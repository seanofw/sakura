using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector4i : IEquatable<Vector4i>, IFormattable
    {
        #region Fields

        public int X;
        public int Y;
        public int Z;
        public int W;

        #endregion

        #region Properties

        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)Math.Sqrt((double)X * X + (double)Y * Y + (double)Z * Z + (double)W * W);
        }

        public int Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y + Z * Z + W * W;
        }

        public float Length2f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)((double)X * X + (double)Y * Y + (double)Z * Z + (double)W * W);
        }

        public Vector3i XYZ
        {
            get => new Vector3i(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector2i XY
        {
            get => new Vector2i(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        #endregion

        #region Static instances

        public static Vector4i Zero => new Vector4i();
        public static Vector4i One => new Vector4i(1, 1, 1, 1);
        public static Vector4i UnitX => new Vector4i(1, 0, 0, 0);
        public static Vector4i UnitY => new Vector4i(0, 1, 0, 0);
        public static Vector4i UnitZ => new Vector4i(0, 0, 1, 0);
        public static Vector4i UnitW => new Vector4i(0, 0, 0, 1);

        #endregion

        #region Construction and conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i(Vector3i v, int w)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i(Vector2i v, int z, int w)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
            W = w;
        }

        public static Vector4i RoundFrom(Vector4f v)
            => new Vector4i((int)(v.X + 0.5f), (int)(v.Y + 0.5f), (int)(v.Z + 0.5f), (int)(v.W + 0.5f));
        public static Vector4i RoundFrom(Vector4d v)
            => new Vector4i((int)(v.X + 0.5), (int)(v.Y + 0.5), (int)(v.Z + 0.5), (int)(v.W + 0.5));
        public static Vector4i RoundFrom(Vector4 v)
            => new Vector4i((int)(v.X + 0.5f), (int)(v.Y + 0.5f), (int)(v.Z + 0.5f), (int)(v.W + 0.5f));

        public static explicit operator Vector4i(Vector4f v)
            => new Vector4i((int)v.X, (int)v.Y, (int)v.Z, (int)v.W);
        public static explicit operator Vector4i(Vector4d v)
            => new Vector4i((int)v.X, (int)v.Y, (int)v.Z, (int)v.W);

        public static explicit operator Vector4i(Vector4 v)
            => new Vector4i((int)v.X, (int)v.Y, (int)v.Z, (int)v.W);
        public static implicit operator Vector4(Vector4i v)
            => new Vector4(v.X, v.Y, v.Z, v.W);

        #endregion

        #region Equality and hash codes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4i other)
            => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector4i other && X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked(((W * 65599 + Z) * 65599 + Y) * 65599 + X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NearlyEquals(Vector4i other, int epsilon)
            => Math.Abs(X - other.X) <= epsilon
                && Math.Abs(Y - other.Y) <= epsilon
                && Math.Abs(Z - other.Z) <= epsilon
                && Math.Abs(W - other.W) <= epsilon;

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector4i v)
            => X * v.X + Y * v.Y + Z * v.Z + W * v.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i Min(Vector4i v)
            => new Vector4i(Math.Min(X, v.X), Math.Min(Y, v.Y), Math.Min(Z, v.Z), Math.Min(W, v.W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i Max(Vector4i v)
            => new Vector4i(Math.Max(X, v.X), Math.Max(Y, v.Y), Math.Max(Z, v.Z), Math.Max(W, v.W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i Abs()
            => new Vector4i(Math.Abs(X), Math.Abs(Y), Math.Abs(Z), Math.Abs(W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance(Vector4i v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;
            double dw = W - v.W;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance2(Vector4i v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;
            double dw = W - v.W;

            return (float)(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i Clamp(Vector4i min, Vector4i max)
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

            int w = W;
            w = (w > max.W) ? max.W : w;
            w = (w < min.W) ? min.W : w;

            return new Vector4i(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i Lerp(Vector4i v, float amount)
            => new Vector4i(
                (int)(X + (v.X - X) * amount + 0.5f),
                (int)(Y + (v.Y - Y) * amount + 0.5f),
                (int)(Z + (v.Z - Z) * amount + 0.5f),
                (int)(W + (v.W - W) * amount + 0.5f));

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator +(Vector4i a, Vector4i b)
            => new Vector4i(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator -(Vector4i a, Vector4i b)
            => new Vector4i(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator *(Vector4i a, Vector4i b)
            => new Vector4i(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator *(Vector4i v, int scalar)
            => new Vector4i(v.X * scalar, v.Y * scalar, v.Z * scalar, v.W * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator *(int scalar, Vector4i v)
            => new Vector4i(scalar * v.X, scalar * v.Y, scalar * v.Z, scalar * v.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator /(Vector4i a, Vector4i b)
            => new Vector4i(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator /(Vector4i v, int scalar)
            => new Vector4i(v.X / scalar, v.Y / scalar, v.Z / scalar, v.W / scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator -(Vector4i v)
            => new Vector4i(-v.X, -v.Y, -v.Z, -v.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4i a, Vector4i b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4i a, Vector4i b)
            => a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W;

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
            stringBuilder.Append(separator);
            stringBuilder.Append(' ');
            stringBuilder.Append(W.ToString(format, formatProvider));
            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }

        #endregion
    }
}

