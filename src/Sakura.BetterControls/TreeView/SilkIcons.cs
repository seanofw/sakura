using System.Drawing;

namespace Sakura.BetterControls.TreeView
{
	public static class SilkIcons
	{
		public static Image Application { get => (_application ?? (_application = LoadFromImageBytes(SilkIconData.Application))); }
		private static Image? _application;

		public static Image Book { get => (_book ?? (_book = LoadFromImageBytes(SilkIconData.Book))); }
		private static Image? _book;

		public static Image Brick { get => (_brick ?? (_brick = LoadFromImageBytes(SilkIconData.Brick))); }
		private static Image? _brick;

		public static Image Building { get => (_building ?? (_building = LoadFromImageBytes(SilkIconData.Building))); }
		private static Image? _building;

		public static Image Database { get => (_database ?? (_database = LoadFromImageBytes(SilkIconData.Database))); }
		private static Image? _database;

		public static Image Drive { get => (_drive ?? (_drive = LoadFromImageBytes(SilkIconData.Drive))); }
		private static Image? _drive;

		public static Image DriveNetwork { get => (_driveNetwork ?? (_driveNetwork = LoadFromImageBytes(SilkIconData.DriveNetwork))); }
		private static Image? _driveNetwork;

		public static Image Folder { get => (_folder ?? (_folder = LoadFromImageBytes(SilkIconData.Folder))); }
		private static Image? _folder;

		public static Image Page { get => (_page ?? (_page = LoadFromImageBytes(SilkIconData.Page))); }
		private static Image? _page;

		public static Image PageWhite { get => (_pageWhite ?? (_pageWhite = LoadFromImageBytes(SilkIconData.PageWhite))); }
		private static Image? _pageWhite;

		public static Image PageWhiteText { get => (_pageWhiteText ?? (_pageWhiteText = LoadFromImageBytes(SilkIconData.PageWhiteText))); }
		private static Image? _pageWhiteText;

		public static Image PageWhiteStack { get => (_pageWhiteStack ?? (_pageWhiteStack = LoadFromImageBytes(SilkIconData.PageWhiteStack))); }
		private static Image? _pageWhiteStack;

		public static Image Picture { get => (_picture ?? (_picture = LoadFromImageBytes(SilkIconData.Picture))); }
		private static Image? _picture;

		public static Image Script { get => (_script ?? (_script = LoadFromImageBytes(SilkIconData.Script))); }
		private static Image? _script;

		public static Image Server { get => (_server ?? (_server = LoadFromImageBytes(SilkIconData.Server))); }
		private static Image? _server;

		public static Image Table { get => (_table ?? (_table = LoadFromImageBytes(SilkIconData.Table))); }
		private static Image? _table;

		private static Image LoadFromImageBytes(byte[] imageData)
		{
			using (MemoryStream stream = new MemoryStream(imageData))
			{
				return Image.FromStream(stream);
			}
		}
	}
}