namespace Sakura.BetterControls.Themes
{
	public abstract class BetterControlsTheme
	{
		public abstract BetterScrollbarTheme BetterScrollbar { get; }
		public abstract BetterTreeViewTheme BetterTreeView { get; }

		protected BetterControlsTheme()
		{
		}

		/// <summary>
		/// This is how you control which theme is in use.  If you change this,
		/// you will need to either force existing controls to repaint or you will
		/// need to recreate them.
		/// </summary>
		public static BetterControlsTheme Current { get; set; } = BetterControlsDefaultTheme.Instance;
	}

	//--------------------------------------------------------------------------
	//  Theme variants.

	/// <summary>
	/// Match Windows's current color scheme, as-is.
	/// </summary>
	public class BetterControlsDefaultTheme : BetterControlsTheme
	{
		public override BetterScrollbarTheme BetterScrollbar { get; } = new BetterScrollbarDefaultTheme();
		public override BetterTreeViewTheme BetterTreeView { get; } = new BetterTreeViewDefaultTheme();

		public static readonly BetterControlsDefaultTheme Instance = new BetterControlsDefaultTheme();

		private BetterControlsDefaultTheme()
		{
		}
	}

	/// <summary>
	/// Use mostly black text on white or very light backgrounds, but use
	/// Windows's current accent colors.
	/// </summary>
	public class BetterControlsLightTheme : BetterControlsTheme
	{
		public override BetterScrollbarTheme BetterScrollbar { get; } = new BetterScrollbarLightTheme();
		public override BetterTreeViewTheme BetterTreeView { get; } = new BetterTreeViewLightTheme();

		public static readonly BetterControlsLightTheme Instance = new BetterControlsLightTheme();

		private BetterControlsLightTheme()
		{
		}
	}

	/// <summary>
	/// Use mostly white text on dark backgrounds, but use Windows's current
	/// accent colors if possible.
	/// </summary>
	public class BetterControlsDarkTheme : BetterControlsTheme
	{
		public override BetterScrollbarTheme BetterScrollbar { get; } = new BetterScrollbarDarkTheme();
		public override BetterTreeViewTheme BetterTreeView { get; } = new BetterTreeViewDarkTheme();

		public static readonly BetterControlsDarkTheme Instance = new BetterControlsDarkTheme();

		private BetterControlsDarkTheme()
		{
		}
	}

	/// <summary>
	/// Use mostly black text on light-gray backgrounds, with blue accent colors.
	/// </summary>
	public class BetterControlsBlueTheme : BetterControlsTheme
	{
		public override BetterScrollbarTheme BetterScrollbar { get; } = new BetterScrollbarBlueTheme();
		public override BetterTreeViewTheme BetterTreeView { get; } = new BetterTreeViewBlueTheme();

		public static readonly BetterControlsBlueTheme Instance = new BetterControlsBlueTheme();

		private BetterControlsBlueTheme()
		{
		}
	}
}
