using System;
using System.Collections.Generic;
using System.Linq;

namespace Ankama.Cube.Data
{
	public static class ListComparisonUtility
	{
		public static bool ValidateCondition<T>(IReadOnlyCollection<T> currents, ListComparison comparison, IReadOnlyList<T> expected)
		{
			int count = expected.Count;
			switch (comparison)
			{
			case ListComparison.ContainsAll:
				for (int k = 0; k < count; k++)
				{
					if (!currents.Contains(expected[k]))
					{
						return false;
					}
				}
				return true;
			case ListComparison.ContainsAtLeastOne:
				for (int j = 0; j < count; j++)
				{
					if (currents.Contains(expected[j]))
					{
						return true;
					}
				}
				return false;
			case ListComparison.ContainsExactlyOne:
			{
				bool flag = false;
				for (int l = 0; l < count; l++)
				{
					if (currents.Contains(expected[l]))
					{
						if (flag)
						{
							return false;
						}
						flag = true;
					}
				}
				return flag;
			}
			case ListComparison.ContainsNone:
				for (int i = 0; i < count; i++)
				{
					if (currents.Contains(expected[i]))
					{
						return false;
					}
				}
				return true;
			default:
				throw new ArgumentOutOfRangeException("comparison", comparison, null);
			}
		}
	}
}
