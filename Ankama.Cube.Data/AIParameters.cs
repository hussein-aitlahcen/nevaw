using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class AIParameters : IEditableContent
	{
		private int m_playingOrder;

		private IAIActionTargets m_actionTargets;

		private AIMovementDefinition m_moveIfNoValidTarget;

		private AIMovementDefinition m_moveIfNoTarget;

		public int playingOrder => m_playingOrder;

		public IAIActionTargets actionTargets => m_actionTargets;

		public AIMovementDefinition moveIfNoValidTarget => m_moveIfNoValidTarget;

		public AIMovementDefinition moveIfNoTarget => m_moveIfNoTarget;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static AIParameters FromJsonToken(JToken token)
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
			AIParameters aIParameters = new AIParameters();
			aIParameters.PopulateFromJson(jsonObject);
			return aIParameters;
		}

		public static AIParameters FromJsonProperty(JObject jsonObject, string propertyName, AIParameters defaultValue = null)
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
			m_playingOrder = Serialization.JsonTokenValue<int>(jsonObject, "playingOrder", 0);
			m_actionTargets = IAIActionTargetsUtils.FromJsonProperty(jsonObject, "actionTargets");
			m_moveIfNoValidTarget = AIMovementDefinition.FromJsonProperty(jsonObject, "moveIfNoValidTarget");
			m_moveIfNoTarget = AIMovementDefinition.FromJsonProperty(jsonObject, "moveIfNoTarget");
		}
	}
}
