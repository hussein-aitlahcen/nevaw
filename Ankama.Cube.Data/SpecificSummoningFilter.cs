using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpecificSummoningFilter : IEditableContent, ISpecificEntityFilter, IEntityFilter, ITargetFilter
	{
		private ShouldBeInOrNot m_condition;

		private List<Id<SummoningDefinition>> m_summonings;

		public ShouldBeInOrNot condition => m_condition;

		public IReadOnlyList<Id<SummoningDefinition>> summonings => m_summonings;

		public override string ToString()
		{
			switch (m_summonings.Count)
			{
			case 0:
				return "summoning is <unset>";
			case 1:
				return $"summoning {condition} {m_summonings[0]}";
			default:
				return $"summoning {condition} \n - " + string.Join("\n - ", m_summonings);
			}
		}

		public static SpecificSummoningFilter FromJsonToken(JToken token)
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
			SpecificSummoningFilter specificSummoningFilter = new SpecificSummoningFilter();
			specificSummoningFilter.PopulateFromJson(jsonObject);
			return specificSummoningFilter;
		}

		public static SpecificSummoningFilter FromJsonProperty(JObject jsonObject, string propertyName, SpecificSummoningFilter defaultValue = null)
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

		public void PopulateFromJson(JObject jsonObject)
		{
			m_condition = (ShouldBeInOrNot)Serialization.JsonTokenValue<int>(jsonObject, "condition", 1);
			JArray val = Serialization.JsonArray(jsonObject, "summonings");
			m_summonings = new List<Id<SummoningDefinition>>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item2 in val)
				{
					Id<SummoningDefinition> item = Serialization.JsonTokenIdValue<SummoningDefinition>(item2);
					m_summonings.Add(item);
				}
			}
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			foreach (IEntity entity in entities)
			{
				if (ValidFor(entity))
				{
					yield return entity;
				}
			}
		}

		public bool ValidFor(IEntity entity)
		{
			SummoningStatus summoningStatus;
			if ((summoningStatus = (entity as SummoningStatus)) == null)
			{
				return false;
			}
			bool flag = m_condition == ShouldBeInOrNot.ShouldBeIn;
			return Contains(summoningStatus.definition.get_id()) == flag;
		}

		private bool Contains(int id)
		{
			int count = m_summonings.Count;
			for (int i = 0; i < count; i++)
			{
				if (id == m_summonings[i].value)
				{
					return true;
				}
			}
			return false;
		}
	}
}
