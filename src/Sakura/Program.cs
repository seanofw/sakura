using System.Windows.Forms;

namespace Sakura
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			ApplicationConfiguration.Initialize();
			Application.Run(new MainWindow());
		}
	}
}