using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class MultipleCastTargetDefinition : IEditableContent, ICastTargetDefinition
	{
		private List<ISelectorForCast> m_selectors;

		public IReadOnlyList<ISelectorForCast> selectors => m_selectors;

		public int count => m_selectors.Count;

		public override string ToString()
		{
			return $"{m_selectors.Count} targets:\n - " + string.Join("\n - ", selectors);
		}

		public static MultipleCastTargetDefinition FromJsonToken(JToken token)
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
			MultipleCastTargetDefinition multipleCastTargetDefinition = new MultipleCastTargetDefinition();
			multipleCastTargetDefinition.PopulateFromJson(jsonObject);
			return multipleCastTargetDefinition;
		}

		public static MultipleCastTargetDefinition FromJsonProperty(JObject jsonObject, string propertyName, MultipleCastTargetDefinition defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "selectors");
			m_selectors = new List<ISelectorForCast>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_selectors.Add(ISelectorForCastUtils.FromJsonToken(item));
				}
			}
		}

		public CastTargetContext CreateCastTargetContext(FightStatus fightStatus, int playerId, DynamicValueHolderType type, int definitionId, int level, int instanceId)
		{
			int count = m_selectors.Count;
			return new MultipleCastTargetContext(fightStatus, playerId, type, definitionId, level, instanceId, count);
		}

		public IEnumerable<Target> EnumerateTargets(CastTargetContext castTargetContext)
		{
			return m_selectors[castTargetContext.selectedTargetCount].EnumerateTargets(castTargetContext);
		}
	}
}
