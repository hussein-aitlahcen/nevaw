using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FightMapRegionDefinition : IEditableContent
	{
		[SerializeField]
		private Vector2Int m_sizeMin;

		[SerializeField]
		private Vector2Int m_sizeMax;

		[SerializeField]
		private SpawnCell[] m_spawnCells;

		public Vector2Int sizeMin => m_sizeMin;

		public Vector2Int sizeMax => m_sizeMax;

		public SpawnCell[] spawnCells => m_spawnCells;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static FightMapRegionDefinition FromJsonToken(JToken token)
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
			FightMapRegionDefinition fightMapRegionDefinition = new FightMapRegionDefinition();
			fightMapRegionDefinition.PopulateFromJson(jsonObject);
			return fightMapRegionDefinition;
		}

		public static FightMapRegionDefinition FromJsonProperty(JObject jsonObject, string propertyName, FightMapRegionDefinition defaultValue = null)
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
