using Ankama.Utilities;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Audio
{
	[Serializable]
	public sealed class AudioEventParameterDictionary : SerializableDictionary<string, float>
	{
		public AudioEventParameterDictionary()
			: base((IEqualityComparer<string>)StringComparer.Ordinal)
		{
		}
	}
}
