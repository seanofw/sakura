using Sakura.BetterControls.Themes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sakura.BetterControls.Scrollbar
{
	public class BetterHorizontalScrollbar : BetterScrollbar
	{
		protected override int PrimaryAxisStart => Padding.Left;
		protected override int PrimaryAxisLength => Width - Padding.Left - Padding.Right;
		protected override int CrossAxisStart => Padding.Top;
		protected override int CrossAxisLength => Height - Padding.Top - Padding.Bottom;
		protected override int? PixelToPosition(Point point)
			=> point.Y > -Height * 2 && point.Y < Height * 3 ? (int?)point.X : null;
		protected override Rectangle MakeRectangle(int primaryStart, int crossStart, int primaryLength, int crossLength)
			=> new Rectangle(primaryStart, crossStart, primaryLength, crossLength);
		protected override ScrollOrientation ScrollOrientation => ScrollOrientation.HorizontalScroll;

		protected override void DrawArrowButton(Graphics graphics, ScrollbarButtonKind buttonKind,
			bool hovered, bool pressed, Rectangle square)
		{
			Color color = pressed ? BetterControlsTheme.Current.BetterScrollbar.ArrowPressedColor
				: hovered ? BetterControlsTheme.Current.BetterScrollbar.ArrowHoverColor
				: BetterControlsTheme.Current.BetterScrollbar.ArrowColor;

			Point center = new Point(square.X + square.Width / 2, square.Y + square.Height / 2);
			int size = square.Height * 7 / 16;
			int barThickness = size / 2;

			switch (buttonKind)
			{
				case ScrollbarButtonKind.StartArrow:
					DrawLeftArrow(graphics, color, new Point(center.X + size / 2 + barThickness / 2, center.Y), size);
					DrawBar(graphics, color, new Point(center.X + size / 2 + barThickness / 2 - size, center.Y), size, barThickness);
					break;
				case ScrollbarButtonKind.PrevPageArrow:
					DrawLeftArrow(graphics, color, new Point(center.X + size, center.Y), size);
					DrawLeftArrow(graphics, color, new Point(center.X, center.Y), size);
					break;
				case ScrollbarButtonKind.PrevArrow:
					DrawLeftArrow(graphics, color, new Point(center.X + size / 2, center.Y), size);
					break;
				case ScrollbarButtonKind.NextArrow:
					DrawRightArrow(graphics, color, new Point(center.X - size / 2, center.Y), size);
					break;
				case ScrollbarButtonKind.NextPageArrow:
					DrawRightArrow(graphics, color, new Point(center.X - size, center.Y), size);
					DrawRightArrow(graphics, color, new Point(center.X, center.Y), size);
					break;
				case ScrollbarButtonKind.EndArrow:
					DrawRightArrow(graphics, color, new Point(center.X - size / 2 - barThickness / 2, center.Y), size);
					DrawBar(graphics, color, new Point(center.X - size / 2 - barThickness / 2 + size + barThickness, center.Y), size, barThickness);
					break;
			}
		}

		private void DrawLeftArrow(Graphics graphics, Color color, Point center, int size)
		{
			SmoothingMode oldSmoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.HighQuality;

			graphics.FillPolygon(new SolidBrush(color),
				new[]
				{
					new Point(center.X, center.Y - size),
					new Point(center.X - size, center.Y),
					new Point(center.X, center.Y + size),
				});

			graphics.SmoothingMode = oldSmoothingMode;
		}

		private void DrawRightArrow(Graphics graphics, Color color, Point center, int size)
		{
			SmoothingMode oldSmoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.HighQuality;

			graphics.FillPolygon(new SolidBrush(color),
				new[]
				{
					new Point(center.X, center.Y + size),
					new Point(center.X + size, center.Y),
					new Point(center.X, center.Y - size),
				});

			graphics.SmoothingMode = oldSmoothingMode;
		}

		private void DrawBar(Graphics graphics, Color color, Point center, int size, int thickness)
		{
			graphics.FillPolygon(new SolidBrush(color),
				new[]
				{
					new Point(center.X, center.Y - size),
					new Point(center.X - thickness, center.Y - size),
					new Point(center.X - thickness, center.Y + size),
					new Point(center.X, center.Y + size),
				});
		}
	}
}
