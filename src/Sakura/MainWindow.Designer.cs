namespace Sakura
{
	partial class MainWindow
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.File_New = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
			this.File_SaveAll = new System.Windows.Forms.ToolStripMenuItem();
			this.File_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Close = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.File_Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.EditMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Undo = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Redo = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.Edit_Cut = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Copy = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Paste = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Delete = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Sep2 = new System.Windows.Forms.ToolStripSeparator();
			this.Edit_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Sep3 = new System.Windows.Forms.ToolStripSeparator();
			this.Edit_Find = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_FindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_FindPrevious = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_FindAndReplace = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Tools_Options = new System.Windows.Forms.ToolStripMenuItem();
			this.PanelsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_ToolboxMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_Left = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_Right = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_Top = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_Bottom = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.Panels_Toolbox_Hidden = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.Panels_ObjectManager = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_ColorPanel = new System.Windows.Forms.ToolStripMenuItem();
			this.WindowMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Window_CloseAllDocuments = new System.Windows.Forms.ToolStripMenuItem();
			this.Window_CloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
			this.Window_ListSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Help_AboutSakura = new System.Windows.Forms.ToolStripMenuItem();
			this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
			this.MainStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.ZoomStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.DockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.Toolbox = new Sakura.Tools.Toolbox();
			this.Panels_Toolbox_Sep2 = new System.Windows.Forms.ToolStripSeparator();
			this.Panels_Toolbox_XSmall = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_Small = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_Medium = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_Large = new System.Windows.Forms.ToolStripMenuItem();
			this.Panels_Toolbox_XLarge = new System.Windows.Forms.ToolStripMenuItem();
			this.MainMenu.SuspendLayout();
			this.MainStatusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainMenu
			// 
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.EditMenu,
            this.ToolsMenu,
            this.PanelsMenu,
            this.WindowMenu,
            this.HelpMenu});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.MdiWindowListItem = this.WindowMenu;
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(984, 24);
			this.MainMenu.TabIndex = 0;
			this.MainMenu.Text = "MainMenu";
			// 
			// FileMenu
			// 
			this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_New,
            this.File_Open,
            this.File_Save,
			this.File_SaveAll,
			this.File_SaveAs,
            this.File_Close,
            this.File_Sep1,
            this.File_Exit});
			this.FileMenu.Name = "FileMenu";
			this.FileMenu.Size = new System.Drawing.Size(37, 20);
			this.FileMenu.Text = "&File";
			// 
			// File_New
			// 
			this.File_New.Name = "File_New";
			this.File_New.ShortcutKeyDisplayString = "";
			this.File_New.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.File_New.Size = new System.Drawing.Size(195, 22);
			this.File_New.Text = "&New";
			this.File_New.Click += new System.EventHandler(this.File_New_Click);
			// 
			// File_Open
			// 
			this.File_Open.Name = "File_Open";
			this.File_Open.ShortcutKeyDisplayString = "";
			this.File_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.File_Open.Size = new System.Drawing.Size(195, 22);
			this.File_Open.Text = "&Open...";
			// 
			// File_Save
			// 
			this.File_Save.Name = "File_Save";
			this.File_Save.ShortcutKeyDisplayString = "";
			this.File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.File_Save.Size = new System.Drawing.Size(195, 22);
			this.File_Save.Text = "&Save";
			// 
			// File_SaveAll
			// 
			this.File_SaveAll.Name = "File_SaveAll";
			this.File_SaveAll.ShortcutKeyDisplayString = "";
			this.File_SaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.File_SaveAll.Size = new System.Drawing.Size(195, 22);
			this.File_SaveAll.Text = "S&ave all";
			// 
			// File_SaveAs
			// 
			this.File_SaveAs.Name = "File_SaveAs";
			this.File_SaveAs.ShortcutKeyDisplayString = "";
			this.File_SaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
			this.File_SaveAs.Size = new System.Drawing.Size(195, 22);
			this.File_SaveAs.Text = "Save as...";
			// 
			// File_Close
			// 
			this.File_Close.Name = "File_Close";
			this.File_Close.ShortcutKeyDisplayString = "Ctrl+F4";
			this.File_Close.Size = new System.Drawing.Size(195, 22);
			this.File_Close.Text = "&Close";
			this.File_Close.Click += new System.EventHandler(this.File_Close_Click);
			// 
			// File_Sep1
			// 
			this.File_Sep1.Name = "File_Sep1";
			this.File_Sep1.Size = new System.Drawing.Size(192, 6);
			// 
			// File_Exit
			// 
			this.File_Exit.Name = "File_Exit";
			this.File_Exit.ShortcutKeyDisplayString = "Alt+F4";
			this.File_Exit.Size = new System.Drawing.Size(195, 22);
			this.File_Exit.Text = "E&xit";
			this.File_Exit.Click += new System.EventHandler(this.File_Exit_Click);
			// 
			// EditMenu
			// 
			this.EditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Edit_Undo,
            this.Edit_Redo,
            this.Edit_Sep1,
            this.Edit_Cut,
            this.Edit_Copy,
            this.Edit_Paste,
            this.Edit_Delete,
            this.Edit_Sep2,
            this.Edit_SelectAll,
            this.Edit_Sep3,
            this.Edit_Find,
            this.Edit_FindNext,
            this.Edit_FindPrevious,
            this.Edit_FindAndReplace});
			this.EditMenu.Name = "EditMenu";
			this.EditMenu.Size = new System.Drawing.Size(39, 20);
			this.EditMenu.Text = "&Edit";
			// 
			// Edit_Undo
			// 
			this.Edit_Undo.Name = "Edit_Undo";
			this.Edit_Undo.ShortcutKeyDisplayString = "";
			this.Edit_Undo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.Edit_Undo.Size = new System.Drawing.Size(196, 22);
			this.Edit_Undo.Text = "&Undo";
			// 
			// Edit_Redo
			// 
			this.Edit_Redo.Name = "Edit_Redo";
			this.Edit_Redo.ShortcutKeyDisplayString = "";
			this.Edit_Redo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.Edit_Redo.Size = new System.Drawing.Size(196, 22);
			this.Edit_Redo.Text = "R&edo";
			// 
			// Edit_Sep1
			// 
			this.Edit_Sep1.Name = "Edit_Sep1";
			this.Edit_Sep1.Size = new System.Drawing.Size(193, 6);
			// 
			// Edit_Cut
			// 
			this.Edit_Cut.Name = "Edit_Cut";
			this.Edit_Cut.ShortcutKeyDisplayString = "";
			this.Edit_Cut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.Edit_Cut.Size = new System.Drawing.Size(196, 22);
			this.Edit_Cut.Text = "C&ut";
			// 
			// Edit_Copy
			// 
			this.Edit_Copy.Name = "Edit_Copy";
			this.Edit_Copy.ShortcutKeyDisplayString = "";
			this.Edit_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.Edit_Copy.Size = new System.Drawing.Size(196, 22);
			this.Edit_Copy.Text = "&Copy";
			// 
			// Edit_Paste
			// 
			this.Edit_Paste.Name = "Edit_Paste";
			this.Edit_Paste.ShortcutKeyDisplayString = "";
			this.Edit_Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.Edit_Paste.Size = new System.Drawing.Size(196, 22);
			this.Edit_Paste.Text = "&Paste";
			// 
			// Edit_Delete
			// 
			this.Edit_Delete.Name = "Edit_Delete";
			this.Edit_Delete.ShortcutKeyDisplayString = "Del";
			this.Edit_Delete.Size = new System.Drawing.Size(196, 22);
			this.Edit_Delete.Text = "&Delete";
			// 
			// Edit_Sep2
			// 
			this.Edit_Sep2.Name = "Edit_Sep2";
			this.Edit_Sep2.Size = new System.Drawing.Size(193, 6);
			// 
			// Edit_SelectAll
			// 
			this.Edit_SelectAll.Name = "Edit_SelectAll";
			this.Edit_SelectAll.ShortcutKeyDisplayString = "";
			this.Edit_SelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.Edit_SelectAll.Size = new System.Drawing.Size(196, 22);
			this.Edit_SelectAll.Text = "Select &all";
			// 
			// Edit_Sep3
			// 
			this.Edit_Sep3.Name = "Edit_Sep3";
			this.Edit_Sep3.Size = new System.Drawing.Size(193, 6);
			// 
			// Edit_Find
			// 
			this.Edit_Find.Name = "Edit_Find";
			this.Edit_Find.ShortcutKeyDisplayString = "";
			this.Edit_Find.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.Edit_Find.Size = new System.Drawing.Size(196, 22);
			this.Edit_Find.Text = "&Find...";
			// 
			// Edit_FindNext
			// 
			this.Edit_FindNext.Name = "Edit_FindNext";
			this.Edit_FindNext.ShortcutKeyDisplayString = "";
			this.Edit_FindNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.Edit_FindNext.Size = new System.Drawing.Size(196, 22);
			this.Edit_FindNext.Text = "Find &Next";
			// 
			// Edit_FindPrevious
			// 
			this.Edit_FindPrevious.Name = "Edit_FindPrevious";
			this.Edit_FindPrevious.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
			this.Edit_FindPrevious.Size = new System.Drawing.Size(196, 22);
			this.Edit_FindPrevious.Text = "Find &Previous";
			// 
			// Edit_FindAndReplace
			// 
			this.Edit_FindAndReplace.Name = "Edit_FindAndReplace";
			this.Edit_FindAndReplace.ShortcutKeyDisplayString = "";
			this.Edit_FindAndReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
			this.Edit_FindAndReplace.Size = new System.Drawing.Size(196, 22);
			this.Edit_FindAndReplace.Text = "&Replace...";
			// 
			// ToolsMenu
			// 
			this.ToolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tools_Options});
			this.ToolsMenu.Name = "ToolsMenu";
			this.ToolsMenu.Size = new System.Drawing.Size(46, 20);
			this.ToolsMenu.Text = "&Tools";
			// 
			// Tools_Options
			// 
			this.Tools_Options.Name = "Tools_Options";
			this.Tools_Options.Size = new System.Drawing.Size(125, 22);
			this.Tools_Options.Text = "&Options...";
			// 
			// PanelsMenu
			// 
			this.PanelsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Panels_ToolboxMenu,
            this.Panels_Sep1,
            this.Panels_ObjectManager,
            this.Panels_ColorPanel});
			this.PanelsMenu.Name = "PanelsMenu";
			this.PanelsMenu.Size = new System.Drawing.Size(53, 20);
			this.PanelsMenu.Text = "&Panels";
			// 
			// Panels_ToolboxMenu
			// 
			this.Panels_ToolboxMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Panels_Toolbox_Left,
            this.Panels_Toolbox_Right,
            this.Panels_Toolbox_Top,
            this.Panels_Toolbox_Bottom,
            this.Panels_Toolbox_Sep1,
            this.Panels_Toolbox_Hidden,
            this.Panels_Toolbox_Sep2,
            this.Panels_Toolbox_XSmall,
            this.Panels_Toolbox_Small,
            this.Panels_Toolbox_Medium,
            this.Panels_Toolbox_Large,
            this.Panels_Toolbox_XLarge});
			this.Panels_ToolboxMenu.Name = "Panels_ToolboxMenu";
			this.Panels_ToolboxMenu.Size = new System.Drawing.Size(180, 22);
			this.Panels_ToolboxMenu.Text = "&Toolbox";
			// 
			// Panels_Toolbox_Left
			// 
			this.Panels_Toolbox_Left.Name = "Panels_Toolbox_Left";
			this.Panels_Toolbox_Left.Size = new System.Drawing.Size(132, 22);
			this.Panels_Toolbox_Left.Text = "&Left";
			this.Panels_Toolbox_Left.Click += new System.EventHandler(this.Panels_Toolbox_Left_Click);
			// 
			// Panels_Toolbox_Right
			// 
			this.Panels_Toolbox_Right.Name = "Panels_Toolbox_Right";
			this.Panels_Toolbox_Right.Size = new System.Drawing.Size(132, 22);
			this.Panels_Toolbox_Right.Text = "&Right";
			this.Panels_Toolbox_Right.Click += new System.EventHandler(this.Panels_Toolbox_Right_Click);
			// 
			// Panels_Toolbox_Top
			// 
			this.Panels_Toolbox_Top.Name = "Panels_Toolbox_Top";
			this.Panels_Toolbox_Top.Size = new System.Drawing.Size(132, 22);
			this.Panels_Toolbox_Top.Text = "&Top";
			this.Panels_Toolbox_Top.Click += new System.EventHandler(this.Panels_Toolbox_Top_Click);
			// 
			// Panels_Toolbox_Bottom
			// 
			this.Panels_Toolbox_Bottom.Name = "Panels_Toolbox_Bottom";
			this.Panels_Toolbox_Bottom.Size = new System.Drawing.Size(132, 22);
			this.Panels_Toolbox_Bottom.Text = "&Bottom";
			this.Panels_Toolbox_Bottom.Click += new System.EventHandler(this.Panels_Toolbox_Bottom_Click);
			// 
			// Panels_Toolbox_Sep1
			// 
			this.Panels_Toolbox_Sep1.Name = "Panels_Toolbox_Sep1";
			this.Panels_Toolbox_Sep1.Size = new System.Drawing.Size(129, 6);
			// 
			// Panels_Toolbox_Hidden
			// 
			this.Panels_Toolbox_Hidden.Name = "Panels_Toolbox_Hidden";
			this.Panels_Toolbox_Hidden.Size = new System.Drawing.Size(132, 22);
			this.Panels_Toolbox_Hidden.Text = "&Hidden";
			this.Panels_Toolbox_Hidden.Click += new System.EventHandler(this.Panels_Toolbox_Hidden_Click);
			// 
			// Panels_Sep1
			// 
			this.Panels_Sep1.Name = "Panels_Sep1";
			this.Panels_Sep1.Size = new System.Drawing.Size(177, 6);
			// 
			// Panels_ObjectManager
			// 
			this.Panels_ObjectManager.Name = "Panels_ObjectManager";
			this.Panels_ObjectManager.Size = new System.Drawing.Size(180, 22);
			this.Panels_ObjectManager.Text = "&Object Manager";
			this.Panels_ObjectManager.Click += new System.EventHandler(this.Panels_ObjectManager_Click);
			// 
			// Panels_ColorPanel
			// 
			this.Panels_ColorPanel.Name = "Panels_ColorPanel";
			this.Panels_ColorPanel.Size = new System.Drawing.Size(180, 22);
			this.Panels_ColorPanel.Text = "&Colors";
			this.Panels_ColorPanel.Click += new System.EventHandler(this.Panels_ColorPanel_Click);
			// 
			// WindowMenu
			// 
			this.WindowMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Window_CloseAllDocuments,
            this.Window_CloseAllButThis,
            this.Window_ListSeparator});
			this.WindowMenu.Name = "WindowMenu";
			this.WindowMenu.Size = new System.Drawing.Size(63, 20);
			this.WindowMenu.Text = "&Window";
			// 
			// Window_CloseAllDocuments
			// 
			this.Window_CloseAllDocuments.Name = "Window_CloseAllDocuments";
			this.Window_CloseAllDocuments.Size = new System.Drawing.Size(181, 22);
			this.Window_CloseAllDocuments.Text = "Close &all documents";
			this.Window_CloseAllDocuments.Click += new System.EventHandler(this.Window_CloseAll_Click);
			// 
			// Window_CloseAllButThis
			// 
			this.Window_CloseAllButThis.Name = "Window_CloseAllButThis";
			this.Window_CloseAllButThis.Size = new System.Drawing.Size(181, 22);
			this.Window_CloseAllButThis.Text = "Close all but &this";
			this.Window_CloseAllButThis.Click += new System.EventHandler(this.Window_CloseAllButThis_Click);
			// 
			// Window_ListSeparator
			// 
			this.Window_ListSeparator.Name = "Window_ListSeparator";
			this.Window_ListSeparator.Size = new System.Drawing.Size(178, 6);
			// 
			// HelpMenu
			// 
			this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Help_AboutSakura});
			this.HelpMenu.Name = "HelpMenu";
			this.HelpMenu.Size = new System.Drawing.Size(44, 20);
			this.HelpMenu.Text = "&Help";
			// 
			// Help_AboutSakura
			// 
			this.Help_AboutSakura.Name = "Help_AboutSakura";
			this.Help_AboutSakura.Size = new System.Drawing.Size(154, 22);
			this.Help_AboutSakura.Text = "&About Sakura...";
			this.Help_AboutSakura.Click += new System.EventHandler(this.Help_About_Click);
			// 
			// MainStatusStrip
			// 
			this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainStatusLabel,
            this.ZoomStatusLabel});
			this.MainStatusStrip.Location = new System.Drawing.Point(0, 637);
			this.MainStatusStrip.Name = "MainStatusStrip";
			this.MainStatusStrip.Size = new System.Drawing.Size(984, 24);
			this.MainStatusStrip.TabIndex = 1;
			// 
			// MainStatusLabel
			// 
			this.MainStatusLabel.Name = "MainStatusLabel";
			this.MainStatusLabel.Size = new System.Drawing.Size(876, 19);
			this.MainStatusLabel.Spring = true;
			this.MainStatusLabel.Text = "Ready";
			this.MainStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ZoomStatusLabel
			// 
			this.ZoomStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.ZoomStatusLabel.Name = "ZoomStatusLabel";
			this.ZoomStatusLabel.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
			this.ZoomStatusLabel.Size = new System.Drawing.Size(93, 19);
			this.ZoomStatusLabel.Text = "Zoom: 100%";
			// 
			// DockPanel
			// 
			this.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DockPanel.Location = new System.Drawing.Point(0, 0);
			this.DockPanel.Name = "DockPanel";
			this.DockPanel.Size = new System.Drawing.Size(984, 661);
			this.DockPanel.TabIndex = 12;
			// 
			// Toolbox
			// 
			this.Toolbox.Dock = System.Windows.Forms.DockStyle.Left;
			this.Toolbox.Location = new System.Drawing.Point(0, 24);
			this.Toolbox.Name = "Toolbox";
			this.Toolbox.Size = new System.Drawing.Size(56, 613);
			this.Toolbox.TabIndex = 11;
			this.Toolbox.ToolboxOrientation = Sakura.Tools.ToolboxOrientation.Left;
			// 
			// Panels_Toolbox_Sep2
			// 
			this.Panels_Toolbox_Sep2.Name = "Panels_Toolbox_Sep2";
			this.Panels_Toolbox_Sep2.Size = new System.Drawing.Size(129, 6);
			// 
			// Panels_Toolbox_XSmall
			// 
			this.Panels_Toolbox_XSmall.Name = "Panels_Toolbox_XSmall";
			this.Panels_Toolbox_XSmall.Size = new System.Drawing.Size(180, 22);
			this.Panels_Toolbox_XSmall.Text = "Extra Small";
			this.Panels_Toolbox_XSmall.Click += new System.EventHandler(this.Panels_Toolbox_XSmall_Click);
			// 
			// Panels_Toolbox_Small
			// 
			this.Panels_Toolbox_Small.Name = "Panels_Toolbox_Small";
			this.Panels_Toolbox_Small.Size = new System.Drawing.Size(180, 22);
			this.Panels_Toolbox_Small.Text = "&Small";
			this.Panels_Toolbox_Small.Click += new System.EventHandler(this.Panels_Toolbox_Small_Click);
			// 
			// Panels_Toolbox_Medium
			// 
			this.Panels_Toolbox_Medium.Name = "Panels_Toolbox_Medium";
			this.Panels_Toolbox_Medium.Size = new System.Drawing.Size(180, 22);
			this.Panels_Toolbox_Medium.Text = "&Medium";
			this.Panels_Toolbox_Medium.Click += new System.EventHandler(this.Panels_Toolbox_Medium_Click);
			// 
			// Panels_Toolbox_Large
			// 
			this.Panels_Toolbox_Large.Name = "Panels_Toolbox_Large";
			this.Panels_Toolbox_Large.Size = new System.Drawing.Size(180, 22);
			this.Panels_Toolbox_Large.Text = "&Large";
			this.Panels_Toolbox_Large.Click += new System.EventHandler(this.Panels_Toolbox_Large_Click);
			// 
			// Panels_Toolbox_XLarge
			// 
			this.Panels_Toolbox_XLarge.Name = "Panels_Toolbox_XLarge";
			this.Panels_Toolbox_XLarge.Size = new System.Drawing.Size(180, 22);
			this.Panels_Toolbox_XLarge.Text = "E&xtra Large";
			this.Panels_Toolbox_XLarge.Click += new System.EventHandler(this.Panels_Toolbox_XLarge_Click);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(984, 661);
			this.Controls.Add(this.Toolbox);
			this.Controls.Add(this.MainMenu);
			this.Controls.Add(this.MainStatusStrip);
			this.Controls.Add(this.DockPanel);
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.MainMenu;
			this.Name = "MainWindow";
			this.Text = "Sakura Studio";
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.MainStatusStrip.ResumeLayout(false);
			this.MainStatusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem File_New;
        private System.Windows.Forms.ToolStripMenuItem File_Open;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
		private System.Windows.Forms.ToolStripMenuItem File_SaveAll;
		private System.Windows.Forms.ToolStripMenuItem File_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem File_Close;
        private System.Windows.Forms.ToolStripSeparator File_Sep1;
        private System.Windows.Forms.ToolStripMenuItem File_Exit;
        private System.Windows.Forms.ToolStripMenuItem EditMenu;
        private System.Windows.Forms.ToolStripMenuItem Edit_Undo;
        private System.Windows.Forms.ToolStripMenuItem Edit_Redo;
        private System.Windows.Forms.ToolStripSeparator Edit_Sep1;
        private System.Windows.Forms.ToolStripMenuItem Edit_Cut;
        private System.Windows.Forms.ToolStripMenuItem Edit_Copy;
        private System.Windows.Forms.ToolStripMenuItem Edit_Paste;
        private System.Windows.Forms.ToolStripMenuItem Edit_Delete;
        private System.Windows.Forms.ToolStripSeparator Edit_Sep2;
        private System.Windows.Forms.ToolStripMenuItem Edit_SelectAll;
        private System.Windows.Forms.ToolStripSeparator Edit_Sep3;
        private System.Windows.Forms.ToolStripMenuItem Edit_Find;
        private System.Windows.Forms.ToolStripMenuItem Edit_FindNext;
        private System.Windows.Forms.ToolStripMenuItem Edit_FindPrevious;
        private System.Windows.Forms.ToolStripMenuItem Edit_FindAndReplace;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenu;
        private System.Windows.Forms.ToolStripMenuItem Tools_Options;
		private System.Windows.Forms.ToolStripMenuItem PanelsMenu;
		private System.Windows.Forms.ToolStripSeparator Panels_Sep1;
		private System.Windows.Forms.ToolStripMenuItem Panels_ToolboxMenu;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_Left;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_Right;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_Top;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_Bottom;
		private System.Windows.Forms.ToolStripSeparator Panels_Toolbox_Sep1;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_Hidden;
		private System.Windows.Forms.ToolStripMenuItem Panels_ObjectManager;
		private System.Windows.Forms.ToolStripMenuItem Panels_ColorPanel;
		private System.Windows.Forms.ToolStripMenuItem WindowMenu;
        private System.Windows.Forms.ToolStripMenuItem Window_CloseAllDocuments;
        private System.Windows.Forms.ToolStripMenuItem Window_CloseAllButThis;
        private System.Windows.Forms.ToolStripSeparator Window_ListSeparator;
		private System.Windows.Forms.ToolStripMenuItem HelpMenu;
		private System.Windows.Forms.ToolStripMenuItem Help_AboutSakura;
		private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel MainStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel ZoomStatusLabel;
		private Sakura.Tools.Toolbox Toolbox;
		private WeifenLuo.WinFormsUI.Docking.DockPanel DockPanel;
		private System.Windows.Forms.ToolStripSeparator Panels_Toolbox_Sep2;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_XSmall;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_Small;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_Medium;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_Large;
		private System.Windows.Forms.ToolStripMenuItem Panels_Toolbox_XLarge;
	}
}