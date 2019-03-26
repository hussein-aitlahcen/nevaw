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
	public sealed class SpellsInHandValue : DynamicValue
	{
		private OwnerFilter m_player;

		private bool m_countMissingSpells;

		public OwnerFilter player => m_player;

		public bool countMissingSpells => m_countMissingSpells;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static SpellsInHandValue FromJsonToken(JToken token)
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
			SpellsInHandValue spellsInHandValue = new SpellsInHandValue();
			spellsInHandValue.PopulateFromJson(jsonObject);
			return spellsInHandValue;
		}

		public static SpellsInHandValue FromJsonProperty(JObject jsonObject, string propertyName, SpellsInHandValue defaultValue = null)
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
			m_countMissingSpells = Serialization.JsonTokenValue<bool>(jsonObject, "countMissingSpells", false);
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			Debug.LogWarning((object)"Unable to compute SpellsInHandValue client-side. Invalid data ?");
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
