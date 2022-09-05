using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sakura.BetterControls.TreeView
{
	public class SimpleTreeItemDataSource : ITreeItemDataSource, IList<SimpleTreeItem>
	{
		public event EventHandler<TreeItemEventArgs>? ItemChanged;
		public event EventHandler<TreeItemEventArgs>? ChildrenChanged;

		private readonly SimpleTreeItem _rootItem;

		public SimpleTreeItemDataSource()
		{
			_rootItem = new SimpleTreeItem
			{
				DataSource = this
			};
		}

		protected internal virtual void OnItemChanged(SimpleTreeItem simpleTreeItem)
		{
			ItemChanged?.Invoke(this, new TreeItemEventArgs(simpleTreeItem));
		}

		protected internal virtual void OnChildrenChanged(SimpleTreeItem simpleTreeItem)
		{
			ChildrenChanged?.Invoke(this, new TreeItemEventArgs(simpleTreeItem));
		}

		IEnumerable<ITreeItem> ITreeItemDataSource.GetChildren(ITreeItem? treeItem)
		{
			if (treeItem == null)
				return _rootItem.Children;

			if (!(treeItem is SimpleTreeItem simpleTreeItem))
				return Enumerable.Empty<ITreeItem>();

			return simpleTreeItem.Children;
		}

		void ITreeItemDataSource.SetState(ITreeItem treeItem, TreeItemState state)
		{
			if (!(treeItem is SimpleTreeItem simpleTreeItem))
				throw new ArgumentException("treeItem");

			simpleTreeItem.State = state;
		}

		public int Count => _rootItem.Children.Count;

		public bool IsReadOnly => false;

		public SimpleTreeItem this[int index]
		{
			get => _rootItem.Children[index];
			set => _rootItem.Children[index] = value;
		}

		public int IndexOf(SimpleTreeItem item)
			=> _rootItem.Children.IndexOf(item);

		public void Insert(int index, SimpleTreeItem item)
			=> _rootItem.Children.Insert(index, item);

		public void RemoveAt(int index)
			=> _rootItem.Children.RemoveAt(index);

		public void Add(SimpleTreeItem item)
			=> _rootItem.Children.Add(item);

		public void Clear()
			=> _rootItem.Children.Clear();

		public bool Contains(SimpleTreeItem item)
			=> _rootItem.Children.Contains(item);

		public void CopyTo(SimpleTreeItem[] array, int arrayIndex)
			=> _rootItem.Children.CopyTo(array, arrayIndex);

		public bool Remove(SimpleTreeItem item)
			=> _rootItem.Children.Remove(item);

		public IEnumerator<SimpleTreeItem> GetEnumerator()
			=> _rootItem.Children.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
			=> _rootItem.Children.GetEnumerator();
	}
}
