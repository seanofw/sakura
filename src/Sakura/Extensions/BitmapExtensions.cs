using System.Drawing;

namespace Sakura.Extensions
{
	public static class BitmapExtensions
	{
		public static Img.Image ToImage(this Bitmap bitmap)
		{
			Img.Image image = new Img.Image(bitmap.Width, bitmap.Height);

			System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			try
			{
				unsafe
				{
					fixed (MathLib.Color* destBase = image.Data)
					{
						byte* srcBase = (byte*)bitmapData.Scan0;
						for (int y = 0; y < bitmap.Height; y++)
						{
							uint* srcRowPtr = (uint*)(srcBase + y * bitmapData.Stride);
							uint* destRowPtr = (uint*)(destBase + y * image.Width);
							for (int x = 0; x < bitmap.Width; x++)
							{
								*destRowPtr++ = *srcRowPtr++;
							}
						}
					}
				}
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}

			return image;
		}

		public static Bitmap ToBitmap(this Img.Image image)
		{
			Bitmap bitmap = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			try
			{
				unsafe
				{
					fixed (MathLib.Color* srcBase = image.Data)
					{
						byte* destBase = (byte*)bitmapData.Scan0;
						for (int y = 0; y < bitmap.Height; y++)
						{
							uint* srcRowPtr = (uint*)(srcBase + y * image.Width);
							uint* destRowPtr = (uint*)(destBase + y * bitmapData.Stride);
							for (int x = 0; x < bitmap.Width; x++)
							{
								*destRowPtr++ = *srcRowPtr++;
							}
						}
					}
				}
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}

			return bitmap;
		}
	}
}
