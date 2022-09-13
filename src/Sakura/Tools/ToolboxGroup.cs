
namespace Sakura.Tools
{
	public class ToolboxGroup
	{
		public ToolKind Icon { get; set; }
		public ToolboxGroupKind Kind { get; set; }
		public List<ToolKind> Tools { get; set; } = new List<ToolKind>();
		public string Title { get; set; } = "";
	}
}
