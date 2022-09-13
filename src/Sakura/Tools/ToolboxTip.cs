using System.Drawing;
using System.Windows.Forms;

namespace Sakura.Tools
{
	public class ToolboxTip : Form
	{
		protected override bool ShowWithoutActivation => true;

		public ToolboxTip()
		{
			FormBorderStyle = FormBorderStyle.None;
			ControlBox = false;
			ResizeRedraw = true;
			DoubleBuffered = true;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style = unchecked((int)0x80000000U);  // WS_POPUP
				createParams.ExStyle = 0x8000008;       // WS_EX_TOPMOST | WS_EX_NOACTIVATE
				return createParams;
			}
		}

		public override string Text
		{
			get => base.Text;
			set
			{
				if (base.Text != value)
				{
					base.Text = value;

					Invalidate();
					Update();

					SizeF fracTextSize;
					using (Graphics graphics = CreateGraphics())
						fracTextSize = graphics.MeasureString(Text, Font);
					Size textSize = new Size((int)(fracTextSize.Width + 0.5f), (int)(fracTextSize.Height + 0.5f));
					Size = new Size(textSize.Width + 8, Font.Height + 4);
				}
			}
		}

		public void ShowFor(Control owner, Rectangle target, ToolboxOrientation orientation)
		{
			const int HorzSpacing = 4;
			const int VertSpacing = 3;

			Point topLeft = owner.PointToScreen(new Point(target.Left, target.Top));
			Point bottomRight = owner.PointToScreen(new Point(target.Right - 1, target.Bottom - 1));

			Point point;
			switch (orientation)
			{
				case ToolboxOrientation.Top:
					point = new Point((topLeft.X + bottomRight.X - Width) / 2, bottomRight.Y + VertSpacing);
					break;
				case ToolboxOrientation.Bottom:
					point = new Point((topLeft.X + bottomRight.X - Width) / 2, topLeft.Y - VertSpacing - Height);
					break;
				default:
				case ToolboxOrientation.Left:
					point = new Point(bottomRight.X + HorzSpacing, (topLeft.Y + bottomRight.Y - Height) / 2);
					break;
				case ToolboxOrientation.Right:
					point = new Point(topLeft.X - HorzSpacing - Width, (topLeft.Y + bottomRight.Y - Height) / 2);
					break;
			}

			// This should really clamp to the screen rectangle of the monitor the owner
			// is located on, but clamping to 0,0 is good enough for now.
			point.X = Math.Max(point.X, 0);
			point.Y = Math.Max(point.Y, 0);

			Location = point;
			Show();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 192)),
				0, 0, Width, Height);
			e.Graphics.DrawRectangle(new Pen(Color.FromArgb(192, 192, 96)),
				0, 0, Width - 1, Height - 1);
			e.Graphics.DrawString(Text, Font, new SolidBrush(Color.Black), new Point(4, 2));
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}
	}
}
