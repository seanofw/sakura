using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Sakura.BetterControls.MessageBox;
using Sakura.BetterControls.Themes;
using Sakura.Commands;
using Sakura.Panels;
using Sakura.Tools;

namespace Sakura
{
	public partial class MainWindow : Form
	{
		private readonly VS2015LightTheme _vs2015LightTheme = new VS2015LightTheme();
		private readonly VS2015DarkTheme _vs2015DarkTheme = new VS2015DarkTheme();
		private readonly VS2015BlueTheme _vs2015BlueTheme = new VS2015BlueTheme();

		public DockContent? ActiveDocument =>
			(DockPanel.ActiveDocument is DockContent dockContent) ? dockContent : null;

		public ThemeKind CurrentTheme
		{
			get => _currentTheme;
			set
			{
				_currentTheme = value;
				ApplyTheme();
			}
		}
		private ThemeKind _currentTheme = ThemeKind.Light;

		public CommandTable CommandTable { get; }

		public ToolTable ToolTable { get; }

		private int _newDocumentNumber = 1;

		public ToolKind CurrentTool
		{
			get => _currentTool;
			set
			{
				if (_currentTool != value)
				{
					ToolInfo? toolInfo = ToolTable.Get(_currentTool);
					toolInfo?.ToolMechanics?.OnDeactivate(EventArgs.Empty);

					_currentTool = value;

					CurrentToolInfo = toolInfo = ToolTable.Get(_currentTool);
					toolInfo?.ToolMechanics?.OnActivate(EventArgs.Empty);

					Toolbox.Invalidate();
				}
			}
		}
		private ToolKind _currentTool;

		public ToolInfo? CurrentToolInfo { get; private set; }

		public HashSet<ToolKind> ActiveTools { get; } = new HashSet<ToolKind>();

		public MainWindow()
		{
			_vs2015LightTheme.Measures.DockPadding = 0;
			_vs2015DarkTheme.Measures.DockPadding = 0;
			_vs2015BlueTheme.Measures.DockPadding = 0;

			InitializeComponent();

			Toolbox.MainWindow = this;

			CommandTable = new CommandTable();
			PopulateCommands(CommandTable);

			ToolTable = new ToolTable(this);

			ApplyTheme();

			CurrentTool = ToolKind.Object_Select;

			OpenObjectManager();
			OpenColorPanel();
			ShowToolbox(DockStyle.Left);
			SetToolboxSize(ToolboxSize.Medium);
		}

		#region Window management

		protected override void OnLayout(LayoutEventArgs levent)
		{
			const int MenuHeight = 24;
			const int StatusHeight = 24;

			int ToolboxWidth = Toolbox.ToolboxPixelSize * 2 + 2;
			int ToolboxHeight = ToolboxWidth;

			int clientWidth = ClientRectangle.Width;
			int clientHeight = ClientRectangle.Height;

			System.Drawing.Rectangle contentRect = new System.Drawing.Rectangle(0, 0, clientWidth, clientHeight);

			MainMenu.Bounds = CalculateDocking(DockStyle.Top, MenuHeight, MenuHeight, ref contentRect);
			MainStatusStrip.Bounds = CalculateDocking(DockStyle.Bottom, StatusHeight, StatusHeight, ref contentRect);

			if (ToolboxDockStyle != DockStyle.None)
				Toolbox.Bounds = CalculateDocking(ToolboxDockStyle, ToolboxWidth, ToolboxHeight, ref contentRect);

			DockPanel.Bounds = contentRect;
		}

		public static System.Drawing.Rectangle CalculateDocking(
			DockStyle dockStyle, int dockWidth, int dockHeight, ref System.Drawing.Rectangle contentRect)
		{
			System.Drawing.Rectangle dockRect;

			(dockRect, contentRect) = dockStyle switch
			{
				DockStyle.Top => (
					new System.Drawing.Rectangle(contentRect.X, contentRect.Y, contentRect.Width, dockHeight),
					new System.Drawing.Rectangle(contentRect.X, contentRect.Y + dockHeight, contentRect.Width, contentRect.Height - dockHeight)
				),
				DockStyle.Bottom => (
					new System.Drawing.Rectangle(contentRect.X, contentRect.Bottom - dockHeight, contentRect.Width, dockHeight),
					new System.Drawing.Rectangle(contentRect.X, contentRect.Y, contentRect.Width, contentRect.Height - dockHeight)
				),
				DockStyle.Right => (
					new System.Drawing.Rectangle(contentRect.Right - dockWidth, contentRect.Y, dockWidth, contentRect.Height),
					new System.Drawing.Rectangle(contentRect.X, contentRect.Y, contentRect.Width - dockWidth, contentRect.Height)
				),
				DockStyle.Left => (
					new System.Drawing.Rectangle(contentRect.X, contentRect.Y, dockWidth, contentRect.Height),
					new System.Drawing.Rectangle(contentRect.X + dockWidth, contentRect.Y, contentRect.Width - dockWidth, contentRect.Height)
				),
				_ => (new System.Drawing.Rectangle(), contentRect),
			};

			return dockRect;
		}

		public void OnContentClosing(object? sender, FormClosingEventArgs e)
		{
		}

		public void OnContentClosed(object? sender, FormClosedEventArgs e)
		{
			UpdateWindowMenu();
		}

		public void CloseAll()
		{
			while (DockPanel.Documents.Any())
			{
				IDockContent target = DockPanel.Documents.Last();
				if (target is DockContent dockContent)
					dockContent.Close();
				if (DockPanel.Documents.Contains(target))
					break;
			}
		}

		public void CloseAllButThis()
		{
			while (DockPanel.DocumentsCount > 1)
			{
				IDockContent target;
				if (ActiveDocument == DockPanel.Documents.Last())
					target = DockPanel.Documents.ElementAt(DockPanel.DocumentsCount - 2);
				else target = DockPanel.Documents.Last();
				if (target is DockContent dockContent)
					dockContent.Close();
				if (DockPanel.Documents.Contains(target))
					break;
			}
		}

		public void UpdateWindowMenu()
		{
			int separatorIndex = WindowMenu.DropDownItems.IndexOf(Window_ListSeparator);
			if (separatorIndex < 0)
				return;

			while (WindowMenu.DropDownItems.Count > separatorIndex + 1)
				WindowMenu.DropDownItems.RemoveAt(WindowMenu.DropDownItems.Count - 1);

			DockContent? activeDocument = ActiveDocument;

			foreach (IDockContent document in DockPanel.Documents)
			{
				if (!(document is DockContent dockContent))
					continue;
				ToolStripMenuItem menuItem = new ToolStripMenuItem(dockContent.Text);
				menuItem.Checked = (document == activeDocument);
				menuItem.Click += (sender, args) => dockContent.Activate();
				WindowMenu.DropDownItems.Add(menuItem);
			}
		}

		private void OnActiveDocumentChanged(object? sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Active document changed.");
			UpdateWindowMenu();
		}

		#endregion

		#region Panels

		public ObjectManager? ObjectManager { get; private set; }
		public ColorPanel? ColorPanel { get; private set; }
		public DockStyle ToolboxDockStyle { get; private set; }

		private void OpenObjectManager()
		{
			if (ObjectManager == null)
				new ObjectManager(this, p => ObjectManager = p, Panels_ObjectManager);
		}

		private void OpenColorPanel()
		{
			if (ColorPanel == null)
				new ColorPanel(this, p => ColorPanel = p, Panels_ColorPanel);
		}

		private void ShowToolbox(DockStyle toolboxDockStyle)
		{
			ToolboxDockStyle = toolboxDockStyle;

			Panels_Toolbox_Left.Checked = (toolboxDockStyle == DockStyle.Left);
			Panels_Toolbox_Right.Checked = (toolboxDockStyle == DockStyle.Right);
			Panels_Toolbox_Top.Checked = (toolboxDockStyle == DockStyle.Top);
			Panels_Toolbox_Bottom.Checked = (toolboxDockStyle == DockStyle.Bottom);
			Panels_Toolbox_Hidden.Checked = (toolboxDockStyle == DockStyle.None);

			Toolbox.ToolboxOrientation = toolboxDockStyle switch
			{
				DockStyle.Left => ToolboxOrientation.Left,
				DockStyle.Right => ToolboxOrientation.Right,
				DockStyle.Top => ToolboxOrientation.Top,
				DockStyle.Bottom => ToolboxOrientation.Bottom,
				_ => ToolboxOrientation.Left,
			};

			if (toolboxDockStyle != DockStyle.None)
				Toolbox.Show();
			else
				Toolbox.Hide();

			PerformLayout();
		}

		private void SetToolboxSize(ToolboxSize size)
		{
			Toolbox.ToolboxSize = size;

			Panels_Toolbox_XSmall.Checked = (size == ToolboxSize.XSmall);
			Panels_Toolbox_Small.Checked = (size == ToolboxSize.Small);
			Panels_Toolbox_Medium.Checked = (size == ToolboxSize.Medium);
			Panels_Toolbox_Large.Checked = (size == ToolboxSize.Large);
			Panels_Toolbox_XLarge.Checked = (size == ToolboxSize.XLarge);

			PerformLayout();
		}

		#endregion

		#region Theming support

		private readonly VisualStudioToolStripExtender _toolStripExtender = new VisualStudioToolStripExtender
		{
			DefaultRenderer = new ToolStripProfessionalRenderer()
		};

		public void ExtendThemeToToolStrip(ToolStrip toolStrip)
		{
			ThemeBase themeBase = GetCurrentThemeBase();
			_toolStripExtender.SetStyle(toolStrip, VisualStudioToolStripExtender.VsVersion.Vs2015, themeBase);
		}

		private ThemeBase GetCurrentThemeBase()
			=>    CurrentTheme == ThemeKind.Blue ? _vs2015BlueTheme
				: CurrentTheme == ThemeKind.Dark ? _vs2015DarkTheme
				: _vs2015LightTheme;

		private void ApplyTheme()
		{
			ThemeBase themeBase = GetCurrentThemeBase();
			DockPanel.Theme = themeBase;

			ExtendThemeToToolStrip(MainMenu);
			ExtendThemeToToolStrip(MainStatusStrip);

			switch (CurrentTheme)
			{
				case ThemeKind.Light:
					BetterControlsTheme.Current = BetterControlsLightTheme.Instance;
					break;
				case ThemeKind.Dark:
					BetterControlsTheme.Current = BetterControlsDarkTheme.Instance;
					break;
				case ThemeKind.Blue:
					BetterControlsTheme.Current = BetterControlsBlueTheme.Instance;
					break;
				default:
					BetterControlsTheme.Current = BetterControlsDefaultTheme.Instance;
					break;
			}
		}

		#endregion

		#region Commands

		private void PopulateCommands(CommandTable commandTable)
		{
			commandTable.FileNew = new Command("FileNew", m =>
			{
				DocumentWindow newContent = new DocumentWindow(this);
				newContent.Text = "New Drawing " + (_newDocumentNumber++);
				newContent.Dock = DockStyle.Fill;
				newContent.DockAreas = DockAreas.Document | DockAreas.Float;
				newContent.FormClosing += OnContentClosing;
				newContent.FormClosed += OnContentClosed;
				newContent.Show(DockPanel, DockState.Document);

				UpdateWindowMenu();
			});

			commandTable.FileClose = new Command("FileClose", m =>
			{
				DockContent? activeDocument = m.ActiveDocument;
				if (activeDocument == null)
					return;

				activeDocument.Close();
				UpdateWindowMenu();
			});

			commandTable.FileExit = new Command("FileExit", m => m.Close());

			commandTable.WindowCloseAll = new Command("WindowCloseAll", m => m.CloseAll());
			commandTable.WindowCloseAllButThis = new Command("WindowCloseAllButThis", m => m.CloseAllButThis());

			commandTable.ToolboxLeft = new Command("ToolboxLeft", m => ShowToolbox(DockStyle.Left));
			commandTable.ToolboxRight = new Command("ToolboxRight", m => ShowToolbox(DockStyle.Right));
			commandTable.ToolboxTop = new Command("ToolboxTop", m => ShowToolbox(DockStyle.Top));
			commandTable.ToolboxBottom = new Command("ToolboxBottom", m => ShowToolbox(DockStyle.Bottom));
			commandTable.ToolboxHidden = new Command("ToolboxHidden", m => ShowToolbox(DockStyle.None));

			commandTable.ToolboxXSmall = new Command("ToolboxXSmall", m => SetToolboxSize(ToolboxSize.XSmall));
			commandTable.ToolboxSmall = new Command("ToolboxSmall", m => SetToolboxSize(ToolboxSize.Small));
			commandTable.ToolboxMedium = new Command("ToolboxMedium", m => SetToolboxSize(ToolboxSize.Medium));
			commandTable.ToolboxLarge = new Command("ToolboxLarge", m => SetToolboxSize(ToolboxSize.Large));
			commandTable.ToolboxXLarge = new Command("ToolboxXLarge", m => SetToolboxSize(ToolboxSize.XLarge));

			commandTable.ObjectManagerToggle = new Command("ObjectManagerToggle", m =>
			{
				if (ObjectManager != null) ObjectManager.Close();
				else OpenObjectManager();
			});
			commandTable.ObjectManagerShow = new Command("ObjectManagerShow", m => OpenObjectManager());
			commandTable.ObjectManagerHide = new Command("ObjectManagerHide", m => ObjectManager?.Close());

			commandTable.ColorPanelToggle = new Command("ColorPanelToggle", m =>
			{
				if (ColorPanel != null) ColorPanel.Close();
				else OpenColorPanel();
			});
			commandTable.ColorPanelShow = new Command("ColorPanelShow", m => OpenColorPanel());
			commandTable.ColorPanelHide = new Command("ColorPanelHide", m => ColorPanel?.Close());

			commandTable.HelpAbout = new Command("HelpAbout", m => m.ShowAboutDialog());
		}

		#endregion

		private void File_New_Click(object sender, EventArgs e) => CommandTable.FileNew.Invoke(this);
		private void File_Close_Click(object sender, EventArgs e) => CommandTable.FileClose.Invoke(this);
		private void File_Exit_Click(object sender, EventArgs e) => CommandTable.FileExit.Invoke(this);

		private void Window_CloseAll_Click(object sender, EventArgs e) => CommandTable.WindowCloseAll.Invoke(this);
		private void Window_CloseAllButThis_Click(object sender, EventArgs e) => CommandTable.WindowCloseAllButThis.Invoke(this);

		private void Panels_Toolbox_Hidden_Click(object sender, EventArgs e) => CommandTable.ToolboxHidden.Invoke(this);
		private void Panels_Toolbox_Left_Click(object sender, EventArgs e) => CommandTable.ToolboxLeft.Invoke(this);
		private void Panels_Toolbox_Right_Click(object sender, EventArgs e) => CommandTable.ToolboxRight.Invoke(this);
		private void Panels_Toolbox_Top_Click(object sender, EventArgs e) => CommandTable.ToolboxTop.Invoke(this);
		private void Panels_Toolbox_Bottom_Click(object sender, EventArgs e) => CommandTable.ToolboxBottom.Invoke(this);

		private void Panels_Toolbox_XSmall_Click(object sender, EventArgs e) => CommandTable.ToolboxXSmall.Invoke(this);
		private void Panels_Toolbox_Small_Click(object sender, EventArgs e) => CommandTable.ToolboxSmall.Invoke(this);
		private void Panels_Toolbox_Medium_Click(object sender, EventArgs e) => CommandTable.ToolboxMedium.Invoke(this);
		private void Panels_Toolbox_Large_Click(object sender, EventArgs e) => CommandTable.ToolboxLarge.Invoke(this);
		private void Panels_Toolbox_XLarge_Click(object sender, EventArgs e) => CommandTable.ToolboxXLarge.Invoke(this);

		private void Panels_ObjectManager_Click(object sender, EventArgs e) => CommandTable.ObjectManagerToggle.Invoke(this);
		private void Panels_ColorPanel_Click(object sender, EventArgs e) => CommandTable.ColorPanelToggle.Invoke(this);

		private void Help_About_Click(object sender, EventArgs e) => CommandTable.HelpAbout.Invoke(this);

		public DockPanel GetDockPanel()
			=> DockPanel;

		public void ActivateTool(ToolKind toolKind)
		{
			ToolInfo? toolInfo;

			// The toolbox is going to need a redraw regardless.
			Toolbox.Invalidate();

			// Cancel any tool that's currently an "until click" tool.
			List<ToolKind> removeTools = new List<ToolKind>();
			foreach (ToolKind activeToolKind in ActiveTools)
			{
				if ((toolInfo = ToolTable.Get(activeToolKind)) != null
					&& toolInfo.Mode == ToolMode.UntilClick)
					removeTools.Add(activeToolKind);
			}
			foreach (ToolKind removeTool in removeTools)
				ActiveTools.Remove(removeTool);

			// If we're not switching tools, don't do anything else.
			if (CurrentTool == toolKind)
				return;

			// If this new tool doesn't exist, give up.
			if ((toolInfo = ToolTable.Get(toolKind)) == null)
				return;

			// Activate it for real.
			switch (toolInfo.Mode)
			{
				case ToolMode.Normal:
					CurrentTool = toolKind;
					Toolbox.SetCurrentToolGroup(CurrentTool);
					break;

				case ToolMode.OneShot:
					break;

				case ToolMode.Toggle:
					if (ActiveTools.Remove(toolKind))
						ActiveTools.Add(toolKind);
					break;

				case ToolMode.UntilClick:
					ActiveTools.Add(toolKind);
					break;
			}
		}

		public void ShowAboutDialog()
		{
			BetterMessageBox.Caption("About Sakura Studio").Message(
@"Sakura Studio
Copyright © 2022 Sean Werkema. All rights reserved.

This program is FREE SOFTWARE under the terms of the
MIT open-source license.  Free as in speech, free as
in beer, free as in do with it pretty much whatever you
want to do with it, and free as in ""don't complain if
it doesn't work because you didn't pay anything for it.""

Have questions? Found a bug? Want a feature? Visit
http://github.com/seanofw/sakura and open an issue."
				)
				.Button("Groovy", DialogResult.OK, bold: true)
				.StandardImage(StandardImageKind.Success)
				.Show();
		}
	}
}