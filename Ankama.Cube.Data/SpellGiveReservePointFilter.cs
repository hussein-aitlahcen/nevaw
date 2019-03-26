using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpellGiveReservePointFilter : SpellFilter
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public new static SpellGiveReservePointFilter FromJsonToken(JToken token)
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
			SpellGiveReservePointFilter spellGiveReservePointFilter = new SpellGiveReservePointFilter();
			spellGiveReservePointFilter.PopulateFromJson(jsonObject);
			return spellGiveReservePointFilter;
		}

		public static SpellGiveReservePointFilter FromJsonProperty(JObject jsonObject, string propertyName, SpellGiveReservePointFilter defaultValue = null)
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

		public override bool Accept(int spellInstanceId, SpellDefinition spellDef)
		{
			IReadOnlyList<GaugeValue> gaugeToModifyOnSpellPlay = spellDef.gaugeToModifyOnSpellPlay;
			for (int i = 0; i < gaugeToModifyOnSpellPlay.Count; i++)
			{
				if (gaugeToModifyOnSpellPlay[i].element == CaracId.ReservePoints)
				{
					return true;
				}
			}
			return false;
		}
	}
}
