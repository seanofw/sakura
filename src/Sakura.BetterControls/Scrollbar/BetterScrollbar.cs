using System.Drawing;
using System.Windows.Forms;
using Sakura.BetterControls.Themes;

namespace Sakura.BetterControls.Scrollbar
{
	public abstract class BetterScrollbar : Control
	{
		#region Nested types (protected)

		protected enum ScrollbarButtonKind : byte
		{
			None = 0,

			Thumb,

			StartArrow,
			PrevPageArrow,
			PrevArrow,
			NextArrow,
			NextPageArrow,
			EndArrow,

			RangeBeforeThumb,
			RangeAfterThumb,
		}

		protected class ScrollbarCoordinates
		{
			public Range StartArrow, EndArrow;
			public Range PrevArrow, NextArrow;
			public Range PrevPageArrow, NextPageArrow;
			public Range Range, Thumb;

			public Range ThumbCross;
			public Range InitialMarginCross, FinalMarginCross;
		}

		#endregion

		#region Public properties

		[Browsable(true), DefaultValue(0)]
		public int Minimum
		{
			get => _minimum;
			set
			{
				if (_minimum != value)
				{
					_minimum = value;
					EnqueueRefresh();
				}
			}
		}
		private int _minimum;

		[Browsable(true), DefaultValue(0)]
		public int Maximum
		{
			get => _maximum;
			set
			{
				if (_maximum != value)
				{
					_maximum = value;
					EnqueueRefresh();
				}
			}
		}
		private int _maximum;

		[Browsable(true), DefaultValue(0)]
		public int Start
		{
			get => _start;
			set
			{
				if (_start != value)
				{
					_start = value;
					EnqueueRefresh();
				}
			}
		}
		private int _start;

		[Browsable(true), DefaultValue(0)]
		public int Length
		{
			get => _length;
			set
			{
				if (_length != value)
				{
					_length = value > 0 ? value : 0;
					EnqueueRefresh();
				}
			}
		}
		private int _length;

		[Browsable(true), DefaultValue(4)]
		public int MarginThickness
		{
			get => _marginThickness;
			set
			{
				if (_marginThickness != value)
				{
					_marginThickness = value > 0 ? value : 0;
					EnqueueRefresh();
				}
			}
		}
		private int _marginThickness = 4;

		[Browsable(false)]
		public ImmutableDictionary<int, Mark> Marks
		{
			get => _marks;
			set
			{
				_marks = value ?? ImmutableDictionary<int, Mark>.Empty;
				EnqueueRefresh();
			}
		}
		private ImmutableDictionary<int, Mark> _marks = ImmutableDictionary<int, Mark>.Empty;

		[Browsable(true), DefaultValue(ScrollbarStyle.Arrows | ScrollbarStyle.CrossMarks)]
		public ScrollbarStyle ScrollbarStyle
		{
			get => _scrollbarStyle;
			set
			{
				_scrollbarStyle = value;
				EnqueueRefresh();
			}
		}
		private ScrollbarStyle _scrollbarStyle = ScrollbarStyle.Arrows | ScrollbarStyle.CrossMarks;

		[Browsable(true), DefaultValue(1)]
		public int Step
		{
			get => _step;
			set => _step = value > 1 ? value : 1;
		}
		private int _step = 1;

		[Browsable(true), DefaultValue(10)]
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = value > 1 ? value : 1;
		}
		private int _pageSize = 10;

		#endregion

		#region Public events

		public event EventHandler<ScrollEventArgs>? Scroll;

		protected virtual void OnScroll(ScrollEventArgs e)
			=> Scroll?.Invoke(this, e);

		#endregion

		#region Protected/private data

		protected ScrollbarButtonKind Hovered
		{
			get => _hovered;
			private set
			{
				if (_hovered != value)
				{
					_hovered = value;
					EnqueueRefresh();
				}
			}
		}
		private ScrollbarButtonKind _hovered = ScrollbarButtonKind.None;

		protected ScrollbarButtonKind Tracking
		{
			get => _tracking;
			private set
			{
				if (_tracking != value)
				{
					_tracking = value;
					EnqueueRefresh();
				}
			}
		}
		private ScrollbarButtonKind _tracking = ScrollbarButtonKind.None;

		protected int ThumbTrackOffset { get; private set; }
		protected int? ThumbStartOverride { get; private set; }

		protected System.Windows.Forms.Timer? RepeatTimer { get; private set; }

		#endregion

		#region Construction

		protected BetterScrollbar()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.Opaque
				| ControlStyles.ResizeRedraw
				| ControlStyles.UserPaint
				| ControlStyles.UserMouse, true);

			CreateControl();
		}

		#endregion

		#region Abstract parts (for child classes to define)

		protected abstract int? PixelToPosition(Point point);
		protected abstract Rectangle MakeRectangle(int primaryStart, int crossStart, int primaryLength, int crossLength);
		protected abstract void DrawArrowButton(Graphics graphics, ScrollbarButtonKind buttonKind,
			bool hovered, bool pressed, Rectangle square);
		protected abstract int PrimaryAxisStart { get; }
		protected abstract int PrimaryAxisLength { get; }
		protected abstract int CrossAxisStart { get; }
		protected abstract int CrossAxisLength { get; }
		protected abstract ScrollOrientation ScrollOrientation { get; }

		protected virtual void DrawThumb(Graphics graphics, bool hovered, bool pressed, Rectangle thumbRect)
		{
			Color color = pressed ? BetterControlsTheme.Current.BetterScrollbar.ThumbPressedColor
				: hovered ? BetterControlsTheme.Current.BetterScrollbar.ThumbHoverColor
				: BetterControlsTheme.Current.BetterScrollbar.ThumbColor;

			graphics.FillRectangle(new SolidBrush(color), thumbRect);
		}

		#endregion

		#region Internal mechanics

		protected static int RescalePosition(int position,
			int oldMin, int oldMax, int newMin, int newMax)
		{
			long oldRange = (long)oldMax - oldMin;
			long newRange = (long)newMax - newMin;

			long oldOffset = (long)position - oldMin;
			if (oldOffset < 0) oldOffset = 0;
			if (oldOffset > oldRange) oldOffset = oldRange;

			long newOffset = (long)(oldOffset * (double)newRange / oldRange + 0.5);
			if (newOffset < 0) newOffset = 0;
			if (newOffset > newRange) newOffset = newRange;

			return (int)(newOffset + newMin);
		}

		protected static Range RescaleRange(Range range,
			int oldMin, int oldMax, int newMin, int newMax)
		{
			int start = range.Start;
			int length = range.Length;

			if (length < 0) length = 0;
			if (length >= oldMax - oldMin)
			{
				start = oldMin;
				length = oldMax;
			}
			else
			{
				if (start > oldMax - length)
					start = oldMax - length;
				if (start < oldMin) start = oldMin;
			}

			long oldRange = oldMax - oldMin;
			long newRange = newMax - newMin;
			int newLength = (int)(length * (double)newRange / oldRange + 0.5);

			int newStart = RescalePosition(start, oldMin, oldMax - length, newMin, newMax - newLength);

			return new Range(newStart, newLength);
		}

		private ScrollbarCoordinates CalculateLayout()
		{
			int primaryAxisStart = PrimaryAxisStart;
			int primaryAxisLength = PrimaryAxisLength;
			int crossAxisStart = CrossAxisStart;
			int crossAxisLength = CrossAxisLength;

			ScrollbarCoordinates coords = new ScrollbarCoordinates();

			int valueStart, valueLength;
			int valueMinimum, valueMaximum;
			if (DesignMode)
			{
				valueStart = 2;
				valueLength = 4;
				valueMinimum = 0;
				valueMaximum = 10;
			}
			else
			{
				valueStart = Start;
				valueLength = Length;
				valueMinimum = Minimum;
				valueMaximum = Maximum;
			}

			coords.InitialMarginCross = (ScrollbarStyle & ScrollbarStyle.InitialMarks) != 0
				? new Range(crossAxisStart, MarginThickness)
				: new Range(crossAxisStart, 0);
			coords.FinalMarginCross = (ScrollbarStyle & ScrollbarStyle.FinalMarks) != 0
				? new Range(crossAxisStart + crossAxisLength - MarginThickness, MarginThickness)
				: new Range(crossAxisStart + crossAxisLength, 0);
			coords.ThumbCross = new Range(coords.InitialMarginCross.Length,
				crossAxisStart + crossAxisLength - coords.InitialMarginCross.Length - coords.FinalMarginCross.Length);

			int start = primaryAxisStart, end = primaryAxisStart + primaryAxisLength;
			int square = coords.ThumbCross.Length * 4 / 3;

			coords.StartArrow = (ScrollbarStyle & ScrollbarStyle.StartArrow) != 0
				? new Range(start, square)
				: new Range(start, 0);
			start += coords.StartArrow.Length;

			coords.PrevPageArrow = (ScrollbarStyle & ScrollbarStyle.PrevPageArrow) != 0
				? new Range(start, square)
				: new Range(start, 0);
			start += coords.PrevPageArrow.Length;

			coords.PrevArrow = (ScrollbarStyle & ScrollbarStyle.PrevArrow) != 0
				? new Range(start, square)
				: new Range(start, 0);
			start += coords.PrevArrow.Length;

			coords.EndArrow = (ScrollbarStyle & ScrollbarStyle.EndArrow) != 0
				? new Range(end - square, square)
				: new Range(end, 0);
			end -= coords.EndArrow.Length;

			coords.NextPageArrow = (ScrollbarStyle & ScrollbarStyle.NextPageArrow) != 0
				? new Range(end - square, square)
				: new Range(end, 0);
			end -= coords.NextPageArrow.Length;

			coords.NextArrow = (ScrollbarStyle & ScrollbarStyle.NextArrow) != 0
				? new Range(end - square, square)
				: new Range(end, 0);
			end -= coords.NextArrow.Length;

			coords.Range = new Range(start, end > start ? end - start : 0);

			coords.Thumb = RescaleRange(new Range(valueStart, valueLength), valueMinimum, valueMaximum,
				coords.Range.Start, coords.Range.Start + coords.Range.Length);

			return coords;
		}

		private int UpdateHover(Point point, ScrollbarCoordinates layout)
		{
			int? rawPosition = PixelToPosition(point);
			if (!rawPosition.HasValue)
			{
				Hovered = ScrollbarButtonKind.None;
				return -1;
			}

			int position = rawPosition.Value;

			if (layout.Thumb.Contains(position))
				Hovered = ScrollbarButtonKind.Thumb;
			else if (layout.PrevArrow.Contains(position))
				Hovered = ScrollbarButtonKind.PrevArrow;
			else if (layout.NextArrow.Contains(position))
				Hovered = ScrollbarButtonKind.NextArrow;
			else if (layout.PrevPageArrow.Contains(position))
				Hovered = ScrollbarButtonKind.PrevPageArrow;
			else if (layout.NextPageArrow.Contains(position))
				Hovered = ScrollbarButtonKind.NextPageArrow;
			else if (layout.StartArrow.Contains(position))
				Hovered = ScrollbarButtonKind.StartArrow;
			else if (layout.EndArrow.Contains(position))
				Hovered = ScrollbarButtonKind.EndArrow;
			else if (layout.Range.Contains(position))
			{
				if (position > layout.Thumb.Start + layout.Thumb.Length / 2)
					Hovered = ScrollbarButtonKind.RangeAfterThumb;
				else
					Hovered = ScrollbarButtonKind.RangeBeforeThumb;
			}
			else
				Hovered = ScrollbarButtonKind.None;

			return position;
		}

		private void StartRepeatTimer()
		{
			if (RepeatTimer != null) return;

			RepeatTimer = new System.Windows.Forms.Timer();
			RepeatTimer.Interval = 333;
			RepeatTimer.Tick += RepeatMove;
			RepeatTimer.Start();
		}

		private void RepeatMove(object? sender, EventArgs e)
		{
			RepeatTimer!.Interval = 50;

			if (Hovered == Tracking)
			{
				int oldStart = Start;

				switch (Tracking)
				{
					case ScrollbarButtonKind.PrevArrow:
						if (MoveBy(-Step))
							OnScroll(new ScrollEventArgs(ScrollEventType.SmallDecrement, oldStart, Start, ScrollOrientation));
						break;
					case ScrollbarButtonKind.NextArrow:
						if (MoveBy(Step))
							OnScroll(new ScrollEventArgs(ScrollEventType.SmallIncrement, oldStart, Start, ScrollOrientation));
						break;
					case ScrollbarButtonKind.PrevPageArrow:
						if (MoveBy(-PageSize))
							OnScroll(new ScrollEventArgs(ScrollEventType.LargeDecrement, oldStart, Start, ScrollOrientation));
						break;
					case ScrollbarButtonKind.NextPageArrow:
						if (MoveBy(PageSize))
							OnScroll(new ScrollEventArgs(ScrollEventType.LargeIncrement, oldStart, Start, ScrollOrientation));
						break;
					case ScrollbarButtonKind.RangeBeforeThumb:
						switch (ScrollbarStyle & ScrollbarStyle.RangeClickMode)
						{
							case ScrollbarStyle.RangeClickAddsOneStep:
								if (MoveBy(-Step))
									OnScroll(new ScrollEventArgs(ScrollEventType.SmallDecrement, oldStart, Start, ScrollOrientation));
								break;
							case ScrollbarStyle.RangeClickChangesPage:
								if (MoveBy(-PageSize))
									OnScroll(new ScrollEventArgs(ScrollEventType.LargeDecrement, oldStart, Start, ScrollOrientation));
								break;
						}
						break;
					case ScrollbarButtonKind.RangeAfterThumb:
						switch (ScrollbarStyle & ScrollbarStyle.RangeClickMode)
						{
							case ScrollbarStyle.RangeClickAddsOneStep:
								if (MoveBy(Step))
									OnScroll(new ScrollEventArgs(ScrollEventType.SmallIncrement, oldStart, Start, ScrollOrientation));
								break;
							case ScrollbarStyle.RangeClickChangesPage:
								if (MoveBy(PageSize))
									OnScroll(new ScrollEventArgs(ScrollEventType.LargeIncrement, oldStart, Start, ScrollOrientation));
								break;
						}
						break;
				}
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// This is like changing the Start property directly, but it ensures that no matter what
		/// values you pass in, the resulting Start lies within the valid range.
		/// </summary>
		/// <param name="newStart">The desired new value for the Start property.</param>
		/// <returns>True if the Start property changed, false if it has the same value.</returns>
		public bool MoveTo(int newStart)
		{
			if (newStart > Maximum - Length) newStart = Maximum - Length;
			if (newStart < Minimum) newStart = Minimum;

			if (Start != newStart)
			{
				Start = newStart;
				return true;
			}
			else return false;
		}

		/// <summary>
		/// This is like adding the given delta to the Start property directly, but it ensures that
		/// no matter what values you pass in or what Start's current value is, the resulting Start
		/// still lies within the valid range.
		/// </summary>
		/// <param name="delta">How far to move the Start.</param>
		/// <returns>True if the Start property changed, false if it has the same value.</returns>
		public bool MoveBy(int delta)
			=> MoveTo(Start + delta);

		#endregion

		#region Overrides and event handlers

		private bool _isRefreshEnqueued = false;

		private void EnqueueRefresh()
		{
			Invalidate();

			if (_isRefreshEnqueued) return;
			_isRefreshEnqueued = true;

			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			timer.Interval = 25;
			timer.Tick += (sender, e) =>
			{
				timer.Stop();
				timer.Dispose();
				_isRefreshEnqueued = false;
				Refresh();
			};
			timer.Start();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			EnqueueRefresh();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			ScrollbarCoordinates layout = CalculateLayout();

			e.Graphics.FillRectangle(new SolidBrush(BetterControlsTheme.Current.BetterScrollbar.BackColor),
				new Rectangle(0, 0, Width, Height));

			if ((ScrollbarStyle & ScrollbarStyle.StartArrow) != 0)
				DrawArrowButton(e.Graphics, ScrollbarButtonKind.StartArrow,
					Hovered == ScrollbarButtonKind.StartArrow, Hovered == ScrollbarButtonKind.StartArrow && Hovered == Tracking,
					MakeRectangle(layout.StartArrow.Start, layout.ThumbCross.Start,
						layout.StartArrow.Length, layout.ThumbCross.Length));

			if ((ScrollbarStyle & ScrollbarStyle.PrevPageArrow) != 0)
				DrawArrowButton(e.Graphics, ScrollbarButtonKind.PrevPageArrow,
					Hovered == ScrollbarButtonKind.PrevPageArrow, Hovered == ScrollbarButtonKind.PrevPageArrow && Hovered == Tracking,
					MakeRectangle(layout.PrevPageArrow.Start, layout.ThumbCross.Start,
						layout.PrevPageArrow.Length, layout.ThumbCross.Length));

			if ((ScrollbarStyle & ScrollbarStyle.PrevArrow) != 0)
				DrawArrowButton(e.Graphics, ScrollbarButtonKind.PrevArrow,
					Hovered == ScrollbarButtonKind.PrevArrow, Hovered == ScrollbarButtonKind.PrevArrow && Hovered == Tracking,
					MakeRectangle(layout.PrevArrow.Start, layout.ThumbCross.Start,
						layout.PrevArrow.Length, layout.ThumbCross.Length));

			if ((ScrollbarStyle & ScrollbarStyle.NextArrow) != 0)
				DrawArrowButton(e.Graphics, ScrollbarButtonKind.NextArrow,
					Hovered == ScrollbarButtonKind.NextArrow, Hovered == ScrollbarButtonKind.NextArrow && Hovered == Tracking,
					MakeRectangle(layout.NextArrow.Start, layout.ThumbCross.Start,
						layout.NextArrow.Length, layout.ThumbCross.Length));

			if ((ScrollbarStyle & ScrollbarStyle.NextPageArrow) != 0)
				DrawArrowButton(e.Graphics, ScrollbarButtonKind.NextPageArrow,
					Hovered == ScrollbarButtonKind.NextPageArrow, Hovered == ScrollbarButtonKind.NextPageArrow && Hovered == Tracking,
					MakeRectangle(layout.NextPageArrow.Start, layout.ThumbCross.Start,
						layout.NextPageArrow.Length, layout.ThumbCross.Length));

			if ((ScrollbarStyle & ScrollbarStyle.EndArrow) != 0)
				DrawArrowButton(e.Graphics, ScrollbarButtonKind.EndArrow,
					Hovered == ScrollbarButtonKind.EndArrow, Hovered == ScrollbarButtonKind.EndArrow && Hovered == Tracking,
					MakeRectangle(layout.EndArrow.Start, layout.ThumbCross.Start,
						layout.EndArrow.Length, layout.ThumbCross.Length));

			DrawThumb(e.Graphics,
				Hovered == ScrollbarButtonKind.Thumb, Hovered == ScrollbarButtonKind.Thumb && Hovered == Tracking,
				MakeRectangle(ThumbStartOverride ?? layout.Thumb.Start, layout.ThumbCross.Start,
					layout.Thumb.Length, layout.ThumbCross.Length));

			if ((ScrollbarStyle & ScrollbarStyle.AllMarks) != 0)
			{
				ScrollbarStyle[] _markStyleFlagMap =
				{
					0,
					ScrollbarStyle.InitialMarks,
					ScrollbarStyle.InitialMarks,
					ScrollbarStyle.FinalMarks,
					ScrollbarStyle.FinalMarks,
					ScrollbarStyle.CrossMarks,
				};

				foreach (KeyValuePair<int, Mark> pair in Marks)
				{
					if ((_markStyleFlagMap[(int)pair.Value.MarkStyle] & ScrollbarStyle) != 0)
					{
						DrawMark(e.Graphics, layout, pair.Key, pair.Value);
					}
				}
			}
		}

		private void DrawMark(Graphics graphics, ScrollbarCoordinates layout, int position, Mark mark)
		{
			int markPosition = RescalePosition(position, Minimum, Maximum, layout.Range.Start,
				layout.Range.Start + layout.Range.Length);

			Rectangle rectangle;

			switch (mark.MarkStyle)
			{
				case MarkStyle.Cross:
					rectangle = MakeRectangle(markPosition - mark.Thickness / 2, 0, mark.Thickness, CrossAxisLength);
					graphics.FillRectangle(new SolidBrush(mark.Color), rectangle);
					break;
				case MarkStyle.InitialMargin:
					rectangle = MakeRectangle(markPosition - mark.Thickness / 2, 0, mark.Thickness, MarginThickness);
					graphics.FillRectangle(new SolidBrush(mark.Color), rectangle);
					break;
				case MarkStyle.InitialOverlapping:
					rectangle = MakeRectangle(markPosition - mark.Thickness / 2, 0, mark.Thickness, CrossAxisLength / 3);
					graphics.FillRectangle(new SolidBrush(mark.Color), rectangle);
					break;
				case MarkStyle.FinalOverlapping:
					rectangle = MakeRectangle(markPosition - mark.Thickness / 2, CrossAxisLength - (CrossAxisLength / 3), mark.Thickness, CrossAxisLength / 3);
					graphics.FillRectangle(new SolidBrush(mark.Color), rectangle);
					break;
				case MarkStyle.FinalMargin:
					rectangle = MakeRectangle(markPosition - mark.Thickness / 2, CrossAxisLength - MarginThickness, mark.Thickness, MarginThickness);
					graphics.FillRectangle(new SolidBrush(mark.Color), rectangle);
					break;
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Hovered = ScrollbarButtonKind.None;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			ScrollbarCoordinates layout = CalculateLayout();

			int position = UpdateHover(e.Location, layout);

			if (Tracking == ScrollbarButtonKind.Thumb)
			{
				int newValue, oldStart;
				UpdateThumbForTracking(layout, position, out oldStart, out newValue);

				if (oldStart != newValue)
					OnScroll(new ScrollEventArgs(ScrollEventType.ThumbTrack, oldStart, newValue, ScrollOrientation));
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			ScrollbarCoordinates layout = CalculateLayout();
			int position = UpdateHover(e.Location, layout);
			Tracking = Hovered;
			ThumbTrackOffset = position - layout.Thumb.Start;

			int oldStart = Start;

			switch (Tracking)
			{
				case ScrollbarButtonKind.PrevArrow:
					if (MoveBy(-Step))
						OnScroll(new ScrollEventArgs(ScrollEventType.SmallDecrement, oldStart, Start, ScrollOrientation));
					StartRepeatTimer();
					break;
				case ScrollbarButtonKind.NextArrow:
					if (MoveBy(Step))
						OnScroll(new ScrollEventArgs(ScrollEventType.SmallIncrement, oldStart, Start, ScrollOrientation));
					StartRepeatTimer();
					break;
				case ScrollbarButtonKind.PrevPageArrow:
					if (MoveBy(-PageSize))
						OnScroll(new ScrollEventArgs(ScrollEventType.LargeIncrement, oldStart, Start, ScrollOrientation));
					StartRepeatTimer();
					break;
				case ScrollbarButtonKind.NextPageArrow:
					if (MoveBy(PageSize))
						OnScroll(new ScrollEventArgs(ScrollEventType.LargeDecrement, oldStart, Start, ScrollOrientation));
					StartRepeatTimer();
					break;
				case ScrollbarButtonKind.StartArrow:
					if (MoveTo(Minimum))
						OnScroll(new ScrollEventArgs(ScrollEventType.First, oldStart, Start, ScrollOrientation));
					break;
				case ScrollbarButtonKind.EndArrow:
					if (MoveTo(Maximum))
						OnScroll(new ScrollEventArgs(ScrollEventType.Last, oldStart, Start, ScrollOrientation));
					break;

				case ScrollbarButtonKind.RangeBeforeThumb:
					switch (ScrollbarStyle & ScrollbarStyle.RangeClickMode)
					{
						case ScrollbarStyle.RangeClickAddsOneStep:
							if (MoveBy(-Step))
								OnScroll(new ScrollEventArgs(ScrollEventType.SmallDecrement, oldStart, Start, ScrollOrientation));
							StartRepeatTimer();
							break;
						case ScrollbarStyle.RangeClickChangesPage:
							if (MoveBy(-PageSize))
								OnScroll(new ScrollEventArgs(ScrollEventType.LargeDecrement, oldStart, Start, ScrollOrientation));
							StartRepeatTimer();
							break;
						case ScrollbarStyle.RangeClickJumpsToEnd:
							if (MoveTo(Minimum))
								OnScroll(new ScrollEventArgs(ScrollEventType.First, oldStart, Start, ScrollOrientation));
							break;
						case ScrollbarStyle.RangeClickMovesThumb:
							goto jumpThumb;
					}
					break;

				case ScrollbarButtonKind.RangeAfterThumb:
					switch (ScrollbarStyle & ScrollbarStyle.RangeClickMode)
					{
						case ScrollbarStyle.RangeClickAddsOneStep:
							if (MoveBy(Step))
								OnScroll(new ScrollEventArgs(ScrollEventType.SmallIncrement, oldStart, Start, ScrollOrientation));
							StartRepeatTimer();
							break;
						case ScrollbarStyle.RangeClickChangesPage:
							if (MoveBy(PageSize))
								OnScroll(new ScrollEventArgs(ScrollEventType.LargeIncrement, oldStart, Start, ScrollOrientation));
							StartRepeatTimer();
							break;
						case ScrollbarStyle.RangeClickJumpsToEnd:
							if (MoveTo(Maximum))
								OnScroll(new ScrollEventArgs(ScrollEventType.Last, oldStart, Start, ScrollOrientation));
							break;
						case ScrollbarStyle.RangeClickMovesThumb:
							goto jumpThumb;
					}
					break;

				jumpThumb:
					{
						Tracking = ScrollbarButtonKind.Thumb;
						ThumbTrackOffset = layout.Thumb.Length / 2;

						int newValue;
						UpdateThumbForTracking(layout, position, out oldStart, out newValue);

						if (oldStart != newValue)
							OnScroll(new ScrollEventArgs(ScrollEventType.ThumbTrack, oldStart, newValue, ScrollOrientation));
					}
					break;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			ScrollbarCoordinates layout = CalculateLayout();
			int position = UpdateHover(e.Location, layout);

			ThumbStartOverride = null;

			if (RepeatTimer != null)
			{
				RepeatTimer.Stop();
				RepeatTimer.Dispose();
				RepeatTimer = null;
			}

			if (Tracking == ScrollbarButtonKind.Thumb)
			{
				Tracking = ScrollbarButtonKind.None;

				int newValue, oldStart;
				UpdateThumbForTracking(layout, position, out oldStart, out newValue);

				OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, oldStart, newValue, ScrollOrientation));
			}

			Tracking = ScrollbarButtonKind.None;
			ThumbStartOverride = null;
		}

		private void UpdateThumbForTracking(ScrollbarCoordinates layout, int position, out int oldStart, out int newValue)
		{
			int newStart = position - ThumbTrackOffset;
			if (newStart > layout.Range.Start + (layout.Range.Length - layout.Thumb.Length))
				newStart = layout.Range.Start + (layout.Range.Length - layout.Thumb.Length);
			if (newStart < layout.Range.Start) newStart = layout.Range.Start;

			newValue = RescalePosition(newStart,
				layout.Range.Start, layout.Range.Start + layout.Range.Length,
				Minimum, Maximum);
			ThumbStartOverride = newStart;
			oldStart = Start;
			Start = newValue;
		}

		#endregion
	}
}
