using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class StealCaracEffectDefinition : EffectExecutionDefinition
	{
		private DynamicValue m_quantity;

		private ISingleCaracIdSelector m_caracSelector;

		private IEntitySelector m_from;

		public DynamicValue quantity => m_quantity;

		public ISingleCaracIdSelector caracSelector => m_caracSelector;

		public IEntitySelector from => m_from;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static StealCaracEffectDefinition FromJsonToken(JToken token)
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
			StealCaracEffectDefinition stealCaracEffectDefinition = new StealCaracEffectDefinition();
			stealCaracEffectDefinition.PopulateFromJson(jsonObject);
			return stealCaracEffectDefinition;
		}

		public static StealCaracEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, StealCaracEffectDefinition defaultValue = null)
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
			m_quantity = DynamicValue.FromJsonProperty(jsonObject, "quantity");
			m_caracSelector = ISingleCaracIdSelectorUtils.FromJsonProperty(jsonObject, "caracSelector");
			m_from = IEntitySelectorUtils.FromJsonProperty(jsonObject, "from");
		}
	}
}
