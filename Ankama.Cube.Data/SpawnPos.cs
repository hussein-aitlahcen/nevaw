using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpawnPos : IEditableContent
	{
		private Vector2Int m_coords;

		private Direction m_direction;

		public Vector2Int coords => m_coords;

		public Direction direction => m_direction;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static SpawnPos FromJsonToken(JToken token)
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
			SpawnPos spawnPos = new SpawnPos();
			spawnPos.PopulateFromJson(jsonObject);
			return spawnPos;
		}

		public static SpawnPos FromJsonProperty(JObject jsonObject, string propertyName, SpawnPos defaultValue = null)
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
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			m_coords = Serialization.JsonTokenUnityValue(jsonObject, "coords", m_coords);
			m_direction = (Direction)Serialization.JsonTokenValue<int>(jsonObject, "direction", 0);
		}
	}
}
