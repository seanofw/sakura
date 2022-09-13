
namespace Sakura.Commands
{
	public class CommandTable
	{
		public Command FileNew { get; set; } = null!;
		public Command FileClose { get; set; } = null!;
		public Command FileExit { get; set; } = null!;

		public Command WindowCloseAll { get; set; } = null!;
		public Command WindowCloseAllButThis { get; set; } = null!;

		public Command ToolboxHidden { get; set; } = null!;
		public Command ToolboxLeft { get; set; } = null!;
		public Command ToolboxRight { get; set; } = null!;
		public Command ToolboxTop { get; set; } = null!;
		public Command ToolboxBottom { get; set; } = null!;

		public Command ToolboxXSmall { get; set; } = null!;
		public Command ToolboxSmall { get; set; } = null!;
		public Command ToolboxMedium { get; set; } = null!;
		public Command ToolboxLarge { get; set; } = null!;
		public Command ToolboxXLarge { get; set; } = null!;

		public Command ObjectManagerToggle { get; set; } = null!;
		public Command ObjectManagerShow { get; set; } = null!;
		public Command ObjectManagerHide { get; set; } = null!;

		public Command ColorPanelToggle { get; set; } = null!;
		public Command ColorPanelShow { get; set; } = null!;
		public Command ColorPanelHide { get; set; } = null!;

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
