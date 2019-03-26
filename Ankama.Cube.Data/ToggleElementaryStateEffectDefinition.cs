using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ToggleElementaryStateEffectDefinition : EffectExecutionDefinition
	{
		private ElementaryStates m_elementaryState;

		public ElementaryStates elementaryState => m_elementaryState;

		public override string ToString()
		{
			return string.Format("Toggle state {0}{1}", m_elementaryState, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static ToggleElementaryStateEffectDefinition FromJsonToken(JToken token)
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
			ToggleElementaryStateEffectDefinition toggleElementaryStateEffectDefinition = new ToggleElementaryStateEffectDefinition();
			toggleElementaryStateEffectDefinition.PopulateFromJson(jsonObject);
			return toggleElementaryStateEffectDefinition;
		}

		public static ToggleElementaryStateEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, ToggleElementaryStateEffectDefinition defaultValue = null)
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
			m_elementaryState = (ElementaryStates)Serialization.JsonTokenValue<int>(jsonObject, "elementaryState", 0);
		}
	}
}
