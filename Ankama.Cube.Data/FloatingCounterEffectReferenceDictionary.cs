using Ankama.AssetManagement.AssetReferences;
using Ankama.Utilities;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public class FloatingCounterEffectReferenceDictionary : SerializableDictionaryLogic<CaracId, AssetReference>
	{
		[SerializeField]
		private CaracId[] m_keys = new CaracId[0];

		[SerializeField]
		private AssetReference[] m_values = (AssetReference[])new AssetReference[0];

		protected override CaracId[] m_keyArray
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
}
