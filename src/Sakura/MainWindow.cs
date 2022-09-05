using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Sakura.BetterControls.MessageBox;
using Sakura.BetterControls.Themes;
using Sakura.Commands;
using Sakura.Panels;

namespace Sakura
{
	public partial class MainWindow : Form
	{
		private readonly VS2015LightTheme _vs2015LightTheme = new VS2015LightTheme();
		private readonly VS2015DarkTheme _vs2015DarkTheme = new VS2015DarkTheme();
		private readonly VS2015BlueTheme _vs2015BlueTheme = new VS2015BlueTheme();

		public DockPanel DockPanel { get; private set; }

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

		private int _newDocumentNumber = 1;

		public MainWindow()
		{
			_vs2015LightTheme.Measures.DockPadding = 0;
			_vs2015DarkTheme.Measures.DockPadding = 0;
			_vs2015BlueTheme.Measures.DockPadding = 0;

			InitializeComponent();

			CommandTable = new CommandTable();
			PopulateCommands(CommandTable);

			ApplyTheme();

			DockPanel!.Dock = DockStyle.Fill;
			DockPanel.ActiveDocumentChanged += OnActiveDocumentChanged;

			OpenObjectManager();
		}

		#region Window management

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

		public ObjectManager? ObjectManager { get; set; }

		private void OpenObjectManager()
		{
			if (ObjectManager == null)
				new ObjectManager(this, p => ObjectManager = p, Panels_ObjectManager);
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
				DocumentWindow newContent = new DocumentWindow();
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

			commandTable.ObjectManagerToggle = new Command("ObjectManagerToggle", m =>
			{
				if (ObjectManager != null) ObjectManager.Close();
				else OpenObjectManager();
			});
			commandTable.ObjectManagerShow = new Command("ObjectManagerShow", m => OpenObjectManager());
			commandTable.ObjectManagerHide = new Command("ObjectManagerHide", m => ObjectManager?.Close());

			commandTable.HelpAbout = new Command("HelpAbout", m => m.ShowAboutDialog());
		}

		#endregion

		private void File_New_Click(object sender, EventArgs e) => CommandTable.FileNew.Invoke(this);
		private void File_Close_Click(object sender, EventArgs e) => CommandTable.FileClose.Invoke(this);
		private void File_Exit_Click(object sender, EventArgs e) => CommandTable.FileExit.Invoke(this);

		private void Window_CloseAll_Click(object sender, EventArgs e) => CommandTable.WindowCloseAll.Invoke(this);
		private void Window_CloseAllButThis_Click(object sender, EventArgs e) => CommandTable.WindowCloseAllButThis.Invoke(this);

		private void Panels_ObjectManager_Click(object sender, EventArgs e) => CommandTable.ObjectManagerToggle.Invoke(this);

		private void Help_About_Click(object sender, EventArgs e) => CommandTable.HelpAbout.Invoke(this);

		public void ShowAboutDialog()
		{
			BetterMessageBox.Caption("About Sakura").Message(
@"Sakura Art Tool
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