using Sakura.BetterControls.Themes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sakura.BetterControls.Scrollbar
{
	public class BetterVerticalScrollbar : BetterScrollbar
	{
		protected override int PrimaryAxisStart => Padding.Top;
		protected override int PrimaryAxisLength => Height - Padding.Top - Padding.Bottom;
		protected override int CrossAxisStart => Padding.Left;
		protected override int CrossAxisLength => Width - Padding.Left - Padding.Right;
		protected override int? PixelToPosition(Point point)
			=> point.X > -Width * 2 && point.X < Width * 3 ? (int?)point.Y : null;
		protected override Rectangle MakeRectangle(int primaryStart, int crossStart, int primaryLength, int crossLength)
			=> new Rectangle(crossStart, primaryStart, crossLength, primaryLength);
		protected override ScrollOrientation ScrollOrientation => ScrollOrientation.VerticalScroll;

		protected override void DrawArrowButton(Graphics graphics, ScrollbarButtonKind buttonKind,
			bool hovered, bool pressed, Rectangle square)
		{
			Color color = pressed ? BetterControlsTheme.Current.BetterScrollbar.ArrowPressedColor
				: hovered ? BetterControlsTheme.Current.BetterScrollbar.ArrowHoverColor
				: BetterControlsTheme.Current.BetterScrollbar.ArrowColor;

			Point center = new Point(square.X + square.Width / 2, square.Y + square.Height / 2);
			int size = square.Width * 7 / 16;
			int barThickness = size / 2;

			switch (buttonKind)
			{
				case ScrollbarButtonKind.StartArrow:
					DrawUpArrow(graphics, color, new Point(center.X, center.Y + size / 2 + barThickness / 2), size);
					DrawBar(graphics, color, new Point(center.X, center.Y + size / 2 + barThickness / 2 - size), size, barThickness);
					break;
				case ScrollbarButtonKind.PrevPageArrow:
					DrawUpArrow(graphics, color, new Point(center.X, center.Y + size), size);
					DrawUpArrow(graphics, color, new Point(center.X, center.Y), size);
					break;
				case ScrollbarButtonKind.PrevArrow:
					DrawUpArrow(graphics, color, new Point(center.X, center.Y + size / 2), size);
					break;
				case ScrollbarButtonKind.NextArrow:
					DrawDownArrow(graphics, color, new Point(center.X, center.Y - size / 2), size);
					break;
				case ScrollbarButtonKind.NextPageArrow:
					DrawDownArrow(graphics, color, new Point(center.X, center.Y - size), size);
					DrawDownArrow(graphics, color, new Point(center.X, center.Y), size);
					break;
				case ScrollbarButtonKind.EndArrow:
					DrawDownArrow(graphics, color, new Point(center.X, center.Y - size / 2 - barThickness / 2), size);
					DrawBar(graphics, color, new Point(center.X, center.Y - size / 2 - barThickness / 2 + size + barThickness), size, barThickness);
					break;
			}
		}

		private void DrawUpArrow(Graphics graphics, Color color, Point center, int size)
		{
			SmoothingMode oldSmoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.HighQuality;

			graphics.FillPolygon(new SolidBrush(color),
				new[]
				{
					new Point(center.X - size, center.Y),
					new Point(center.X, center.Y - size),
					new Point(center.X + size, center.Y),
				});

			graphics.SmoothingMode = oldSmoothingMode;
		}

		private void DrawDownArrow(Graphics graphics, Color color, Point center, int size)
		{
			SmoothingMode oldSmoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.HighQuality;

			graphics.FillPolygon(new SolidBrush(color),
				new[]
				{
					new Point(center.X + size, center.Y),
					new Point(center.X, center.Y + size),
					new Point(center.X - size, center.Y),
				});

			graphics.SmoothingMode = oldSmoothingMode;
		}

		private void DrawBar(Graphics graphics, Color color, Point center, int size, int thickness)
		{
			graphics.FillPolygon(new SolidBrush(color),
				new[]
				{
					new Point(center.X - size, center.Y),
					new Point(center.X - size, center.Y - thickness),
					new Point(center.X + size, center.Y - thickness),
					new Point(center.X + size, center.Y),
				});
		}
	}
}
