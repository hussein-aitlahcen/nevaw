using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ChargeEffectDefinition : EffectExecutionDefinition
	{
		private ISingleEntitySelector m_direction;

		private DynamicValue m_cellCount;

		private DynamicValue m_attackValue;

		private DynamicValue m_attackBoostByCell;

		public ISingleEntitySelector direction => m_direction;

		public DynamicValue cellCount => m_cellCount;

		public DynamicValue attackValue => m_attackValue;

		public DynamicValue attackBoostByCell => m_attackBoostByCell;

		public override string ToString()
		{
			return string.Format("Charge up to {0} cells and attacks{1}", cellCount, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static ChargeEffectDefinition FromJsonToken(JToken token)
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
			ChargeEffectDefinition chargeEffectDefinition = new ChargeEffectDefinition();
			chargeEffectDefinition.PopulateFromJson(jsonObject);
			return chargeEffectDefinition;
		}

		public static ChargeEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, ChargeEffectDefinition defaultValue = null)
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
			m_direction = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "direction");
			m_cellCount = DynamicValue.FromJsonProperty(jsonObject, "cellCount");
			m_attackValue = DynamicValue.FromJsonProperty(jsonObject, "attackValue");
			m_attackBoostByCell = DynamicValue.FromJsonProperty(jsonObject, "attackBoostByCell");
		}
	}
}
