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

		public MainWindow MainWindow { get; }

		private bool _paintedAtLeastOnce;

		private DeepBitmap _deepBitmap = new DeepBitmap();

		public Vector2i Center => _center;
		private Vector2i _center;

		public VectorSurface(MainWindow mainWindow)
		{
			MainWindow = mainWindow;

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
				_deepBitmap.Dispose();
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

		protected override void OnMouseEnter(EventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnMouseEnter(e);

		protected override void OnMouseLeave(EventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnMouseLeave(e);

		protected override void OnMouseMove(MouseEventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnMouseMove(e);

		protected override void OnMouseDown(MouseEventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnMouseDown(e);

		protected override void OnMouseUp(MouseEventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnMouseUp(e);

		protected override void OnMouseClick(MouseEventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnMouseClick(e);

		protected override void OnMouseDoubleClick(MouseEventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnMouseDoubleClick(e);

		protected override void OnMouseCaptureChanged(EventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnMouseCaptureChanged(e);

		protected override void OnKeyDown(KeyEventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnKeyDown(e);

		protected override void OnKeyUp(KeyEventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnKeyUp(e);

		protected override void OnKeyPress(KeyPressEventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnKeyPress(e);

		protected override void OnGotFocus(EventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnGotFocus(e);

		protected override void OnLostFocus(EventArgs e)
			=> MainWindow.CurrentToolInfo?.ToolMechanics?.OnLostFocus(e);

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

			graphics.Draw(_deepBitmap, new Vector2i(0, 0), new Vector2i(1, 1),
				new Vector2i(ClientSize.Width - 2, ClientSize.Height - 2));
		}

		private unsafe void UpdateBitmap()
		{
			Vector2i size = new Vector2i(ClientSize.Width - 2, ClientSize.Height - 2);

			_deepBitmap.Size = size;
			_center = size / 2;

			if (!DesignMode)
			{
				Render();
			}
		}

		public unsafe void Render()
		{
			RenderToBitmap(new Vector2i(ClientSize.Width - 2, ClientSize.Height - 2), _deepBitmap);
		}

		private unsafe void RenderToBitmap(Vector2i size, DeepBitmap deepBitmap)
		{
			if (size.X <= 0 || size.Y <= 0) return;

			using (SKSurface surface = SKSurface.Create(new SKImageInfo(width: deepBitmap.Size.X, height: deepBitmap.Size.Y,
					colorType: SKColorType.Rgba16161616, alphaType: SKAlphaType.Unpremul), (IntPtr)deepBitmap.GetData(), deepBitmap.NativeSize.X * 8))
			{
				using (SKCanvas canvas = surface.Canvas)
				{
					RenderOnCanvas(size, canvas);
				}
			}
		}

		private void RenderOnCanvas(Vector2i size, SKCanvas canvas)
		{
			canvas.Clear(new SKColor(0xFF, 0xFF, 0xFF));

			using (SKPaint paint = new SKPaint
			{
				Color = new SKColor(64, 96, 128),
				StrokeWidth = 4,
				IsAntialias = false,
			})
			{
				canvas.DrawLine(new SKPoint(0, 0), new SKPoint(size.X, size.Y), paint);
			}

			using (SKPaint paint = new SKPaint
			{
				Color = new SKColor(255, 224, 192),
				StrokeWidth = 4,
				IsAntialias = false,
			})
			{
				canvas.DrawLine(new SKPoint(size.X, 0), new SKPoint(0, size.Y), paint);
			}

			using (SKPaint paint = new SKPaint
			{
				Color = new SKColor(255, 0, 0, 128),
				IsAntialias = false,
			})
			{
				canvas.DrawRect(new SKRect(10, 10, 110, 110), paint);
			}

			using (SKPaint paint = new SKPaint
			{
				Color = new SKColor(0, 255, 0, 128),
				IsAntialias = false,
			})
			{
				canvas.DrawRect(new SKRect(50, 50, 150, 150), paint);
			}

			using (SKPaint paint = new SKPaint
			{
				Color = new SKColor(0, 0, 255, 128),
				IsAntialias = false,
			})
			{
				canvas.DrawRect(new SKRect(90, 90, 190, 190), paint);
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
