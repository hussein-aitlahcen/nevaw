using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CompanionAvailability : IEditableContent
	{
		private Id<CompanionDefinition> m_companion;

		private DataAvailability m_availability;

		public Id<CompanionDefinition> companion => m_companion;

		public DataAvailability availability => m_availability;

		public override string ToString()
		{
			return $"{m_companion} {m_availability}";
		}

		public static CompanionAvailability FromJsonToken(JToken token)
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
			CompanionAvailability companionAvailability = new CompanionAvailability();
			companionAvailability.PopulateFromJson(jsonObject);
			return companionAvailability;
		}

		public static CompanionAvailability FromJsonProperty(JObject jsonObject, string propertyName, CompanionAvailability defaultValue = null)
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
			m_companion = Serialization.JsonTokenIdValue<CompanionDefinition>(jsonObject, "companion");
			m_availability = (DataAvailability)Serialization.JsonTokenValue<int>(jsonObject, "availability", 0);
		}
	}
}
