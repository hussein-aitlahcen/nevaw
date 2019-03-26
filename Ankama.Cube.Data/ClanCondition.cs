using Ankama.Cube.Fight.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ClanCondition : EffectCondition
	{
		private DynamicValue m_count;

		private static readonly FilteredEntitySelector selector = FilteredEntitySelector.From(OwnerFilter.sameAsCaster, new CharacterEntityFilter());

		public DynamicValue count => m_count;

		public override string ToString()
		{
			return $"Clan({m_count})";
		}

		public new static ClanCondition FromJsonToken(JToken token)
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
			ClanCondition clanCondition = new ClanCondition();
			clanCondition.PopulateFromJson(jsonObject);
			return clanCondition;
		}

		public static ClanCondition FromJsonProperty(JObject jsonObject, string propertyName, ClanCondition defaultValue = null)
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
			m_count = DynamicValue.FromJsonProperty(jsonObject, "count");
		}

		public override bool IsValid(DynamicValueContext context)
		{
			count.GetValue(context, out int value);
			int num = 0;
			foreach (IEntity item in selector.EnumerateEntities(context))
			{
				IEntity entity = item;
				if (num >= value)
				{
					return true;
				}
			}
			return false;
		}
	}
}
