using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class RegisterDamageProtectorEffectDefinition : EffectExecutionWithDurationDefinition
	{
		private ISingleEntitySelector m_protector;

		private DynamicValue m_fixedProtectionValue;

		private DynamicValue m_damagePercentProtectionValue;

		public ISingleEntitySelector protector => m_protector;

		public DynamicValue fixedProtectionValue => m_fixedProtectionValue;

		public DynamicValue damagePercentProtectionValue => m_damagePercentProtectionValue;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static RegisterDamageProtectorEffectDefinition FromJsonToken(JToken token)
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
			RegisterDamageProtectorEffectDefinition registerDamageProtectorEffectDefinition = new RegisterDamageProtectorEffectDefinition();
			registerDamageProtectorEffectDefinition.PopulateFromJson(jsonObject);
			return registerDamageProtectorEffectDefinition;
		}

		public static RegisterDamageProtectorEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, RegisterDamageProtectorEffectDefinition defaultValue = null)
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
			m_protector = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "protector");
			m_fixedProtectionValue = DynamicValue.FromJsonProperty(jsonObject, "fixedProtectionValue");
			m_damagePercentProtectionValue = DynamicValue.FromJsonProperty(jsonObject, "damagePercentProtectionValue");
		}
	}
}
