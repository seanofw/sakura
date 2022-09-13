using System.Windows.Forms;
using Sakura.MathLib;
using Sakura.BetterControls.Scrollbar;
using Sakura.Rendering;
using WeifenLuo.WinFormsUI.Docking;

namespace Sakura
{
	public class DocumentWindow : DockContent
	{
		public VectorSurface VectorSurface { get; private set; } = null!;

		private BetterHorizontalScrollbar HorizontalScrollbar = null!;
		private BetterVerticalScrollbar VerticalScrollbar = null!;

		protected MainWindow MainWindow { get; }

		public DocumentWindow(MainWindow mainWindow)
		{
			MainWindow = mainWindow;

			InitializeComponent();

			HorizontalScrollbar.Start = 10;
			HorizontalScrollbar.Step = 1;
			HorizontalScrollbar.PageSize = 10;
			HorizontalScrollbar.Length = 20;
			HorizontalScrollbar.Minimum = 0;
			HorizontalScrollbar.Maximum = 99;

			VerticalScrollbar.Start = 10;
			VerticalScrollbar.Step = 1;
			VerticalScrollbar.PageSize = 10;
			VerticalScrollbar.Length = 20;
			VerticalScrollbar.Minimum = 0;
			VerticalScrollbar.Maximum = 99;
		}

		private void InitializeComponent()
		{
			VectorSurface = new VectorSurface(MainWindow);
			HorizontalScrollbar = new BetterHorizontalScrollbar();
			VerticalScrollbar = new BetterVerticalScrollbar();
			HorizontalScrollbar.ScrollbarStyle = (
				ScrollbarStyle.Arrows | ScrollbarStyle.PageArrows | ScrollbarStyle.EndArrows
				| ScrollbarStyle.RangeClickChangesPage
			);
			VerticalScrollbar.ScrollbarStyle = (
				ScrollbarStyle.Arrows | ScrollbarStyle.PageArrows | ScrollbarStyle.EndArrows
				| ScrollbarStyle.RangeClickChangesPage
			);

			SuspendLayout();

			Controls.Add(VectorSurface);
			Controls.Add(HorizontalScrollbar);
			Controls.Add(VerticalScrollbar);

			ResumeLayout(false);
			PerformLayout();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);

			MathLib.Padding padding = new MathLib.Padding(0, 15, 0, 15);
			System.Drawing.Rectangle client = RestoreBounds;

			VectorSurface.SetBounds(
				client.Left + padding.Left, client.Top + padding.Top,
				client.Width - padding.Horz, client.Height - padding.Vert);

			HorizontalScrollbar.SetBounds(
				client.Left + padding.Left, client.Top + padding.Top + client.Height - padding.Vert,
				client.Width - padding.Horz, padding.Bottom);

			VerticalScrollbar.SetBounds(
				client.Left + padding.Left + client.Width - padding.Horz, client.Top + padding.Top,
				padding.Right, client.Height - padding.Vert);
		}
	}
}
