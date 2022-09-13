namespace Sakura.Tools
{
	public enum ToolMode : ushort
	{
		Normal = 0,		// This tool stays active until a new tool is chosen.
		OneShot,		// This tool activates immediately and does not alter the mode.
		UntilClick,		// This tool activates until the next document click.
		Toggle,			// This tool toggles (either checkbox-style or radio-button-style).
	}
}
