using Sakura.BetterControls.Extensions;
using System.Drawing;

namespace Sakura.BetterControls.Themes
{
	public abstract class BetterScrollbarTheme
	{
		public virtual Color ThumbColor { get; } = SystemColors.ControlDark;
		public virtual Color ThumbHoverColor { get; } = SystemColors.ControlDarkDark.Mix(SystemColors.ControlDark);
		public virtual Color ThumbPressedColor { get; } = SystemColors.ControlDarkDark;
		public virtual Color BackColor { get; } = SystemColors.ScrollBar;
		public virtual Color ArrowColor { get; } = SystemColors.ControlText;
		public virtual Color ArrowHoverColor { get; } = SystemColors.Highlight.Mix(SystemColors.ControlText);
		public virtual Color ArrowPressedColor { get; } = SystemColors.Highlight;
	}

	//--------------------------------------------------------------------------
	//  Theme variants.

	public class BetterScrollbarDefaultTheme : BetterScrollbarTheme
	{
	}

	public class BetterScrollbarLightTheme : BetterScrollbarTheme
	{
		public override Color ThumbColor { get; } = Color.FromArgb(192, 192, 192);
		public override Color ThumbHoverColor { get; } = Color.FromArgb(160, 160, 160);
		public override Color ThumbPressedColor { get; } = Color.FromArgb(128, 128, 128);
		public override Color BackColor { get; } = Color.FromArgb(240, 240, 240);
		public override Color ArrowColor { get; } = Color.FromArgb(192, 192, 192);
		public override Color ArrowHoverColor { get; } = Color.FromArgb(64, 64, 192);
		public override Color ArrowPressedColor { get; } = Color.FromArgb(0, 0, 224);
	}

	public class BetterScrollbarDarkTheme : BetterScrollbarTheme
	{
		public override Color ThumbColor { get; } = Color.FromArgb(64, 64, 64);
		public override Color ThumbHoverColor { get; } = Color.FromArgb(72, 72, 72);
		public override Color ThumbPressedColor { get; } = Color.FromArgb(96, 96, 96);
		public override Color BackColor { get; } = Color.FromArgb(40, 40, 40);
		public override Color ArrowColor { get; } = Color.FromArgb(64, 64, 64);
		public override Color ArrowHoverColor { get; } = Color.FromArgb(80, 80, 80);
		public override Color ArrowPressedColor { get; } = Color.FromArgb(96, 96, 96);
	}

	public class BetterScrollbarBlueTheme : BetterScrollbarTheme
	{
		public override Color ThumbColor { get; } = Color.FromArgb(192, 192, 192);
		public override Color ThumbHoverColor { get; } = Color.FromArgb(160, 160, 160);
		public override Color ThumbPressedColor { get; } = Color.FromArgb(128, 128, 128);
		public override Color BackColor { get; } = Color.FromArgb(240, 240, 240);
		public override Color ArrowColor { get; } = Color.FromArgb(192, 192, 192);
		public override Color ArrowHoverColor { get; } = Color.FromArgb(64, 64, 192);
		public override Color ArrowPressedColor { get; } = Color.FromArgb(0, 0, 224);
	}
}
