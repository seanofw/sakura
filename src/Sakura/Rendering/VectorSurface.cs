using System.Drawing;
using System.Windows.Forms;
using Sakura.MathLib;
using Sakura.Model;
using SkiaSharp;

namespace Sakura.Rendering
{
	public class VectorSurface : Control
	{
		public Document Document
		{
			get => _document;
			set
			{
				_document = value;
				UpdateBitmap();
				if (_paintedAtLeastOnce)
				{
					using (Graphics graphics = CreateGraphics())
					{
						RenderControl(graphics);
					}
				}
				else
				{
					Invalidate();
				}
			}
		}
		private Document _document = new Document();

		public Camera Camera
		{
			get => _camera;
			set
			{
				_camera = value;
				UpdateBitmap();
				if (_paintedAtLeastOnce)
				{
					using (Graphics graphics = CreateGraphics())
					{
						RenderControl(graphics);
					}
				}
				else
				{
					Invalidate();
				}
			}
		}
		private Camera _camera = new Camera();

		private Bitmap? _bitmap;
		private bool _paintedAtLeastOnce;

		public Vector2i BitmapSize => _bitmapSize;
		private Vector2i _bitmapSize;

		public Vector2i Center => _center;
		private Vector2i _center;

		public VectorSurface()
		{
			ResizeRedraw = true;
			DoubleBuffered = true;

			Camera = new Camera(
				position: new Vector2d(1.0, 1.0),
				angle: Math.PI / 2,
				zoom: 2.0
			);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				if (_bitmap != null)
				{
					_bitmap.Dispose();
					_bitmap = null;
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			UpdateBitmap();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			RenderControl(e.Graphics);
			_paintedAtLeastOnce = true;
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			double multiplier = Math.Exp(e.Delta / 300.0);

			Camera = Camera.WithZoom(Camera.Zoom * multiplier);
		}

		private void RenderControl(Graphics graphics)
		{
			Rectangle containerRectangle = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

			containerRectangle.Width--;
			containerRectangle.Height--;
			graphics.DrawRectangle(new Pen(new SolidBrush(System.Drawing.Color.LightGray)), containerRectangle);

			if (DesignMode)
			{
				containerRectangle.X++;
				containerRectangle.Y++;
				containerRectangle.Width--;
				containerRectangle.Height--;
				graphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), containerRectangle);
				return;
			}

			if (_bitmap != null)
			{
				graphics.DrawImageUnscaled(_bitmap, new Point(1, 1));
			}
		}

		private void UpdateBitmap()
		{
			Size size = new Size(ClientSize.Width - 2, ClientSize.Height - 2);

			if (_bitmap != null)
			{
				_bitmap.Dispose();
				_bitmap = null;
			}

			_bitmapSize = new Vector2i(size.Width, size.Height);
			_center = _bitmapSize / 2;
			_bitmap = DesignMode ? null : RenderToBitmap(_bitmapSize);
		}

		private Bitmap? RenderToBitmap(Vector2i size)
		{
			if (size.X <= 0 || size.Y <= 0) return null;

			Bitmap bitmap = new Bitmap(size.X, size.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			try
			{
				System.Drawing.Imaging.BitmapData data = bitmap.LockBits(
					new Rectangle(0, 0, size.X, size.Y),
					System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);
				try
				{
					using (SKSurface surface = SKSurface.Create(new SKImageInfo(width: size.X, height: size.Y,
						colorType: SKImageInfo.PlatformColorType, alphaType: SKAlphaType.Premul), data.Scan0, size.X * 4))
					{
						using (SKCanvas canvas = surface.Canvas)
						{
							RenderOnCanvas(size, canvas);
						}
					}
				}
				finally
				{
					bitmap.UnlockBits(data);
				}

				return bitmap;
			}
			catch (Exception)
			{
				bitmap.Dispose();
				throw;
			}
		}

		private void RenderOnCanvas(Vector2i size, SKCanvas canvas)
		{
			canvas.Clear(new SKColor(0xFF, 0xFF, 0xFF));

			using (SKPaint paint = new SKPaint
			{
				Color = new SKColor(255, 224, 224),
				StrokeWidth = 4,
				IsAntialias = true,
			})
			{
				canvas.DrawLine(new SKPoint(0, 0), new SKPoint(size.X, size.Y), paint);
				canvas.DrawLine(new SKPoint(size.X, 0), new SKPoint(0, size.Y), paint);
			}

			//Document.Render(canvas, camera);
		}

		/// <summary>
		/// Given a point in world space, transform it to the equivalent point in screen
		/// space (still in double-precision floating point), relative to the camera being
		/// at the center of the screen.
		/// </summary>
		/// <param name="worldPoint">The world point to convert to a screen point.</param>
		/// <returns>The screen point, in canvas coordinates.</returns>
		public Vector2d WorldToScreen(Vector2d worldPoint)
		{
			Vector2d transformedPoint = Camera.WorldToScreenMatrix * worldPoint;
			return new Vector2d(Center.X + transformedPoint.X, Center.Y - transformedPoint.Y);
		}

		/// <summary>
		/// Given a point in screen space, transform it to the equivalent point in world
		/// space, relative to the camera being at the center of the screen.
		/// </summary>
		/// <param name="screenPoint">The screen point to convert to a world point.</param>
		/// <returns>The world point, absolute coordinates.</returns>
		public Vector2d ScreenToWorld(Vector2d screenPoint)
		{
			Vector2d transformedPoint = new Vector2d(screenPoint.X - Center.X, Center.Y - screenPoint.Y);
			return Camera.WorldToScreenMatrix * transformedPoint;
		}
	}
}
