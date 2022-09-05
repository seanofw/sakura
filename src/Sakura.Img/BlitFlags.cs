using System;

namespace Sakura.Img
{
	[Flags]
	public enum BlitFlags
	{
		Copy = 0 << 0,        // Copy verbatim
		Transparent = 1 << 0, // Don't copy pixels when alpha = 0
		Alpha = 2 << 0,       // Normal, non-premultiplied colors
		PMAlpha = 3 << 0,     // Premultiplied alpha
		Multiply = 4 << 0,    // Multiply the source and destination values together
		Add = 5 << 0,         // Add the source and destination values together
		AlphaMode = 0xF << 0,

		FastUnsafe = 1 << 4,  // Don't clip, just trust that the values are right

		FlipHorz = 1 << 5,    // Flip the pixels horizontally (not supported by all modes)
		FlipVert = 1 << 6,    // Flip the pixels vertically (not supported by all modes)
	}
}
