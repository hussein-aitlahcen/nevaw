using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class BossFightAdditionalDataDefinition : IEditableContent, IFightAdditionalDataDefinition
	{
		private int m_bossLife;

		public int bossLife => m_bossLife;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static BossFightAdditionalDataDefinition FromJsonToken(JToken token)
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
			BossFightAdditionalDataDefinition bossFightAdditionalDataDefinition = new BossFightAdditionalDataDefinition();
			bossFightAdditionalDataDefinition.PopulateFromJson(jsonObject);
			return bossFightAdditionalDataDefinition;
		}

		public static BossFightAdditionalDataDefinition FromJsonProperty(JObject jsonObject, string propertyName, BossFightAdditionalDataDefinition defaultValue = null)
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
			m_bossLife = Serialization.JsonTokenValue<int>(jsonObject, "bossLife", 2000);
		}
	}
}
