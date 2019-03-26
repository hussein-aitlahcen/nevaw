using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class EffectApplicationDefinition : AbstractEffectApplicationDefinition
	{
		private ITargetSelector m_applicationTargetSelector;

		private List<EffectTrigger> m_applicationEndTriggers;

		private List<EffectTrigger> m_executionStartTriggers;

		private EffectCondition m_condition;

		public ITargetSelector applicationTargetSelector => m_applicationTargetSelector;

		public IReadOnlyList<EffectTrigger> applicationEndTriggers => m_applicationEndTriggers;

		public IReadOnlyList<EffectTrigger> executionStartTriggers => m_executionStartTriggers;

		public EffectCondition condition => m_condition;

		public override string ToString()
		{
			return string.Format("on {0} when {1}", applicationTargetSelector, string.Join(" or ", m_executionStartTriggers));
		}

		public new static EffectApplicationDefinition FromJsonToken(JToken token)
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
			EffectApplicationDefinition effectApplicationDefinition = new EffectApplicationDefinition();
			effectApplicationDefinition.PopulateFromJson(jsonObject);
			return effectApplicationDefinition;
		}

		public static EffectApplicationDefinition FromJsonProperty(JObject jsonObject, string propertyName, EffectApplicationDefinition defaultValue = null)
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
			m_applicationTargetSelector = ITargetSelectorUtils.FromJsonProperty(jsonObject, "applicationTargetSelector");
			JArray val = Serialization.JsonArray(jsonObject, "applicationEndTriggers");
			m_applicationEndTriggers = new List<EffectTrigger>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_applicationEndTriggers.Add(EffectTrigger.FromJsonToken(item));
				}
			}
			JArray val2 = Serialization.JsonArray(jsonObject, "executionStartTriggers");
			m_executionStartTriggers = new List<EffectTrigger>((val2 != null) ? val2.get_Count() : 0);
			if (val2 != null)
			{
				foreach (JToken item2 in val2)
				{
					m_executionStartTriggers.Add(EffectTrigger.FromJsonToken(item2));
				}
			}
			m_condition = EffectCondition.FromJsonProperty(jsonObject, "condition");
		}
	}
}
