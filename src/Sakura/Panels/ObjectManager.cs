using System.Windows.Forms;
using Sakura.BetterControls.TreeView;

namespace Sakura.Panels
{
	public class ObjectManager : PanelBase<ObjectManager>
	{
		private ToolStrip Toolbar = null!;
		private BetterTreeView TreeView = null!;

		public ObjectManager(MainWindow mainWindow, Action<ObjectManager?> updateRef, ToolStripMenuItem menuItem)
			: base(mainWindow, updateRef, menuItem, "ObjectManager", "Object Manager")
		{
		}

		protected override void InitializeComponent()
		{
			Toolbar = new ToolStrip();
			TreeView = new BetterTreeView();

			Controls.Add(Toolbar);
			Controls.Add(TreeView);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			System.Drawing.Rectangle client = ClientRectangle;

			int toolbarHeight = 22;

			Toolbar.SetBounds(client.Left, client.Top, client.Width, toolbarHeight);
			TreeView.SetBounds(client.Left, client.Top + toolbarHeight, client.Width, client.Height - toolbarHeight);
		}
	}
}
