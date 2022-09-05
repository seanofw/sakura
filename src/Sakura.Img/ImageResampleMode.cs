using System;

namespace Sakura.Img
{
	[Flags]
	public enum ImageResampleMode
	{
		Box = 0,        // Nearest-neighbor sampling.
		Triangle,       // Bilinear filtering.
		Hermite,        // Hermite curve.
		Bell,           // The Bell function (3rd-order, or quadratic B-spline).
		BSpline,        // The 4th-order (cubic) B-spline.
		Mitchell,       // The two-parameter cubic function proposed by Mitchell & Netravali (see SIGGRAPH 88).
		Lanczos3,       // 3-lobed sinusoidal Lanczos approximation.
		Lanczos5,       // 5-lobed sinusoidal Lanczos approximation.
		Lanczos7,       // 7-lobed sinusoidal Lanczos approximation.
		Lanczos9,       // 9-lobed sinusoidal Lanczos approximation.
		Lanczos11,      // 11-lobed sinusoidal Lanczos approximation.

		CurveMask = 0xFF,

		// As with all resampling code, boundary conditions occur at the edge
		// of the image.  These flags let you decide how the resample system
		// will resolve those boundary conditions.  You can provide one of
		// these flags for each edge, but you'll probably use the same value
		// for every edge.  Combine these flags with a curve flag above to
		// get a complete mode.
		//    Back  -  If the resampler crosses an edge, it should reverse
		//              direction and continue sampling from pixels it has
		//              already sampled.
		//    Wrap  -  If the resampler crosses an edge, it should continue
		//              sampling from pixels in the same row or column
		//              starting at the opposite edge.
		TopBack		= 0x000000,
		TopWrap		= 0x000100,
		TopMode		= 0x000F00,

		BottomBack	= 0x000000,
		BottomWrap	= 0x001000,
		BottomMode	= 0x00F000,

		LeftBack	= 0x000000,
		LeftWrap	= 0x010000,
		LeftMode	= 0x0F0000,

		RightBack	= 0x000000,
		RightWrap	= 0x100000,
		RightMode	= 0xF00000,

		VertBack = (TopBack | BottomBack),
		VertWrap = (TopWrap | BottomWrap),

		HorzBack = (LeftBack | RightBack),
		HorzWrap = (LeftWrap | RightWrap),

		Back = (HorzBack | VertBack),
		Wrap = (HorzWrap | VertWrap),
	}
}