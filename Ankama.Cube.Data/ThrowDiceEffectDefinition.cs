using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ThrowDiceEffectDefinition : EffectExecutionDefinition
	{
		private DynamicValue m_dice;

		private ISingleEntitySelector m_thrower;

		public DynamicValue dice => m_dice;

		public ISingleEntitySelector thrower => m_thrower;

		public override string ToString()
		{
			return string.Format("Throw dice {0}{1}", dice, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static ThrowDiceEffectDefinition FromJsonToken(JToken token)
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
			ThrowDiceEffectDefinition throwDiceEffectDefinition = new ThrowDiceEffectDefinition();
			throwDiceEffectDefinition.PopulateFromJson(jsonObject);
			return throwDiceEffectDefinition;
		}

		public static ThrowDiceEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, ThrowDiceEffectDefinition defaultValue = null)
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
			m_dice = DynamicValue.FromJsonProperty(jsonObject, "dice");
			m_thrower = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "thrower");
		}
	}
}
