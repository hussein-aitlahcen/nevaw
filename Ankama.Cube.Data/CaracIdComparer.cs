using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public class CaracIdComparer : IEqualityComparer<CaracId>
	{
		public static readonly CaracIdComparer instance;

		static CaracIdComparer()
		{
			instance = new CaracIdComparer();
		}

		public bool Equals(CaracId x, CaracId y)
		{
			return x == y;
		}

		public int GetHashCode(CaracId obj)
		{
			return (int)obj;
		}
	}
}
