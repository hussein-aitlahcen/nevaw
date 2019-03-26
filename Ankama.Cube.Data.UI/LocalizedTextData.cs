using Ankama.AssetManagement.AssetReferences;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data.UI
{
	public class LocalizedTextData : ScriptableObject
	{
		[Serializable]
		private class TextKeyDictionary : SerializableDictionary<int, string>
		{
		}

		[Serializable]
		private class TextCollectionDictionary : SerializableDictionaryLogic<string, AssetReference>
		{
			[SerializeField]
			private string[] m_keys = new string[0];

			[SerializeField]
			private AssetReference[] m_values = (AssetReference[])new AssetReference[0];

			protected override string[] m_keyArray
			{
				get
				{
					return m_keys;
				}
				set
				{
					m_keys = value;
				}
			}

			protected override AssetReference[] m_valueArray
			{
				get
				{
					return m_values;
				}
				set
				{
					m_values = value;
				}
			}
		}

		[UsedImplicitly]
		[SerializeField]
		private TextKeyDictionary m_textKeyDictionary = new TextKeyDictionary();

		[UsedImplicitly]
		[SerializeField]
		private TextCollectionDictionary m_textCollectionDictionary = new TextCollectionDictionary();

		private readonly Dictionary<string, int> m_textKeyNameDictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		public void Initialize()
		{
			m_textKeyNameDictionary.Clear();
			foreach (KeyValuePair<int, string> item in (Dictionary<int, string>)m_textKeyDictionary)
			{
				int key = item.Key;
				string value = item.Value;
				m_textKeyNameDictionary[value] = key;
			}
		}

		public bool TryGetTextCollectionReference(string textCollectionName, out AssetReference textCollectionReference)
		{
			return ((Dictionary<string, AssetReference>)m_textCollectionDictionary).TryGetValue(textCollectionName, out textCollectionReference);
		}

		public bool TryGetKeyId(string keyName, out int id)
		{
			return m_textKeyNameDictionary.TryGetValue(keyName, out id);
		}

		public bool TryGetKeyName(int id, out string keyName)
		{
			return ((Dictionary<int, string>)m_textKeyDictionary).TryGetValue(id, out keyName);
		}

		public LocalizedTextData()
			: this()
		{
		}
	}
}
