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
	public sealed class TwoCastTargetDefinition : IEditableContent, ICastTargetDefinition
	{
		private ISelectorForCast m_selector1;

		private ISelectorForCast m_selector2;

		public ISelectorForCast selector1 => m_selector1;

		public ISelectorForCast selector2 => m_selector2;

		public int count => 2;

		public override string ToString()
		{
			return $"{selector1} THEN\n {selector2}";
		}

		public static TwoCastTargetDefinition FromJsonToken(JToken token)
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
			TwoCastTargetDefinition twoCastTargetDefinition = new TwoCastTargetDefinition();
			twoCastTargetDefinition.PopulateFromJson(jsonObject);
			return twoCastTargetDefinition;
		}

		public static TwoCastTargetDefinition FromJsonProperty(JObject jsonObject, string propertyName, TwoCastTargetDefinition defaultValue = null)
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
			m_selector1 = ISelectorForCastUtils.FromJsonProperty(jsonObject, "selector1");
			m_selector2 = ISelectorForCastUtils.FromJsonProperty(jsonObject, "selector2");
		}

		public CastTargetContext CreateCastTargetContext(FightStatus fightStatus, int playerId, DynamicValueHolderType type, int definitionId, int level, int instanceId)
		{
			return new TwoCastTargetContext(fightStatus, playerId, type, definitionId, level, instanceId);
		}

		public IEnumerable<Target> EnumerateTargets(CastTargetContext castTargetContext)
		{
			return ((castTargetContext.selectedTargetCount == 0) ? m_selector1 : m_selector2).EnumerateTargets(castTargetContext);
		}
	}
}
