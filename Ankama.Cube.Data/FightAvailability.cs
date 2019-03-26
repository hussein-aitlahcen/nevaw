using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FightAvailability : IEditableContent
	{
		private Id<FightDefinition> m_fight;

		private DataAvailability m_availability;

		public Id<FightDefinition> fight => m_fight;

		public DataAvailability availability => m_availability;

		public override string ToString()
		{
			return $"{m_fight} {m_availability}";
		}

		public static FightAvailability FromJsonToken(JToken token)
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
			FightAvailability fightAvailability = new FightAvailability();
			fightAvailability.PopulateFromJson(jsonObject);
			return fightAvailability;
		}

		public static FightAvailability FromJsonProperty(JObject jsonObject, string propertyName, FightAvailability defaultValue = null)
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
			m_fight = Serialization.JsonTokenIdValue<FightDefinition>(jsonObject, "fight");
			m_availability = (DataAvailability)Serialization.JsonTokenValue<int>(jsonObject, "availability", 0);
		}
	}
}
