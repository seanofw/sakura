using Sakura.MathLib;

namespace Sakura.Tools
{
	public class ToolInfo
	{
		public readonly ToolKind Kind;
		public readonly ToolMode Mode;
		public readonly Vector2i IconPosition;
		public readonly string Title;
		public readonly ToolBase? ToolMechanics;

		public string Name => Kind.ToString();

		public ToolInfo(ToolKind kind, ToolMode mode, Vector2i iconPosition, string title,
			ToolBase? toolMechanics = null)
		{
			Kind = kind;
			Mode = mode;
			IconPosition = iconPosition;
			Title = title;
			ToolMechanics = toolMechanics;
		}
	}
}
