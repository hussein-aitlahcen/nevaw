using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SquadAvailability : IEditableContent
	{
		private Id<SquadDefinition> m_squad;

		private DataAvailability m_availability;

		public Id<SquadDefinition> squad => m_squad;

		public DataAvailability availability => m_availability;

		public override string ToString()
		{
			return $"{m_squad} {m_availability}";
		}

		public static SquadAvailability FromJsonToken(JToken token)
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
			SquadAvailability squadAvailability = new SquadAvailability();
			squadAvailability.PopulateFromJson(jsonObject);
			return squadAvailability;
		}

		public static SquadAvailability FromJsonProperty(JObject jsonObject, string propertyName, SquadAvailability defaultValue = null)
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
			m_squad = Serialization.JsonTokenIdValue<SquadDefinition>(jsonObject, "squad");
			m_availability = (DataAvailability)Serialization.JsonTokenValue<int>(jsonObject, "availability", 0);
		}
	}
}
