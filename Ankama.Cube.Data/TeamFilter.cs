using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class TeamFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private bool m_isIdentical;

		private ISingleEntitySelector m_reference;

		public bool isIdentical => m_isIdentical;

		public ISingleEntitySelector reference => m_reference;

		public override string ToString()
		{
			return (m_isIdentical ? "" : "not ") + $"same team as {m_reference}";
		}

		public static TeamFilter FromJsonToken(JToken token)
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
			TeamFilter teamFilter = new TeamFilter();
			teamFilter.PopulateFromJson(jsonObject);
			return teamFilter;
		}

		public static TeamFilter FromJsonProperty(JObject jsonObject, string propertyName, TeamFilter defaultValue = null)
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
			m_isIdentical = Serialization.JsonTokenValue<bool>(jsonObject, "isIdentical", false);
			m_reference = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "reference");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			if (m_reference.TryGetEntity(context, out IEntityWithTeam entity))
			{
				int referenceTeamIndex = entity.teamIndex;
				foreach (IEntity entity2 in entities)
				{
					IEntityWithTeam entityWithTeam = entity2 as IEntityWithTeam;
					if (entityWithTeam != null && entityWithTeam.teamIndex == referenceTeamIndex == isIdentical)
					{
						yield return entity2;
					}
				}
			}
		}
	}
}
