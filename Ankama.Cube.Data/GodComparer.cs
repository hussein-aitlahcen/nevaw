using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public class GodComparer : IEqualityComparer<God>
	{
		public static readonly GodComparer instance;

		static GodComparer()
		{
			instance = new GodComparer();
		}

		public bool Equals(God x, God y)
		{
			return x == y;
		}

		public int GetHashCode(God obj)
		{
			return (int)obj;
		}
	}
}
