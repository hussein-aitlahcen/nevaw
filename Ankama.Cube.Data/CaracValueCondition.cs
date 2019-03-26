using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CaracValueCondition : EffectCondition
	{
		private ISingleEntitySelector m_selector;

		private CaracId m_carac;

		private ValueFilter m_value;

		public ISingleEntitySelector selector => m_selector;

		public CaracId carac => m_carac;

		public ValueFilter value => m_value;

		public override string ToString()
		{
			return $"{m_selector} has {m_carac} {m_value}";
		}

		public new static CaracValueCondition FromJsonToken(JToken token)
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
			CaracValueCondition caracValueCondition = new CaracValueCondition();
			caracValueCondition.PopulateFromJson(jsonObject);
			return caracValueCondition;
		}

		public static CaracValueCondition FromJsonProperty(JObject jsonObject, string propertyName, CaracValueCondition defaultValue = null)
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
			m_selector = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "selector");
			m_carac = (CaracId)Serialization.JsonTokenValue<int>(jsonObject, "carac", 3);
			m_value = ValueFilter.FromJsonProperty(jsonObject, "value");
		}

		public override bool IsValid(DynamicValueContext context)
		{
			IEntity entity = selector.EnumerateEntities(context).First();
			return value.Matches(entity.GetCarac(m_carac), context);
		}
	}
}
