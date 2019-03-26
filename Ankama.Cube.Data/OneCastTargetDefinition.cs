using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class OneCastTargetDefinition : IEditableContent, ICastTargetDefinition
	{
		private ISelectorForCast m_selector;

		public ISelectorForCast selector => m_selector;

		public int count => 1;

		public override string ToString()
		{
			return selector.ToString();
		}

		public static OneCastTargetDefinition FromJsonToken(JToken token)
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
			OneCastTargetDefinition oneCastTargetDefinition = new OneCastTargetDefinition();
			oneCastTargetDefinition.PopulateFromJson(jsonObject);
			return oneCastTargetDefinition;
		}

		public static OneCastTargetDefinition FromJsonProperty(JObject jsonObject, string propertyName, OneCastTargetDefinition defaultValue = null)
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
			m_selector = ISelectorForCastUtils.FromJsonProperty(jsonObject, "selector");
		}

		public CastTargetContext CreateCastTargetContext(FightStatus fightStatus, int playerId, DynamicValueHolderType type, int definitionId, int level, int instanceId)
		{
			return new OneCastTargetContext(fightStatus, playerId, type, definitionId, level, instanceId);
		}

		public IEnumerable<Target> EnumerateTargets(CastTargetContext castTargetContext)
		{
			return m_selector.EnumerateTargets(castTargetContext);
		}
	}
}
