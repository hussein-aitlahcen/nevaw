using Ankama.Cube.Extensions;
using Ankama.Utilities;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public static class ElementsDictionary
	{
		[PublicAPI]
		public static readonly Element[] elementValues = EnumUtility.GetValues<Element>();
	}
	public abstract class ElementsDictionary<T> : SerializableDictionary<Element, T>
	{
		protected ElementsDictionary()
			: base((IEqualityComparer<Element>)ElementComparer.instance)
		{
		}
	}
}
