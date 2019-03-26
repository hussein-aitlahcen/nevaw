using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public class ElementaryStatesComparer : IEqualityComparer<ElementaryStates>
	{
		public static readonly ElementaryStatesComparer instance;

		static ElementaryStatesComparer()
		{
			instance = new ElementaryStatesComparer();
		}

		public bool Equals(ElementaryStates x, ElementaryStates y)
		{
			return x == y;
		}

		public int GetHashCode(ElementaryStates obj)
		{
			return (int)obj;
		}
	}
}
