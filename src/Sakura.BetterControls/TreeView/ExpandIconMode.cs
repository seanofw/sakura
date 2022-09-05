
namespace Sakura.BetterControls.TreeView
{
	public enum ExpandIconMode
	{
		/// <summary>
		/// Never show expando icons next to any item.
		/// </summary>
		ShowNever,

		/// <summary>
		/// Show expando icons next to items with children, and nothing next
		/// to items without children.
		/// </summary>
		ShowIfNeeded,

		/// <summary>
		/// Show expando icons next to items with children, but leave
		/// an equivalently-sized blank space next to items without children.
		/// This is the default behavior.
		/// </summary>
		ShowIconOrSpace,

		/// <summary>
		/// Show expando icons next to all items, including those without children.
		/// </summary>
		ShowAlways,
	}
}
