
namespace Sakura.Commands
{
	public class CommandTable
	{
		public Command FileNew { get; set; } = null!;
		public Command FileClose { get; set; } = null!;
		public Command FileExit { get; set; } = null!;

		public Command WindowCloseAll { get; set; } = null!;
		public Command WindowCloseAllButThis { get; set; } = null!;

		public Command HelpAbout { get; set; } = null!;

		public Command? ByName(string name)
		{
			PropertyInfo? property = typeof(CommandTable).GetProperty(name);
			if (property == null)
				return null;
			return (Command?)property.GetValue(this, null);
		}
	}
}
