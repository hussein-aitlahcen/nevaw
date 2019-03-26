using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class NotCondition : EffectCondition
	{
		private EffectCondition m_condition;

		public EffectCondition condition => m_condition;

		public override string ToString()
		{
			return $"not ({condition})";
		}

		public new static NotCondition FromJsonToken(JToken token)
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
			NotCondition notCondition = new NotCondition();
			notCondition.PopulateFromJson(jsonObject);
			return notCondition;
		}

		public static NotCondition FromJsonProperty(JObject jsonObject, string propertyName, NotCondition defaultValue = null)
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
			m_condition = EffectCondition.FromJsonProperty(jsonObject, "condition");
		}

		public override bool IsValid(DynamicValueContext context)
		{
			return !m_condition.IsValid(context);
		}
	}
}
