using System.Runtime.InteropServices;

namespace Sakura
{
	public static class Native
	{
		[DllImport("Sakura.Native.dll")]
		public static extern void DemoteImage16To8(IntPtr dest, IntPtr src, int width, int height, int srcSpan, int destSpan);
	}
}
