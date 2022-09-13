using System.Windows.Forms;

namespace Sakura
{
	public static class Program
	{
		public static string Name => "Sakura Studio";
		public static string ResourcePath { get; private set; } = "";

		[STAThread]
		static void Main()
		{
			ResourcePath = FindResourcePath()!;
			if (string.IsNullOrEmpty(ResourcePath))
			{
				BetterControls.MessageBox.BetterMessageBox<bool>
					.Button("Uh oh", default, bold: true)
					.Caption(Name)
					.StandardImage(BetterControls.MessageBox.StandardImageKind.Error)
					.Message("Error: The 'Resources/' directory, which contains\r\n"
						+ "critical configuration and data files, is missing.\r\n"
						+ "Please reinstall Sakura Studio.")
					.Show();
				return;
			}

			ApplicationConfiguration.Initialize();
			Application.Run(new MainWindow());
		}

		private static string? FindResourcePath()
		{
			string? currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			while (!string.IsNullOrEmpty(currentPath))
			{
				string resourcePath = Path.Combine(currentPath, "Resources");
				if (Directory.Exists(resourcePath))
					return resourcePath;
				currentPath = Path.GetDirectoryName(currentPath);
			}
			return null;
		}
	}
}