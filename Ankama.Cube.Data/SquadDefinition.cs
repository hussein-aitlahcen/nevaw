using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SquadDefinition : EditableData
	{
		private Id<WeaponDefinition> m_weapon;

		private Gender m_gender;

		private int m_level;

		private List<Id<CompanionDefinition>> m_companions;

		private List<Id<SpellDefinition>> m_spells;

		public Id<WeaponDefinition> weapon => m_weapon;

		public Gender gender => m_gender;

		public int level => m_level;

		public IReadOnlyList<Id<CompanionDefinition>> companions => m_companions;

		public IReadOnlyList<Id<SpellDefinition>> spells => m_spells;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
			m_weapon = Serialization.JsonTokenIdValue<WeaponDefinition>(jsonObject, "weapon");
			m_gender = (Gender)Serialization.JsonTokenValue<int>(jsonObject, "gender", 0);
			m_level = Serialization.JsonTokenValue<int>(jsonObject, "level", 0);
			JArray val = Serialization.JsonArray(jsonObject, "companions");
			m_companions = new List<Id<CompanionDefinition>>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item3 in val)
				{
					Id<CompanionDefinition> item = Serialization.JsonTokenIdValue<CompanionDefinition>(item3);
					m_companions.Add(item);
				}
			}
			JArray val2 = Serialization.JsonArray(jsonObject, "spells");
			m_spells = new List<Id<SpellDefinition>>((val2 != null) ? val2.get_Count() : 0);
			if (val2 != null)
			{
				foreach (JToken item4 in val2)
				{
					Id<SpellDefinition> item2 = Serialization.JsonTokenIdValue<SpellDefinition>(item4);
					m_spells.Add(item2);
				}
			}
		}

		public SquadDefinition()
			: this()
		{
		}
	}
}
