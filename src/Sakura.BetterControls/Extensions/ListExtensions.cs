using System;
using System.Collections.Generic;

namespace Sakura.BetterControls.Extensions
{
	internal static class ListExtensions
	{
		/// <summary>
		/// Perform a binary search on a list, using the provided comparison
		/// function to locate the target item.  This uses a binary search, but it
		/// attempts to optimize small cases by switching to linear search when
		/// the number of items gets small enough (<= 7).
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="compare">A function that will be provided an item within
		/// the list; it should answer whether the target item is before its argument (-1),
		/// after its argument (+1), or matches its argument (0).</param>
		/// <returns>The index of the matching item, or -1 if no item matches (-1
		/// for compatibility with the existing BinarySearch() methods).</returns>
		public static int BinarySearch<T>(this IList<T> list, Func<T, int> compare)
		{
			const int MinForLinear = 8;

			int start = 0;
			int end = list.Count - 1;

			// Below MinForLinear items, we fall back on a linear search, which
			// is typically faster for small sets.
			while (start <= end - MinForLinear)
			{
				// Calculating the midpoint this way avoids possible overflow for large lists.
				int mid = start + ((end - start) >> 1);

				// Compare.
				int cmp = compare(list[mid]);
				if (cmp == 0) return mid;
				if (cmp > 0)
					start = mid + 1;
				else
					end = mid - 1;
			}

			// For small sets, we just iterate linearly from start to end; this would
			// naturally be very fast, but it also unrolls extremely well, so we use
			// an efficient unrolled loop (with a computed goto) here to finish off
			// the smallest part of the search.
			switch (end - start + 1)
			{
				case 8:
					if (compare(list[start]) == 0) return start;
					start++;
					goto case 7;
				case 7:
					if (compare(list[start]) == 0) return start;
					start++;
					goto case 6;
				case 6:
					if (compare(list[start]) == 0) return start;
					start++;
					goto case 5;
				case 5:
					if (compare(list[start]) == 0) return start;
					start++;
					goto case 4;
				case 4:
					if (compare(list[start]) == 0) return start;
					start++;
					goto case 3;
				case 3:
					if (compare(list[start]) == 0) return start;
					start++;
					goto case 2;
				case 2:
					if (compare(list[start]) == 0) return start;
					start++;
					goto case 1;
				case 1:
					if (compare(list[start]) == 0) return start;
					start++;
					break;
			}

			// Didn't find it.
			return -1;
		}
	}
}
