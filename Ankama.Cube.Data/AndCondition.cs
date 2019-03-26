using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class AndCondition : EffectCondition
	{
		private List<EffectCondition> m_conditions;

		public IReadOnlyList<EffectCondition> conditions => m_conditions;

		public override string ToString()
		{
			return "(" + string.Join(" and ", conditions) + ")";
		}

		public new static AndCondition FromJsonToken(JToken token)
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
			AndCondition andCondition = new AndCondition();
			andCondition.PopulateFromJson(jsonObject);
			return andCondition;
		}

		public static AndCondition FromJsonProperty(JObject jsonObject, string propertyName, AndCondition defaultValue = null)
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

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			JArray val = Serialization.JsonArray(jsonObject, "conditions");
			m_conditions = new List<EffectCondition>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_conditions.Add(EffectCondition.FromJsonToken(item));
				}
			}
		}

		public override bool IsValid(DynamicValueContext context)
		{
			for (int i = 0; i < m_conditions.Count; i++)
			{
				if (!m_conditions[i].IsValid(context))
				{
					return false;
				}
			}
			return true;
		}
	}
}
