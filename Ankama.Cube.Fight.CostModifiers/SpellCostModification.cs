using Ankama.Cube.Data;
using Ankama.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ankama.Cube.Fight.CostModifiers
{
	public class SpellCostModification
	{
		public readonly int id;

		public readonly int modificationValue;

		private readonly SpellFilter[] m_filters;

		public SpellCostModification(int id, int modificationValue, string spellFilterJson)
		{
			this.id = id;
			this.modificationValue = modificationValue;
			m_filters = DeserializeSpellFilters(spellFilterJson);
		}

		public bool Accept(int spellInstanceId, SpellDefinition spellDefinition)
		{
			if (m_filters == null || m_filters.Length == 0)
			{
				return true;
			}
			for (int i = 0; i < m_filters.Length; i++)
			{
				if (!m_filters[i].Accept(spellInstanceId, spellDefinition))
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			string text = (m_filters == null) ? string.Empty : string.Join("\n", from f in m_filters
				select f.ToString());
			return string.Format("{0}: {1}, {2}, {3}: {4}", "id", id, ToStringExtensions.ToStringSigned(modificationValue), "m_filters", text);
		}

		private static SpellFilter[] DeserializeSpellFilters(string valueJson)
		{
			try
			{
				JArray val = JsonConvert.DeserializeObject(valueJson) as JArray;
				int count = val.get_Count();
				SpellFilter[] array = new SpellFilter[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = SpellFilter.FromJsonToken(val.get_Item(i));
				}
				return array;
			}
			catch (Exception ex)
			{
				Log.Error("Cannot deserialize " + valueJson, (object)ex, 77, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\CostModifiers\\SpellCostModification.cs");
				return null;
			}
		}

		public static int ApplyCostModification(List<SpellCostModification> modifications, int cost, SpellDefinition spellDefinition, CastTargetContext context)
		{
			int count = modifications.Count;
			for (int i = 0; i < count; i++)
			{
				SpellCostModification spellCostModification = modifications[i];
				if (spellCostModification.Accept(context.instanceId, spellDefinition))
				{
					cost += spellCostModification.modificationValue;
				}
			}
			return Math.Max(0, cost);
		}
	}
}
