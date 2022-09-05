namespace Sakura.BetterControls.TreeView
{
	internal class TreeItemInfo
	{
		public int Index;
		public ITreeItem? Item;
		public ITreeItem? Parent;
		public int Height;
		public TreeItemState? State;
		public char[]? TreeShapes;
		public int Depth;
		public int YOffset;
		public bool HasChildren;

		public override string ToString()
			=> $"{Index}: \"{Item}\" (Parent=\"{Parent}\", Depth={Depth}, Pos=({YOffset},+{Height}), State={State})";
	}
}
