using System.Drawing;

namespace Sakura.BetterControls.TreeView
{
	/// <summary>
	/// This interface describes the data for a given item in the tree.
	/// </summary>
	public interface ITreeItem
	{
		/// <summary>
		/// Retrieve the current visual state of this item:  Selected,
		/// expanded, disabled, etc.
		/// </summary>
		TreeItemState State { get; }

		/// <summary>
		/// Listeners will invoke this method to render this item to the
		/// display.  The item should render its content inside the given rectangle,
		/// using the given graphics context.
		/// </summary>
		void Render(BetterTreeView treeView, Graphics graphics, Rectangle rectangle);

		/// <summary>
		/// Listeners will invoke this to ask the item to measure its maximum
		/// height, which should be returned in pixels.
		/// </summary>
		int GetHeight(BetterTreeView treeView, Graphics graphics);

		/// <summary>
		/// This optional handler can be used to specially handle mouse clicks
		/// on this item.  If you handle the mouse click yourself, return true
		/// to prevent default behavior from happening; to have default behavior
		/// happen (selection), you should return false.
		/// </summary>
		bool OnItemClick(BetterTreeView treeView, ITreeItem treeItem, Point offset);

		/// <summary>
		/// This optional handler can be used to specially handle alternate-button
		/// mouse clicks on this item.  If you handle the mouse click yourself, return
		/// true to prevent default behavior from happening; to have default behavior
		/// happen (propagation of the event), you should return false.
		/// </summary>
		bool OnContextClick(BetterTreeView treeView, ITreeItem treeItem, Point offset);
	}
}
