using System.Drawing;

namespace Sakura.BetterControls.Themes
{
	public abstract class BetterTreeViewTheme
	{
		public virtual Color ForeColor { get; } = SystemColors.Window;
		public virtual Color BackColor { get; } = SystemColors.WindowText;
		public virtual Color SelectionForeColor { get; } = SystemColors.HighlightText;
		public virtual Color SelectionBackColor { get; } = SystemColors.Highlight;
		public virtual Color DefaultFlatBorderColor { get; } = SystemColors.ControlDark;
		public virtual Image ExpandedIcon { get; } = BetterTreeViewIcons.ExpandedBlackIcon;
		public virtual Image CollapsedIcon { get; } = BetterTreeViewIcons.CollapsedBlackIcon;
	}

	//--------------------------------------------------------------------------
	//  Theme variants.

	public class BetterTreeViewDefaultTheme : BetterTreeViewTheme
	{
	}

	public class BetterTreeViewLightTheme : BetterTreeViewTheme
	{
		public override Color ForeColor { get; } = Color.Black;
		public override Color BackColor { get; } = Color.White;
		public override Color DefaultFlatBorderColor { get; } = Color.FromArgb(224, 224, 224);
	}

	public class BetterTreeViewDarkTheme : BetterTreeViewTheme
	{
		public override Color ForeColor { get; } = Color.FromArgb(204, 204, 204);
		public override Color BackColor { get; } = Color.FromArgb(32, 32, 32);
		public override Color DefaultFlatBorderColor { get; } = Color.FromArgb(64, 64, 64);
		public override Image ExpandedIcon { get; } = BetterTreeViewIcons.ExpandedWhiteIcon;
		public override Image CollapsedIcon { get; } = BetterTreeViewIcons.CollapsedWhiteIcon;
	}

	public class BetterTreeViewBlueTheme : BetterTreeViewTheme
	{
		public override Color ForeColor { get; } = Color.Black;
		public override Color BackColor { get; } = Color.FromArgb(234, 237, 244);
		public override Color DefaultFlatBorderColor { get; } = Color.FromArgb(142, 155, 188);
	}
}
