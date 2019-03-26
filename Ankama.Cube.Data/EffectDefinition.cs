using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class EffectDefinition : AbstractEffectDefinition
	{
		private AbstractEffectApplicationDefinition m_application;

		private AbstractEffectExecutionDefinition m_execution;

		public AbstractEffectApplicationDefinition application => m_application;

		public AbstractEffectExecutionDefinition execution => m_execution;

		public override string ToString()
		{
			if (m_execution.ToString().Length != 0)
			{
				return m_execution.ToString();
			}
			return "[[" + m_execution.GetType().Name.Replace("EffectDefinition", "") + "]]";
		}

		public new static EffectDefinition FromJsonToken(JToken token)
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
			EffectDefinition effectDefinition = new EffectDefinition();
			effectDefinition.PopulateFromJson(jsonObject);
			return effectDefinition;
		}

		public static EffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, EffectDefinition defaultValue = null)
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
			m_application = AbstractEffectApplicationDefinition.FromJsonProperty(jsonObject, "application");
			m_execution = AbstractEffectExecutionDefinition.FromJsonProperty(jsonObject, "execution");
		}
	}
}
