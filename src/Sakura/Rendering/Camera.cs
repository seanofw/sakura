using Sakura.MathLib;

namespace Sakura.Rendering
{
	public class Camera
	{
		public Vector2d Position { get; }
		public bool FlipHorz { get; }
		public bool FlipVert { get; }
		public double Angle { get; }
		public double Zoom { get; }

		public double ZoomMultiplier => _zoomMultiplier.HasValue
			? _zoomMultiplier.Value
			: (_zoomMultiplier = Math.Pow(2.0, Zoom)).Value;
		private double? _zoomMultiplier;

		/// <summary>
		/// A matrix that converts from world space to screen space (but oriented around
		/// the *zero* of the screen).
		/// </summary>
		public Matrix3x2d WorldToScreenMatrix => _worldToScreenMatrix ??=
			  Matrix3x2d.Rotate(Angle)
			* Matrix3x2d.Scale(FlipHorz ? ZoomMultiplier : -ZoomMultiplier,
				FlipVert ? ZoomMultiplier : -ZoomMultiplier)
			* Matrix3x2d.Translate(Position.X, Position.Y);
		private Matrix3x2d? _worldToScreenMatrix;

		/// <summary>
		/// A matrix that converts from screen space to world space (but oriented around
		/// the *zero* of the screen).
		/// </summary>
		public Matrix3x2d ScreenToWorldMatrix => _screenToWorldMatrix ??= WorldToScreenMatrix.Invert();
		private Matrix3x2d? _screenToWorldMatrix;

		/// <summary>
		/// Create a new camera.
		/// </summary>
		/// <param name="position">The camera's position over the world.</param>
		/// <param name="flipHorz">Whether to flip the camera horizontally.</param>
		/// <param name="flipVert">Whether to flip the camera vertically.</param>
		/// <param name="angle">The camera's rotation angle, in radians.</param>
		/// <param name="zoom">The camera's zoom exponent (-2 means quarter size,
		/// -1 means half size, 0 is one-to-one, 1 means double size, 2 means quadruple size, etc.).</param>
		public Camera(
			Vector2d position = default,
			bool flipHorz = false,
			bool flipVert = false,
			double angle = 0.0,
			double zoom = 0.0)
		{
			Position = position;
			Angle = angle;
			FlipHorz = flipHorz;
			FlipVert = flipVert;
			Zoom = zoom;
		}

		public Camera WithPosition(Vector2d position)
			=> new Camera(position: position, flipHorz: FlipHorz, flipVert: FlipVert, angle: Angle, zoom: Zoom);
		public Camera WithFlip(bool flipHorz, bool flipVert)
			=> new Camera(position: Position, flipHorz: flipHorz, flipVert: flipVert, angle: Angle, zoom: Zoom);
		public Camera WithAngle(double angle)
			=> new Camera(position: Position, flipHorz: FlipHorz, flipVert: FlipVert, angle: angle, zoom: Zoom);
		public Camera WithZoom(double zoom)
			=> new Camera(position: Position, flipHorz: FlipHorz, flipVert: FlipVert, angle: Angle, zoom: zoom);
	}
}
