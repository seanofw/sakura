using System.Drawing;
using System.Windows.Forms;
using Sakura.BetterControls.Collections;
using Sakura.BetterControls.Themes;

namespace Sakura.BetterControls.TreeView
{
	public class SimpleTreeItem : ITreeItem, ICollection<SimpleTreeItem>
	{
		public SimpleTreeItemDataSource? DataSource { get; internal set; }

		public IList<SimpleTreeItem> Children {
			get => _children ?? (_children = new NotifyingList<SimpleTreeItem>(
				onAdd: NotifyDataSourceOfChildAddRemove,
				onRemove: NotifyDataSourceOfChildAddRemove,
				onChange: NotifyDataSourceOfChildChange
			));
		}
		private NotifyingList<SimpleTreeItem>? _children;

		private void NotifyDataSourceOfChildChange(int index,
			SimpleTreeItem oldItem, SimpleTreeItem newItem)
			=> DataSource?.OnChildrenChanged(this);
		private void NotifyDataSourceOfChildAddRemove(int index, SimpleTreeItem item)
			=> DataSource?.OnChildrenChanged(this);

		public SimpleTreeItem()
		{
		}

		public SimpleTreeItem(string text,
			Color? foreColor = null, Font? font = null, IEnumerable<Image>? icons = null)
		{
			_text = text;
			_font = font;
			_foreColor = foreColor;

			if (icons != null)
			{
				NotifyingList<Image> myIcons = (NotifyingList<Image>)Icons;
				myIcons.AddRange(icons);
			}
		}

		public SimpleTreeItem(string text, params Image[] icons)
			: this(text, null, null, icons)
		{
		}

		public SimpleTreeItem(string text, Color? foreColor, params Image[] icons)
			: this(text, foreColor, null, icons)
		{
		}

		public SimpleTreeItem(string text, Font? font, params Image[] icons)
			: this(text, null, font, icons)
		{
		}

		public SimpleTreeItem(string text, Color? foreColor, Font? font, params Image[] icons)
			: this(text, foreColor, font, (IEnumerable<Image>)icons)
		{
		}

		public Color? ForeColor
		{
			get => _foreColor;
			set
			{
				_foreColor = value;
				DataSource?.OnItemChanged(this);
			}
		}
		private Color? _foreColor;

		public Padding Padding
		{
			get => _padding;
			set
			{
				_padding = value;
				DataSource?.OnItemChanged(this);
			}
		}
		private Padding _padding;

		public int IconSpacing
		{
			get => _iconSpacing;
			set
			{
				_iconSpacing = value;
				DataSource?.OnItemChanged(this);
			}
		}
		private int _iconSpacing = 2;

		public Font? Font
		{
			get => _font;
			set
			{
				_font = value;
				DataSource?.OnItemChanged(this);
			}
		}
		private Font? _font;

		public string? Text
		{
			get => _text;
			set
			{
				_text = value;
				DataSource?.OnItemChanged(this);
			}
		}
		private string? _text;

		public IList<Image> Icons
		{
			get => _icons ?? (_icons = new NotifyingList<Image>(
				onAdd: NotifyDataSourceOfImageAddRemove,
				onRemove: NotifyDataSourceOfImageAddRemove,
				onChange: NotifyDataSourceOfImageChange
			));
		}
		private NotifyingList<Image>? _icons;

		private void NotifyDataSourceOfImageAddRemove(int index, Image image)
			=> DataSource?.OnItemChanged(this);
		private void NotifyDataSourceOfImageChange(int index, Image oldImage, Image newImage)
			=> DataSource?.OnItemChanged(this);

		public TreeItemState State
		{
			get => _state;
			set
			{
				_state = value;
				DataSource?.OnItemChanged(this);
			}
		}
		private TreeItemState _state;

		public int GetHeight(BetterTreeView treeView, Graphics graphics)
		{
			int maxIconHeight = 0;
			if (_icons != null)
			{
				foreach (Image image in _icons)
				{
					if (image.Height > maxIconHeight)
						maxIconHeight = image.Height;
				}
			}

			int textHeight = (int)Math.Ceiling(graphics.MeasureString(Text ?? string.Empty, Font ?? treeView.Font).Height);

			int maxHeight = Math.Max(maxIconHeight, textHeight);
			return maxHeight + Padding.Top + Padding.Bottom;
		}

		public bool OnContextClick(BetterTreeView treeView, ITreeItem treeItem, Point offset)
		{
			return false;
		}

		public bool OnItemClick(BetterTreeView treeView, ITreeItem treeItem, Point offset)
		{
			return false;
		}

		public void Render(BetterTreeView treeView, Graphics graphics, Rectangle rectangle)
		{
			int x = Padding.Left + rectangle.X;
			int y = rectangle.Y + Padding.Top;

			if (_icons != null)
			{
				foreach (Image image in _icons)
				{
					int iy = y + (rectangle.Height - image.Height) / 2;
					graphics.DrawImageUnscaled(image, x, iy);
					x += image.Width + IconSpacing;
				}
			}

			Font font = Font ?? treeView.Font;
			SizeF textSize = graphics.MeasureString(Text, font);
			int ty = y + (int)((rectangle.Height - textSize.Height) * 0.5 + 0.5);
			Color foreColor = ForeColor
				?? treeView.ForeColor
				?? BetterControlsTheme.Current.BetterTreeView.ForeColor;
			graphics.DrawString(Text, font, new SolidBrush(foreColor), new PointF(x, ty));
		}

		#region ICollection<SimpleTreeItem> support

		public int Count => Children.Count;

		public bool IsReadOnly => Children.IsReadOnly;

		public void Add(SimpleTreeItem item)
			=> Children.Add(item);

		public void Clear()
			=> Children.Clear();

		public bool Contains(SimpleTreeItem item)
			=> Children.Contains(item);

		public void CopyTo(SimpleTreeItem[] array, int arrayIndex)
			=> Children.CopyTo(array, arrayIndex);

		public bool Remove(SimpleTreeItem item)
			=> Children.Remove(item);

		public IEnumerator<SimpleTreeItem> GetEnumerator()
			=> Children.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
			=> Children.GetEnumerator();

		#endregion

		public override string ToString()
			=> Text ?? string.Empty;
	}
}
