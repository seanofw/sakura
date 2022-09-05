using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector4f : IEquatable<Vector4f>, IFormattable
    {
        #region Fields

        public float X;
        public float Y;
        public float Z;
        public float W;

        #endregion

        #region Properties

        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)Math.Sqrt((double)X * X + (double)Y * Y + (double)Z * Z + (double)W * W);
        }

        public float Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y + Z * Z + W * W;
        }

        public Vector3f XYZ
        {
            get => new Vector3f(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
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

        #endregion

        #region Static instances

        public static Vector4f Zero => new Vector4f();
        public static Vector4f One => new Vector4f(1, 1, 1, 1);
        public static Vector4f UnitX => new Vector4f(1, 0, 0, 0);
        public static Vector4f UnitY => new Vector4f(0, 1, 0, 0);
        public static Vector4f UnitZ => new Vector4f(0, 0, 1, 0);
        public static Vector4f UnitW => new Vector4f(0, 0, 0, 1);

        #endregion

        #region Construction and conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f(Vector3f v, float w)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f(Vector2f v, float z, float w)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
            W = w;
        }

        public static implicit operator Vector4f(Vector4i v)
            => new Vector4f(v.X, v.Y, v.Z, v.W);
        public static explicit operator Vector4f(Vector4d v)
            => new Vector4f((float)v.X, (float)v.Y, (float)v.Z, (float)v.W);

        public static implicit operator Vector4f(Vector4 v)
            => new Vector4f(v.X, v.Y, v.Z, v.W);
        public static implicit operator Vector4(Vector4f v)
            => new Vector4(v.X, v.Y, v.Z, v.W);

        #endregion

        #region Equality and hash codes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4f other)
            => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector4f other && X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked(((W.GetHashCode() * 65599 + Z.GetHashCode()) * 65599 + Y.GetHashCode()) * 65599 + X.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NearlyEquals(Vector4f other, float epsilon = 0.00001f)
            => Math.Abs(X - other.X) < epsilon
                && Math.Abs(Y - other.Y) < epsilon
                && Math.Abs(Z - other.Z) < epsilon
                && Math.Abs(W - other.W) < epsilon;

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector4f v)
            => X * v.X + Y * v.Y + Z * v.Z + W * v.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Normalized()
        {
            float ool = 1.0f / Length;
            return new Vector4f(X * ool, Y * ool, Z * ool, W * ool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            float ool = 1.0f / Length;
            X *= ool;
            Y *= ool;
            Z *= ool;
            W *= ool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Min(Vector4f v)
            => new Vector4f(Math.Min(X, v.X), Math.Min(Y, v.Y), Math.Min(Z, v.Z), Math.Min(W, v.W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Max(Vector4f v)
            => new Vector4f(Math.Max(X, v.X), Math.Max(Y, v.Y), Math.Max(Z, v.Z), Math.Max(W, v.W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Abs()
            => new Vector4f(Math.Abs(X), Math.Abs(Y), Math.Abs(Z), Math.Abs(W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance(Vector4f v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;
            double dw = W - v.W;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Distance2(Vector4f v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;
            double dw = W - v.W;

            return (float)(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Clamp(Vector4f min, Vector4f max)
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

            float w = W;
            w = (w > max.W) ? max.W : w;
            w = (w < min.W) ? min.W : w;

            return new Vector4f(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Lerp(Vector4f v, float amount)
            => new Vector4f(
                X + (v.X - X) * amount,
                Y + (v.Y - Y) * amount,
                Z + (v.Z - Z) * amount,
                W + (v.W - W) * amount);

        public Vector4f Towards(Vector4f target, float stepSize)
        {
            Vector4f delta = target - this;
            float distance = delta.Length;
            return distance <= stepSize || distance == 0.0f
                ? target
                : this + delta * stepSize / distance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Floor()
            => new Vector4f(MathF.Floor(X), MathF.Floor(Y), MathF.Floor(Z), MathF.Floor(W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Ceiling()
            => new Vector4f(MathF.Ceiling(X), MathF.Ceiling(Y), MathF.Ceiling(Z), MathF.Ceiling(W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Round()
            => new Vector4f(MathF.Floor(X + 0.5f), MathF.Floor(Y + 0.5f), MathF.Floor(Z + 0.5f), MathF.Floor(W + 0.5f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Truncate()
            => new Vector4f(MathF.Truncate(X), MathF.Truncate(Y), MathF.Truncate(Z), MathF.Truncate(W));

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator +(Vector4f a, Vector4f b)
            => new Vector4f(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator -(Vector4f a, Vector4f b)
            => new Vector4f(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator *(Vector4f a, Vector4f b)
            => new Vector4f(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator *(Vector4f v, float scalar)
            => new Vector4f(v.X * scalar, v.Y * scalar, v.Z * scalar, v.W * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator *(float scalar, Vector4f v)
            => new Vector4f(scalar * v.X, scalar * v.Y, scalar * v.Z, scalar * v.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator /(Vector4f a, Vector4f b)
            => new Vector4f(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator /(Vector4f v, float scalar)
        {
            double oos = 1.0 / scalar;
            return new Vector4f((float)(v.X * oos), (float)(v.Y * oos), (float)(v.Z * oos), (float)(v.W * oos));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator -(Vector4f v)
            => new Vector4f(-v.X, -v.Y, -v.Z, -v.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4f a, Vector4f b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4f a, Vector4f b)
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
