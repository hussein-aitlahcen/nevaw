using Ankama.Cube.Extensions;
using Ankama.Utilities;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace Ankama.Cube.Data
{
	public static class StatesDictionary
	{
		[PublicAPI]
		public static readonly ElementaryStates[] stateValues = (from s in EnumUtility.GetValues<ElementaryStates>()
			where s != ElementaryStates.None
			select s).ToArray();
	}
	public abstract class StatesDictionary<T> : SerializableDictionary<ElementaryStates, T>
	{
		protected StatesDictionary()
			: base((IEqualityComparer<ElementaryStates>)ElementaryStatesComparer.instance)
		{
		}
	}
}
