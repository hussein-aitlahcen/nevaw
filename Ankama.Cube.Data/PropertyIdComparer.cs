using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public class PropertyIdComparer : IEqualityComparer<PropertyId>
	{
		public static readonly PropertyIdComparer instance;

		static PropertyIdComparer()
		{
			instance = new PropertyIdComparer();
		}

		public bool Equals(PropertyId x, PropertyId y)
		{
			return x == y;
		}

		public int GetHashCode(PropertyId obj)
		{
			return (int)obj;
		}
	}
}
