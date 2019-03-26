using Ankama.AssetManagement.AssetReferences;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SummoningDefinition : CharacterDefinition, IDefinitionWithTooltip, IDefinitionWithDescription, IDefinitionWithPrecomputedData
	{
		[LocalizedString("SUMMONINGS_{id}_NAME", "Summonings", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[LocalizedString("SUMMONINGS_{id}_DESCRIPTION", "Summonings", 3)]
		[SerializeField]
		private int m_i18nDescriptionId;

		private List<Cost> m_cost;

		private SummonSelection m_growInto;

		[SerializeField]
		private AssetReference m_illustrationReference;

		public int i18nNameId => m_i18nNameId;

		public int i18nDescriptionId => m_i18nDescriptionId;

		public IReadOnlyList<Cost> cost => m_cost;

		public SummonSelection growInto => m_growInto;

		public AssetReference illustrationReference => m_illustrationReference;

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			JArray val = Serialization.JsonArray(jsonObject, "cost");
			m_cost = new List<Cost>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_cost.Add(Cost.FromJsonToken(item));
				}
			}
			m_growInto = SummonSelection.FromJsonProperty(jsonObject, "growInto");
		}
	}
}
