using Ankama.Utilities;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public abstract class ElementCaracsDictionary<T> : SerializableDictionary<CaracId, T>
	{
		protected ElementCaracsDictionary()
			: base((IEqualityComparer<CaracId>)CaracIdComparer.instance)
		{
		}
	}
}
