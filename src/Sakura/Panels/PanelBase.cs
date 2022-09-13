using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Sakura.Panels
{
	public abstract class PanelBase<T> : DockContent
		where T : PanelBase<T>
	{
		protected MainWindow MainWindow { get; }

		private Action<T?> _updateRef;
		private System.Drawing.Size? _defaultSize;
		private ToolStripMenuItem _menuItem;

		public PanelBase(MainWindow mainWindow, Action<T?> updateRef, ToolStripMenuItem menuItem,
			string name, string title,
			DockState defaultDockState = DockState.DockRight,
			DockAreas dockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom | DockAreas.Float,
			System.Drawing.Size? defaultSize = null,
			System.Drawing.Size? minimumSize = null)
		{
			SuspendLayout();

			MainWindow = mainWindow;
			_updateRef = updateRef;
			updateRef((T?)this);

			Name = name;
			Text = title;

			MinimumSize = minimumSize ?? new System.Drawing.Size(100, 100);
			_defaultSize = defaultSize;

			FormClosing += mainWindow.OnContentClosing;
			FormClosed += mainWindow.OnContentClosed;

			DockAreas = dockAreas;

			Size = defaultSize ?? MinimumSize;

			_menuItem = menuItem;
			menuItem.Checked = true;

			InitializeComponent();

			Show(mainWindow.GetDockPanel(), defaultDockState);

			ResumeLayout(false);
			PerformLayout();
		}

		protected abstract void InitializeComponent();

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			_updateRef(null);
			_menuItem.Checked = false;
			base.OnFormClosed(e);
		}
	}
}
