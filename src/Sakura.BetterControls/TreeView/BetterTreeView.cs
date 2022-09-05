using System.Drawing;
using System.Windows.Forms;
using Sakura.BetterControls.Extensions;
using Sakura.BetterControls.Themes;

namespace Sakura.BetterControls.TreeView
{
	public class BetterTreeView : Control
	{
		private ConditionalWeakTable<ITreeItem, TreeItemInfo> _infoLookup = new ConditionalWeakTable<ITreeItem, TreeItemInfo>();
		private List<TreeItemInfo>? _infos = null;

		/// <summary>
		/// The icon to display beside a tree item when that tree item has children
		/// but is collapsed and the children are not shown.  The default is an
		/// 8-pixel-wide hollow black triangle pointing right.
		/// </summary>
		public Image? CollapsedIcon
		{
			get => _collapsedIcon;
			set
			{
				if (_collapsedIcon != value)
				{
					_collapsedIcon = value;
					RefreshAll();
				}
			}
		}
		private Image? _collapsedIcon;

		/// <summary>
		/// The icon to display beside a tree item when that tree item has children
		/// and is expanded to show them.  The default is an 8-pixel-wide filled
		/// black triangle pointing diagonally downward.
		/// </summary>
		public Image? ExpandedIcon
		{
			get => _expandedIcon;
			set
			{
				if (_expandedIcon != value)
				{
					_expandedIcon = value;
					RefreshAll();
				}
			}
		}
		private Image? _expandedIcon;

		/// <summary>
		/// When to show the expand/collapse icons.
		/// </summary>
		public ExpandIconMode ExpandIconMode
		{
			get => _expandIconMode;
			set
			{
				if (_expandIconMode != value)
				{
					_expandIconMode = value;
					Invalidate();
				}
			}
		}
		private ExpandIconMode _expandIconMode = ExpandIconMode.ShowIconOrSpace;

		/// <summary>
		/// How much space to add after the expando icons.  (default is 4 pixels)
		/// </summary>
		public int ExpandIconSpacing
		{
			get => _expandIconSpacing;
			set
			{
				if (_expandIconSpacing != value)
				{
					_expandIconSpacing = value;
					Invalidate();
				}
			}
		}
		private int _expandIconSpacing = 4;

		/// <summary>
		/// What style of border to draw around the TreeView, if any.  For FixedSingle
		/// borders (the default), you can control the border color with the
		/// BorderColor property.
		/// </summary>
		public BorderStyle BorderStyle
		{
			get => _borderStyle;
			set
			{
				if (_borderStyle != value)
				{
					_borderStyle = value;
					Invalidate();
				}
			}
		}
		private BorderStyle _borderStyle = BorderStyle.FixedSingle;

		/// <summary>
		/// What color to use for BorderStyle.FixedSingle.  If not assigned,
		/// this will choose a suitable default color automatically.
		/// </summary>
		public Color? BorderColor
		{
			get => _borderColor;
			set
			{
				if (_borderColor != value)
				{
					_borderColor = value;
					if (BorderStyle == BorderStyle.FixedSingle)
						Invalidate();
				}
			}
		}
		private Color? _borderColor;

		/// <summary>
		/// What color to use for the BetterTreeView's background.  If not assigned,
		/// this will choose a suitable default color automatically.
		/// </summary>
		public new Color? BackColor
		{
			get => _backColor;
			set
			{
				if (_backColor != value)
				{
					_backColor = value;
					Invalidate();
				}
			}
		}
		private Color? _backColor;

		/// <summary>
		/// What color to use for the BetterTreeView's text, unless overridden by specific
		/// tree items.  If not assigned, the control will choose a suitable default color
		/// automatically.
		/// </summary>
		public new Color? ForeColor
		{
			get => _foreColor;
			set
			{
				if (_foreColor != value)
				{
					_foreColor = value;
					Invalidate();
				}
			}
		}
		private Color? _foreColor;

		/// <summary>
		/// This controls where the BetterTreeView has scrolled to; it is the position
		/// the upper-left corner of the "viewport" relative to the overall "document".
		/// </summary>
		[Browsable(false)]
		public Point ScrollOffset
		{
			get => _scrollOffset;
			set
			{
				if (value != _scrollOffset)
				{
					_scrollOffset = value;
					Invalidate();   // Lazy; should use BitBlt() and then Invalidate() the rest.
				}
			}
		}
		private Point _scrollOffset;

		/// <summary>
		/// How deeply to indent each child item relative to its parent, in pixels.
		/// The default is 16 pixels.
		/// </summary>
		public int IndentSize
		{
			get => _indentSize;
			set
			{
				_indentSize = value;
				Invalidate();
			}
		}
		private int _indentSize = 16;

		/// <summary>
		/// This property controls the source of data that the BetterTreeView displays.
		/// </summary>
		[Browsable(false)]
		public ITreeItemDataSource? DataSource
		{
			get => _dataSource;

			set
			{
				if (_dataSource == value)
					return;
				if (_dataSource != null)
				{
					_dataSource.ItemChanged -= TreeItemDataSource_ItemChanged;
					_dataSource.ChildrenChanged -= TreeItemDataSource_ChildrenChanged;
				}
				_dataSource = value;
				if (_dataSource != null)
				{
					_dataSource.ItemChanged += TreeItemDataSource_ItemChanged;
					_dataSource.ChildrenChanged += TreeItemDataSource_ChildrenChanged;
				}
				ScrollOffset = new Point(0, 0);
				RefreshAll();
			}
		}
		private ITreeItemDataSource? _dataSource;

		/// <summary>
		/// Make a new one of these.
		/// </summary>
		public BetterTreeView()
		{
			_collapsedIcon = BetterControlsTheme.Current.BetterTreeView.CollapsedIcon;
			_expandedIcon = BetterControlsTheme.Current.BetterTreeView.ExpandedIcon;

			Padding = new Padding(4, 2, 4, 2);  // Default padding.

			SetStyle(ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.Opaque
				| ControlStyles.ResizeRedraw
				| ControlStyles.UserPaint
				| ControlStyles.Selectable, true);
		}

		/// <summary>
		/// If your data has changed a lot, or you think that the display has
		/// somehow been corrupted, you can invoke this to redraw the entire UI,
		/// reflowing the tree layout, and repainting ever pixel.  You generally
		/// should never need to explicitly call this.
		/// </summary>
		public void RefreshAll()
		{
			_infos = null;
			_infoLookup = new ConditionalWeakTable<ITreeItem, TreeItemInfo>();
			Invalidate();
		}

		private void TreeItemDataSource_ItemChanged(object? sender, TreeItemEventArgs e)
		{
			// If we don't know what this item is, just refresh the whole display.
			if (!_infoLookup.TryGetValue(e.TreeItem, out TreeItemInfo? info))
			{
				RefreshAll();
				return;
			}

			// If some of the ancestors are collapsed, there's nothing
			// to do here, since this item is currently hidden.
			if (!AllAncestorsExpanded(info.Parent)) return;

			// If the item changed height, we'll need to shift down the children,
			// and then repaint it and and everything after it.
			int newHeight;
			using (Graphics graphics = CreateGraphics())
			{
				newHeight = e.TreeItem.GetHeight(this, graphics);
			}
			if (info.Height != newHeight)
			{
				int delta = newHeight - info.Height;
				info.Height = newHeight;
				if (_infos != null)
				{
					for (int i = info.Index + 1; i < _infos.Count; i++)
					{
						info.Height += delta;
					}
				}
				Invalidate(new Rectangle(0, info.YOffset, Width, Height));
				return;
			}

			// If the item's state changed such that its children were
			// expanded or collapsed, then recalculate the whole layout,
			// and invalidate it and everything after it.
			if (((info.State ^ e.TreeItem.State) & TreeItemState.Expanded) != 0)
			{
				_infos = new List<TreeItemInfo>();
				_infoLookup = new ConditionalWeakTable<ITreeItem, TreeItemInfo>();
				Invalidate(new Rectangle(0, info.YOffset, Width, Height));
				return;
			}

			// Seems only this item's content changed, so invalidate just its row.
			Invalidate(new Rectangle(0, info.YOffset, Width, info.Height));
		}

		/// <summary>
		/// Determine if the given folder-like item and all of its ancestors
		/// are expanded.
		/// </summary>
		/// <param name="treeItem">The item at which to start the search.</param>
		/// <returns>True if this folder and all of its ancestors are expanded;
		/// false if even one of them is collapsed.</returns>
		private bool AllAncestorsExpanded(ITreeItem? treeItem)
		{
			while (treeItem != null)
			{
				// If we don't know what this item is, consider it to be collapsed.
				if (!_infoLookup.TryGetValue(treeItem, out TreeItemInfo? info))
					return false;

				// If it's not expanded, it's collapsed.
				if ((info.State & TreeItemState.Expanded) == 0)
					return false;

				treeItem = info.Parent;
			}

			return true;
		}

		private void TreeItemDataSource_ChildrenChanged(object? sender, TreeItemEventArgs e)
		{
			// If we don't know what this item is, just refresh the whole display.
			if (!_infoLookup.TryGetValue(e.TreeItem, out TreeItemInfo? info))
			{
				RefreshAll();
				return;
			}

			// If some of this item's ancestors are collapsed, there's nothing
			// to do here, since this item is currently hidden.
			if (!AllAncestorsExpanded(info.Parent)) return;

			// If this item is currently collapsed, it needs to be repainted,
			// but nothing else needs to happen.
			if ((info.State & TreeItemState.Expanded) == 0)
			{
				Invalidate(new Rectangle(0, info.YOffset, Width, info.Height));
				return;
			}

			// This item is visible, and its children have changed, so we need to
			// reload the dataset and repaint everything.
			RefreshAll();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (_infos == null)
			{
				TreeLoader treeLoader = new TreeLoader();
				treeLoader.Load(this);
				_infos = treeLoader.Infos;
				_infoLookup = treeLoader.InfoLookup;
			}

			int borderThickness;
			switch (BorderStyle)
			{
				case BorderStyle.FixedSingle:
					Color color = BorderColor
						?? BetterControlsTheme.Current.BetterTreeView.DefaultFlatBorderColor;
					e.Graphics.DrawRectangle(new Pen(color), new Rectangle(0, 0, Width - 1, Height - 1));
					borderThickness = 1;
					break;
				case BorderStyle.Fixed3D:
					ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0, 0, Width - 1, Height - 1), Border3DStyle.SunkenOuter);
					ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(1, 1, Width - 3, Height - 3), Border3DStyle.SunkenInner);
					borderThickness = 2;
					break;
				default:
					borderThickness = 0;
					break;
			}

			e.Graphics.FillRectangle(new SolidBrush(BackColor
					?? BetterControlsTheme.Current.BetterTreeView.BackColor),
				new Rectangle(borderThickness, borderThickness, Width - borderThickness * 2, Height - borderThickness * 2));

			int startIndex = FindItemByYCoordinate(e.ClipRectangle.Top + ScrollOffset.Y + Padding.Top + borderThickness);
			int endIndex = FindItemByYCoordinate(e.ClipRectangle.Bottom + ScrollOffset.Y + Padding.Top + borderThickness);
			if (startIndex < 0) startIndex = 0;
			if (endIndex < 0) endIndex = _infos.Count - 1;

			for (int i = startIndex; i <= endIndex; i++)
			{
				TreeItemInfo itemInfo = _infos[i];

				// Draw this item, shifted by the scroll offset, padding, and border.
				int x = itemInfo.Depth * IndentSize + ScrollOffset.X + Padding.Left + borderThickness;
				int y = itemInfo.YOffset + ScrollOffset.Y + Padding.Top + borderThickness;

				// Figure out which expando icon to show, and if so, move the item itself over a bit.
				x += DrawExpandoIcon(e.Graphics, itemInfo, x, y, itemInfo.Height);

				// Draw the item itself.
				Rectangle rectangle = new Rectangle(x, y, Width, itemInfo.Height);
				itemInfo.Item?.Render(this, e.Graphics, rectangle);
			}
		}

		private int DrawExpandoIcon(Graphics graphics, TreeItemInfo itemInfo, int x, int y, int minHeight)
		{
			Image? expandoIcon = ((itemInfo.State & TreeItemState.Expanded) != 0)
				? ExpandedIcon : CollapsedIcon;

			bool showExpandoIcon;
			switch (ExpandIconMode)
			{
				case ExpandIconMode.ShowAlways:
					showExpandoIcon = true;
					break;
				case ExpandIconMode.ShowIfNeeded:
					showExpandoIcon = itemInfo.HasChildren;
					break;
				default:
				case ExpandIconMode.ShowIconOrSpace:
					expandoIcon = itemInfo.HasChildren ? expandoIcon : null;
					showExpandoIcon = true;
					break;
				case ExpandIconMode.ShowNever:
					showExpandoIcon = false;
					break;
			}

			if (!showExpandoIcon)
				return 0;

			int expandoContainerWidth = Math.Max(ExpandedIcon?.Width ?? 0,
				CollapsedIcon?.Width ?? 0);
			int expandoContainerHeight = Math.Max(Math.Max(ExpandedIcon?.Height ?? 0,
				CollapsedIcon?.Height ?? 0),
				minHeight);

			if (expandoIcon != null)
			{
				int ix = x + ((expandoContainerWidth - expandoIcon.Width) >> 1);
				int iy = y + ((expandoContainerHeight - expandoIcon.Height) >> 1);
				graphics.DrawImage(expandoIcon, ix, iy, expandoIcon.Width, expandoIcon.Height);
			}

			return expandoContainerWidth + ExpandIconSpacing;
		}

		private int FindItemByYCoordinate(int y)
		{
			// Efficiently binary search through the item coordinates to find
			// the one that contains this provided Y coordinate.
			if (_infos == null)
				return 0;
			return _infos.BinarySearch(item =>
				  y < item.YOffset ? -1
				: y >= item.YOffset + item.Height ? +1
				: 0);
		}
	}
}
