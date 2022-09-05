
namespace Sakura.Commands
{
	public class Command
	{
		public string Name { get; }
		public Action<MainWindow> Action { get; }

		public Command(string name, Action<MainWindow> action)
		{
			Name = name;
			Action = action;
		}

		public virtual void Invoke(MainWindow mainWindow)
			=> Action(mainWindow);
	}
}
