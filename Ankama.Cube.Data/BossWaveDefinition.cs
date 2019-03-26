using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class BossWaveDefinition : IEditableContent
	{
		private List<SpawnEnemy> m_spawn;

		public IReadOnlyList<SpawnEnemy> spawn => m_spawn;

		public override string ToString()
		{
			return base.ToString();
		}

		public static BossWaveDefinition FromJsonToken(JToken token)
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
			BossWaveDefinition bossWaveDefinition = new BossWaveDefinition();
			bossWaveDefinition.PopulateFromJson(jsonObject);
			return bossWaveDefinition;
		}

		public static BossWaveDefinition FromJsonProperty(JObject jsonObject, string propertyName, BossWaveDefinition defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "spawn");
			m_spawn = new List<SpawnEnemy>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_spawn.Add(SpawnEnemy.FromJsonToken(item));
				}
			}
		}
	}
}
