using Sakura.Panels.ColorControls;
using System.Windows.Forms;

namespace Sakura.Panels
{
	public class ColorPanel : PanelBase<ColorPanel>
	{
		private ColorWheel _colorWheel = null!;

		public ColorPanel(MainWindow mainWindow, Action<ColorPanel?> updateRef, ToolStripMenuItem menuItem)
			: base(mainWindow, updateRef, menuItem, "ColorPanel", "Colors")
		{
		}

		protected override void InitializeComponent()
		{
			_colorWheel = new ColorWheel();
			_colorWheel.Mode = ColorWheelMode.HueSaturation;
			Controls.Add(_colorWheel);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			_colorWheel.SetBounds(0, 0, Width, Height);
		}
	}
}
