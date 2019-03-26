using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Utility;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CompanionDefinition : CharacterDefinition, ICastableDefinition, IDefinitionWithTooltip, IDefinitionWithDescription, IDefinitionWithPrecomputedData
	{
		private List<EventCategory> m_eventsInvalidatingCost;

		private List<EventCategory> m_eventsInvalidatingCasting;

		[LocalizedString("COMPANION_{id}_NAME", "Companions", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[LocalizedString("COMPANION_{id}_DESCRIPTION", "Companions", 3)]
		[SerializeField]
		private int m_i18nDescriptionId;

		private ICoordSelector m_spawnLocation;

		private List<Cost> m_cost;

		private List<SpellOnSpawnWithDestination> m_spells;

		private bool m_autoResurrect;

		[SerializeField]
		private AssetReference m_illustrationReference;

		public IReadOnlyList<EventCategory> eventsInvalidatingCost => m_eventsInvalidatingCost;

		public IReadOnlyList<EventCategory> eventsInvalidatingCasting => m_eventsInvalidatingCasting;

		public int i18nNameId => m_i18nNameId;

		public int i18nDescriptionId => m_i18nDescriptionId;

		public ICoordSelector spawnLocation => m_spawnLocation;

		public IReadOnlyList<Cost> cost => m_cost;

		public IReadOnlyList<SpellOnSpawnWithDestination> spells => m_spells;

		public bool autoResurrect => m_autoResurrect;

		public AssetReference illustrationReference => m_illustrationReference;

		public string illustrationBundleName => AssetBundlesUtility.GetUICharacterResourcesBundleName(this);

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_eventsInvalidatingCost = Serialization.JsonArrayAsList<EventCategory>(jsonObject, "eventsInvalidatingCost");
			m_eventsInvalidatingCasting = Serialization.JsonArrayAsList<EventCategory>(jsonObject, "eventsInvalidatingCasting");
			m_spawnLocation = ICoordSelectorUtils.FromJsonProperty(jsonObject, "spawnLocation");
			JArray val = Serialization.JsonArray(jsonObject, "cost");
			m_cost = new List<Cost>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_cost.Add(Cost.FromJsonToken(item));
				}
			}
			JArray val2 = Serialization.JsonArray(jsonObject, "spells");
			m_spells = new List<SpellOnSpawnWithDestination>((val2 != null) ? val2.get_Count() : 0);
			if (val2 != null)
			{
				foreach (JToken item2 in val2)
				{
					m_spells.Add(SpellOnSpawnWithDestination.FromJsonToken(item2));
				}
			}
			m_autoResurrect = Serialization.JsonTokenValue<bool>(jsonObject, "autoResurrect", false);
		}
	}
}
