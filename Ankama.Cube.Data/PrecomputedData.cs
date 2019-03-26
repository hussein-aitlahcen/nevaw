using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class PrecomputedData : IEditableContent
	{
		private List<ILevelOnlyDependant> m_dynamicValueReferences;

		private bool m_checkNumberOfSummonings;

		private bool m_checkNumberOfMechanisms;

		private static readonly KeywordReference[] NoKeywordReference = new KeywordReference[0];

		private KeywordReference[] m_keywordReferences;

		public IReadOnlyList<ILevelOnlyDependant> dynamicValueReferences => m_dynamicValueReferences;

		public bool checkNumberOfSummonings => m_checkNumberOfSummonings;

		public bool checkNumberOfMechanisms => m_checkNumberOfMechanisms;

		public KeywordReference[] keywordReferences => m_keywordReferences;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static PrecomputedData FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject jsonObject = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			PrecomputedData precomputedData = new PrecomputedData();
			precomputedData.PopulateFromJson(jsonObject);
			return precomputedData;
		}

		public static PrecomputedData FromJsonProperty(JObject jsonObject, string propertyName, PrecomputedData defaultValue = null)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			JProperty val = jsonObject.Property(propertyName);
			if (val == null || (int)val.get_Value().get_Type() == 10)
			{
				return defaultValue;
			}
			return FromJsonToken(val.get_Value());
		}

		public void PopulateFromJson(JObject jsonObject)
		{
			JArray val = Serialization.JsonArray(jsonObject, "dynamicValueReferences");
			m_dynamicValueReferences = new List<ILevelOnlyDependant>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_dynamicValueReferences.Add(ILevelOnlyDependantUtils.FromJsonToken(item));
				}
			}
			m_checkNumberOfSummonings = Serialization.JsonTokenValue<bool>(jsonObject, "checkNumberOfSummonings", false);
			m_checkNumberOfMechanisms = Serialization.JsonTokenValue<bool>(jsonObject, "checkNumberOfMechanisms", false);
			AdditionalPopulateFromJson(jsonObject);
		}

		private void AdditionalPopulateFromJson(JObject jsonObject)
		{
			JArray val = Serialization.JsonArray(jsonObject, "keywordReferences");
			int num = (val != null) ? val.get_Count() : 0;
			if (num > 0)
			{
				m_keywordReferences = new KeywordReference[num];
				for (int i = 0; i < num; i++)
				{
					m_keywordReferences[i] = KeywordReference.FromJson(val.get_Item(i));
				}
			}
			else
			{
				m_keywordReferences = NoKeywordReference;
			}
		}
	}
}
