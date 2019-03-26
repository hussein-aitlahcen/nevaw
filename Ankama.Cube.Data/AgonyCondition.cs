using Ankama.Cube.Fight.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.LifeArmorChanged
	})]
	public sealed class AgonyCondition : EffectCondition
	{
		private static readonly CasterHeroSelector selector = new CasterHeroSelector();

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static AgonyCondition FromJsonToken(JToken token)
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
			AgonyCondition agonyCondition = new AgonyCondition();
			agonyCondition.PopulateFromJson(jsonObject);
			return agonyCondition;
		}

		public static AgonyCondition FromJsonProperty(JObject jsonObject, string propertyName, AgonyCondition defaultValue = null)
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
		}

		public override bool IsValid(DynamicValueContext context)
		{
			if (selector.TryGetEntity(context, out IEntityWithLife entity))
			{
				return entity.life * 100 / entity.baseLife < RuntimeData.constantsDefinition.agonyThreshold;
			}
			return false;
		}
	}
}
