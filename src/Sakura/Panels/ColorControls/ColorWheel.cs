using System.Windows.Forms;
using Sakura.MathLib;

namespace Sakura.Panels.ColorControls
{
	public class ColorWheel : Control
	{
		public DeepColor Color
		{
			get => _color;
			set
			{
				if (value != _color)
				{
					_color = value;
					_hsvColor = new DeepHsvColor(new DeepColor(value.R, value.G, value.B, value.A));
					_point = PointFromColor(_color);
					Invalidate();
				}
			}
		}
		private DeepColor _color = DeepColor.White;
		private DeepHsvColor _hsvColor = new DeepHsvColor(0, 65535, 65535);

		public float WheelSize
		{
			get => _wheelSize;
			set
			{
				if (value != _wheelSize)
				{
					_wheelSize = value;
					_wheelSizeInternal = (int)(_wheelSize * 65536 + 0.5f);
					_point = PointFromColor(_color);
					_bitmapNeedsRedraw = true;
					Invalidate();
				}
			}
		}
		private float _wheelSize = 0.25f;
		private int _wheelSizeInternal = 65536 / 4;

		public ColorWheelMode Mode
		{
			get => _mode;
			set
			{
				if (value != _mode)
				{
					_mode = value;
					_point = PointFromColor(_color);
					_bitmapNeedsRedraw = true;
					Invalidate();
				}
			}
		}
		private ColorWheelMode _mode;

		public event EventHandler? Changing;
		public event EventHandler? Changed;
		public event EventHandler? ChangeCanceled;

		public bool IsDragging { get; private set; }

		public bool Focusable { get; set; }

		private DeepColor _cancelColor;
		private System.Drawing.Point _point;
		private System.Drawing.Point _center;
		private int _radius;
		private bool _bitmapNeedsRedraw;
		private System.Drawing.Bitmap? _bitmap;
		private System.Drawing.Pen _blackPen, _whitePen;

		public ColorWheel()
		{
			_bitmap = new System.Drawing.Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			_blackPen = new System.Drawing.Pen(System.Drawing.Color.Black);
			_whitePen = new System.Drawing.Pen(System.Drawing.Color.White);
			ResizeRedraw = true;
			DoubleBuffered = true;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				_bitmap?.Dispose();
				_bitmap = null;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			System.Drawing.Point newCenter = new System.Drawing.Point(
				(ClientRectangle.Left + ClientRectangle.Right) / 2,
				(ClientRectangle.Top + ClientRectangle.Bottom) / 2);

			int newRadius = (Math.Min(ClientRectangle.Width / 2, ClientRectangle.Height / 2) - 1) * 256;
			if (newRadius != _radius || _center != newCenter)
			{
				_radius = newRadius;
				_center = newCenter;
				_point = PointFromColor(_color);

				_bitmap?.Dispose();
				_bitmap = new System.Drawing.Bitmap(ClientRectangle.Width, ClientRectangle.Height,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				_bitmapNeedsRedraw = true;
			}
		}

		public System.Drawing.Point PointFromColor(DeepColor color)
		{
			DeepHsvColor hsvColor = (DeepHsvColor)(new DeepColor(color.R, color.G, color.B, color.A));

			float angle = hsvColor.H * (MathF.PI / 18000);
			float sin = MathF.Sin(angle);
			float cos = MathF.Cos(angle);

			float radius = Mode switch
			{
				ColorWheelMode.HueSaturation => _radius * hsvColor.Sf,
				ColorWheelMode.HueValue => _radius * hsvColor.Vf,
				_ => _radius * (1 - WheelSize * 0.5f),
			};

			return new System.Drawing.Point(
				_center.X + (int)(sin * radius * (1f/256) + 0.5f),
				_center.Y - (int)(cos * radius * (1f/256) + 0.5f));
		}

		public DeepColor ColorFromPoint(System.Drawing.Point point)
		{
			ColorFromPointInternal(point.X, point.Y, _center.X, _center.Y,
				Mode, _radius, _wheelSizeInternal, _hsvColor, out DeepColor result);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private static int ColorFromPointInternal(
			int px, int py, int cx, int cy,
			ColorWheelMode mode, int radius, int wheelSize,
			in DeepHsvColor baseline,
			out DeepColor result)
		{
			result = default;

			// Medium-sized power-of-2, larger than 256, smaller than 10922 (=65536/6).  This will be the
			// range of the hue value that we'll calculate before transforming it into RGB.
			const int Magic = 4096;

			int sin = px - cx;
			int cos = cy - py;
			int hue = (int)(MathF.Atan2(sin, cos) * (Magic / (2 * MathF.PI)) + 0.5f);
			int dist = (int)(MathF.Sqrt(sin * sin + cos * cos) * 256 + 0.5f);
			if (hue < 0)
				hue += Magic;
			int amount;

			int mix;

			ushort sat, value;
			if (mode == ColorWheelMode.HueSaturation || mode == ColorWheelMode.HueValue)
			{
				if (dist >= radius + 256)
					(mix, amount) = (65536, 65535);
				else if (dist >= radius)
					(mix, amount) = ((dist - radius) * 256, 65535);
				else
					(mix, amount) = (0, (int)(radius > 0 ? (long)dist * 65535 / radius : 0));
				(sat, value) = (mode == ColorWheelMode.HueSaturation
					? ((ushort)amount, baseline.V)
					: (baseline.S, (ushort)amount));
			}
			else
			{
				int innerRadius = (int)((long)radius * (65536 - wheelSize) / 65536);
				if (dist >= radius + 256)
					mix = 65536;
				else if (dist >= radius)
					mix = (dist - radius) * 256;
				else if (dist >= innerRadius)
					mix = 0;
				else if (dist >= innerRadius - 256)
					mix = (innerRadius - dist) * 256;
				else
					mix = 65536;
				sat = baseline.S;
				value = baseline.V;
			}

			result.A = baseline.A;

			if (sat == 0)
			{
				result.R = result.G = result.B = value;
				return mix;
			}

			int v = value;
			hue *= 6;
			int vs = (int)((long)v * sat / 65535);
			int vsf = (vs * (hue % Magic)) / Magic;

			switch (hue / Magic)
			{
				case 0: result.R = (ushort)v; result.G = (ushort)(v - vs + vsf); result.B = (ushort)(v - vs); return mix;
				case 1: result.R = (ushort)(v - vsf); result.G = (ushort)v; result.B = (ushort)(v - vs); return mix;
				case 2: result.R = (ushort)(v - vs); result.G = (ushort)v; result.B = (ushort)(v - vs + vsf); return mix;
				case 3: result.R = (ushort)(v - vs); result.G = (ushort)(v - vsf); result.B = (ushort)v; return mix;
				case 4: result.R = (ushort)(v - vs + vsf); result.G = (ushort)(v - vs); result.B = (ushort)v; return mix;
				case 5: result.R = (ushort)v; result.G = (ushort)(v - vs); result.B = (ushort)(v - vsf); return mix;
				default: result.R = result.G = result.B = 0; return mix;  // Should never get here.
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			IsDragging = true;
			_cancelColor = Color;
			UpdatePoint(e.Location);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			IsDragging = false;
			if (_cancelColor != Color)
			{
				EventHandler? changedEventHandler = Changed;
				changedEventHandler?.Invoke(this, EventArgs.Empty);
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (IsDragging && e.KeyCode == Keys.Escape)
			{
				IsDragging = false;
				Color = _cancelColor;
				ChangeCanceled?.Invoke(this, EventArgs.Empty);
			}

			if (Mode != ColorWheelMode.HueOnly)
			{
				int distance = e.Control || e.Shift ? Math.Max((int)(_radius * (1f/256) / 10 + 0.5f), 1) : 1;
				if (e.KeyCode == Keys.Left)
				{
					UpdatePoint(new System.Drawing.Point(_point.X - distance, _point.Y));
					return;
				}
				if (e.KeyCode == Keys.Right)
				{
					UpdatePoint(new System.Drawing.Point(_point.X + distance, _point.Y));
					return;
				}
				if (e.KeyCode == Keys.Up)
				{
					UpdatePoint(new System.Drawing.Point(_point.X, _point.Y - distance));
					return;
				}
				if (e.KeyCode == Keys.Down)
				{
					UpdatePoint(new System.Drawing.Point(_point.X, _point.Y + distance));
					return;
				}
			}
			else
			{
				int distance = e.Control || e.Shift ? 36 : 1;
				if (e.KeyCode == Keys.Left)
				{
					UpdateHue(_hsvColor.H + distance);
					return;
				}
				if (e.KeyCode == Keys.Right)
				{
					UpdateHue(_hsvColor.H - distance);
					return;
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!IsDragging)
				return;

			UpdatePoint(e.Location);
		}

		private void UpdatePoint(System.Drawing.Point point)
		{
			DeepColor newColor = ColorFromPoint(point);
			if (newColor != Color)
			{
				Color = newColor;
				EventHandler? changingEventHandler = Changing;
				changingEventHandler?.Invoke(this, EventArgs.Empty);
			}
		}

		private void UpdateHue(int newHue)
		{
			DeepColor newColor = (DeepColor)new DeepHsvColor(newHue, _hsvColor.S, _hsvColor.V, _hsvColor.A);
			if (newColor != Color)
			{
				Color = newColor;
				EventHandler? changingEventHandler = Changing;
				changingEventHandler?.Invoke(this, EventArgs.Empty);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (_bitmapNeedsRedraw)
			{
				RedrawBitmap();
				_bitmapNeedsRedraw = false;
			}

			if (_bitmap != null)
				e.Graphics.DrawImageUnscaled(_bitmap, ClientRectangle.X, ClientRectangle.Y);

			e.Graphics.DrawRectangle(_blackPen, _point.X - 2, _point.Y - 2, 4, 4);
			e.Graphics.DrawRectangle(_whitePen, _point.X - 3, _point.Y - 3, 6, 6);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// We do all painting during OnPaint, so there's nothing to do here.
		}

		private void RedrawBitmap()
		{
			if (_bitmap == null)
				return;

			DeepColor b = new DeepColor(new Color(BackColor.R, BackColor.G, BackColor.B, BackColor.A));

			DeepHsvColor baseline = _hsvColor;

			System.Drawing.Bitmap bitmap = _bitmap;
			System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			try
			{
				unsafe
				{
					byte* imagePtr = (byte*)bitmapData.Scan0;
					for (int y = 0; y < bitmap.Height; y++)
					{
						byte* rowPtr = imagePtr + y * bitmapData.Stride;
						for (int x = 0; x < bitmap.Width; x++)
						{
							int mix = ColorFromPointInternal(
								x + ClientRectangle.X, y + ClientRectangle.Y,
								_center.X, _center.Y,
								Mode, _radius, _wheelSizeInternal,
								baseline, out DeepColor c);
							if (mix >= 65536)
								c = b;
							else if (mix > 0)
								c = c.Mix(b, mix);
							rowPtr[0] = (byte)(c.B >> 8);
							rowPtr[1] = (byte)(c.G >> 8);
							rowPtr[2] = (byte)(c.R >> 8);
							rowPtr[3] = (byte)(c.A >> 8);
							rowPtr += 4;
						}
					}
				}
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}
	}
}
