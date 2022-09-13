using System.Drawing;
using System.Runtime.InteropServices;
using Sakura.MathLib;

namespace Sakura.Rendering
{
	/// <summary>
	/// A bitmap that contains DeepColor data but can still be rendered on screen.
	/// </summary>
	public class DeepBitmap : IDisposable
	{
		private IntPtr _bitmap;
		private Vector2i _bitmapSize;

		private unsafe DeepColor* _renderBuffer;
		private long _renderBufferSize;

		private unsafe byte* _bitmapBuffer;

		private bool _isReady;

		/// <summary>
		/// Get the effective size of this bitmap, or resize it.  Note that resizing it
		/// *will* destroy its contents.
		/// </summary>
		public Vector2i Size
		{
			get => _size;
			set
			{
				if (_size != value)
				{
					_size = value;
					SetSize(value);
				}
			}
		}
		private Vector2i _size;

		/// <summary>
		/// Get the dimensions of the underlying image data.  You will need this when
		/// accessing the data pointer, because it determines the actual width of each
		/// row of pixels.  Accessing this does *not* mark the bitmap as being in need
		/// of reification.
		/// </summary>
		public Vector2i NativeSize => _bitmapSize;

		/// <summary>
		/// Construct a new, empty DeepBitmap.
		/// </summary>
		public DeepBitmap()
		{
			unsafe
			{
				_renderBufferSize = 0;
				_renderBuffer = (DeepColor*)Marshal.AllocHGlobal((IntPtr)(_renderBufferSize * sizeof(DeepColor)));
				_bitmapBuffer = null;
			}

			_bitmap = IntPtr.Zero;
		}

		/// <summary>
		/// Contruct a new DeepBitmap of the given size.
		/// </summary>
		/// <param name="size">The size of the new DeepBitmap, in pixels.</param>
		public DeepBitmap(Vector2i size)
			: this()
		{
			Size = size;
		}

		/// <summary>
		/// Clean up after this DeepBitmap if it is being reclaimed by the GC.
		/// </summary>
		~DeepBitmap()
		{
			Dispose(false);
		}

		/// <summary>
		/// Dispose of this DeepBitmap, and release all of its resources.
		/// </summary>
		/// <param name="isDisposing"></param>
		private unsafe void Dispose(bool isDisposing)
		{
			if (_bitmap != IntPtr.Zero)
			{
				Win32.DeleteObject(_bitmap);
				_bitmap = IntPtr.Zero;
			}

			if (_renderBuffer != null)
			{
				Marshal.FreeHGlobal((IntPtr)_renderBuffer);
				_renderBuffer = null;
			}
		}

		/// <summary>
		/// Dispose of this DeepBitmap, and release all of its resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Draw the entire DeepBitmap on the given graphics surface, reifying it
		/// for display if necessary.
		/// </summary>
		/// <param name="graphics">The graphics surface to draw on.</param>
		/// <param name="destPoint">The location on the graphics surface where the top-left pixel
		/// of the DeepBitmap should be drawn.</param>
		public void Draw(Graphics graphics, Vector2i destPoint)
			=> Draw(graphics, Vector2i.Zero, destPoint, Size);

		/// <summary>
		/// Draw a subrect of the DeepBitmap on the given graphics surface, reifying it
		/// for display if necessary.
		/// </summary>
		/// <param name="graphics">The graphics surface to draw on.</param>
		/// <param name="srcPoint">The location of the top-left pixel in the DeepBitmap to draw.</param>
		/// <param name="destPoint">The location on the graphics surface where the top-left pixel should be.</param>
		/// <param name="size">The size of the subrect of the DeepBitmap to draw.</param>
		public unsafe void Draw(Graphics graphics, Vector2i srcPoint, Vector2i destPoint, Vector2i size)
		{
			if (_bitmap == IntPtr.Zero)
				return;

			if (!_isReady)
				Reify();

			IntPtr hdc = graphics.GetHdc();
			try
			{
				IntPtr hmemdc = Win32.CreateCompatibleDC(hdc);
				try
				{
					IntPtr oldBitmap = Win32.SelectObject(hmemdc, _bitmap);
					try
					{
						bool result = Win32.BitBlt(hdc, destPoint.X, destPoint.Y, size.X, size.Y,
							hmemdc, srcPoint.X, srcPoint.Y, Win32.TernaryRasterOperations.SRCCOPY);
					}
					finally
					{
						Win32.SelectObject(hmemdc, oldBitmap);
					}
				}
				finally
				{
					Win32.DeleteDC(hmemdc);
				}
			}
			finally
			{
				graphics.ReleaseHdc(hdc);
			}
		}

		/// <summary>
		/// Obtain direct access to the DeepBitmap's pixel data.  This will implicitly
		/// cause the DeepBitmap to become invalidated and need reification the next time
		/// that it is drawn.
		/// </summary>
		/// <returns>A pointer to the top-left pixel of the DeepBitmap.  This pointer
		/// will be valid until either the DeepBitmap is destroyed or resized.</returns>
		public unsafe DeepColor* GetData()
		{
			_isReady = false;
			return _renderBuffer;
		}

		/// <summary>
		/// Mark this DeepBitmap as having been modified.  This will cause it to be
		/// reified during the next time it is Draw()n.  If you modify the DeepBitmap's
		/// data, you should always invoke this method to ensure that when it is drawn, it
		/// is drawn correctly.  This only sets a flag, though, so you can call it as often
		/// as you need to.
		/// </summary>
		public void Update()
		{
			_isReady = false;
		}

		/// <summary>
		/// Reify this DeepBitmap:  Transform it into something that Windows knows how
		/// to actually draw on the screen.
		/// </summary>
		private unsafe void Reify()
		{
			Native.DemoteImage16To8((IntPtr)_bitmapBuffer, (IntPtr)_renderBuffer,
				_bitmapSize.X, _bitmapSize.Y, _bitmapSize.X * 8, _bitmapSize.X * 4);
		}

		/// <summary>
		/// Resize this DeepBitmap, avoiding reallocation if possible.
		/// </summary>
		/// <param name="size">The new size for this DeepBitmap.</param>
		private unsafe void SetSize(Vector2i size)
		{
			Vector2i renderSize = new Vector2i((size.X + 0x3F) & ~0x3F, (size.Y + 0x3F) & ~0x3F);

			if (renderSize.X > _bitmapSize.X || renderSize.Y > _bitmapSize.Y)
			{
				if (_bitmap != IntPtr.Zero)
				{
					Win32.DeleteObject(_bitmap);
					_bitmap = IntPtr.Zero;
				}
				_bitmapSize = renderSize.Max(_bitmapSize);
			}
			else if (renderSize.X * 2 < _bitmapSize.X || renderSize.Y * 2 < _bitmapSize.Y)
			{
				if (_bitmap != IntPtr.Zero)
				{
					Win32.DeleteObject(_bitmap);
					_bitmap = IntPtr.Zero;
				}
				_bitmapSize = renderSize.Max(new Vector2i(16, 16));
			}

			long newRenderSize = (long)_bitmapSize.X * _bitmapSize.Y;
			if (newRenderSize > _renderBufferSize || newRenderSize * 2 < _renderBufferSize)
			{
				if (_renderBuffer != null)
				{
					Marshal.FreeHGlobal((IntPtr)_renderBuffer);
					_renderBuffer = null;
				}
				_renderBufferSize = newRenderSize;
				_renderBuffer = (DeepColor*)Marshal.AllocHGlobal((IntPtr)(_renderBufferSize * sizeof(DeepColor)));
			}

			if (_bitmap == IntPtr.Zero)
			{
				Win32.BITMAPINFO bmih = new Win32.BITMAPINFO
				{
					bmiHeader = new Win32.BITMAPINFOHEADER
					{
						biSize = (uint)sizeof(Win32.BITMAPINFOHEADER),
						biWidth = _bitmapSize.X,
						biHeight = -_bitmapSize.Y,
						biPlanes = 1,
						biBitCount = 32,
						biCompression = Win32.BitmapCompressionMode.BI_RGB,
						biSizeImage = (uint)(_bitmapSize.X * _bitmapSize.Y * 4),
						biXPelsPerMeter = 2835,
						biYPelsPerMeter = 2835,
						biClrUsed = 0,
						biClrImportant = 0,
					}
				};
				IntPtr hdc = Win32.GetDC(IntPtr.Zero);
				try
				{
					_bitmap = Win32.CreateDIBSection(hdc, ref bmih, 0, out IntPtr bits, IntPtr.Zero, 0);
					_bitmapBuffer = _bitmap != IntPtr.Zero ? (byte*)bits : null;
				}
				finally
				{
					Win32.ReleaseDC(IntPtr.Zero, hdc);
				}
			}
		}
	}

	public static class DeepBitmapGraphicsExtensions
	{
		/// <summary>
		/// Draw a subrect of the DeepBitmap on the given graphics surface, reifying it
		/// for display if necessary.
		/// </summary>
		/// <param name="graphics">The graphics surface.</param>
		/// <param name="deepBitmap">The DeepBitmap to draw.</param>
		/// <param name="srcPoint">The location of the top-left pixel in the DeepBitmap to draw.</param>
		/// <param name="destPoint">The location on the graphics surface where the top-left pixel should be.</param>
		/// <param name="size">The size of the subrect of the DeepBitmap to draw.</param>
		public static void Draw(this Graphics graphics, DeepBitmap deepBitmap,
			Vector2i srcPoint, Vector2i destPoint, Vector2i size)
		{
			deepBitmap.Draw(graphics, srcPoint, destPoint, size);
		}

		/// <summary>
		/// Draw the entire DeepBitmap on the given graphics surface, reifying it
		/// for display if necessary.
		/// </summary>
		/// <param name="graphics">The graphics surface.</param>
		/// <param name="deepBitmap">The DeepBitmap to draw.</param>
		/// <param name="destPoint">The location on the graphics surface where the top-left pixel should be.</param>
		public static void Draw(this Graphics graphics, DeepBitmap deepBitmap, Vector2i destPoint)
		{
			deepBitmap.Draw(graphics, Vector2i.Zero, destPoint, deepBitmap.Size);
		}
	}
}
