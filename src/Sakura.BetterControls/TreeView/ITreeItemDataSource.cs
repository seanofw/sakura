
namespace Sakura.BetterControls.TreeView
{
	/// <summary>
	/// This interface describes a data source that provides BetterTreeView items.
	/// </summary>
	public interface ITreeItemDataSource
	{
		/// <summary>
		/// Listeners use this to retrieve the current complete set of (known)
		/// children for a given item.  The root item(s) of the tree should be
		/// returned when a listener requests the children of 'null'.
		/// </summary>
		IEnumerable<ITreeItem> GetChildren(ITreeItem? treeItem = null);

		/// <summary>
		/// Listeners use this to ask the data source to change the state of one
		/// of its items, either to select it, expand it, or collapse it.
		/// </summary>
		void SetState(ITreeItem treeItem, TreeItemState state);

		/// <summary>
		/// The data source should use this to notify listeners when the state
		/// or content of one of its items has changed.
		/// </summary>
		event EventHandler<TreeItemEventArgs> ItemChanged;

		/// <summary>
		/// The data source should use this to notify listeners that the set
		/// of children belonging to a given item have changed, either by
		/// new children being added or old children being removed.  Listeners
		/// should then call GetChildren() to request a new set of children.
		/// </summary>
		event EventHandler<TreeItemEventArgs> ChildrenChanged;
	}
}
