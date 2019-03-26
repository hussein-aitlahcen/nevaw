using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class BossEvolutionWaveDefinition : IEditableContent
	{
		private int m_evolutionStep;

		private BossWaveDefinition m_wave;

		private bool m_resetEvolution;

		public int evolutionStep => m_evolutionStep;

		public BossWaveDefinition wave => m_wave;

		public bool resetEvolution => m_resetEvolution;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static BossEvolutionWaveDefinition FromJsonToken(JToken token)
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
			BossEvolutionWaveDefinition bossEvolutionWaveDefinition = new BossEvolutionWaveDefinition();
			bossEvolutionWaveDefinition.PopulateFromJson(jsonObject);
			return bossEvolutionWaveDefinition;
		}

		public static BossEvolutionWaveDefinition FromJsonProperty(JObject jsonObject, string propertyName, BossEvolutionWaveDefinition defaultValue = null)
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
			m_evolutionStep = Serialization.JsonTokenValue<int>(jsonObject, "evolutionStep", 1);
			m_wave = BossWaveDefinition.FromJsonProperty(jsonObject, "wave");
			m_resetEvolution = Serialization.JsonTokenValue<bool>(jsonObject, "resetEvolution", true);
		}
	}
}
