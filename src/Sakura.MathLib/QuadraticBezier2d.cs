
namespace Sakura.MathLib
{
	public struct QuadraticBezier2d
	{
		public Vector2d Start;
		public Vector2d C1;
		public Vector2d End;

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Vector2d At(double t)
        {
            double it = 1 - t;
            double it2 = it * it;
            double t2 = t * t;

            double a = it2;
            double b = 2 * it * t;
            double c = t2;

            double x = a * Start.X + b * C1.X + c * End.X;
            double y = a * Start.Y + b * C1.Y + c * End.Y;

            return new Vector2d(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Vector2d Derivative(double t)
        {
            double it = 1 - t;

            double a = 2 * it;
            double b = 2 * t;

            double x = a * (C1.X - Start.X) + b * (End.X - C1.X);
            double y = a * (C1.Y - Start.Y) + b * (End.Y - C1.Y);

            return new Vector2d(x, y);
        }

    }
}
