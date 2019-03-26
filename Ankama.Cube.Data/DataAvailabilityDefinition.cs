using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class DataAvailabilityDefinition : EditableData
	{
		private List<GodAvailability> m_gods;

		private List<FightAvailability> m_fights;

		private List<SquadAvailability> m_squads;

		private List<CompanionAvailability> m_companions;

		private static DataAvailabilityDefinition s_instance;

		public IReadOnlyList<GodAvailability> gods => m_gods;

		public IReadOnlyList<FightAvailability> fights => m_fights;

		public IReadOnlyList<SquadAvailability> squads => m_squads;

		public IReadOnlyList<CompanionAvailability> companions => m_companions;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
			JArray val = Serialization.JsonArray(jsonObject, "gods");
			m_gods = new List<GodAvailability>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_gods.Add(GodAvailability.FromJsonToken(item));
				}
			}
			JArray val2 = Serialization.JsonArray(jsonObject, "fights");
			m_fights = new List<FightAvailability>((val2 != null) ? val2.get_Count() : 0);
			if (val2 != null)
			{
				foreach (JToken item2 in val2)
				{
					m_fights.Add(FightAvailability.FromJsonToken(item2));
				}
			}
			JArray val3 = Serialization.JsonArray(jsonObject, "squads");
			m_squads = new List<SquadAvailability>((val3 != null) ? val3.get_Count() : 0);
			if (val3 != null)
			{
				foreach (JToken item3 in val3)
				{
					m_squads.Add(SquadAvailability.FromJsonToken(item3));
				}
			}
			JArray val4 = Serialization.JsonArray(jsonObject, "companions");
			m_companions = new List<CompanionAvailability>((val4 != null) ? val4.get_Count() : 0);
			if (val4 != null)
			{
				foreach (JToken item4 in val4)
				{
					m_companions.Add(CompanionAvailability.FromJsonToken(item4));
				}
			}
		}

		private void OnEnable()
		{
			s_instance = this;
		}

		public static DataAvailability GetAvailability(God god)
		{
			foreach (GodAvailability god2 in s_instance.gods)
			{
				if (god2.god == god)
				{
					return god2.availability;
				}
			}
			return DataAvailability.NotUsed;
		}

		public static DataAvailability GetAvailability(FightDefinition fight)
		{
			foreach (FightAvailability fight2 in s_instance.fights)
			{
				if (fight2.fight.value == fight.get_id())
				{
					return fight2.availability;
				}
			}
			return DataAvailability.NotUsed;
		}

		public static DataAvailability GetAvailability(SquadDefinition squad)
		{
			foreach (SquadAvailability squad2 in s_instance.squads)
			{
				if (squad2.squad.value == squad.get_id())
				{
					return squad2.availability;
				}
			}
			return DataAvailability.NotUsed;
		}

		public static DataAvailability GetAvailability(CompanionDefinition companion)
		{
			foreach (CompanionAvailability companion2 in s_instance.companions)
			{
				if (companion2.companion.value == companion.get_id())
				{
					return companion2.availability;
				}
			}
			return DataAvailability.NotUsed;
		}

		public DataAvailabilityDefinition()
			: this()
		{
		}
	}
}
