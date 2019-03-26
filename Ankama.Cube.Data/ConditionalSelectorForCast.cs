using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ConditionalSelectorForCast : IEditableContent, ITargetSelector, ISelectorForCast
	{
		private EffectCondition m_condition;

		private ISelectorForCast m_selectorIfTrue;

		private ISelectorForCast m_selectorIfFalse;

		public EffectCondition condition => m_condition;

		public ISelectorForCast selectorIfTrue => m_selectorIfTrue;

		public ISelectorForCast selectorIfFalse => m_selectorIfFalse;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static ConditionalSelectorForCast FromJsonToken(JToken token)
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
			ConditionalSelectorForCast conditionalSelectorForCast = new ConditionalSelectorForCast();
			conditionalSelectorForCast.PopulateFromJson(jsonObject);
			return conditionalSelectorForCast;
		}

		public static ConditionalSelectorForCast FromJsonProperty(JObject jsonObject, string propertyName, ConditionalSelectorForCast defaultValue = null)
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
			m_condition = EffectCondition.FromJsonProperty(jsonObject, "condition");
			m_selectorIfTrue = ISelectorForCastUtils.FromJsonProperty(jsonObject, "selectorIfTrue");
			m_selectorIfFalse = ISelectorForCastUtils.FromJsonProperty(jsonObject, "selectorIfFalse");
		}

		public IEnumerable<Target> EnumerateTargets(DynamicValueContext context)
		{
			ISelectorForCast selectorForCast = m_condition.IsValid(context) ? m_selectorIfTrue : m_selectorIfFalse;
			if (selectorForCast == null)
			{
				return Empty();
			}
			return selectorForCast.EnumerateTargets(context);
		}

		private static IEnumerable<Target> Empty()
		{
			yield break;
		}
	}
}
