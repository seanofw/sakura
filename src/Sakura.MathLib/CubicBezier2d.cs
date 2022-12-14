
namespace Sakura.MathLib
{
    /// <summary>
    /// A representation of a cubic Bezier curve, with methods to
    /// manipulate it and evaluate it.
    /// </summary>
	public struct CubicBezier2d
	{
		public Vector2d Start;
		public Vector2d C1;
		public Vector2d C2;
		public Vector2d End;

        /// <summary>
        /// Construct a representation of a cubic Bezier curve, with the given four
        /// control points.
        /// </summary>
        /// <param name="start">The start point of the Bezier curve (P0).</param>
        /// <param name="c1">The first control point on the Bezier curve (P1),
        /// associated with the start point.</param>
        /// <param name="c2">The second control point on the Bezier curve (P2),
        /// associated with the end point.</param>
        /// <param name="end">The end point of the Bezier curve (P3).</param>
        public CubicBezier2d(Vector2d start, Vector2d c1, Vector2d c2, Vector2d end)
		{
            Start = start;
            C1 = c1;
            C2 = c2;
            End = end;
		}

        /// <summary>
        /// Bezier control point distance to represent a circle/ellipse.  See
        /// https://stackoverflow.com/questions/2172798/how-to-draw-an-oval-in-html5-canvas
        /// and http://www.whizkidtech.redprince.net/bezier/circle/kappa/ .
        /// </summary>
        public const double Kappa = 0.5522847498307934;

        /// <summary>
        /// Evaluate the position of a cubic Bezier curve at the given parameter t, from 0 to 1.
        /// </summary>
        /// <param name="t">The Bezier curve parameter, from 0 to 1.</param>
        /// <returns>The position along the curve at t.</returns>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Vector2d At(double t)
        {
            double it = 1 - t;
            double it2 = it * it;
            double t2 = t * t;

            double a = it * it2;
            double b = 3 * it2 * t;
            double c = 3 * it * t2;
            double d = t2 * t;

            double x = a * Start.X + b * C1.X + c * C2.X + d * End.X;
            double y = a * Start.Y + b * C1.Y + c * C2.Y + d * End.Y;

            return new Vector2d(x, y);
        }

        /// <summary>
        /// Evaluate the first derivative of a cubic Bezier curve at the given parameter t, from 0 to 1.
        /// </summary>
        /// <param name="t">The Bezier curve parameter, from 0 to 1.</param>
        /// <returns>The derivative along the curve at t.</returns>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Vector2d Deriv(double t)
        {
            double it = 1 - t;

            double a = 3 * it * it;
            double b = 6 * it * t;
            double c = 3 * t * t;

            double x = a * (C1.X - Start.X) + b * (C2.X - C1.X) + c * (End.X - C2.X);
            double y = a * (C1.Y - Start.Y) + b * (C2.Y - C1.Y) + c * (End.Y - C2.Y);

            return new Vector2d(x, y);
        }

        /// <summary>
        /// Calculate a cubic Bezier curve that represents a quadrant of an ellipse (including
        /// ellipses with an affine transform applied to them).
        /// </summary>
        /// <param name="center">The center of the ellipse.</param>
        /// <param name="axis1">The endpoint of one of the ellipse's axes, which will be the start of the
        /// Bezier curve.</param>
        /// <param name="axis2">The endpoint of the other of the ellipse's axes, which will be the end
        /// of the Bezier curve.</param>
        /// <returns>The Bezier curve that matches the given quadrant of the ellipse.</returns>
        public static CubicBezier2d FromEllipse(Vector2d center, Vector2d axis1, Vector2d axis2)
		{
            Vector2d c1 = axis1 + (axis2 - center).Normalized() * Kappa;
            Vector2d c2 = axis2 + (axis1 - center).Normalized() * Kappa;
            return new CubicBezier2d(axis1, c1, c2, axis2);
		}

        /// <summary>
        /// Calculate a cubic Bezier curve that represents a straight line segment from a to b
        /// with uniform velocity across the curve.
        /// </summary>
        /// <param name="a">The start of the line segment.</param>
        /// <param name="b">The end of the line segment.</param>
        /// <returns>The Bezier curve that matches the line segment.</returns>
        public static CubicBezier2d FromLine(Vector2d a, Vector2d b)
		{
            Vector2d d = (b - a).Normalized();
            Vector2d c1 = a + d * (1 / 3);
            Vector2d c2 = a + d * (2 / 3);
            return new CubicBezier2d(a, c1, c2, b);
        }
    }
}
