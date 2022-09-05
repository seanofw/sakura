namespace Sakura.Model
{
	public class Group : Item
	{
		public ImmutableList<Item> Items { get; }
		public ImmutableSortedDictionary<long, (int Index, Item Item)> ItemLookup { get; }

		public Group()
			: base(Bounds.None)
		{
			Items = ImmutableList<Item>.Empty;
			ItemLookup = ImmutableSortedDictionary<long, (int, Item)>.Empty;
		}

		public Group(params Item[] items)
			: this(ImmutableList<Item>.Empty.AddRange(items))
		{
		}

		public Group(IEnumerable<Item> items)
			: this(ImmutableList<Item>.Empty.AddRange(items))
		{
		}

		public Group(ImmutableList<Item> items,
			ImmutableSortedDictionary<long, (int, Item)>? itemLookup = null, Bounds? bounds = null,
			ItemMetadata? metadata = null, long id = -1)
			: base(bounds ?? CalcBounds(items), metadata, id)
		{
			Items = items;
			ItemLookup = itemLookup ?? MakeItemLookup(items);
		}

		private ImmutableSortedDictionary<long, (int, Item)> MakeItemLookup(IReadOnlyList<Item> items)
		{
			ImmutableSortedDictionary<long, (int, Item)> lookup = ImmutableSortedDictionary<long, (int, Item)>.Empty;

			for (int i = 0; i < items.Count; i++)
				lookup = lookup.Add(items[i].Id, (i, items[i]));

			return lookup;
		}

		#region Deep immutable updates

		public Item? GetByPath(ReadOnlySpan<long> path)
		{
			if (path.Length == 1)
				return GetById(path[0]);

			int childIndex = ItemLookup[path[0]].Index;
			return Items[childIndex] is Group childGroup
				? childGroup.GetByPath(path.Slice(1))
				: null;
		}

		public (int, Item?) GetByPathWithIndex(ReadOnlySpan<long> path)
		{
			if (path.Length == 1)
				return GetByIdWithIndex(path[0]);

			int childIndex = ItemLookup[path[0]].Index;
			return Items[childIndex] is Group childGroup
				? childGroup.GetByPathWithIndex(path.Slice(1))
				: (0, null);
		}

		public Group AddAtPath(ReadOnlySpan<long> path, Item item)
		{
			if (path.Length == 0)
				return AddItem(item);

			int childIndex = ItemLookup[path[0]].Index;
			return Items[childIndex] is Group childGroup
				? SetItem(childIndex, childGroup.AddAtPath(path.Slice(1), item))
				: this;
		}

		public Group AddAtPath(ReadOnlySpan<long> path, IEnumerable<Item> items)
		{
			if (path.Length == 0)
				return AddItems(items);

			int childIndex = ItemLookup[path[0]].Index;
			return Items[childIndex] is Group childGroup
				? SetItem(childIndex, childGroup.AddAtPath(path.Slice(1), items))
				: this;
		}

		public Group RemoveAtPath(ReadOnlySpan<long> path)
		{
			if (path.Length == 1)
				return RemoveItem(path[0]);

			int childIndex = ItemLookup[path[0]].Index;
			return Items[childIndex] is Group childGroup
				? SetItem(childIndex, childGroup.RemoveAtPath(path.Slice(1)))
				: this;
		}

		public Group SetAtPath(ReadOnlySpan<long> path, Item item)
		{
			if (path.Length == 1)
				return SetItemById(path[0], item);

			int childIndex = ItemLookup[path[0]].Index;
			return Items[childIndex] is Group childGroup
				? SetItem(childIndex, childGroup.SetAtPath(path.Slice(1), item))
				: this;
		}

		public Group SwapPaths(ReadOnlySpan<long> a, ReadOnlySpan<long> b)
		{
			if (a.Length != b.Length || a.Length == 0)
				throw new ArgumentException("Paths must be of the same length to swap items");

			if (a.Length == 1)
				return SwapItem(ItemLookup[a[0]].Index, ItemLookup[b[1]].Index);

			int childIndex = ItemLookup[a[0]].Index;
			return Items[childIndex] is Group childGroup
				? SetItem(childIndex, childGroup.SwapPaths(a.Slice(1), b.Slice(1)))
				: this;
		}

		#endregion

		#region Manipulation of this group

		public Item? GetById(long id)
			=> ItemLookup.TryGetValue(id, out (int Index, Item Item) value) ? value.Item : null;

		public (int Index, Item? Item) GetByIdWithIndex(long id)
			=> ItemLookup.TryGetValue(id, out (int Index, Item Item) value) ? value : (0, null);

		public Item? GetByIndex(int index)
			=> index >= 0 && index <= Items.Count ? Items[index] : null;

		public Group AddItems(IEnumerable<Item> newItems)
		{
			ImmutableList<Item> items = Items;
			ImmutableSortedDictionary<long, (int, Item)> itemLookup = ItemLookup;

			foreach (Item newItem in newItems)
			{
				items = items.Add(newItem);
				itemLookup = itemLookup.SetItem(newItem.Id, (items.Count - 1, newItem));
			}

			return WithItems(items, itemLookup);
		}

		public Group AddItem(Item item)
		{
			ImmutableList<Item> items = Items.Add(item);
			ImmutableSortedDictionary<long, (int, Item)> itemLookup = ItemLookup.SetItem(item.Id, (items.Count - 1, item));
			return WithItems(items, itemLookup);
		}

		public Group InsertItem(int index, Item item)
		{
			ImmutableList<Item> items = Items.Insert(index, item);
			ImmutableSortedDictionary<long, (int, Item)> itemLookup = ItemLookup.SetItem(item.Id, (index, item));
			for (int i = index + 1; i < items.Count; i++)
			{
				(int oldIndex, Item oldItem) = itemLookup[items[i].Id];
				itemLookup = itemLookup.SetItem(oldItem.Id, (oldIndex + 1, oldItem));
			}
			return WithItems(items, itemLookup);
		}

		public Group ReorderItem(int oldIndex, int newIndex)
		{
			if (newIndex == oldIndex)
				return this;

			Item item = Items[oldIndex];
			ImmutableList<Item> items = Items.RemoveAt(oldIndex);
			items = items.Insert(newIndex > oldIndex ? newIndex - 1 : newIndex, item);

			ImmutableSortedDictionary<long, (int, Item)> itemLookup = ItemLookup;
			int start = Math.Min(newIndex, oldIndex);
			int end = Math.Max(newIndex, oldIndex);
			for (int i = start; i <= end; i++)
			{
				Item movedItem = items[i];
				itemLookup = itemLookup.SetItem(movedItem.Id, (i, movedItem));
			}

			return WithItems(items, itemLookup, Bounds);
		}

		public Group SwapItem(int a, int b)
		{
			if (a == b)
				return this;

			Item item1 = Items[a];
			Item item2 = Items[b];
			ImmutableList<Item> items = Items.SetItem(b, item1).SetItem(a, item2);

			ImmutableSortedDictionary<long, (int, Item)> itemLookup = ItemLookup;
			itemLookup = itemLookup.SetItem(item1.Id, (b, item1));
			itemLookup = itemLookup.SetItem(item2.Id, (a, item2));

			return WithItems(items, itemLookup, Bounds);
		}

		public Group RemoveItem(long id)
		{
			if (!ItemLookup.TryGetValue(id, out (int Index, Item Item) value))
				return this;

			ImmutableList<Item> items = Items.RemoveAt(value.Index);
			ImmutableSortedDictionary<long, (int, Item)> itemLookup = ItemLookup.Remove(id);
			for (int i = value.Index; i < items.Count; i++)
			{
				(int oldIndex, Item oldItem) = itemLookup[items[i].Id];
				itemLookup = itemLookup.SetItem(oldItem.Id, (oldIndex - 1, oldItem));
			}

			return WithItems(items, itemLookup);
		}

		public Group SetItem(int index, Item item)
		{
			Item oldItem = Items[index];
			ImmutableSortedDictionary<long, (int, Item)> itemLookup = ItemLookup;
			if (item.Id != oldItem.Id)
			{
				itemLookup = itemLookup.Remove(oldItem.Id);
				itemLookup = itemLookup.Add(item.Id, (index, item));
			}
			ImmutableList<Item> items = Items.SetItem(index, item);

			return WithItems(items, itemLookup,
				item.Bounds == oldItem.Bounds ? Bounds : null);
		}

		public Group SetItemById(long id, Item item)
		{
			(int index, Item oldItem) = ItemLookup[id];
			ImmutableSortedDictionary<long, (int, Item)> itemLookup = ItemLookup;
			if (item.Id != oldItem.Id)
			{
				itemLookup = itemLookup.Remove(oldItem.Id);
				itemLookup = itemLookup.Add(item.Id, (index, item));
			}
			ImmutableList<Item> items = Items.SetItem(index, item);

			return WithItems(items, itemLookup,
				item.Bounds == oldItem.Bounds ? Bounds : null);
		}

		#endregion

		public Group WithItems(ImmutableList<Item> items,
			ImmutableSortedDictionary<long, (int, Item)>? itemLookup = null, Bounds? bounds = null)
			=> new Group(items, itemLookup, bounds, Metadata, Id);
		public override Group WithMetadata(ItemMetadata? metadata)
			=> new Group(Items, ItemLookup, Bounds, metadata, Id);
		public override Group Clone()
			=> new Group(Items, ItemLookup, Bounds, Metadata, Id);
		public override Group CloneWithNewId()
			=> new Group(Items, ItemLookup, Bounds, Metadata);

		private static Bounds CalcBounds(IEnumerable<Item> items)
		{
			Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

			foreach (Item item in items)
			{
				min = Vector2.Min(min, item.Bounds.Min);
				max = Vector2.Max(max, item.Bounds.Max);
			}

			return new Bounds(min, max);
		}
	}
}
