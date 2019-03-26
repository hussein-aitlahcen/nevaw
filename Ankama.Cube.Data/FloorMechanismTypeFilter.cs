using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FloorMechanismTypeFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private FloorMechanismType m_floorType;

		public FloorMechanismType floorType => m_floorType;

		public override string ToString()
		{
			return $"{m_floorType}";
		}

		public static FloorMechanismTypeFilter FromJsonToken(JToken token)
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
			FloorMechanismTypeFilter floorMechanismTypeFilter = new FloorMechanismTypeFilter();
			floorMechanismTypeFilter.PopulateFromJson(jsonObject);
			return floorMechanismTypeFilter;
		}

		public static FloorMechanismTypeFilter FromJsonProperty(JObject jsonObject, string propertyName, FloorMechanismTypeFilter defaultValue = null)
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
			m_floorType = (FloorMechanismType)Serialization.JsonTokenValue<int>(jsonObject, "floorType", 1);
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			FloorMechanismType searchType = m_floorType;
			foreach (IEntity entity in entities)
			{
				FloorMechanismStatus floorMechanismStatus = entity as FloorMechanismStatus;
				if (floorMechanismStatus != null && ((FloorMechanismDefinition)floorMechanismStatus.definition).floorType == searchType)
				{
					yield return entity;
				}
			}
		}
	}
}
