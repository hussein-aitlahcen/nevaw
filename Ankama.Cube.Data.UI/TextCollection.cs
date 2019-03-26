using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data.UI
{
	[CreateAssetMenu(fileName = "New Text Collection", menuName = "Waven/Data/UI/Text Collection")]
	public class TextCollection : ScriptableObject
	{
		[Serializable]
		private class DataDictionary : SerializableDictionary<int, string>
		{
		}

		[UsedImplicitly]
		[SerializeField]
		private CultureCode m_cultureCode;

		[UsedImplicitly]
		[SerializeField]
		private DataDictionary m_dataDictionary = new DataDictionary();

		public CultureCode cultureCode => m_cultureCode;

		public void FeedDictionary(Dictionary<int, string> dictionary)
		{
			foreach (KeyValuePair<int, string> item in (Dictionary<int, string>)m_dataDictionary)
			{
				dictionary[item.Key] = item.Value;
			}
		}

		public void StarveDictionary(Dictionary<int, string> dictionary)
		{
			foreach (KeyValuePair<int, string> item in (Dictionary<int, string>)m_dataDictionary)
			{
				dictionary.Remove(item.Key);
			}
		}

		public bool TryGetValue(int id, out string value)
		{
			return ((Dictionary<int, string>)m_dataDictionary).TryGetValue(id, out value);
		}

		public TextCollection()
			: this()
		{
		}
	}
}
