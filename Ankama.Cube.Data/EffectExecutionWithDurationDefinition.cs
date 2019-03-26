using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class EffectExecutionWithDurationDefinition : EffectExecutionDefinition
	{
		protected List<EffectTrigger> m_executionEndTriggers;

		public IReadOnlyList<EffectTrigger> executionEndTriggers => m_executionEndTriggers;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static EffectExecutionWithDurationDefinition FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject val = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			JToken val2 = default(JToken);
			if (!val.TryGetValue("type", ref val2))
			{
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class EffectExecutionWithDurationDefinition");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			EffectExecutionWithDurationDefinition effectExecutionWithDurationDefinition;
			switch (text)
			{
			case "StoppableCaracChangedEffectDefinition":
				effectExecutionWithDurationDefinition = new StoppableCaracChangedEffectDefinition();
				break;
			case "PropertyChangeEffectDefinition":
				effectExecutionWithDurationDefinition = new PropertyChangeEffectDefinition();
				break;
			case "SpellCostModifierEffect":
				effectExecutionWithDurationDefinition = new SpellCostModifierEffect();
				break;
			case "RegisterDamageProtectorEffectDefinition":
				effectExecutionWithDurationDefinition = new RegisterDamageProtectorEffectDefinition();
				break;
			case "ChangeEntitySkinEffectDefinition":
				effectExecutionWithDurationDefinition = new ChangeEntitySkinEffectDefinition();
				break;
			case "FloatingCounterModificationEffectDefinition":
				effectExecutionWithDurationDefinition = new FloatingCounterModificationEffectDefinition();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			effectExecutionWithDurationDefinition.PopulateFromJson(val);
			return effectExecutionWithDurationDefinition;
		}

		public static EffectExecutionWithDurationDefinition FromJsonProperty(JObject jsonObject, string propertyName, EffectExecutionWithDurationDefinition defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "executionEndTriggers");
			m_executionEndTriggers = new List<EffectTrigger>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_executionEndTriggers.Add(EffectTrigger.FromJsonToken(item));
				}
			}
		}
	}
}
