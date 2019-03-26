using Ankama.Cube.Extensions;
using Ankama.Cube.Fight.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class DistanceValue : DynamicValue
	{
		private ISingleEntitySelector m_start;

		private ISingleEntitySelector m_end;

		public ISingleEntitySelector start => m_start;

		public ISingleEntitySelector end => m_end;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static DistanceValue FromJsonToken(JToken token)
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
			DistanceValue distanceValue = new DistanceValue();
			distanceValue.PopulateFromJson(jsonObject);
			return distanceValue;
		}

		public static DistanceValue FromJsonProperty(JObject jsonObject, string propertyName, DistanceValue defaultValue = null)
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
			m_start = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "start");
			m_end = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "end");
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null && m_start.TryGetEntity(context, out IEntityWithBoardPresence entity) && m_end.TryGetEntity(context, out IEntityWithBoardPresence entity2))
			{
				Vector2Int refCoord = entity.area.refCoord;
				Vector2Int refCoord2 = entity2.area.refCoord;
				value = refCoord.DistanceTo(refCoord2);
				return true;
			}
			value = 0;
			return false;
		}
	}
}
