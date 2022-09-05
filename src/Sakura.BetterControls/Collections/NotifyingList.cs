
namespace Sakura.BetterControls.Collections
{
	internal class NotifyingList<T> : IList<T>
	{
		private readonly Action<int, T> _onAdd;
		private readonly Action<int, T> _onRemove;
		private readonly Action<int, T, T> _onChange;

		private readonly List<T> _list = new List<T>();

		public NotifyingList(Action<int, T> onAdd, Action<int, T> onRemove,
			Action<int, T, T> onChange)
		{
			_onAdd = onAdd;
			_onRemove = onRemove;
			_onChange = onChange;
		}

		public T this[int index]
		{
			get => _list[index];
			set
			{
				T oldItem = _list[index];
				_list[index] = value;
				_onChange(index, oldItem, value);
			}
		}

		public int Count => _list.Count;

		public bool IsReadOnly => false;

		public void Add(T item)
		{
			_list.Add(item);
			_onAdd(_list.Count - 1, item);
		}

		public void AddRange(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				Add(item);
			}
		}

		public void Clear()
		{
			for (int i = _list.Count - 1; i >= 0; i--)
			{
				_onRemove(i, _list[i]);
			}
			_list.Clear();
		}

		public bool Contains(T item)
			=> _list.Contains(item);

		public void CopyTo(T[] array, int arrayIndex)
			=> _list.CopyTo(array, arrayIndex);

		public IEnumerator<T> GetEnumerator()
			=> _list.GetEnumerator();

		public int IndexOf(T item)
			=> _list.IndexOf(item);

		public void Insert(int index, T item)
		{
			_list.Insert(index, item);

			_onChange(_list.Count - 1, default!, _list[_list.Count - 1]);
			for (int i = _list.Count - 2; i > index; i--)
			{
				_onChange(i, _list[i], _list[i - 1]);
			}
			_onAdd(index, item);
		}

		public bool Remove(T item)
		{
			bool hadMatch = false;
			int index = 0;

			while (true)
			{
				index = _list.IndexOf(item, index);
				if (index < 0)
					return hadMatch;

				RemoveAt(index);
				hadMatch = true;
			}
		}

		public void RemoveAt(int index)
		{
			T oldItem = _list[index];
			_list.RemoveAt(index);
			_onRemove(index, oldItem);
			for (int i = index; i < _list.Count; i++)
			{
				_onChange(i, _list[i], i < _list.Count - 1 ? _list[i + 1] : default!);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
			=> _list.GetEnumerator();
	}
}
