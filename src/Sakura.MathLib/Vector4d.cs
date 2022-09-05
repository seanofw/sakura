using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector4d : IEquatable<Vector4d>, IFormattable
    {
        #region Fields

        public double X;
        public double Y;
        public double Z;
        public double W;        // For historical/compatibility reasons, W is last.

        #endregion

        #region Properties

        public double Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (double)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public double Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y + Z * Z + W * W;
        }

        #endregion

        #region Static instances

        public static Vector4d Zero => new Vector4d();
        public static Vector4d One => new Vector4d(1, 1, 1, 1);
        public static Vector4d UnitX => new Vector4d(1, 0, 0, 0);
        public static Vector4d UnitY => new Vector4d(0, 1, 0, 0);
        public static Vector4d UnitZ => new Vector4d(0, 0, 1, 0);
        public static Vector4d UnitW => new Vector4d(0, 0, 0, 1);

        #endregion

        #region Construction and conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static implicit operator Vector4d(Vector4i v)
            => new Vector4d(v.X, v.Y, v.Z, v.W);
        public static implicit operator Vector4d(Vector4f v)
            => new Vector4d(v.X, v.Y, v.Z, v.W);

        public static implicit operator Vector4d(Vector4 v)
            => new Vector4d(v.X, v.Y, v.Z, v.W);
        public static explicit operator Vector4(Vector4d v)
            => new Vector4((float)v.X, (float)v.Y, (float)v.Z, (float)v.W);

        #endregion

        #region Equality and hash codes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4d other)
            => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector4d other && X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked(((W.GetHashCode() * 65599 + Z.GetHashCode()) * 65599 + Y.GetHashCode()) * 65599 + X.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NearlyEquals(Vector4d other, double epsilon = 0.00001)
            => Math.Abs(X - other.X) < epsilon
                && Math.Abs(Y - other.Y) < epsilon
                && Math.Abs(Z - other.Z) < epsilon
                && Math.Abs(W - other.W) < epsilon;

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Dot(Vector4d v)
            => X * v.X + Y * v.Y + Z * v.Z + W * v.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Normalized()
        {
            double ool = 1.0 / Length;
            return new Vector4d(X * ool, Y * ool, Z * ool, W * ool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            double ool = 1.0 / Length;
            X *= ool;
            Y *= ool;
            Z *= ool;
            W *= ool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Min(Vector4d v)
            => new Vector4d(Math.Min(X, v.X), Math.Min(Y, v.Y), Math.Min(Z, v.Z), Math.Min(W, v.W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Max(Vector4d v)
            => new Vector4d(Math.Max(X, v.X), Math.Max(Y, v.Y), Math.Max(Z, v.Z), Math.Max(W, v.W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Abs()
            => new Vector4d(Math.Abs(X), Math.Abs(Y), Math.Abs(Z), Math.Abs(W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Distance(Vector4d v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;
            double dw = W - v.W;

            return (double)Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Distance2(Vector4d v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;
            double dw = W - v.W;

            return (double)(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Clamp(Vector4d min, Vector4d max)
        {
            double x = X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            double y = Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            double z = Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            double w = W;
            w = (w > max.W) ? max.W : w;
            w = (w < min.W) ? min.W : w;

            return new Vector4d(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Lerp(Vector4d v, double amount)
            => new Vector4d(
                X + (v.X - X) * amount,
                Y + (v.Y - Y) * amount,
                Z + (v.Z - Z) * amount,
                W + (v.W - W) * amount);

        public Vector4d Towards(Vector4d target, double stepSize)
        {
            Vector4d delta = target - this;
            double distance = delta.Length;
            return distance <= stepSize || distance == 0.0
                ? target
                : this + delta * stepSize / distance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Floor()
            => new Vector4d(Math.Floor(X), Math.Floor(Y), Math.Floor(Z), Math.Floor(W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Ceiling()
            => new Vector4d(Math.Ceiling(X), Math.Ceiling(Y), Math.Ceiling(Z), Math.Ceiling(W));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Round()
            => new Vector4d(Math.Floor(X + 0.5f), Math.Floor(Y + 0.5f), Math.Floor(Z + 0.5f), Math.Floor(W + 0.5f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Truncate()
            => new Vector4d(Math.Truncate(X), Math.Truncate(Y), Math.Truncate(Z), Math.Truncate(W));

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator +(Vector4d a, Vector4d b)
            => new Vector4d(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator -(Vector4d a, Vector4d b)
            => new Vector4d(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(Vector4d a, Vector4d b)
            => new Vector4d(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(Vector4d v, double scalar)
            => new Vector4d(v.X * scalar, v.Y * scalar, v.Z * scalar, v.W * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(double scalar, Vector4d v)
            => new Vector4d(scalar * v.X, scalar * v.Y, scalar * v.Z, scalar * v.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator /(Vector4d a, Vector4d b)
            => new Vector4d(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator /(Vector4d v, double scalar)
        {
            double oos = 1.0 / scalar;
            return new Vector4d(v.X * oos, v.Y * oos, v.Z * oos, v.W * oos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator -(Vector4d v)
            => new Vector4d(-v.X, -v.Y, -v.Z, -v.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4d a, Vector4d b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4d a, Vector4d b)
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
