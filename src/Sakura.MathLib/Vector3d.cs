﻿using System.Globalization;

namespace Sakura.MathLib
{
    public struct Vector3d : IEquatable<Vector3d>, IFormattable
    {
        #region Fields

        public double X;
        public double Y;
        public double Z;

        #endregion

        #region Properties

        public double Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (double)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public double Length2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y + Z * Z;
        }

        #endregion

        #region Static instances

        public static Vector3d Zero => new Vector3d();
        public static Vector3d One => new Vector3d(1, 1, 1);
        public static Vector3d UnitX => new Vector3d(1, 0, 0);
        public static Vector3d UnitY => new Vector3d(0, 1, 0);
        public static Vector3d UnitZ => new Vector3d(0, 0, 1);

        #endregion

        #region Construction and conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator Vector3d(Vector3i v)
            => new Vector3d(v.X, v.Y, v.Z);
        public static implicit operator Vector3d(Vector3f v)
            => new Vector3d(v.X, v.Y, v.Z);

        public static implicit operator Vector3d(Vector3 v)
            => new Vector3d(v.X, v.Y, v.Z);
        public static explicit operator Vector3(Vector3d v)
            => new Vector3((float)v.X, (float)v.Y, (float)v.Z);

        #endregion

        #region Equality and hash codes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3d other)
            => X == other.X && Y == other.Y && Z == other.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
            => obj is Vector3d other && X == other.X && Y == other.Y && Z == other.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => unchecked((Z.GetHashCode() * 65599 + Y.GetHashCode()) * 65599 + X.GetHashCode());

        public bool NearlyEquals(Vector3i other, double epsilon = 0.00001)
            => Math.Abs(X - other.X) < epsilon
                && Math.Abs(Y - other.Y) < epsilon
                && Math.Abs(Z - other.Z) < epsilon;

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Dot(Vector3d v)
            => X * v.X + Y * v.Y + Z * v.Z;

        public Vector3d Cross(Vector3d v)
            => new Vector3d(
                Y * v.Z - Z * v.Y,
                Z * v.X - X * v.Z,
                X * v.Y - Y * v.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Normalized()
        {
            double ool = 1.0 / Length;
            return new Vector3d(X * ool, Y * ool, Z * ool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            double ool = 1.0 / Length;
            X *= ool;
            Y *= ool;
            Z *= ool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Min(Vector3d v)
            => new Vector3d(Math.Min(X, v.X), Math.Min(Y, v.Y), Math.Min(Z, v.Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Max(Vector3d v)
            => new Vector3d(Math.Max(X, v.X), Math.Max(Y, v.Y), Math.Max(Z, v.Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Abs()
            => new Vector3d(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Distance(Vector3d v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;

            return (double)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Distance2(Vector3d v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;
            double dz = Z - v.Z;

            return (double)(dx * dx + dy * dy + dz * dz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Clamp(Vector3d min, Vector3d max)
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

            return new Vector3d(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Lerp(Vector3d v, double amount)
            => new Vector3d(
                X + (v.X - X) * amount,
                Y + (v.Y - Y) * amount,
                Z + (v.Z - Z) * amount);

        public Vector3d Towards(Vector3d target, double stepSize)
        {
            Vector3d delta = target - this;
            double distance = delta.Length;
            return distance <= stepSize || distance == 0.0
                ? target
                : this + delta * stepSize / distance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Floor()
            => new Vector3d(Math.Floor(X), Math.Floor(Y), Math.Floor(Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Ceiling()
            => new Vector3d(Math.Ceiling(X), Math.Ceiling(Y), Math.Ceiling(Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Round()
            => new Vector3d(Math.Floor(X + 0.5f), Math.Floor(Y + 0.5f), Math.Floor(Z + 0.5f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Truncate()
            => new Vector3d(Math.Truncate(X), Math.Truncate(Y), Math.Truncate(Z));

        #endregion

        #region Static methods

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static Vector3d Bezier2(Vector3d start, Vector3d c1, Vector3d end, double t)
        {
            double it = 1 - t;
            double it2 = it * it;
            double t2 = t * t;

            double a = it2;
            double b = 2 * it * t;
            double c = t2;

            double x = a * start.X + b * c1.X + c * end.X;
            double y = a * start.Y + b * c1.Y + c * end.Y;
            double z = a * start.Z + b * c1.Z + c * end.Z;

            return new Vector3d(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static Vector3d Bezier2Deriv(Vector3d start, Vector3d c1, Vector3d end, double t)
        {
            double it = 1 - t;

            double a = 2 * it;
            double b = 2 * t;

            double x = a * (c1.X - start.X) + b * (end.X - c1.X);
            double y = a * (c1.Y - start.Y) + b * (end.Y - c1.Y);
            double z = a * (c1.Z - start.Z) + b * (end.Z - c1.Z);

            return new Vector3d(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static Vector3d Bezier3(Vector3d start, Vector3d c1, Vector3d c2, Vector3d end, double t)
        {
            double it = 1 - t;
            double it2 = it * it;
            double t2 = t * t;

            double a = it * it2;
            double b = 3 * it2 * t;
            double c = 3 * it * t2;
            double d = t2 * t;

            double x = a * start.X + b * c1.X + c * c2.X + d * end.X;
            double y = a * start.Y + b * c1.Y + c * c2.Y + d * end.Y;
            double z = a * start.Z + b * c1.Z + c * c2.Z + d * end.Z;

            return new Vector3d(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static Vector3d Bezier3Deriv(Vector3d start, Vector3d c1, Vector3d c2, Vector3d end, double t)
        {
            double it = 1 - t;

            double a = 3 * it * it;
            double b = 6 * it * t;
            double c = 3 * t * t;

            double x = a * (c1.X - start.X) + b * (c2.X - c1.X) + c * (end.X - c2.X);
            double y = a * (c1.Y - start.Y) + b * (c2.Y - c1.Y) + c * (end.Y - c2.Y);
            double z = a * (c1.Z - start.Z) + b * (c2.Z - c1.Z) + c * (end.Z - c2.Z);

            return new Vector3d(x, y, z);
        }

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator +(Vector3d a, Vector3d b)
            => new Vector3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d a, Vector3d b)
            => new Vector3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d a, Vector3d b)
            => new Vector3d(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d v, double scalar)
            => new Vector3d(v.X * scalar, v.Y * scalar, v.Z * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(double scalar, Vector3d v)
            => new Vector3d(scalar * v.X, scalar * v.Y, scalar * v.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(Vector3d a, Vector3d b)
            => new Vector3d(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(Vector3d v, double scalar)
            => new Vector3d(v.X / scalar, v.Y / scalar, v.Z / scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d value)
            => new Vector3d(-value.X, -value.Y, -value.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3d a, Vector3d b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3d a, Vector3d b)
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