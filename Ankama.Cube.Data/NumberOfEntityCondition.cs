using Ankama.Cube.Fight.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class NumberOfEntityCondition : EffectCondition
	{
		private FilteredEntitySelector m_selector;

		private ValueFilter m_value;

		public FilteredEntitySelector selector => m_selector;

		public ValueFilter value => m_value;

		public override string ToString()
		{
			return $"number of entities {m_value}";
		}

		public new static NumberOfEntityCondition FromJsonToken(JToken token)
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
			NumberOfEntityCondition numberOfEntityCondition = new NumberOfEntityCondition();
			numberOfEntityCondition.PopulateFromJson(jsonObject);
			return numberOfEntityCondition;
		}

		public static NumberOfEntityCondition FromJsonProperty(JObject jsonObject, string propertyName, NumberOfEntityCondition defaultValue = null)
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
			m_selector = FilteredEntitySelector.FromJsonProperty(jsonObject, "selector");
			m_value = ValueFilter.FromJsonProperty(jsonObject, "value");
		}

		public override bool IsValid(DynamicValueContext context)
		{
			int num = 0;
			foreach (IEntity item in selector.EnumerateEntities(context))
			{
				IEntity entity = item;
				num++;
			}
			return value.Matches(num, context);
		}
	}
}
