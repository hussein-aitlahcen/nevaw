using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.SpellsMoved
	})]
	public sealed class SpellsPlayedThisTurnValue : DynamicValue
	{
		private OwnerFilter m_player;

		private bool m_includingThisSpellCast;

		private bool m_countThisSpellOnly;

		public OwnerFilter player => m_player;

		public bool includingThisSpellCast => m_includingThisSpellCast;

		public bool countThisSpellOnly => m_countThisSpellOnly;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static SpellsPlayedThisTurnValue FromJsonToken(JToken token)
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
			SpellsPlayedThisTurnValue spellsPlayedThisTurnValue = new SpellsPlayedThisTurnValue();
			spellsPlayedThisTurnValue.PopulateFromJson(jsonObject);
			return spellsPlayedThisTurnValue;
		}

		public static SpellsPlayedThisTurnValue FromJsonProperty(JObject jsonObject, string propertyName, SpellsPlayedThisTurnValue defaultValue = null)
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
			m_player = OwnerFilter.FromJsonProperty(jsonObject, "player");
			m_includingThisSpellCast = Serialization.JsonTokenValue<bool>(jsonObject, "includingThisSpellCast", false);
			m_countThisSpellOnly = Serialization.JsonTokenValue<bool>(jsonObject, "countThisSpellOnly", false);
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			Debug.LogWarning((object)"Unable to compute SpellsPlayedThisTurnValue client-side. Invalid data ?");
			value = 0;
			return false;
		}

		public override bool ToString(DynamicValueContext context, out string value)
		{
			value = "0";
			return false;
		}
	}
}
