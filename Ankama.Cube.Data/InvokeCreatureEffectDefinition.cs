using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class InvokeCreatureEffectDefinition : EffectExecutionDefinition
	{
		private AbstractInvocationSelection m_selection;

		private ISingleEntitySelector m_invocationOwner;

		private bool m_canBeInvokedOnOtherEntity;

		public AbstractInvocationSelection selection => m_selection;

		public ISingleEntitySelector invocationOwner => m_invocationOwner;

		public bool canBeInvokedOnOtherEntity => m_canBeInvokedOnOtherEntity;

		public override string ToString()
		{
			return string.Format("Invoke {0}{1}", m_selection, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static InvokeCreatureEffectDefinition FromJsonToken(JToken token)
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
			InvokeCreatureEffectDefinition invokeCreatureEffectDefinition = new InvokeCreatureEffectDefinition();
			invokeCreatureEffectDefinition.PopulateFromJson(jsonObject);
			return invokeCreatureEffectDefinition;
		}

		public static InvokeCreatureEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, InvokeCreatureEffectDefinition defaultValue = null)
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
			m_selection = AbstractInvocationSelection.FromJsonProperty(jsonObject, "selection");
			m_invocationOwner = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "invocationOwner");
			m_canBeInvokedOnOtherEntity = Serialization.JsonTokenValue<bool>(jsonObject, "canBeInvokedOnOtherEntity", false);
		}
	}
}
