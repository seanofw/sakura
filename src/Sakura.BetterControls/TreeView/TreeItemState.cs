using System;

namespace Sakura.BetterControls.TreeView
{
	/// <summary>
	/// Possible state flags for a single tree item.
	/// </summary>
	[Flags]
	public enum TreeItemState : byte
	{
		/// <summary>
		/// This item is selected by the user.
		/// </summary>
		Selected = (1 << 0),

		/// <summary>
		/// This item has been expanded (not collapsed).
		/// </summary>
		Expanded = (1 << 1),

		/// <summary>
		/// This item is disabled and cannot be selected by the user.
		/// </summary>
		Disabled = (1 << 2),
	}
}
