using Ankama.AssetManagement.AssetReferences;
using Ankama.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public class SpellEffectReferenceDictionary : SerializableDictionaryLogic<SpellEffectKey, AssetReference>
	{
		[SerializeField]
		private SpellEffectKey[] m_keys = new SpellEffectKey[0];

		[SerializeField]
		private AssetReference[] m_values = (AssetReference[])new AssetReference[0];

		protected override SpellEffectKey[] m_keyArray
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

		public SpellEffectReferenceDictionary()
			: base((IEqualityComparer<SpellEffectKey>)SpellEffectKeyComparer.instance)
		{
		}
	}
}
