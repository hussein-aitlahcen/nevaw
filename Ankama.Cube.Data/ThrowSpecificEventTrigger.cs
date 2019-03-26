using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ThrowSpecificEventTrigger : EffectExecutionDefinition
	{
		private SpecificEventTrigger m_trigger;

		public SpecificEventTrigger trigger => m_trigger;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static ThrowSpecificEventTrigger FromJsonToken(JToken token)
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
			ThrowSpecificEventTrigger throwSpecificEventTrigger = new ThrowSpecificEventTrigger();
			throwSpecificEventTrigger.PopulateFromJson(jsonObject);
			return throwSpecificEventTrigger;
		}

		public static ThrowSpecificEventTrigger FromJsonProperty(JObject jsonObject, string propertyName, ThrowSpecificEventTrigger defaultValue = null)
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
			m_trigger = (SpecificEventTrigger)Serialization.JsonTokenValue<int>(jsonObject, "trigger", 0);
		}
	}
}
