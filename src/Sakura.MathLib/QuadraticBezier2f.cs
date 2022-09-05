
namespace Sakura.MathLib
{
    public struct QuadraticBezier2f
    {
        public Vector2f Start;
        public Vector2f C1;
        public Vector2f End;

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Vector2f At(float t)
        {
            double it = 1 - t;
            double it2 = it * it;
            double t2 = (double)t * t;

            double a = it2;
            double b = 2 * it * t;
            double c = t2;

            double x = a * Start.X + b * C1.X + c * End.X;
            double y = a * Start.Y + b * C1.Y + c * End.Y;

            return new Vector2f((float)x, (float)y);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Vector2f Derivative(float t)
        {
            double it = 1 - (double)t;

            double a = 2 * it;
            double b = 2 * (double)t;

            double x = a * (C1.X - Start.X) + b * (End.X - C1.X);
            double y = a * (C1.Y - Start.Y) + b * (End.Y - C1.Y);

            return new Vector2f((float)x, (float)y);
        }

    }
}
