using System.Runtime.InteropServices;
using Sakura.MathLib;

namespace Sakura.Img
{
	/// <summary>
	/// An image abstraction:  A rectangular grid of colors, in 24-bit (plus 8-bit alpha) mode,
	/// with methods to query and manipulate it.  Importantly, the image is always stored in an
	/// ordinary managed array of pixel data, so that it can be directly accessed and
	/// manipulated by the program.
	/// </summary>
	/// <remarks>
	/// Note that because the data is always stored in managed memory, all built-in image
	/// operations provided by this class are therefore performed on the CPU, not the GPU.  They
	/// are unrolled and optimized and written with unsafe and pointers so that they'll run as
	/// fast as possible on the CPU, but this class is written with *direct access* to the
	/// underlying data as a key design constraint:  Unlike many image/bitmap systems, you
	/// always have direct access to the underlying bytes of the image if you want it.
	/// 
	/// So this class is intended to be as capable as possible, and to offer many useful
	/// drawing/transformation/mutation operations directly, while still being completely
	/// managed code that runs on an ordinary CPU.
	/// </remarks>
	[DebuggerDisplay("Image {Width}x{Height}")]
	public class Image : IEquatable<Image>
	{
		#region Core properties and fields

		/// <summary>
		/// The actual underlying data of this image, which is always guaranteed to be
		/// a one-dimensional contiguous array of 32-bit RGBA color tuples, in order of
		/// left-to-right, top-to-bottom, with no gaps or padding.  (i.e., the length
		/// of the data will always equal Width*Height.)
		/// </summary>
		private readonly Color[] _data;

		/// <summary>
		/// The width of the image, in pixels.
		/// </summary>
		public int Width { get; }

		/// <summary>
		/// The height of the image, in pixels.
		/// </summary>
		public int Height { get; }

		#endregion

		#region Derived properties and static data

		/// <summary>
		/// Obtain direct access to the underlying array of color data, which you
		/// can then manipulate directly, or even pin and manipulate using pointers.
		/// </summary>
		/// <returns>The underlying array of color data.</returns>
		public Color[] Data => _data;

		/// <summary>
		/// The size of this image, represented as a 2D vector.
		/// </summary>
		public Vector2i Size => new Vector2i(Width, Height);

		/// <summary>
		/// A static "empty" image that never has pixel data in it.
		/// </summary>
		public static Image Empty { get; } = new Image(0, 0);

		#endregion

		#region Pixel-plotting

		/// <summary>
		/// Access the pixels using "easy" 2D array-brackets.  This is safe, easy, reliable,
		/// and slower than almost every other way of accessing the pixels, but it's very useful
		/// for simple cases.  Reading a pixel outside the image will return 'Transparent,'
		/// and writing a pixel outside the image is a no-op.
		/// </summary>
		/// <param name="x">The X coordinate of the pixel to read or write.</param>
		/// <param name="y">The Y coordinate of the pixel to read or write.</param>
		/// <returns>The color at that pixel.</returns>
		public Color this[int x, int y]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => x < 0 || x >= Width || y < 0 || y >= Height ? Color.Transparent : _data[y * Width + x];

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (x < 0 || x >= Width || y < 0 || y >= Height)
					return;
				_data[y * Width + x] = value;
			}
		}

		#endregion

		#region Construction and conversion

		/// <summary>
		/// Construct an empty image of the given size.  The pixels will initially
		/// be set to 'Transparent'.
		/// </summary>
		/// <param name="size">The dimensions of the image, in pixels.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Image(Vector2i size)
		{
			Width = size.X;
			Height = size.Y;
			_data = new Color[size.X * size.Y];
		}

		/// <summary>
		/// Construct an image of the given size.  The pixels will initially
		/// be set to the given fill color.
		/// </summary>
		/// <param name="size">The dimensions of the image, in pixels.</param>
		/// <param name="fillColor">The color for all of the new pixels in the image.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Image(Vector2i size, Color fillColor)
		{
			Width = size.X;
			Height = size.Y;
			_data = new Color[size.X * size.Y];
			Fill(fillColor);
		}

		/// <summary>
		/// Construct an image of the given size, using the provided 8-bit image data
		/// and color palette to construct it.
		/// </summary>
		/// <param name="width">The width of the new image, in pixels.</param>
		/// <param name="height">The height of the new image, in pixels.</param>
		/// <param name="srcData">The source pixel data, which will be used as indexes
		/// into the provided color palette.</param>
		/// <param name="palette">The color palette, which should be a span of
		/// up to 256 color values.</param>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public Image(int width, int height, ReadOnlySpan<byte> srcData, ReadOnlySpan<Color> palette)
		{
			int size = width * height;

			if (srcData.Length < size)
				throw new ArgumentException("Source data ({srcData.Length} bytes) is too small for image ({Width}x{Height} pixels).");

			Width = width;
			Height = height;
			_data = new Color[size];

			unsafe
			{
				fixed (byte* srcBase = srcData)
				fixed (Color* destBase = _data)
				{
					int count = size;
					byte* src = srcBase;
					Color* dest = destBase;
					while (count > 8)
					{
						dest[0] = palette[src[0]];
						dest[1] = palette[src[1]];
						dest[2] = palette[src[2]];
						dest[3] = palette[src[3]];
						dest[4] = palette[src[4]];
						dest[5] = palette[src[5]];
						dest[6] = palette[src[6]];
						dest[7] = palette[src[7]];
						dest += 8;
						src += 8;
						count -= 8;
					}
					if ((count & 4) != 0)
					{
						dest[0] = palette[src[0]];
						dest[1] = palette[src[1]];
						dest[2] = palette[src[2]];
						dest[3] = palette[src[3]];
						dest += 4;
						src += 4;
					}
					if ((count & 2) != 0)
					{
						dest[0] = palette[src[0]];
						dest[1] = palette[src[1]];
						dest += 2;
						src += 2;
					}
					if ((count & 1) != 0)
						dest[0] = palette[src[0]];
				}
			}
		}

		/// <summary>
		/// Construct an empty image of the given size.  The pixels will initially
		/// be set to 'Transparent'.
		/// </summary>
		/// <param name="width">The width of the new image, in pixels.</param>
		/// <param name="height">The height of the new image, in pixels.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Image(int width, int height)
		{
			Width = width;
			Height = height;
			_data = new Color[width * height];
		}

		/// <summary>
		/// Construct an empty image of the given size.  The pixels will initially
		/// be set to the provided fill color.
		/// </summary>
		/// <param name="width">The width of the new image, in pixels.</param>
		/// <param name="height">The height of the new image, in pixels.</param>
		/// <param name="fillColor">The color for all of the new pixels in the image.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Image(int width, int height, Color fillColor)
		{
			Width = width;
			Height = height;
			_data = new Color[width * height];
			Fill(fillColor);
		}

		/// <summary>
		/// Construct an image from the embedded resource with the given name.  The embedded
		/// resource must be of a file format that this image library is capable of
		/// decoding, like PNG or JPEG.
		/// </summary>
		/// <param name="assembly">The assembly containing the embedded resource.</param>
		/// <param name="name">The name of the embedded image resource to load.  This may use
		/// slashes in the pathname to separate components.</param>
		/// <returns>The newly-loaded image.</returns>
		/// <exception cref="ArgumentException">Thrown if no embedded resource exists with the
		/// given name.</exception>
		public static Image FromEmbeddedResource(Assembly assembly, string name)
		{
			byte[] bytes;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (Stream? stream = assembly.GetManifestResourceStream(name.Replace('/', '.').Replace('\\', '.')))
				{
					if (stream == null)
						throw new ArgumentException($"Embedded resource '{name}' not found.");
					stream.CopyTo(memoryStream);
				}
				bytes = memoryStream.ToArray();
			}

			return LoadFile(bytes);
		}

		/// <summary>
		/// Load the given file as a new image, which must be of a supported image
		/// format like PNG or JPEG.
		/// </summary>
		/// <param name="filename">The filename of the image to load.</param>
		/// <returns>The new image.</returns>
		public static Image LoadFile(string filename)
		{
			byte[] bytes = File.ReadAllBytes(filename);
			return LoadFile(bytes);
		}

		/// <summary>
		/// Load the given chunk of bytes in memory as a new image, which must be
		/// encoded as a supported image file format like PNG or JPEG.
		/// </summary>
		/// <param name="data">The data of the image file to load.</param>
		/// <returns>The new image.</returns>
		public static Image LoadFile(ReadOnlySpan<byte> data)
		{
			return new Image(1, 1);
		}

		/// <summary>
		/// Save the image to disk as the given file format.  This does not use maximum
		/// compression; it just ensures that the output is an acceptable implementation
		/// of the given file format.
		/// </summary>
		/// <param name="filename">The filename to write.</param>
		/// <param name="format">The image format to produce.</param>
		public void SaveFile(string filename, ImageFormat format)
		{
			byte[] bytes = SaveFile(format);
			File.WriteAllBytes(filename, bytes);
		}

		/// <summary>
		/// Save the image to an array of bytes as the given file format.  This does not
		/// use maximum compression; it just ensures that the output is an acceptable
		/// implementation of the given file format.
		/// </summary>
		/// <param name="format">The image format to produce.</param>
		/// <returns>An array of bytes that represents the image in the given file format.</returns>
		public byte[] SaveFile(ImageFormat format)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				SaveFile(stream, format);
				return stream.ToArray();
			}
		}

		/// <summary>
		/// Save the image to the given output stream as the given file format.  This does not
		/// use maximum compression; it just ensures that the output is an acceptable
		/// implementation of the given file format.
		/// </summary>
		/// <param name="stream">The stream to write the image file to.</param>
		/// <param name="format">The image format to produce.</param>
		public unsafe void SaveFile(Stream stream, ImageFormat format)
		{

		}

		/// <summary>
		/// Extract out a copy of the raw data in this image, as bytes.  This is
		/// slower than pinning the raw data and then casting it to a byte pointer, but
		/// it is considerably safer.
		/// </summary>
		public byte[] GetBytes()
		{
			unsafe
			{
				byte[] dest = new byte[Width * Height * 4];
				fixed (byte* destBase = dest)
				fixed (Color* srcBase = _data)
				{
					Buffer.MemoryCopy(srcBase, destBase, Width * Height * 4, Width * Height * 4);
				}
				return dest;
			}
		}

		/// <summary>
		/// Construct an image around the given color data.  This does NOT make a copy
		/// of the data; it *wraps* the provided array directly, which must be at least
		/// width*height in size.  You're responsible for making sure this is used sanely;
		/// if you misuse it, you can break stuff.
		/// </summary>
		public Image(int width, int height, Color[] data)
		{
			if (data.Length < width * height)
				throw new ArgumentException("Cannot construct a new image using an undersized color array;"
					+ $" array is {data.Length} values, but width {width} x height {height} requires {width*height} values.");
			_data = data;
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Construct a new image by treating the given raw byte data as a sequence of
		/// RGBA tuples and copying them into the new image's memory.
		/// </summary>
		/// <param name="width">The width of the new image.</param>
		/// <param name="height">The height of the new image.</param>
		/// <param name="rawData">The raw color data to copy into the image.  This must
		/// be large enough for the given image.</param>
		/// <exception cref="ArgumentException">Thrown if rawData is too short for the image dimensions.</exception>
		public Image(int width, int height, ReadOnlySpan<byte> rawData)
		{
			Width = width;
			Height = height;
			_data = new Color[width * height];

			Overwrite(rawData);
		}

		/// <summary>
		/// Construct a new image by treating the given raw byte data as a sequence of
		/// RGBA tuples and copying them into the new image's memory.
		/// </summary>
		/// <param name="width">The width of the new image.</param>
		/// <param name="height">The height of the new image.</param>
		/// <param name="rawData">The raw color data to copy into the image.  This must
		/// be large enough for the given image.</param>
		/// <exception cref="ArgumentException">Thrown if rawData is too short for the image dimensions.</exception>
		public unsafe Image(int width, int height, byte* rawData)
		{
			Width = width;
			Height = height;
			_data = new Color[width * height];

			Overwrite(rawData);
		}

		/// <summary>
		/// Construct a new image from the given a sequence of RGBA tuples, copying them
		/// into the new image's memory.
		/// </summary>
		/// <param name="width">The width of the new image.</param>
		/// <param name="height">The height of the new image.</param>
		/// <param name="data">The color data to copy into the image.  This must
		/// be large enough for the given image.</param>
		/// <exception cref="ArgumentException">Thrown if rawData is too short for the image dimensions.</exception>
		public Image(int width, int height, ReadOnlySpan<Color> data)
		{
			Width = width;
			Height = height;
			_data = new Color[width * height];

			Overwrite(data);
		}

		/// <summary>
		/// Construct a new image from the given a sequence of RGBA tuples, copying them
		/// into the new image's memory.
		/// </summary>
		/// <param name="width">The width of the new image.</param>
		/// <param name="height">The height of the new image.</param>
		/// <param name="data">The color data to copy into the image.  This must
		/// be large enough for the given image.</param>
		public unsafe Image(int width, int height, Color* data)
		{
			Width = width;
			Height = height;
			_data = new Color[width * height];

			Overwrite(data);
		}

		/// <summary>
		/// Overwrite all of the pixels in this image with the given raw pixel data,
		/// represented as a sequence of RGBA tuples, which must be at least as large
		/// as this image.
		/// </summary>
		/// <param name="rawData">The raw data to overwrite this image with.</param>
		/// <exception cref="ArgumentException">Thrown if the provided source data isn't large enough.</exception>
		public void Overwrite(ReadOnlySpan<byte> rawData)
		{
			if (rawData.Length < _data.Length * 4)
				throw new ArgumentException($"Not enough data for RGBA image of size {Width}x{Height} (only {rawData.Length} bytes provided).");

			unsafe
			{
				fixed (byte* rawDataBase = rawData)
				{
					Overwrite(rawDataBase);
				}
			}
		}

		/// <summary>
		/// Overwrite all of the pixels in this image with the given pixel data,
		/// which must be at least as large as this image.
		/// </summary>
		/// <param name="data">The data to overwrite this image with.</param>
		/// <exception cref="ArgumentException">Thrown if the provided source data isn't large enough.</exception>
		public void Overwrite(ReadOnlySpan<Color> data)
		{
			if (data.Length < _data.Length)
				throw new ArgumentException($"Not enough data for RGBA image of size {Width}x{Height} (only {data.Length} pixels provided).");

			unsafe
			{
				fixed (Color* dataBase = data)
				{
					Overwrite(dataBase);
				}
			}
		}

		/// <summary>
		/// Overwrite all of the pixels in this image with the given pixel data,
		/// which must be at least as large as this image.
		/// </summary>
		/// <param name="data">The data to overwrite this image with.</param>
		/// <exception cref="ArgumentException">Thrown if the provided source data isn't large enough.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Overwrite(Color* data)
			=> Overwrite((byte*)data);

		/// <summary>
		/// Overwrite all of the pixels in this image with the given raw pixel data,
		/// represented as a sequence of RGBA tuples, which must be at least as large
		/// as this image.
		/// </summary>
		/// <param name="rawData">The raw data to overwrite this image with.</param>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public unsafe void Overwrite(byte* rawData)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				{
					Color* dest = destBase;
					Color* src = (Color*)rawData;
					int count = Width * Height;

					while (count >= 8)
					{
						dest[0] = src[0];
						dest[1] = src[1];
						dest[2] = src[2];
						dest[3] = src[3];
						dest[4] = src[4];
						dest[5] = src[5];
						dest[6] = src[6];
						dest[7] = src[7];
						dest += 8;
						src += 8;
						count -= 8;
					}
					if ((count & 4) != 0)
					{
						dest[0] = src[0];
						dest[1] = src[1];
						dest[2] = src[2];
						dest[3] = src[3];
						dest += 4;
						src += 4;
					}
					if ((count & 2) != 0)
					{
						dest[0] = src[0];
						dest[1] = src[1];
						dest += 2;
						src += 2;
					}
					if ((count & 1) != 0)
						dest[0] = src[0];
				}
			}
		}

		/// <summary>
		/// Make a perfect duplicate of this image and return it.
		/// </summary>
		/// <returns>The newly-cloned image.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Image Clone()
			=> new Image(Width, Height, _data);

		#endregion

		#region Resizing

		/// <summary>
		/// Resize the image using nearest-neighbor sampling.  Fast, but can
		/// be really, really inaccurate.
		/// </summary>
		/// <param name="newWidth">The new width of the image.</param>
		/// <param name="newHeight">The new height of the image.</param>
		/// <returns>The image, resized.</returns>
		public Image Resize(int newWidth, int newHeight)
		{
			Image newImage = new Image(newWidth, newHeight);

			int xStep = (int)(((long)Width << 16) / newWidth);
			int yStep = (int)(((long)Height << 16) / newHeight);

			unsafe
			{
				fixed (Color* destBase = newImage._data)
				fixed (Color* srcBase = _data)
				{
					for (int destY = 0, srcYScaled = 0; destY < newHeight; destY++, srcYScaled += yStep)
					{
						int srcY = srcYScaled >> 16;
						Color* srcRow = srcBase + srcY * Width;
						Color* dest = destBase + destY * newWidth;
						Color* destEnd = dest + newWidth;

						for (int srcXScaled = 0; dest < destEnd; srcXScaled += xStep)
						{
							int srcX = srcXScaled >> 16;
							*dest++ = srcRow[srcX];
						}
					}
				}
			}

			return newImage;
		}

		#endregion

		#region Resampling

		/// <summary>
		/// Perform resampling using the chosen mode.  This is slower than nearest-neighbor
		/// resampling, but it can produce much higher-fidelity results.
		/// </summary>
		/// <param name="width">The new image width.  If omitted/null, this will be determined
		/// automatically from the given height.</param>
		/// <param name="height">The new image height.  If omitted/null, this will be determined
		/// automatically from the given width.</param>
		/// <param name="mode">The mode (sampling function) to use.  If omitted/null, this will
		/// be taken as a simple cubic B-spline.</param>
		/// <returns>The new, resampled image.</returns>
		/// <exception cref="ArgumentException">Raised if the new image size is illegal.</exception>
		public Image Resample(int? width = null, int? height = null, ImageResampleMode mode = ImageResampleMode.BSpline)
			=> Resample(CalculateResampleSize(width, height), mode);

		/// <summary>
		/// Calculate the new dimensions of the resampled image, given that one or both
		/// of the new dimensions may have been omitted.
		/// </summary>
		/// <param name="userWidth">The new desired width, which may be omitted.</param>
		/// <param name="userHeight">The new desired height, which may be omitted.</param>
		/// <returns>The fully-calculated dimensions of the new image.</returns>
		private Vector2i CalculateResampleSize(int? userWidth, int? userHeight)
		{
			int width, height;
			if (userWidth.HasValue)
			{
				width = userWidth.Value;
				if (userHeight.HasValue)
					height = userHeight.Value;
				else
				{
					double ooAspectRatio = (double)Height / Width;
					height = (int)(width * ooAspectRatio + 0.5);
				}
			}
			else if (userHeight.HasValue)
			{
				height = userHeight.Value;
				double aspectRatio = (double)Width / Height;
				width = (int)(height * aspectRatio + 0.5);
			}
			else
			{
				width = Width;
				height = Height;
			}

			return new Vector2i(width, height);
		}

		/// <summary>
		/// Perform resampling using the chosen mode.  This is slower than nearest-neighbor
		/// resampling, but it can produce much higher-fidelity results.
		/// </summary>
		/// <param name="newSize">The new image size.</param>
		/// <param name="mode">The mode (sampling function) to use.  If omitted/null, this will
		/// be taken as a simple cubic B-spline.</param>
		/// <returns>The new, resampled image.</returns>
		/// <exception cref="ArgumentException">Raised if the new image size is illegal.</exception>
		public Image Resample(Vector2i newSize, ImageResampleMode mode = ImageResampleMode.BSpline)
		{
			if (newSize == Size)
				return Clone();

			if (newSize.X <= 0 || newSize.Y <= 0)
				throw new ArgumentException("Size of new image must be greater than zero.");

			Image dest = new Image(newSize);

			ResampleTo(dest, mode);

			return dest;
		}

		/// <summary>
		/// Demote a value from [0.0, 1.0] to integer, rounding correctly, and clamp it to
		/// the range of [0, 255] (saturating).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static byte ClampValue(int value)
		{
			value = (value + 32768) >> 16;                          // Round to nearest integer.
			if ((value & 0xFFFFFF00) == 0)
				return (byte)value;                                 // No clamping needed.
			else
				return (byte)Math.Min(Math.Max(value, 0), 255);     // Clamp to [0, 255].
		}

		/// <summary>
		/// Perform resampling using the chosen mode.  This is slower than nearest-neighbor
		/// resampling, but it can produce much higher-fidelity results.  This overwrites all
		/// pixels in the given destination image with new data.
		/// </summary>
		/// <param name="dest">The destination image; the source will be resampled to exactly fit it.</param>
		/// <param name="mode">The mode (sampling function) to use.</param>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public unsafe void ResampleTo(Image dest, ImageResampleMode mode)
		{
			// Resample by one of a variety of different algorithms.  We use
			// the general methodology suggested by Dale Schumacher (see the book
			// Graphics Gems III).  The basic idea is that we set up a sampling
			// kernel across each line of data and determine how much of each
			// source sample factors into each destination sample; this will vary
			// depending on the source sample's position.  This is a very general
			// solution, allowing a variety of curves to be used to describe how
			// individual pixels get included.  However, for simple box (nearest)
			// filtering and triangular (linear) filtering, this solution is not as
			// efficient as hand-coded solutions, but then you didn't really want
			// to use those crappy filtering methods anyway, did you?

			if (dest.Width <= 0 || dest.Height <= 0)
				return;

			const int SentinelValue = unchecked((int)0x8BADF00D);

			ContribList* contribX = null;
			Contrib* contribDataX = null;
			ContribList* contribY = null;
			Contrib* contribDataY = null;
			Color* temp = null;

			try
			{
				(Func<float, float> resampleFunc, float support) = _resampleFuncs[(int)mode];

				// Compute the spans (the maximum number of contributing source
				// pixels per destination pixel).
				float spanX = dest.Width < Width ? support * Width / dest.Width : support;
				float spanY = dest.Height < Height ? support * Height / dest.Height : support;

				// Allocate intermediate buffers.
				int maxHorzContribs = (int)(spanX * 2 + 3) * dest.Width;
				int maxVertContribs = (int)(spanY * 2 + 3) * dest.Height;
				contribDataX = (Contrib*)Marshal.AllocHGlobal((maxHorzContribs + 1) * sizeof(Contrib));
				contribDataY = (Contrib*)Marshal.AllocHGlobal((maxVertContribs + 1) * sizeof(Contrib));
				contribX = (ContribList*)Marshal.AllocHGlobal((dest.Width + 1) * sizeof(ContribList));
				contribY = (ContribList*)Marshal.AllocHGlobal((dest.Height + 1) * sizeof(ContribList));

				// Add some sentinels so we can be sure we haven't overrun our memory buffers.
				contribX[dest.Width].Contrib = null;
				contribX[dest.Width].NumContributions = SentinelValue;
				contribY[dest.Height].Contrib = null;
				contribY[dest.Height].NumContributions = SentinelValue;
				contribDataX[maxHorzContribs].Weight = 0;
				contribDataX[maxHorzContribs].Pixel = SentinelValue;
				contribDataY[maxVertContribs].Weight = 0;
				contribDataY[maxVertContribs].Pixel = SentinelValue;

				int tw = dest.Width, th = Height;
				temp = (Color*)Marshal.AllocHGlobal(tw * th * sizeof(Color));

				// Now it's time to do the setup.  We precalculate the contributions
				// of each pixel in the horizontal scans and vertical scans and
				// use a method similar to that of Schumacher to do it.  We can
				// precalculate this because we only need to know the relative
				// sizes of each image and the resampling method; this data has
				// nothing to do with the current pixel values.

				// Set up the contributions in each scan-line.
				SetupContributions(contribX, contribDataX, dest.Width,
					Width, resampleFunc, support, (ImageResampleMode)((int)mode >> 8));

				// Set up the contributions in each column.
				SetupContributions(contribY, contribDataY, dest.Height,
					Height, resampleFunc, support, mode);

				// Now that we have the contributions figured out, the rest of this
				// is (mostly) easy:  Iterate across the destination image, and
				// sum the source pixels that contribute to each destination pixel.
				// Do this once into tempimage to resolve the horizontal part of the
				// resampling; and do it again into dest to resolve the vertical.

				// Horizontal resample first.
				fixed (Color* srcBase = _data)
				{
					Color* destStart = temp;
					Color* srcPtr = srcBase;

					for (int row = 0; row < th; row++)
					{
						Color* destPtr = destStart;

						for (int col = 0; col < tw; col++)
						{
							// Build up the correct fractional color.
							int r = 0, g = 0, b = 0, a = 0;
							int len = contribX[col].NumContributions;
							Contrib* contribs = contribX[col].Contrib;
							for (int i = 0; i < len; i++)
							{
								int index = contribs[i].Pixel;
								Color color = srcPtr[index];
								int weight = contribs[i].IntWeight;
								r += color.R * weight;
								g += color.G * weight;
								b += color.B * weight;
								a += color.A * weight;
							}

							// Scale and clamp the result to [0, 255].
							byte br = ClampValue(r);
							byte bg = ClampValue(g);
							byte bb = ClampValue(b);
							byte ba = ClampValue(a);

							*destPtr++ = new Color(br, bg, bb, ba);
						}

						// Move to the next row and do it again.
						destStart += tw;
						srcPtr += Width;
					}
				}

				// Okay, the horizontal resampling is done, and the horizontally-
				// resampled image is correctly stored in temp.  Now we need to do
				// the vertical resampling.
				fixed (Color* destBase = dest._data)
				{
					Color* destStart = destBase;
					Color* srcPtr = temp;
					int dw = dest.Width, dh = dest.Height;

					for (int row = 0; row < dh; row++)
					{
						Color* destPtr = destStart;

						for (int col = 0; col < dw; col++)
						{
							// Build up the correct fractional color.
							int r = 0, g = 0, b = 0, a = 0;
							int len = contribY[row].NumContributions;
							Contrib* contribs = contribY[row].Contrib;
							for (int i = 0; i < len; i++)
							{
								int index = contribs[i].Pixel * tw + col;
								Color color = srcPtr[index];
								int weight = contribs[i].IntWeight;
								r += color.R * weight;
								g += color.G * weight;
								b += color.B * weight;
								a += color.A * weight;
							}

							// Scale and clamp the result to [0, 255].
							byte br = ClampValue(r);
							byte bg = ClampValue(g);
							byte bb = ClampValue(b);
							byte ba = ClampValue(a);

							*destPtr++ = new Color(br, bg, bb, ba);
						}

						// Go to the next row and do it again.
						destStart += dw;
					}
				}

				// Critical safety check.
				if (contribX[dest.Width].NumContributions != SentinelValue
					|| contribY[dest.Height].NumContributions != SentinelValue
					|| contribDataX[maxHorzContribs].Pixel != SentinelValue
					|| contribDataY[maxVertContribs].Pixel != SentinelValue)
					throw new InvalidOperationException("Fatal error: Internal buffer overrun!");
			}
			finally
			{
				if (temp != null)
					Marshal.FreeHGlobal((IntPtr)temp);
				if (contribX != null)
					Marshal.FreeHGlobal((IntPtr)contribX);
				if (contribY != null)
					Marshal.FreeHGlobal((IntPtr)contribY);
				if (contribDataX != null)
					Marshal.FreeHGlobal((IntPtr)contribDataX);
				if (contribDataY != null)
					Marshal.FreeHGlobal((IntPtr)contribDataY);
			}
		}

		// A set of Contrib structures jointly describe which pixels will be
		// joined to form the resulting pixel, and how much of each.  For
		// example, with triangular (linear) filtering, when we're exactly
		// doubling an image, the resulting pixel at (1,0) will be exactly 0.5
		// of the value of the source pixel at (0,0) and exactly 0.5 of the
		// value of the source pixel at (1,0).  Thus there will be two Contrib
		// structures to describe that pixel relative to that scan-line; one
		// containing (0,0.5) and the other containing (1,0.5).
		private struct Contrib
		{
			public int Pixel;              // Source pixel to use
			public float Weight;           // Weight to give it
			public int IntWeight;          // That weight converted to a 16.16 fixed-point int
		};

		// We are really resampling the image twice:  Once horizontally, and
		// once vertically.  However, unlike Schumacher's original algorithm,
		// we do not actually do it in two separate stages.  Instead, we use
		// two arrays of Contrib structures to describe the horizontal and
		// vertical contributions to each pixel, and crunch out the resulting
		// image all at once.

		private unsafe struct ContribList
		{
			public Contrib* Contrib;
			public int NumContributions;
		};

		// Set up the pixel contribution factors for a given row/column.
		private static unsafe void SetupContributions(ContribList* contrib,
			Contrib* contribData, int destSize, int srcSize,
			Func<float, float> resampleFunc, float support, ImageResampleMode edgeMode)
		{
			// Determine the inverse scaling factor.
			float scale = (float)srcSize / destSize;

			if (destSize > srcSize)
			{
				// We're scaling up (interpolating), so we can actually use
				// the resampling function we were provided (they really make
				// very little sense when scaling down).

				SetupContributionsForScalingUp(contrib, contribData, destSize, srcSize,
					edgeMode, scale, resampleFunc, support);
			}
			else if (resampleFunc != Box)
			{
				// We're scaling down, and they want something nicer than
				// just dropping pixels.  The Schumacher algorithms for this
				// are terrible; they may work with some kinds of signals,
				// but they don't do very well with images (for example,
				// the "filter.c" program that he provides produces near-
				// garbage when asked to scale down images using triangular
				// filtering).  We could make the various sampling functions
				// work correctly but then we'd have to also include the
				// discrete integral of each function, and the whole thing
				// would be hoary and complex (and not significantly more
				// accurate than what we're going to do here).  So instead,
				// we fall back on a bilinear filtering solution that seems
				// to produce very good results (and is compatible with the
				// methods everybody else seems to be using).

				SetupContributionsForScalingDown(contrib, contribData, destSize, srcSize,
					edgeMode, scale);
			}
			else
			{
				// We're scaling down, and this is a straightforward
				// decimation-by-dropping-pixels, so the contributions
				// for each pixel are not hard to figure out: the
				// contributing source pixel is simply whichever one is
				// at the center.

				SetupContributionsForBoxMode(contrib, contribData, destSize, scale);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private static unsafe void SetupContributionsForScalingDown(ContribList* contrib, Contrib* contribData,
			int destSize, int srcSize, ImageResampleMode edgeMode, float scale)
		{
			int i;
			for (i = 0; i < destSize; i++)
			{
				int contribOffset = (int)(scale * 2 + 3) * i;
				contrib[i].Contrib = contribData + contribOffset;

				float min = i * scale;
				float max = min + scale;
				int top = (int)min;
				int bottom = (int)MathF.Ceiling(max);
				float totalWeight = 0.0f;

				// Calculate which source pixels will contribute to this
				// destination pixel.
				int j, k = 0;
				for (j = top; j < bottom; j++)
				{
					int n = CalculateSourcePixelCoordinate(srcSize, edgeMode, j);

					// Each pixel is weighted solely based on its position
					// relative to the center.
					float weight = j == top ? 1.0f - (min - top)
						: j == bottom - 1 ? 1.0f - (bottom - max)
						: 1.0f;
					if (weight != 0.0f)
					{
						contribData[contribOffset + k].Pixel = n;
						contribData[contribOffset + k++].Weight = weight;
						totalWeight += weight;
					}
				}

				// Reduce the weights to the range of [0,1].
				float ooTotalWeight = 1.0f / totalWeight;
				for (j = 0; j < k; j++)
				{
					contribData[contribOffset + j].Weight *= ooTotalWeight;
					contribData[contribOffset + j].IntWeight = (int)(contribData[contribOffset + j].Weight * 65536.0f);
				}

				contrib[i].NumContributions = k;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private static unsafe void SetupContributionsForScalingUp(ContribList* contrib, Contrib* contribData,
			int destSize, int srcSize, ImageResampleMode edgeMode, float scale,
			Func<float, float> resampleFunc, float support)
		{
			int i;
			for (i = 0; i < destSize; i++)
			{
				int contribOffset = (int)(support * 2 + 3) * i;
				contrib[i].Contrib = contribData + contribOffset;

				float center = i * scale;
				int top = (int)(center - support);
				int bottom = (int)MathF.Ceiling(center + support);

				// Calculate which source pixels will contribute to this
				// destination pixel.
				int j, k = 0;
				for (j = top; j <= bottom; j++)
				{
					int n = CalculateSourcePixelCoordinate(srcSize, edgeMode, j);

					float weight = resampleFunc(j - center);
					if (weight != 0.0f)
					{
						contribData[contribOffset + k].Pixel = n;
						contribData[contribOffset + k].Weight = weight;
						contribData[contribOffset + k].IntWeight = (int)(contribData[contribOffset + k].Weight * 65536.0f);
						k++;
					}
				}

				contrib[i].NumContributions = k;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static int CalculateSourcePixelCoordinate(int srcSize, ImageResampleMode edgeMode, int x)
			=> x < 0 ? ((edgeMode & ImageResampleMode.TopMode) == ImageResampleMode.TopWrap ? x + srcSize : -x)
				: x >= srcSize ? ((edgeMode & ImageResampleMode.BottomMode) == ImageResampleMode.BottomWrap ? x - srcSize : srcSize * 2 - x - 1)
				: x;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private static unsafe void SetupContributionsForBoxMode(ContribList* contrib, Contrib* contribData,
			int destSize, float scale)
		{
			int i;
			for (i = 0; i < destSize; i++)
			{
				contrib[i].Contrib = contribData + i;
				contrib[i].NumContributions = 1;
				contribData[i].Pixel = (int)(i * scale + 0.5f);
				contribData[i].Weight = 1.0f;
				contribData[i].IntWeight = 65536;
			}
		}

		///////////////////////////////////////////////////////////////////////
		//  Filtering functions.
		//    Each filtering function is a 1-dimensional function f(x) that
		//    satisfies the following four properties:
		//      1.  At 0, the function's value is +1.
		//      2.  At -inf and +inf, the function's value is 0.
		//      3.  The discrete integral of the function over [-inf, +inf]
		//           is equal to 1.
		//      4.  On [-inf, -1] and [+1, +inf], f(x) is approximately 0.
		//    Other than that, they're all different.  These functions below
		//    are the most common ones used, because (A) they are efficient
		//    to compute and (B) they seem to achieve good results.

		// The standard box (pulse, Fourier, 1st-order B-spline) function.
		// Also known as the "nearest-neighbor" function.
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Box(float x)
			=> x >= -0.5f && x < 0.5f ? 1.0f
				: 0.0f;

		// The standard triangle (linear, Bartlett, 2nd-order B-spline)
		// function.  Also known as "bilinear filtering".
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Triangle(float x)
		{
			x = MathF.Abs(x);

			return x >= 1.0f ? 0.0f
				: 1.0f - x;
		}

		// The Hermite curve, a simple cubic function that looks a little
		// nicer than the triangle filter.
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Hermite(float x)
		{
			x = MathF.Abs(x);

			return x >= 1.0f ? 0.0f
				: (x * 2.0f - 3.0f) * x * x + 1.0f;
		}

		// The Bell function (3rd-order, or quadratic B-spline).
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Bell(float x)
		{
			x = Math.Abs(x);

			if (x < 0.5f)
				return 0.75f - x * x;
			if (x >= 1.5f)
				return 0.0f;
			x -= 1.5f;
			return (x * x) * 0.5f;
		}

		// The 4th-order (cubic) B-spline.
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float BSpline(float x)
		{
			x = MathF.Abs(x);

			if (x < 1.0f)
			{
				float x2 = x * x;
				return 0.5f * x2 * x - x2 + (2.0f / 3.0f);
			}
			else if (x < 2.0f)
			{
				x = 2.0f - x;
				return (1.0f / 6.0f) * x * x * x;
			}
			else return 0.0f;
		}

		// The two-parameter cubic function proposed by
		// Mitchell & Netravali (see SIGGRAPH 88).
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Mitchell(float x)
		{
			const float B = (1.0f / 3.0f);
			const float C = (1.0f / 3.0f);

			x = MathF.Abs(x);

			if (x < 1.0f)
			{
				float x2 = x * x;
				x = (((12.0f - 9.0f * B - 6.0f * C) * (x * x2))
				   + ((-18.0f + 12.0f * B + 6.0f * C) * x2)
				   + (6.0f - 2 * B));
				return x * (1.0f / 6.0f);
			}
			else if (x < 2.0f)
			{
				float x2 = x * x;
				x = (((-1.0f * B - 6.0f * C) * (x * x2))
				   + ((6.0f * B + 30.0f * C) * x2)
				   + ((-12.0f * B - 48.0f * C) * x)
				   + (8.0f * B + 24 * C));
				return x * (1.0f / 6.0f);
			}
			else return 0.0f;
		}

		// The standard sinc() function, defined as sinc(x) = sin(x * pi) / (x * pi).
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Sinc(float x)
		{
			float nx = MathF.PI * x;
			return MathF.Sin(nx) / nx;
		}

		// A three-lobed Lanczos filter.
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Lanczos3(float x)
		{
			x = MathF.Abs(x);
			return x >= 3.0f ? 0.0f
				: x == 0.0f ? 1.0f
				: Sinc(x) * Sinc(x * (1.0f / 3.0f));
		}

		// A five-lobed Lanczos filter.
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Lanczos5(float x)
		{
			x = MathF.Abs(x);
			return x >= 5.0f ? 0.0f
				: x == 0.0f ? 1.0f
				: Sinc(x) * Sinc(x * (1.0f / 5.0f));
		}

		// A seven-lobed Lanczos filter.
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Lanczos7(float x)
		{
			x = MathF.Abs(x);
			return x >= 7.0f ? 0.0f
				: x == 0.0f ? 1.0f
				: Sinc(x) * Sinc(x * (1.0f / 7.0f));
		}

		// A nine-lobed Lanczos filter.
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Lanczos9(float x)
		{
			x = MathF.Abs(x);
			return x >= 9.0f ? 0.0f
				: x == 0.0f ? 1.0f
				: Sinc(x) * Sinc(x * (1.0f / 9.0f));
		}

		// An eleven-lobed Lanczos filter.
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static float Lanczos11(float x)
		{
			x = MathF.Abs(x);
			return x >= 11.0f ? 0.0f
				: x == 0.0f ? 1.0f
				: Sinc(x) * Sinc(x * (1.0f / 11.0f));
		}

		/// <summary>
		/// This table contains the complete list of filtering functions,
		/// as well as the required support for each function.  The support
		/// value is the width of the source data around 0 where the function's
		/// values are significant.  For finite functions, like Box and Triangle,
		/// this support value is exact; for infinite functions, like Lanczos,
		/// this value simply defines a window of the "most significant" data
		/// points.  This is in an order that must match that of the
		/// ImageResampleMode enum.
		/// </summary>
		private static readonly (Func<float, float>, float)[] _resampleFuncs = {
			(Box, 0.5f),
			(Triangle, 1.0f),
			(Hermite, 1.0f),
			(Bell, 1.5f),
			(BSpline, 2.0f),
			(Mitchell, 2.0f),
			(Lanczos3, 3.0f),
			(Lanczos5, 5.0f),
			(Lanczos7, 7.0f),
			(Lanczos9, 9.0f),
			(Lanczos11, 11.0f),
		};

		#endregion

		#region Clipping helpers

		/// <summary>
		/// Clip a blit to be within the given bounds of the image.
		/// </summary>
		/// <param name="srcImage">The source image we're blitting.</param>
		/// <param name="srcX">The source X coordinate of the blit, which will be updated to within bounds of both images.</param>
		/// <param name="srcY">The source Y coordinate of the blit, which will be updated to within bounds of both images.</param>
		/// <param name="destX">The destination X coordinate of the blit, which will be updated to within bounds of both images.</param>
		/// <param name="destY">The destination Y coordinate of the blit, which will be updated to within bounds of both images.</param>
		/// <param name="width">The width of the blit, which will be updated to within bounds of both images.</param>
		/// <param name="height">The height of the blit, which will be updated to within bounds of both images.</param>
		/// <returns>True if the blit can proceed, or false if the blit should be aborted due to illegal/unusable values.</returns>
		public bool ClipBlit(Image srcImage, ref int srcX, ref int srcY,
			ref int destX, ref int destY, ref int width, ref int height)
		{
			if (width <= 0 || height <= 0
				|| srcX >= srcImage.Width || srcY >= srcImage.Height
				|| destX >= Width || destY >= Height)
				return false;

			if (srcX < 0)
			{
				width += srcX;
				destX -= srcX;
				srcX = 0;
			}
			if (srcY < 0)
			{
				height += srcY;
				destY -= srcY;
				srcY = 0;
			}
			if (destX < 0)
			{
				width += destX;
				srcX -= destX;
				destX = 0;
			}
			if (destY < 0)
			{
				height += destY;
				srcY -= destY;
				destY = 0;
			}

			if (width > srcImage.Width - srcX)
				width = srcImage.Width - srcX;
			if (height > srcImage.Height - srcY)
				height = srcImage.Height - srcY;
			if (width > Width - destX)
				width = Width - destX;
			if (height > Height - destY)
				height = Height - destY;

			return width > 0 && height > 0;
		}

		/// <summary>
		/// Clip the given drawing rectangle to be within the image.
		/// </summary>
		/// <param name="x">The left coordinate of the rectangle, which will be updated to be within the image.</param>
		/// <param name="y">The top coordinate of the rectangle, which will be updated to be within the image.</param>
		/// <param name="width">The width of the rectangle, which will be updated to be within the image.</param>
		/// <param name="height">The height of the rectangle, which will be updated to be within the image.</param>
		/// <returns>True if the drawing may proceed, or false if the rectangle is invalid/unusable.</returns>
		public bool ClipRect(ref int x, ref int y, ref int width, ref int height)
		{
			if (width <= 0 || height <= 0 || x >= Width || y >= Height)
				return false;

			if (x < 0)
			{
				width += x;
				x = 0;
			}
			if (y < 0)
			{
				height += y;
				y = 0;
			}
			if (width > Width - x)
				width = Width - x;
			if (height > Height - y)
				height = Height - y;

			return width > 0 && height > 0;
		}

		#endregion

		#region Blitting

		/// <summary>
		/// Extract a subimage from this image.  Really just a blit in disguise.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Image Extract(int x, int y, int width, int height)
		{
			Image image = new Image(width, height);
			image.Blit(this, x, y, 0, 0, width, height);
			return image;
		}

		/// <summary>
		/// Copy from src image rectangle to dest image rectangle, but copy all colors as black,
		/// of the given alpha (factoring in any existing alpha).  This will by default clip
		/// the provided coordinates to perform a safe blit (all pixels outside an image
		/// will be ignored).
		/// </summary>
		public void ShadowBlit(Image srcImage, int srcX, int srcY, int destX, int destY, int width, int height, int alpha)
		{
			if (!ClipBlit(srcImage, ref srcX, ref srcY, ref destX, ref destY, ref width, ref height))
				return;

			FastUnsafeShadowBlit(srcImage, srcX, srcY, destX, destY, width, height, alpha);
		}

		/// <summary>
		/// Copy from src image rectangle to dest image rectangle.  This will by default clip
		/// the provided coordinates to perform a safe blit (all pixels outside an image
		/// will be ignored).
		/// </summary>
		public void Blit(Image srcImage, int srcX, int srcY, int destX, int destY, int width, int height,
			BlitFlags blitFlags = default)
		{
			if ((blitFlags & BlitFlags.FastUnsafe) == 0)
			{
				if (!ClipBlit(srcImage, ref srcX, ref srcY, ref destX, ref destY, ref width, ref height))
					return;
			}

			switch (blitFlags & (BlitFlags.AlphaMode | BlitFlags.FlipHorz))
			{
				case BlitFlags.Copy:
					FastUnsafeBlit(srcImage, srcX, srcY, destX, destY, width, height,
						(blitFlags & BlitFlags.FlipVert) != 0);
					break;
				case BlitFlags.Transparent:
					FastUnsafeTransparentBlit(srcImage, srcX, srcY, destX, destY, width, height,
						(blitFlags & BlitFlags.FlipVert) != 0);
					break;
				case BlitFlags.Alpha:
					FastUnsafeAlphaBlit(srcImage, srcX, srcY, destX, destY, width, height);
					break;
				case BlitFlags.PMAlpha:
					FastUnsafePMAlphaBlit(srcImage, srcX, srcY, destX, destY, width, height);
					break;
				case BlitFlags.Copy | BlitFlags.FlipHorz:
					FastUnsafeBlitFlipHorz(srcImage, srcX, srcY, destX, destY, width, height,
						(blitFlags & BlitFlags.FlipVert) != 0);
					break;
				case BlitFlags.Transparent | BlitFlags.FlipHorz:
					FastUnsafeTransparentBlitFlipHorz(srcImage, srcX, srcY, destX, destY, width, height,
						(blitFlags & BlitFlags.FlipVert) != 0);
					break;
				case BlitFlags.Multiply:
					FastUnsafeMultiplyBlit(srcImage, srcX, srcY, destX, destY, width, height);
					break;
				case BlitFlags.Add:
					FastUnsafeAddBlit(srcImage, srcX, srcY, destX, destY, width, height);
					break;
				default:
					throw new InvalidOperationException($"Unsupported blit mode: {blitFlags & BlitFlags.AlphaMode}");
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle.  This doesn't apply alpha;
		/// it just slams new data over top of old.  Make sure all values are within range; this is as
		/// unsafe as it sounds.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FastUnsafeBlit(Image srcImage, int srcX, int srcY, int destX, int destY, int width, int height, bool flipVert)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width - width;

					if (flipVert)
					{
						src += srcImage.Width * (height - 1);
						srcSkip = -width - srcImage.Width;
					}

					do
					{
						int count = width;
						while (count >= 8)
						{
							dest[0] = src[0];
							dest[1] = src[1];
							dest[2] = src[2];
							dest[3] = src[3];
							dest[4] = src[4];
							dest[5] = src[5];
							dest[6] = src[6];
							dest[7] = src[7];
							count -= 8;
							src += 8;
							dest += 8;
						}
						if (count != 0)
						{
							if ((count & 4) != 0)
							{
								dest[0] = src[0];
								dest[1] = src[1];
								dest[2] = src[2];
								dest[3] = src[3];
								src += 4;
								dest += 4;
							}
							if ((count & 2) != 0)
							{
								dest[0] = src[0];
								dest[1] = src[1];
								src += 2;
								dest += 2;
							}
							if ((count & 1) != 0)
							{
								*dest++ = *src++;
							}
						}
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle.  This skips
		/// any Color.Transparent pixels.  Make sure all values are within range; this is as
		/// unsafe as it sounds.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public void FastUnsafeTransparentBlit(Image srcImage,
			int srcX, int srcY, int destX, int destY, int width, int height, bool flipVert)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width - width;

					if (flipVert)
					{
						src += srcImage.Width * (height - 1);
						srcSkip = -width - srcImage.Width;
					}

					do
					{
						int count = width;
						Color c;
						while (count >= 8)
						{
							if ((c = src[0]).A != 0) dest[0] = c;
							if ((c = src[1]).A != 0) dest[1] = c;
							if ((c = src[2]).A != 0) dest[2] = c;
							if ((c = src[3]).A != 0) dest[3] = c;
							if ((c = src[4]).A != 0) dest[4] = c;
							if ((c = src[5]).A != 0) dest[5] = c;
							if ((c = src[6]).A != 0) dest[6] = c;
							if ((c = src[7]).A != 0) dest[7] = c;
							count -= 8;
							src += 8;
							dest += 8;
						}
						if (count != 0)
						{
							if ((count & 4) != 0)
							{
								if ((c = src[0]).A != 0) dest[0] = c;
								if ((c = src[1]).A != 0) dest[1] = c;
								if ((c = src[2]).A != 0) dest[2] = c;
								if ((c = src[3]).A != 0) dest[3] = c;
								src += 4;
								dest += 4;
							}
							if ((count & 2) != 0)
							{
								if ((c = src[0]).A != 0) dest[0] = c;
								if ((c = src[1]).A != 0) dest[1] = c;
								src += 2;
								dest += 2;
							}
							if ((count & 1) != 0)
							{
								if ((c = src[0]).A != 0) dest[0] = c;
								src++;
								dest++;
							}
						}
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle, applying
		/// alpha from the source, and treating the dest as opaque.  Make sure all values are
		/// within range; this is as unsafe as it sounds.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FastUnsafeAlphaBlit(Image srcImage, int srcX, int srcY, int destX, int destY, int width, int height)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width - width;

					do
					{
						int count = width;
						do
						{
							Color s = *src++, d = *dest;
							if (s.A == 255)
							{
								// No need to apply alpha math; this pixel is opaque.
								*dest++ = s;
							}
							else if (s.A != 0)
							{
								// We only apply alpha math if this pixel isn't 100% transparent.  Otherwise,
								// we do the math for real, but exclusively using integers (for performance).
								uint a = s.A, ia = (uint)(255 - s.A);
								uint r = s.R * a + d.R * ia + 127;
								uint g = s.G * a + d.G * ia + 127;
								uint b = s.B * a + d.B * ia + 127;
								r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
								g = (g + 1 + (g >> 8)) >> 8;
								b = (b + 1 + (b >> 8)) >> 8;
								*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)255);
							}
							else dest++;
						} while (--count != 0);
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle, applying
		/// alpha from the (premultiplied!) source, and treating the dest as opaque.  Make sure all values are
		/// within range; this is as unsafe as it sounds.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FastUnsafePMAlphaBlit(Image srcImage, int srcX, int srcY, int destX, int destY, int width, int height)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width - width;

					do
					{
						int count = width;
						do
						{
							Color s = *src++, d = *dest;
							if (s.A == 255)
							{
								// No need to apply alpha math; this pixel is opaque.
								*dest++ = s;
							}
							else if (s.A != 0)
							{
								// We only apply alpha math if this pixel isn't 100% transparent.  Otherwise,
								// we do the math for real, but exclusively using integers (for performance).
								// This uses premultiplied source colors for even more performance.
								uint ia = (uint)(255 - s.A);
								uint r = d.R * ia + 127;
								uint g = d.G * ia + 127;
								uint b = d.B * ia + 127;
								r = s.R + ((r + 1 + (r >> 8)) >> 8);
								g = s.G + ((g + 1 + (g >> 8)) >> 8);
								b = s.B + ((b + 1 + (b >> 8)) >> 8);
								*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)255);
							}
							else dest++;
						} while (--count != 0);
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle, applying
		/// alpha from the source, treating the dest as opaque, and treating all source
		/// colors as black.  Make sure all values are within range; this is as unsafe as it sounds.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FastUnsafeShadowBlit(Image srcImage, int srcX, int srcY, int destX, int destY,
			int width, int height, int alpha)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width - width;

					do
					{
						int count = width;
						do
						{
							Color s = *src++, d = *dest;
							uint a = s.A * (uint)alpha;
							a = (a + 1 + (a >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
							if (a == 255)
							{
								// No need to apply alpha math; this pixel is opaque.
								*dest++ = Color.Black;
							}
							else if (a != 0)
							{
								// We only apply alpha math if this pixel isn't 100% transparent.  Otherwise,
								// we do the math for real, but exclusively using integers (for performance).
								uint ia = (255 - a);
								uint r = d.R * ia + 127;
								uint g = d.G * ia + 127;
								uint b = d.B * ia + 127;
								uint da = d.A;
								r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
								g = (g + 1 + (g >> 8)) >> 8;
								b = (b + 1 + (b >> 8)) >> 8;
								da = Math.Min(da + a, 255);
								*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)da);
							}
							else dest++;
						} while (--count != 0);
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle.  This doesn't apply alpha;
		/// it just slams new data over top of old.  Make sure all values are within range; this is as
		/// unsafe as it sounds.  This flips the pixels horizontally while copying.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FastUnsafeBlitFlipHorz(Image srcImage, int srcX, int srcY, int destX, int destY, int width, int height, bool flipVert)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX + width;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width + width;

					if (flipVert)
					{
						src += srcImage.Width * (height - 1);
						srcSkip = -width - srcImage.Width;
					}

					do
					{
						int count = width;
						while (count >= 8)
						{
							dest -= 8;
							dest[7] = src[0];
							dest[6] = src[1];
							dest[5] = src[2];
							dest[4] = src[3];
							dest[3] = src[4];
							dest[2] = src[5];
							dest[1] = src[6];
							dest[0] = src[7];
							count -= 8;
							src += 8;
						}
						if (count != 0)
						{
							if ((count & 4) != 0)
							{
								dest -= 4;
								dest[3] = src[0];
								dest[2] = src[1];
								dest[1] = src[2];
								dest[0] = src[3];
								src += 4;
							}
							if ((count & 2) != 0)
							{
								dest -= 2;
								dest[1] = src[0];
								dest[0] = src[1];
								src += 2;
							}
							if ((count & 1) != 0)
							{
								*dest-- = *src++;
							}
						}
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle.  This skips
		/// any Color.Transparent pixels.  Make sure all values are within range; this is as
		/// unsafe as it sounds.  This flips the pixels horizontally while copying.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public void FastUnsafeTransparentBlitFlipHorz(Image srcImage,
			int srcX, int srcY, int destX, int destY, int width, int height, bool flipVert)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX + width;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width + width;

					if (flipVert)
					{
						src += srcImage.Width * (height - 1);
						srcSkip = -width - srcImage.Width;
					}

					do
					{
						int count = width;
						Color c;
						while (count >= 8)
						{
							dest -= 8;
							if ((c = src[0]).A != 0) dest[7] = c;
							if ((c = src[1]).A != 0) dest[6] = c;
							if ((c = src[2]).A != 0) dest[5] = c;
							if ((c = src[3]).A != 0) dest[4] = c;
							if ((c = src[4]).A != 0) dest[3] = c;
							if ((c = src[5]).A != 0) dest[2] = c;
							if ((c = src[6]).A != 0) dest[1] = c;
							if ((c = src[7]).A != 0) dest[0] = c;
							count -= 8;
							src += 8;
						}
						if (count != 0)
						{
							if ((count & 4) != 0)
							{
								dest -= 4;
								if ((c = src[0]).A != 0) dest[3] = c;
								if ((c = src[1]).A != 0) dest[2] = c;
								if ((c = src[2]).A != 0) dest[1] = c;
								if ((c = src[3]).A != 0) dest[0] = c;
								src += 4;
							}
							if ((count & 2) != 0)
							{
								dest -= 2;
								if ((c = src[0]).A != 0) dest[1] = c;
								if ((c = src[1]).A != 0) dest[0] = c;
								src += 2;
							}
							if ((count & 1) != 0)
							{
								dest--;
								if ((c = src[0]).A != 0) dest[0] = c;
								src++;
							}
						}
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle, adding
		/// all color values in the source and destination together (i.e., result.r = src.r + dest.r).
		/// Make sure all values are within range; this is as unsafe as it sounds.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FastUnsafeAddBlit(Image srcImage, int srcX, int srcY, int destX, int destY, int width, int height)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width - width;

					do
					{
						int count = width;
						do
						{
							Color s = *src++, d = *dest;
							byte r = (byte)Math.Min(s.R + d.R, 255);
							byte g = (byte)Math.Min(s.G + d.G, 255);
							byte b = (byte)Math.Min(s.B + d.B, 255);
							byte a = (byte)Math.Min(s.A + d.A, 255);
							*dest++ = new Color(r, g, b, a);
						} while (--count != 0);
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle, multiplying
		/// all color values in the source and destination together (i.e., result.r = src.r * dest.r / 255).
		/// Make sure all values are within range; this is as unsafe as it sounds.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FastUnsafeMultiplyBlit(Image srcImage, int srcX, int srcY, int destX, int destY, int width, int height)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* src = srcBase + srcImage.Width * srcY + srcX;
					Color* dest = destBase + Width * destY + destX;
					int srcSkip = srcImage.Width - width;
					int destSkip = Width - width;

					do
					{
						int count = width;
						do
						{
							Color s = *src++, d = *dest;
							uint r = (uint)(s.R * d.R);
							uint g = (uint)(s.G * d.G);
							uint b = (uint)(s.B * d.B);
							uint a = (uint)(s.A * d.A);
							r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
							g = (g + 1 + (g >> 8)) >> 8;
							b = (b + 1 + (b >> 8)) >> 8;
							a = (a + 1 + (a >> 8)) >> 8;
							*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)a);
						} while (--count != 0);
						src += srcSkip;
						dest += destSkip;
					} while (--height != 0);
				}
			}
		}

		/// <summary>
		/// Mix another image with this by combining their color values according to the given
		/// opacity level.  The other image must be exactly the same width and height as this image.
		/// </summary>
		/// <param name="other">The other image whose color values should be mixed with this
		/// image's color values.</param>
		/// <param name="amount">How much of the other image to mix in; 0.0 = 100% this image,
		/// 1.0 = 100% the other image.</param>
		public void Mix(Image other, float amount = 0.5f)
		{
			if (other.Width != Width || other.Height != Height)
				throw new ArgumentException("Dimensions of other image are not the same as dimensions of this image.");
			if (Width <= 0 || Height <= 0)
				return;

			int sa = (int)(amount * 65536 + 0.5f);
			int da = 65536 - sa;

			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = other._data)
				{
					Color* src = srcBase;
					Color* dest = destBase;

					int count = Width * Height;
					do
					{
						Color s = *src++, d = *dest;
						uint r = (uint)(s.R * sa + d.R * da) >> 16;
						uint g = (uint)(s.G * sa + d.G * da) >> 16;
						uint b = (uint)(s.B * sa + d.B * da) >> 16;
						uint a = (uint)(s.A * sa + d.A * da) >> 16;
						*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)a);
					} while (--count != 0);
				}
			}
		}

		/// <summary>
		/// Multiply every color value in this by the given scalar value.
		/// </summary>
		public void Multiply(float r, float g, float b, float a)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				{
					Color* dest = destBase;

					int count = Width * Height;
					do
					{
						Color d = *dest;
						*dest++ = new Color((int)(d.R * r + 0.5f), (int)(d.G * g + 0.5f), (int)(d.B * b + 0.5f), (int)(d.A * a + 0.5f));
					} while (--count != 0);
				}
			}
		}

		/// <summary>
		/// Premultiply the alpha value to the red, green, and blue values of every pixel.
		/// </summary>
		public void PremultiplyAlpha()
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				{
					byte* dest = (byte*)destBase;
					int count = Width * Height;

					byte* ptr = (byte*)destBase;
					byte* end = ptr + count * 4;

					for (; ptr < end; ptr += 4)
					{
						uint a = ptr[3];

						uint r = ptr[0];
						r *= a;
						r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
						ptr[0] = (byte)r;

						uint g = ptr[1];
						g *= a;
						g = (g + 1 + (g >> 8)) >> 8;
						ptr[1] = (byte)g;

						uint b = ptr[2];
						b *= a;
						b = (b + 1 + (b >> 8)) >> 8;
						ptr[2] = (byte)b;
					}
				}
			}
		}

		#endregion

		#region Pattern blits

		/// <summary>
		/// Copy from src image rectangle to dest image rectangle, repeating the source rectangle
		/// if the destination rectangle is larger than the source.  This will by default clip
		/// the provided destination coordinates to perform a safe blit (all pixels outside an
		/// image will be ignored).  If the source coordinates lie outside the srcImage, this
		/// will throw an exception.
		/// </summary>
		public void PatternBlit(Image srcImage, Rect srcRect, Rect destRect)
		{
			if (srcRect.X < 0 || srcRect.Y < 0 || srcRect.Width < 1 || srcRect.Height < 1
				|| srcRect.X > srcImage.Width - srcRect.Width
				|| srcRect.Y > srcImage.Height - srcRect.Height)
				throw new ArgumentException($"Illegal source rectangle for PatternBlit: Rect is {srcRect}, but image is {srcImage.Size}.");

			int offsetX = 0, offsetY = 0;
			int destX = destRect.X, destY = destRect.Y, width = destRect.Width, height = destRect.Height;
			if (!ClipRect(ref destX, ref destY, ref width, ref height))
				return;

			if (destX > destRect.X)
				offsetX = (destX - destRect.X) % srcRect.Width;
			if (destY > destRect.Y)
				offsetY = (destY - destRect.Y) % srcRect.Height;

			FastUnsafePatternBlit(srcImage, srcRect, new Vector2i(offsetX, offsetY),
				new Rect(destX, destY, width, height));
		}


		/// <summary>
		/// Fast, unsafe copy from src image rectangle to dest image rectangle.  This doesn't apply alpha;
		/// it just slams new data over top of old.  Make sure all values are within range; this is as
		/// unsafe as it sounds.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FastUnsafePatternBlit(Image srcImage, Rect srcRect, Vector2i offset, Rect destRect)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				fixed (Color* srcBase = srcImage._data)
				{
					Color* dest = destBase + Width * destRect.Y + destRect.X;
					int destSkip = Width - destRect.Width;
					int srcX = srcRect.X;
					int srcY = srcRect.Y;
					int offsetY = offset.Y;
					int height = destRect.Height;
					int srcWidth = srcImage.Width;

					do
					{
						int offsetX = offset.X;
						int count = destRect.Width;
						Color* src = srcBase + (srcY + offsetY) * srcWidth;
						do
						{
							*dest++ = src[srcX + offsetX];
							if (++offsetX >= srcRect.Width)
								offsetX = 0;
						} while (--count != 0);
						dest += destSkip;
						if (++offsetY >= srcRect.Height)
							offsetY = 0;
					} while (--height != 0);
				}
			}
		}

		#endregion

		#region Transformations

		/// <summary>
		/// Vertically flip the image, in-place.
		/// </summary>
		public void FlipVert()
		{
			unsafe
			{
				fixed (Color* dataBase = _data)
				{
					Color* topRow = dataBase;
					Color* bottomRow = dataBase + Width * (Height - 1);
					for (int y = 0; y < Height / 2; y++)
					{
						for (int x = 0; x < Width; x++)
						{
							Color temp = *topRow;
							*topRow = *bottomRow;
							*bottomRow = temp;
							topRow++;
							bottomRow++;
						}
						bottomRow -= Width * 2;
					}
				}
			}
		}

		/// <summary>
		/// Horizontally flip the image, in-place.
		/// </summary>
		public void FlipHorz()
		{
			unsafe
			{
				fixed (Color* dataBase = _data)
				{
					Color* ptr = dataBase;
					for (int y = 0; y < Height; y++)
					{
						Color* start = ptr;
						Color* end = ptr + Width;
						for (int x = 0; x < Width / 2; x++)
						{
							end--;
							Color temp = *start;
							*start = *end;
							*end = temp;
							start++;
						}
						ptr += Width;
					}
				}
			}
		}

		#endregion

		#region Color remapping

		/// <summary>
		/// Skim through the image and replace all exact instances of the given color with another.
		/// </summary>
		/// <param name="src">The color to replace.</param>
		/// <param name="dest">Its replacement color.</param>
		public void RemapColor(Color src, Color dest)
		{
			unsafe
			{
				fixed (Color* imageBase = _data)
				{
					Color* ptr = imageBase;
					Color* end = imageBase + Width * Height;

					while (ptr < end - 8)
					{
						if (ptr[0] == src) ptr[0] = dest;
						if (ptr[1] == src) ptr[1] = dest;
						if (ptr[2] == src) ptr[2] = dest;
						if (ptr[3] == src) ptr[3] = dest;
						if (ptr[4] == src) ptr[4] = dest;
						if (ptr[5] == src) ptr[5] = dest;
						if (ptr[6] == src) ptr[6] = dest;
						if (ptr[7] == src) ptr[7] = dest;
						ptr += 8;
					}

					int count = (int)(end - ptr);
					if (count != 0)
					{
						if ((count & 4) != 0)
						{
							if (ptr[0] == src) ptr[0] = dest;
							if (ptr[1] == src) ptr[1] = dest;
							if (ptr[2] == src) ptr[2] = dest;
							if (ptr[3] == src) ptr[3] = dest;
							ptr += 4;
						}
						if ((count & 2) != 0)
						{
							if (ptr[0] == src) ptr[0] = dest;
							if (ptr[1] == src) ptr[1] = dest;
							ptr += 2;
						}
						if ((count & 1) != 0)
						{
							if (ptr[0] == src) ptr[0] = dest;
						}
					}
				}
			}
		}

		/// <summary>
		/// Skim through the image and replace many colors at once via a dictionary.
		/// This is typically much slower than many calls to Remap() above, unless the
		/// replacement table is large relative to the image size.
		/// </summary>
		/// <param name="dictionary">The dictionary that describes all replacement values.
		/// If a color does not exist in the dictionary, it will not be changed.</param>
		public void RemapTable(Dictionary<Color, Color> dictionary)
		{
			unsafe
			{
				fixed (Color* imageBase = _data)
				{
					Color* ptr = imageBase;
					Color* end = imageBase + Width * Height;

					while (ptr < end - 8)
					{
						if (dictionary.TryGetValue(ptr[0], out Color replacement))
							ptr[0] = replacement;
						if (dictionary.TryGetValue(ptr[1], out replacement))
							ptr[1] = replacement;
						if (dictionary.TryGetValue(ptr[2], out replacement))
							ptr[2] = replacement;
						if (dictionary.TryGetValue(ptr[3], out replacement))
							ptr[3] = replacement;
						if (dictionary.TryGetValue(ptr[4], out replacement))
							ptr[4] = replacement;
						if (dictionary.TryGetValue(ptr[5], out replacement))
							ptr[5] = replacement;
						if (dictionary.TryGetValue(ptr[6], out replacement))
							ptr[6] = replacement;
						if (dictionary.TryGetValue(ptr[7], out replacement))
							ptr[7] = replacement;
						ptr += 8;
					}

					int count = (int)(end - ptr);
					if (count != 0)
					{
						if ((count & 4) != 0)
						{
							if (dictionary.TryGetValue(ptr[0], out Color replacement))
								ptr[0] = replacement;
							if (dictionary.TryGetValue(ptr[1], out replacement))
								ptr[1] = replacement;
							if (dictionary.TryGetValue(ptr[2], out replacement))
								ptr[2] = replacement;
							if (dictionary.TryGetValue(ptr[3], out replacement))
								ptr[3] = replacement;
							ptr += 4;
						}
						if ((count & 2) != 0)
						{
							if (dictionary.TryGetValue(ptr[0], out Color replacement))
								ptr[0] = replacement;
							if (dictionary.TryGetValue(ptr[1], out replacement))
								ptr[1] = replacement;
							ptr += 2;
						}
						if ((count & 1) != 0)
						{
							if (dictionary.TryGetValue(ptr[0], out Color replacement))
								ptr[0] = replacement;
						}
					}
				}
			}
		}

		/// <summary>
		/// Remap each color by passing it through the given matrix transform.
		/// </summary>
		/// <param name="matrix">The matrix to multiply each color vector by.</param>
		public void RemapMatrix(Matrix4x4f matrix)
		{
			unsafe
			{
				fixed (Color* imageBase = _data)
				{
					Color* ptr = imageBase;
					Color* end = imageBase + Width * Height;

					while (ptr < end)
					{
						Color src = *ptr;
						Vector3f v = new Vector3f(src.R * (1 / 255f), src.G * (1 / 255f), src.B * (1 / 255f));
						v = matrix * v;
						Color result = new Color((int)(v.X * 255 + 0.5f), (int)(v.Y * 255 + 0.5f), (int)(v.Z * 255 + 0.5f), src.A);
						*ptr++ = result;
					}
				}
			}
		}

		/// <summary>
		/// Apply gamma to all three color values (i.e., convert each color to [0, 1] and then
		/// raise it to the given power).
		/// </summary>
		/// <param name="amount">The amount of gamma adjustment to apply.  &gt;1 is brighter, &lt;1 is darker.</param>
		public void Gamma(float amount)
		{
			Span<byte> remapTable = stackalloc byte[256];
			for (int i = 0; i < 256; i++)
			{
				int raised = (int)(Math.Pow(i / 255.0f, amount) * 255.0f + 0.5f);
				remapTable[i] = (byte)Math.Max(Math.Min(raised, 255), 0);
			}

			unsafe
			{
				fixed (Color* imageBase = _data)
				fixed (byte* remap = remapTable)
				{
					Color* ptr = imageBase;
					Color* end = imageBase + Width * Height;

					while (ptr < end)
					{
						*ptr = new Color(remap[ptr->R], remap[ptr->G], remap[ptr->B], ptr->A);
						ptr++;
					}
				}
			}
		}

		/// <summary>
		/// Convert the image to grayscale, factoring in relative brightnesses.
		/// </summary>
		public void Grayscale()
		{
			unsafe
			{
				fixed (Color* imageBase = _data)
				{
					Color* ptr = imageBase;
					Color* end = imageBase + Width * Height;

					while (ptr < end)
					{
						Color src = *ptr;
						int y = (
								( (int)(65536 * 0.299f + 0.5f) * src.R
								+ (int)(65536 * 0.587f + 0.5f) * src.G
								+ (int)(65536 * 0.114f + 0.5f) * src.B) + 32768
							) >> 16;
						Color result = new Color(y, y, y, src.A);
						*ptr++ = result;
					}
				}
			}
		}

		/// <summary>
		/// Remap the image to a sepia-tone version of itself by forcing
		/// every color's position in YIQ-space.
		/// </summary>
		/// <param name="amount">How saturated the sepia is.  0.0 = grayscale, 1.0 = orange, -1.0 = blue.</param>
		public void Sepia(float amount = 0.25f)
		{
			int newI = (int)(amount * 255 + 0.5f);

			unsafe
			{
				fixed (Color* imageBase = _data)
				{
					Color* ptr = imageBase;
					Color* end = imageBase + Width * Height;

					while (ptr < end)
					{
						Color src = *ptr;
						float y = 0.299f * src.R + 0.587f * src.G + 0.114f * src.B;
						float i = 0.5959f * src.R - 0.2746f * src.G - 0.3213f * src.B;
						float q = 0.2115f * src.R - 0.5227f * src.G + 0.3112f * src.B;
						i = newI;
						q = 0;
						float r = 1f * y + 0.956f * i + 0.619f * q;
						float g = 1f * y - 0.272f * i - 0.647f * q;
						float b = 1f * y - 1.106f * i + 1.703f * q;
						Color result = new Color((int)(r + 0.5f), (int)(g + 0.5f), (int)(b + 0.5f), src.A);
						*ptr++ = result;
					}
				}
			}
		}

		#endregion

		#region Filling

		/// <summary>
		/// Fill the given image as fast as possible with the given color.
		/// </summary>
		public void Fill(Color color)
		{
			int count = _data.Length;
			if (count <= 0)
				return;

			unsafe
			{
				fixed (Color* bufferStart = _data)
				{
					Color* dest = bufferStart;

					// Unroll and do 8 at a time, when possible.
					while (count >= 8)
					{
						dest[0] = color;
						dest[1] = color;
						dest[2] = color;
						dest[3] = color;
						dest[4] = color;
						dest[5] = color;
						dest[6] = color;
						dest[7] = color;
						dest += 8;
						count -= 8;
					}

					// Do whatever's left over.
					while (count-- > 0)
						*dest++ = color;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FillRect(Rect rect, Color drawColor, BlitFlags blitFlags = BlitFlags.Copy)
			=> FillRect(rect.X, rect.Y, rect.Width, rect.Height, drawColor, blitFlags);

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public void FillRect(int x, int y, int width, int height, Color drawColor,
			BlitFlags blitFlags = BlitFlags.Copy)
		{
			if ((blitFlags & BlitFlags.FastUnsafe) == 0
				&& !ClipRect(ref x, ref y, ref width, ref height))
				return;

			unsafe
			{
				fixed (Color* destBase = _data)
				{
					Color* dest = destBase + Width * y + x;
					int nextLine = Width - width;

					BlitFlags drawMode = 0;
					switch (blitFlags & BlitFlags.AlphaMode)
					{
						default:
						case BlitFlags.Copy:
							drawMode = BlitFlags.Copy;
							break;

						case BlitFlags.Transparent:
							if (drawColor.A == 0)
								return;
							drawMode = BlitFlags.Copy;
							break;

						case BlitFlags.Alpha:
							if (drawColor.A == 0)
								return;
							drawMode = drawColor.A == 255 ? BlitFlags.Copy : BlitFlags.Alpha;
							break;

						case BlitFlags.PMAlpha:
							if (drawColor.A == 0)
								return;
							drawMode = drawColor.A == 255 ? BlitFlags.Copy : BlitFlags.PMAlpha;
							break;
					}

					switch (drawMode)
					{
						case BlitFlags.Copy:
							while (height-- != 0)
							{
								int count = width;
								while (count-- != 0)
									*dest++ = drawColor;
								dest += nextLine;
							}
							break;

						case BlitFlags.Alpha:
							while (height-- != 0)
							{
								int count = width;
								while (count-- != 0)
								{
									Color d = *dest;
									uint a = drawColor.A, ia = (uint)(255 - drawColor.A);
									uint r = drawColor.R * a + d.R * ia + 127;
									uint g = drawColor.G * a + d.G * ia + 127;
									uint b = drawColor.B * a + d.B * ia + 127;
									r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
									g = (g + 1 + (g >> 8)) >> 8;
									b = (b + 1 + (b >> 8)) >> 8;
									*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)255);
								}
								dest += nextLine;
							}
							break;

						case BlitFlags.PMAlpha:
							while (height-- != 0)
							{
								int count = width;
								while (count-- != 0)
								{
									Color d = *dest;
									uint ia = (uint)(255 - drawColor.A);
									uint r = drawColor.R + d.R * ia + 127;
									uint g = drawColor.G + d.G * ia + 127;
									uint b = drawColor.B + d.B * ia + 127;
									r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
									g = (g + 1 + (g >> 8)) >> 8;
									b = (b + 1 + (b >> 8)) >> 8;
									*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)255);
								}
								dest += nextLine;
							}
							break;
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FillGradientRect(Rect rect, Color topLeft, Color topRight, Color bottomLeft, Color bottomRight,
			BlitFlags blitFlags = BlitFlags.Copy)
			=> FillGradientRect(rect.X, rect.Y, rect.Width, rect.Height,
				topLeft, topRight, bottomLeft, bottomRight, blitFlags);

		private struct GradientValues
		{
			public int x;
			public int y;
			public int width;

			public uint r1;
			public uint g1;
			public uint b1;
			public uint a1;
			public uint r2;
			public uint g2;
			public uint b2;
			public uint a2;

			public int rd1;
			public int gd1;
			public int bd1;
			public int ad1;
			public int rd2;
			public int gd2;
			public int bd2;
			public int ad2;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public void FillGradientRect(int x, int y, int width, int height,
			Color topLeft, Color topRight, Color bottomLeft, Color bottomRight,
			BlitFlags blitFlags = BlitFlags.Copy)
		{
			if ((blitFlags & BlitFlags.FastUnsafe) == 0
				&& !ClipRect(ref x, ref y, ref width, ref height))
				return;

			int ooCount = 65536 / (height - 1);

			GradientValues v = default;

			v.x = x;
			v.y = y;
			v.width = width;

			v.r1 = (uint)topLeft.R << 16;
			v.g1 = (uint)topLeft.G << 16;
			v.b1 = (uint)topLeft.B << 16;
			v.a1 = (uint)topLeft.A << 16;
			v.r2 = (uint)topRight.R << 16;
			v.g2 = (uint)topRight.G << 16;
			v.b2 = (uint)topRight.B << 16;
			v.a2 = (uint)topRight.A << 16;

			v.rd1 = (bottomLeft.R - topLeft.R) * ooCount;
			v.gd1 = (bottomLeft.G - topLeft.G) * ooCount;
			v.bd1 = (bottomLeft.B - topLeft.B) * ooCount;
			v.ad1 = (bottomLeft.A - topLeft.A) * ooCount;
			v.rd2 = (bottomRight.R - topRight.R) * ooCount;
			v.gd2 = (bottomRight.G - topRight.G) * ooCount;
			v.bd2 = (bottomRight.B - topRight.B) * ooCount;
			v.ad2 = (bottomRight.A - topRight.A) * ooCount;

			BlitFlags drawMode = blitFlags & BlitFlags.AlphaMode;

			while (height-- != 0)
			{
				FillGradientSpanFast(v, drawMode);

				v.r1 = (uint)((int)v.r1 + v.rd1);
				v.g1 = (uint)((int)v.g1 + v.gd1);
				v.b1 = (uint)((int)v.b1 + v.bd1);
				v.a1 = (uint)((int)v.a1 + v.ad1);
				v.r2 = (uint)((int)v.r2 + v.rd2);
				v.g2 = (uint)((int)v.g2 + v.gd2);
				v.b2 = (uint)((int)v.b2 + v.bd2);
				v.a2 = (uint)((int)v.a2 + v.ad2);

				v.y++;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FillGradientSpanFast(in GradientValues v,
			BlitFlags drawMode = BlitFlags.Copy)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				{
					Color* row = destBase + Width * v.y;

					int ooCount = 65536 / (v.width - 1);

					uint r = v.r1;
					uint g = v.g1;
					uint b = v.b1;
					uint a = v.a1;

					int rd = (int)((((long)v.r2 - (long)v.r1) * ooCount) >> 16);
					int gd = (int)((((long)v.g2 - (long)v.g1) * ooCount) >> 16);
					int bd = (int)((((long)v.b2 - (long)v.b1) * ooCount) >> 16);
					int ad = (int)((((long)v.a2 - (long)v.a1) * ooCount) >> 16);

					Color* dest = row + v.x;

					switch (drawMode)
					{
						case BlitFlags.Copy:
							int count = v.width;
							while (count-- != 0)
							{
								*dest++ = new Color(
									(byte)((r + 32768) >> 16),
									(byte)((g + 32768) >> 16),
									(byte)((b + 32768) >> 16),
									(byte)((a + 32768) >> 16));
								r = (uint)((int)r + rd);
								g = (uint)((int)g + gd);
								b = (uint)((int)b + bd);
								a = (uint)((int)a + ad);
							}
							break;

						case BlitFlags.Alpha:
							count = v.width;
							while (count-- != 0)
							{
								Color d = *dest;
								uint ca = ((a + 32768) >> 16);
								uint ax = ca, iax = (uint)(255 - ca);
								uint rx = ((r + 32768) >> 16) * ax + d.R * iax + 127;
								uint gx = ((g + 32768) >> 16) * ax + d.G * iax + 127;
								uint bx = ((b + 32768) >> 16) * ax + d.B * iax + 127;
								rx = (rx + 1 + (rx >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
								gx = (gx + 1 + (gx >> 8)) >> 8;
								bx = (bx + 1 + (bx >> 8)) >> 8;
								*dest++ = new Color((byte)rx, (byte)gx, (byte)bx, (byte)255);
								r = (uint)((int)r + rd);
								g = (uint)((int)g + gd);
								b = (uint)((int)b + bd);
								a = (uint)((int)a + ad);
							}
							break;

						case BlitFlags.PMAlpha:
							count = v.width;
							while (count-- != 0)
							{
								Color d = *dest;
								uint ca = ((a + 32768) >> 16);
								uint iax = (uint)(255 - ca);
								uint rx = ((r + 32768) >> 16) + d.R * iax + 127;
								uint gx = ((g + 32768) >> 16) + d.G * iax + 127;
								uint bx = ((b + 32768) >> 16) + d.B * iax + 127;
								rx = (rx + 1 + (rx >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
								gx = (gx + 1 + (gx >> 8)) >> 8;
								bx = (bx + 1 + (bx >> 8)) >> 8;
								*dest++ = new Color((byte)rx, (byte)gx, (byte)bx, (byte)255);
								r = (uint)((int)r + rd);
								g = (uint)((int)g + gd);
								b = (uint)((int)b + bd);
								a = (uint)((int)a + ad);
							}
							break;
					}
				}
			}
		}

		#endregion

		#region Line drawing

		/// <summary>
		/// An implementation of Bresenham's line-drawing routine, with Cohen-Sutherland
		/// clipping to the image (unless you pass BlitFlags.FastUnsafe).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawLine(Vector2i p1, Vector2i p2, Color color, BlitFlags blitFlags = BlitFlags.Copy, bool skipStart = false)
			=> DrawLine(p1.X, p1.Y, p2.X, p2.Y, color, blitFlags, skipStart);

		/// <summary>
		/// An implementation of Bresenham's line-drawing routine, with Cohen-Sutherland
		/// clipping to the image (unless you pass BlitFlags.FastUnsafe).
		/// </summary>
		public void DrawLine(int x1, int y1, int x2, int y2, Color color, BlitFlags blitFlags = BlitFlags.Copy, bool skipStart = false)
		{
			if ((blitFlags & BlitFlags.FastUnsafe) != 0)
			{
				DrawLineFastUnsafe(x1, y1, x2, y2, color, blitFlags, false);
				return;
			}

			OutCode code1 = ComputeOutCode(x1, y1);
			OutCode code2 = ComputeOutCode(x2, y2);

			while (true)
			{
				if ((code1 | code2) == 0)
				{
					DrawLineFastUnsafe(x1, y1, x2, y2, color, blitFlags, skipStart);
					return;
				}
				else if ((code1 & code2) != 0)
				{
					return;
				}
				else
				{
					OutCode outCode = code1 != 0 ? code1 : code2;

					int x = 0, y = 0;
					if ((outCode & OutCode.Bottom) != 0)
					{
						x = x1 + (int)((long)(x2 - x1) * (Height - 1 - y1) / (y2 - y1));
						y = Height - 1;
					}
					else if ((outCode & OutCode.Top) != 0)
					{
						x = x1 + (int)((long)(x2 - x1) * -y1 / (y2 - y1));
						y = 0;
					}
					else if ((outCode & OutCode.Right) != 0)
					{
						y = y1 + (int)((long)(y2 - y1) * (Width - 1 - x1) / (x2 - x1));
						x = Width - 1;
					}
					else if ((outCode & OutCode.Left) != 0)
					{
						y = y1 + (int)((long)(y2 - y1) * -x1 / (x2 - x1));
						x = 0;
					}

					if (outCode == code1)
					{
						x1 = x;
						y1 = y;
						code1 = ComputeOutCode(x1, y1);
						skipStart = false;
					}
					else
					{
						x2 = x;
						y2 = y;
						code2 = ComputeOutCode(x2, y2);
					}
				}
			}
		}

		/// <summary>
		/// OutCode flags.
		/// </summary>
		[Flags]
		private enum OutCode
		{
			Inside = 0,

			Left = 1 << 0,
			Right = 1 << 1,
			Top = 1 << 2,
			Bottom = 1 << 3,
		}

		/// <summary>
		/// Calculate outcodes for Cohen-Sutherland clipping against the image boundaries.
		/// </summary>
		private OutCode ComputeOutCode(int x, int y)
		{
			OutCode code = OutCode.Inside;

			if (x < 0)
				code |= OutCode.Left;
			else if (x >= Width)
				code |= OutCode.Right;

			if (y < 0)
				code |= OutCode.Top;
			else if (y >= Height)
				code |= OutCode.Bottom;

			return code;
		}

		/// <summary>
		/// An implementation of Bresenham's line-drawing routine.  This is very
		/// fast, but it performs no clipping.
		/// </summary>
		private void DrawLineFastUnsafe(int x1, int y1, int x2, int y2, Color color, BlitFlags blitFlags, bool skipStart)
		{
			int dx = Math.Abs(x2 - x1);
			int sx = x1 < x2 ? 1 : -1;
			int dy = -Math.Abs(y2 - y1);
			int sy = y1 < y2 ? Width : -Width;
			int index = x1 + y1 * Width;
			int end = x2 + y2 * Width;

			int err = dx + dy;

			if ((blitFlags & BlitFlags.AlphaMode) != BlitFlags.Copy)
			{
				if (color.A == 0)
					return;
				if (color.A == 255 || (blitFlags & BlitFlags.AlphaMode) == BlitFlags.Transparent)
					blitFlags = blitFlags & ~BlitFlags.AlphaMode | BlitFlags.Copy;
			}

			if (skipStart)
			{
				if (index == end)
					return;

				int e2 = 2 * err;
				if (e2 >= dy) { err += dy; index += sx; }
				if (e2 <= dx) { err += dx; index += sy; }
			}

			unsafe
			{
				switch (blitFlags & BlitFlags.AlphaMode)
				{
					case BlitFlags.Copy:
						while (true)
						{
							_data[index] = color;

							if (index == end)
								break;

							int e2 = 2 * err;
							if (e2 >= dy) { err += dy; index += sx; }
							if (e2 <= dx) { err += dx; index += sy; }
						}
						break;

					case BlitFlags.Alpha:
						while (true)
						{
							Color d = _data[index];
							uint a = color.A, ia = (uint)(255 - color.A);
							uint r = color.R * a + d.R * ia + 127;
							uint g = color.G * a + d.G * ia + 127;
							uint b = color.B * a + d.B * ia + 127;
							r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
							g = (g + 1 + (g >> 8)) >> 8;
							b = (b + 1 + (b >> 8)) >> 8;
							_data[index] = new Color((byte)r, (byte)g, (byte)b, (byte)255);

							if (index == end)
								break;

							int e2 = 2 * err;
							if (e2 >= dy) { err += dy; index += sx; }
							if (e2 <= dx) { err += dx; index += sy; }
						}
						break;

					case BlitFlags.PMAlpha:
						while (true)
						{
							Color d = _data[index];
							uint ia = (uint)(255 - color.A);
							uint r = d.R * ia + 127;
							uint g = d.G * ia + 127;
							uint b = d.B * ia + 127;
							r = color.R + ((r + 1 + (r >> 8)) >> 8);
							g = color.G + ((g + 1 + (g >> 8)) >> 8);
							b = color.B + ((b + 1 + (b >> 8)) >> 8);
							_data[index] = new Color((byte)r, (byte)g, (byte)b, (byte)255);

							if (index == end)
								break;

							int e2 = 2 * err;
							if (e2 >= dy) { err += dy; index += sx; }
							if (e2 <= dx) { err += dx; index += sy; }
						}
						break;
				}
			}
		}

		#endregion

		#region Polygon filling

		/// <summary>
		/// Draw a "thick" line.  This does *not* include end caps.
		/// </summary>
		/// <param name="x1">The start X coordinate of the line.</param>
		/// <param name="y1">The start Y of the line.</param>
		/// <param name="x2">The end X coordinate of the line.</param>
		/// <param name="y2">The end Y of the line.</param>
		/// <param name="thickness">The thickness of the line, in pixels.  This can be fractional.</param>
		/// <param name="color">The color of the line.</param>
		/// <param name="blitFlags">The blitting mode (opaque, alpha, premultiplied alpha).</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawThickLine(int x1, int y1, int x2, int y2, float thickness,
			Color color, BlitFlags blitFlags = BlitFlags.Copy)
			=> DrawThickLine(new Vector2f(x1 + 0.5f, y1 + 0.5f), new Vector2f(x2 + 0.5f, y2 + 0.5f),
				thickness, color, blitFlags);

		/// <summary>
		/// Draw a "thick" line.  This does *not* include end caps.
		/// </summary>
		/// <param name="start">The start point of the line.</param>
		/// <param name="end">The end point of the line.</param>
		/// <param name="thickness">The thickness of the line, in pixels.  This can be fractional.</param>
		/// <param name="color">The color of the line.</param>
		/// <param name="blitFlags">The blitting mode (opaque, alpha, premultiplied alpha).</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawThickLine(Vector2i start, Vector2i end, float thickness,
			Color color, BlitFlags blitFlags = BlitFlags.Copy)
			=> DrawThickLine(new Vector2f(start.X + 0.5f, start.Y + 0.5f), new Vector2f(end.X + 0.5f, end.Y + 0.5f),
				thickness, color, blitFlags);

		/// <summary>
		/// Draw a "thick" line.  This does *not* include end caps.
		/// </summary>
		/// <param name="start">The start point of the line.</param>
		/// <param name="end">The end point of the line.</param>
		/// <param name="thickness">The thickness of the line, in pixels.  This can be fractional.</param>
		/// <param name="color">The color of the line.</param>
		/// <param name="blitFlags">The blitting mode (opaque, alpha, premultiplied alpha).</param>
		public void DrawThickLine(Vector2f start, Vector2f end, float thickness,
			Color color, BlitFlags blitFlags = BlitFlags.Copy)
		{
			Span<Vector2f> points = stackalloc Vector2f[4];

			Vector2f unit = (end - start).Normalized();

			Vector2f perpUnit = new Vector2f(-unit.Y, unit.X);
			float halfThickness = thickness * 0.5f;
			Vector2f offset = perpUnit * halfThickness;

			points[0] = start + offset;
			points[1] = end + offset;
			points[2] = end - offset;
			points[3] = start - offset;

			FillPolygon(points, color, blitFlags);
		}

		/// <summary>
		/// Fill a polygon using the odd/even technique.  The polygon will be
		/// clipped to the image if it exceeds the image's boundary.
		/// </summary>
		/// <param name="points">The points describing the polygon.</param>
		/// <param name="color">The color to fill the polygon with.</param>
		/// <param name="blitFlags">The blitting mode (opaque, alpha, premultiplied alpha).</param>
		public void FillPolygon(ReadOnlySpan<Vector2f> points, Color color, BlitFlags blitFlags = BlitFlags.Copy)
		{
			BlitFlags drawMode = 0;
			switch (blitFlags & BlitFlags.AlphaMode)
			{
				default:
				case BlitFlags.Copy:
					drawMode = BlitFlags.Copy;
					break;

				case BlitFlags.Transparent:
					if (color.A == 0)
						return;
					drawMode = BlitFlags.Copy;
					break;

				case BlitFlags.Alpha:
					if (color.A == 0)
						return;
					drawMode = color.A == 255 ? BlitFlags.Copy : BlitFlags.Alpha;
					break;

				case BlitFlags.PMAlpha:
					if (color.A == 0)
						return;
					drawMode = color.A == 255 ? BlitFlags.Copy : BlitFlags.PMAlpha;
					break;
			}

			int numXes;
			Span<double> xes = points.Length < 128
				? stackalloc double[points.Length + 2]
				: new double[points.Length + 2];

			// Find the polygon's vertical extent, and clamp it to the image boundary.
			double minY = double.PositiveInfinity;
			double maxY = double.NegativeInfinity;
			foreach (Vector2f point in points)
			{
				minY = Math.Min(minY, point.Y);
				maxY = Math.Max(maxY, point.Y);
			}

			// Clamp to the extent of the image.
			int iMinY = (int)Math.Min(Math.Max(minY, 0), Height + 1);
			int iMaxY = (int)(Math.Min(Math.Max(minY, 0), Height + 1) + 1f);

			// Loop through the rows of the image.
			for (int y = iMinY; y < iMaxY; y++)
			{
				// Build a list of nodes on this row.
				double dy = y + 0.5f;
				numXes = 0;
				Vector2f prev = points[points.Length - 1];
				for (int i = 0; i < points.Length; i++)
				{
					Vector2f current = points[i];
					if (current.Y < dy && dy <= prev.Y || prev.Y < dy && dy <= current.Y)
					{
						xes[numXes++] = current.X
							+ (dy - current.Y) / (prev.Y - current.Y) * (prev.X - current.X);
					}
					prev = current;
				}
				Span<double> usedXes = xes.Slice(0, numXes);

				// Sort the X coordinates in ascending order.
				usedXes.Sort();

				// Fill the pixels between edge pairs.
				FillSpansFast(y, usedXes, color, drawMode);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void FillSpansFast(int y, ReadOnlySpan<double> points, Color color,
			BlitFlags drawMode = BlitFlags.Copy)
		{
			unsafe
			{
				fixed (Color* destBase = _data)
				{
					Color* row = destBase + Width * y;

					switch (drawMode)
					{
						case BlitFlags.Copy:
							for (int i = 0; i < points.Length; i += 2)
							{
								if (points[i] >= Width)
									break;
								if (points[i + 1] <= 0)
									continue;
								int start = Math.Max((int)points[i], 0);
								int end = Math.Min((int)points[i + 1], Width - 1);
								int count = end - start;
								Color* dest = row + start;
								while (count-- != 0)
								{
									*dest++ = color;
								}
							}
							break;

						case BlitFlags.Alpha:
							for (int i = 0; i < points.Length; i += 2)
							{
								if (points[i] >= Width)
									break;
								if (points[i + 1] <= 0)
									continue;
								int start = Math.Max((int)points[i], 0);
								int end = Math.Min((int)points[i + 1], Width - 1);
								int count = end - start;
								Color* dest = row + start;
								while (count-- != 0)
								{
									Color d = *dest;
									uint a = color.A, ia = (uint)(255 - color.A);
									uint r = color.R * a + d.R * ia + 127;
									uint g = color.G * a + d.G * ia + 127;
									uint b = color.B * a + d.B * ia + 127;
									r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
									g = (g + 1 + (g >> 8)) >> 8;
									b = (b + 1 + (b >> 8)) >> 8;
									*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)255);
								}
							}
							break;

						case BlitFlags.PMAlpha:
							for (int i = 0; i < points.Length; i += 2)
							{
								if (points[i] >= Width)
									break;
								if (points[i + 1] <= 0)
									continue;
								int start = Math.Max((int)points[i], 0);
								int end = Math.Min((int)points[i + 1], Width - 1);
								int count = end - start;
								Color* dest = row + start;
								while (count-- != 0)
								{
									Color d = *dest;
									uint ia = (uint)(255 - color.A);
									uint r = color.R + d.R * ia + 127;
									uint g = color.G + d.G * ia + 127;
									uint b = color.B + d.B * ia + 127;
									r = (r + 1 + (r >> 8)) >> 8;    // Divide by 255, faster than "r /= 255"
									g = (g + 1 + (g >> 8)) >> 8;
									b = (b + 1 + (b >> 8)) >> 8;
									*dest++ = new Color((byte)r, (byte)g, (byte)b, (byte)255);
								}
							}
							break;
					}
				}
			}
		}

		#endregion

		#region Special drawing shapes

		/// <summary>
		/// Draw a rectangle, with the given thickness.  The outer coordinates of the rectangle
		/// are provided, and the thickness goes *inward*.
		/// </summary>
		public void DrawRect(Rect rect, Color color, int thickness = 1, BlitFlags blitFlags = BlitFlags.Copy)
			=> DrawRect(rect.X, rect.Y, rect.Width, rect.Height, color, thickness, blitFlags);

		/// <summary>
		/// Draw a rectangle, with the given thickness.  The outer coordinates of the rectangle
		/// are provided, and the thickness goes *inward*.
		/// </summary>
		public void DrawRect(int x, int y, int width, int height, Color color, int thickness = 1,
			BlitFlags blitFlags = BlitFlags.Copy)
		{
			if (thickness >= width / 2 || thickness >= height / 2)
			{
				FillRect(x, y, width, height, color, blitFlags);
				return;
			}

			// Top edge.
			FillRect(x, y, width, thickness, color, blitFlags);

			// Bottom edge.
			FillRect(x, y + height - thickness, width, thickness, color, blitFlags);

			// Left edge.
			int edgeY = y + thickness;
			int edgeHeight = height - thickness * 2;
			FillRect(x, edgeY, thickness, edgeHeight, color, blitFlags);

			// Right edge.
			FillRect(x + width - thickness, edgeY, thickness, edgeHeight, color, blitFlags);
		}

		#endregion

		#region Curve drawing

		/// <summary>
		/// Draw a cubic Bézier spline.
		/// </summary>
		/// <param name="p1">The start point.</param>
		/// <param name="c1">The control point for the start point.</param>
		/// <param name="c2">The control point for the end point.</param>
		/// <param name="p2">The end point.</param>
		/// <param name="color">The color to draw the curve in.</param>
		/// <param name="steps">How many steps (line segments) to use to approximate the curve.
		/// By default, this is derived from the distances between the points.</param>
		/// <param name="blitFlags">Flags to control how the line is drawn.</param>
		/// <param name="skipStart">Whether to plot the first point in the curve or to skip it.</param>
		public void DrawBezier(Vector2f p1, Vector2f c1, Vector2f c2, Vector2f p2,
			Color color, int steps = 0, BlitFlags blitFlags = BlitFlags.Copy, bool skipStart = false)
		{
			if (steps <= 0)
			{
				double d1 = (c1 - p1).Length;
				double d2 = (c2 - c1).Length;
				double d3 = (p2 - c2).Length;
				steps = Math.Max((int)((d1 + d2 + d3) * 0.25f), 20);
			}

			double ooSteps = 1.0 / steps;

			int lastX = (int)(p1.X + 0.5f), lastY = (int)(p1.Y + 0.5f);

			// Plot the first point.
			if (!skipStart)
				DrawLine(lastX, lastY, lastX, lastY, color, blitFlags);

			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			static Vector2f CalcBezier(Vector2f start, Vector2f c1, Vector2f c2, Vector2f end, double t)
			{
				double it = 1 - t;
				double it2 = it * it;
				double t2 = (double)t * t;

				double a = it * it2;
				double b = 3 * it2 * t;
				double c = 3 * it * t2;
				double d = t2 * t;

				double x = a * start.X + b * c1.X + c * c2.X + d * end.X;
				double y = a * start.Y + b * c1.Y + c * c2.Y + d * end.Y;

				return new Vector2f((float)x, (float)y);
			}

			// Draw successive line segments, skipping the start of each line segment.
			for (int i = 1; i <= steps; i++)
			{
				Vector2f p = CalcBezier(p1, c1, c2, p2, i * ooSteps);

				int ix = (int)(p.X + 0.5f), iy = (int)(p.Y + 0.5f);

				if (ix != lastX || iy != lastY)
					DrawLine(lastX, lastY, ix, iy, color, blitFlags, skipStart: true);

				lastX = ix;
				lastY = iy;
			}
		}

		#endregion

		#region Content testing

		/// <summary>
		/// Given a rectangle that may surround content, find the smallest width of
		/// an equivalent rectangle that does not contain any right-side columns that
		/// are transparent.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <returns>The smallest width that surrounds actual non-transparent content
		/// within that rectangle, which may be the original width.</returns>
		public int MeasureContentWidth(Rect rect)
		{
			for (int width = rect.Width; width > 0; width--)
			{
				if (!IsColumnTransparent(rect.X + width - 1, rect.Y, rect.Height))
					return width;
			}
			return 0;
		}

		/// <summary>
		/// Given a rectangle that may surround content, find the smallest height of
		/// an equivalent rectangle that does not contain any bottom-edge rows that
		/// are transparent.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <returns>The smallest height that surrounds actual non-transparent content
		/// within that rectangle, which may be the original height.</returns>
		public int MeasureContentHeight(Rect rect)
		{
			for (int height = rect.Height; height > 0; height--)
			{
				if (!IsRowTransparent(rect.X, rect.Y + height - 1, rect.Width))
					return height;
			}
			return 0;
		}

		/// <summary>
		/// Determine if the row of pixels starting at (x, y) and of the given width
		/// is entirely transparent (color.A == 0).  Pixels outside the image will be
		/// treated as transparent, and a zero-width row will as well.
		/// </summary>
		/// <param name="x">The starting X offset of the row.</param>
		/// <param name="y">The vertical offset of the row.</param>
		/// <param name="width">The row's width in pixels.</param>
		/// <returns>True if the row is entirely transparent, false if it contains at
		/// least one opaque or transparent pixel.</returns>
		public bool IsRowTransparent(int x, int y, int width)
		{
			if (y < 0 || y >= Height)
				return true;
			if (x < 0)
			{
				width += x;
				x = 0;
			}
			if (x + width > Width)
				width = Width - x;
			if (width <= 0)
				return true;

			unsafe
			{
				fixed (Color* basePtr = _data)
				{
					Color* src = basePtr + y * Width + x;
					for (int i = 0; i < width; i++)
					{
						if (src->A != 0)
							return false;
						src++;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Determine if the column of pixels starting at (x, y) and of the given height
		/// is entirely transparent (color.A == 0).  Pixels outside the image will be
		/// treated as transparent, and a zero-height column will as well.
		/// </summary>
		/// <param name="x">The horizontal offset of the column.</param>
		/// <param name="y">The starting Y offset of the column.</param>
		/// <param name="height">The column's height in pixels.</param>
		/// <returns>True if the column is entirely transparent, false if it contains at
		/// least one opaque or transparent pixel.</returns>
		public bool IsColumnTransparent(int x, int y, int height)
		{
			if (x < 0 || x >= Width)
				return true;
			if (y < 0)
			{
				height += y;
				y = 0;
			}
			if (y + height > Height)
				height = Height - y;
			if (height <= 0)
				return true;

			unsafe
			{
				fixed (Color* basePtr = _data)
				{
					Color* src = basePtr + y * Width + x;
					for (int i = 0; i < height; i++)
					{
						if (src->A != 0)
							return false;
						src += Width;
					}
				}
			}

			return true;
		}

		#endregion

		#region Equality and hash codes

		/// <summary>
		/// Compare this image against another to determine if they're pixel-for-pixel identical.
		/// </summary>
		/// <param name="obj">The other object to compare against.</param>
		/// <returns>True if the other object is an identical image to this image, false otherwise.</returns>
		public override bool Equals(object? obj)
			=> obj is Image other && Equals(other);

		/// <summary>
		/// Compare this image against another to determine if they're pixel-for-pixel
		/// identical.  This runs in O(n) worst-case time, but that's only hit if the
		/// two images have identical dimensions:  For images with unequal dimensions,
		/// this always runs in O(1) time.
		/// </summary>
		/// <param name="other">The other image to compare against.</param>
		/// <returns>True if the other image is an identical image to this image, false otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public unsafe bool Equals(Image? other)
		{
			if (ReferenceEquals(this, other))
				return true;
			if (ReferenceEquals(other, null))
				return false;

			if (Width != other.Width
				|| Height != other.Height
				|| _data.Length != other._data.Length)
				return false;

			fixed (Color* dataBase = _data)
			fixed (Color* otherDataBase = other._data)
			{
				uint* data = (uint*)dataBase;
				uint* otherData = (uint*)otherDataBase;
				int count = _data.Length;
				while (count >= 8)
				{
					if (data[0] != otherData[0]) return false;
					if (data[1] != otherData[1]) return false;
					if (data[2] != otherData[2]) return false;
					if (data[3] != otherData[3]) return false;
					if (data[4] != otherData[4]) return false;
					if (data[5] != otherData[5]) return false;
					if (data[6] != otherData[6]) return false;
					if (data[7] != otherData[7]) return false;
					data += 8;
					otherData += 8;
					count -= 8;
				}
				if ((count & 4) != 0)
				{
					if (data[0] != otherData[0]) return false;
					if (data[1] != otherData[1]) return false;
					if (data[2] != otherData[2]) return false;
					if (data[3] != otherData[3]) return false;
					data += 4;
					otherData += 4;
				}
				if ((count & 2) != 0)
				{
					if (data[0] != otherData[0]) return false;
					if (data[1] != otherData[1]) return false;
					data += 2;
					otherData += 2;
				}
				if ((count & 1) != 0)
				{
					if (data[0] != otherData[0]) return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Compare two images for equality.  This will perform a pixel-for-pixel test
		/// if necessary; it is not just a reference-equality test.
		/// </summary>
		/// <param name="a">The first image to compare.</param>
		/// <param name="b">The second image to compare.</param>
		/// <returns>True if the images are identical, false otherwise.</returns>
		public static bool operator ==(Image? a, Image? b)
			=> !ReferenceEquals(a, null) && a.Equals(b);

		/// <summary>
		/// Compare two images for equality.  This will perform a pixel-for-pixel test
		/// if necessary; it is not just a reference-equality test.
		/// </summary>
		/// <param name="a">The first image to compare.</param>
		/// <param name="b">The second image to compare.</param>
		/// <returns>False if the images are identical, true otherwise.</returns>
		public static bool operator !=(Image? a, Image? b)
			=> ReferenceEquals(a, null) || !a.Equals(b);

		/// <summary>
		/// Calculate a hash code representing the pixels of this image.  This runs
		/// in O(width*height) time and does not cache its result, so don't invoke this
		/// unless you need it.
		/// </summary>
		public unsafe override int GetHashCode()
		{
			fixed (Color* dataBase = _data)
			{
				uint hashCode = 0;

				uint* data = (uint*)dataBase;
				int count = _data.Length;
				while (count-- != 0)
				{
					hashCode = unchecked(hashCode * 65599 + *data++);
				}

				return unchecked((int)hashCode);
			}
		}

		#endregion
	}
}
