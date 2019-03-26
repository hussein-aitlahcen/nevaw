using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public class ElementComparer : IEqualityComparer<Element>
	{
		public static readonly ElementComparer instance;

		static ElementComparer()
		{
			instance = new ElementComparer();
		}

		public bool Equals(Element x, Element y)
		{
			return x == y;
		}

		public int GetHashCode(Element obj)
		{
			return (int)obj;
		}
	}
}
