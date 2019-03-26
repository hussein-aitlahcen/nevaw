using Ankama.Cube.Fight.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class UniqueCondition : EffectCondition
	{
		private ISpecificEntityFilter m_specificEntity;

		private static readonly FilteredEntitySelector selector = FilteredEntitySelector.From(OwnerFilter.sameAsCaster);

		public ISpecificEntityFilter specificEntity => m_specificEntity;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static UniqueCondition FromJsonToken(JToken token)
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
			UniqueCondition uniqueCondition = new UniqueCondition();
			uniqueCondition.PopulateFromJson(jsonObject);
			return uniqueCondition;
		}

		public static UniqueCondition FromJsonProperty(JObject jsonObject, string propertyName, UniqueCondition defaultValue = null)
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
			m_specificEntity = ISpecificEntityFilterUtils.FromJsonProperty(jsonObject, "specificEntity");
		}

		public override bool IsValid(DynamicValueContext context)
		{
			foreach (IEntity item in selector.EnumerateEntities(context))
			{
				if (m_specificEntity.ValidFor(item))
				{
					return false;
				}
			}
			return true;
		}
	}
}
