using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpawnCell : IEditableContent
	{
		[SerializeField]
		private Vector2Int m_coords;

		[SerializeField]
		private Direction m_orientation;

		public Vector2Int coords => m_coords;

		public Direction orientation => m_orientation;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static SpawnCell FromJsonToken(JToken token)
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
			SpawnCell spawnCell = new SpawnCell();
			spawnCell.PopulateFromJson(jsonObject);
			return spawnCell;
		}

		public static SpawnCell FromJsonProperty(JObject jsonObject, string propertyName, SpawnCell defaultValue = null)
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
		}
	}
}
