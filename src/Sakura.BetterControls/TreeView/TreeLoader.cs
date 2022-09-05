using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Sakura.BetterControls.TreeView
{
	/// <summary>
	/// This class recursively loads the entire tree from the data source.
	/// </summary>
	internal class TreeLoader
	{
		private readonly List<TreeItemInfo> _infos = new List<TreeItemInfo>();
		private readonly ConditionalWeakTable<ITreeItem, TreeItemInfo> _infoLookup = new ConditionalWeakTable<ITreeItem, TreeItemInfo>();

		private BetterTreeView _owner = null!;
		private ITreeItemDataSource? _treeItemDataSource = null;
		private Graphics _graphics = null!;
		private int _yOffset;
		private int _minHeight;

		public List<TreeItemInfo> Infos => _infos;
		public ConditionalWeakTable<ITreeItem, TreeItemInfo> InfoLookup => _infoLookup;

		public void Load(BetterTreeView owner)
		{
			_owner = owner;
			_treeItemDataSource = owner.DataSource;
			_minHeight = Math.Max((owner.CollapsedIcon?.Height ?? 0),
				(owner.ExpandedIcon?.Height ?? 0));
			if (_treeItemDataSource != null)
			{
				using (Graphics graphics = owner.CreateGraphics())
				{
					_graphics = graphics;
					RecursivelyLoad(null, 0);
				}
			}
		}

		private bool RecursivelyLoad(ITreeItem? rootItem, int depth)
		{
			if (_treeItemDataSource == null)
				return false;

			IEnumerable<ITreeItem> childItems = _treeItemDataSource.GetChildren(rootItem);
			bool hasChildren = false;

			foreach (ITreeItem childItem in childItems)
			{
				TreeItemInfo info = new TreeItemInfo
				{
					Index = _infos.Count,
					Item = childItem,
					Parent = rootItem,
					Height = Math.Max(childItem.GetHeight(_owner, _graphics), _minHeight),
					State = childItem.State,
					TreeShapes = new char[0],
					Depth = depth,
					YOffset = _yOffset,
				};

				_infos.Add(info);
				_infoLookup.Add(childItem, info);

				_yOffset += info.Height;

				info.HasChildren = RecursivelyLoad(childItem, depth + 1);

				hasChildren = true;
			}

			return hasChildren;
		}
	}
}
