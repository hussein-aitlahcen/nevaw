using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class TeleportEffectDefinition : EffectExecutionDefinition
	{
		private ISingleCoordSelector m_destination;

		private MovementType m_movementType;

		private bool m_allowTeleportOnTargetEntity;

		public ISingleCoordSelector destination => m_destination;

		public MovementType movementType => m_movementType;

		public bool allowTeleportOnTargetEntity => m_allowTeleportOnTargetEntity;

		public override string ToString()
		{
			return string.Format("TP {0} to {1}{2}", m_executionTargetSelector, destination, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static TeleportEffectDefinition FromJsonToken(JToken token)
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
			TeleportEffectDefinition teleportEffectDefinition = new TeleportEffectDefinition();
			teleportEffectDefinition.PopulateFromJson(jsonObject);
			return teleportEffectDefinition;
		}

		public static TeleportEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, TeleportEffectDefinition defaultValue = null)
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
			m_destination = ISingleCoordSelectorUtils.FromJsonProperty(jsonObject, "destination");
			m_movementType = (MovementType)Serialization.JsonTokenValue<int>(jsonObject, "movementType", 3);
			m_allowTeleportOnTargetEntity = Serialization.JsonTokenValue<bool>(jsonObject, "allowTeleportOnTargetEntity", false);
		}
	}
}
