using System;

namespace Sakura.BetterControls.Scrollbar
{
	[Flags]
	public enum ScrollbarStyle
	{
		PrevArrow = (1 << 0),
		NextArrow = (1 << 1),
		PrevPageArrow = (1 << 2),
		NextPageArrow = (1 << 3),
		StartArrow = (1 << 4),
		EndArrow = (1 << 5),

		Arrows = PrevArrow | NextArrow,
		PageArrows = PrevPageArrow | NextPageArrow,
		EndArrows = StartArrow | EndArrow,

		InitialMarks = (1 << 6),
		FinalMarks = (1 << 7),
		CrossMarks = (1 << 8),

		AllMarks = InitialMarks | FinalMarks | CrossMarks,

		RangeClickMode = (0xF << 10),

		// A click in the range area causes the document to jump up or down a page.
		// This is the default for most scrollbars in most environments.
		RangeClickChangesPage = (0 << 10),

		// A click in the range area causes the document to jump up or down by one step.
		// Most scrollbars don't usually work like this.
		RangeClickAddsOneStep = (1 << 10),

		// A click in the range area causes the document to jump to the farthest
		// endpoint in that direction.  Most scrollbars don't usually work like this.
		RangeClickJumpsToEnd = (2 << 10),

		// A click in the range area jumps the thumb to the click location, and then
		// begins tracking the thumb from there.  Some scrollbars work like this in
		// some environments.
		RangeClickMovesThumb = (3 << 10),
	}
}
