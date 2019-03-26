using System.Collections.Generic;
using System.Linq;

namespace Ankama.Cube.Data.Utility
{
	public class ListsDiff<T>
	{
		private static readonly IReadOnlyList<T> EmptyList = new T[0];

		public IReadOnlyList<T> added
		{
			get;
		}

		public IReadOnlyList<T> removed
		{
			get;
		}

		public ListsDiff(IReadOnlyList<T> oldList, IReadOnlyList<T> newList)
		{
			removed = FindOnlyInFirst(oldList, newList);
			added = FindOnlyInFirst(newList, oldList);
		}

		private static IReadOnlyList<T> FindOnlyInFirst(IReadOnlyList<T> first, IReadOnlyList<T> second)
		{
			List<T> list = null;
			foreach (T item in first)
			{
				if (!second.Contains(item))
				{
					if (list == null)
					{
						list = new List<T>();
					}
					list.Add(item);
				}
			}
			IReadOnlyList<T> readOnlyList = list;
			return readOnlyList ?? EmptyList;
		}
	}
}
