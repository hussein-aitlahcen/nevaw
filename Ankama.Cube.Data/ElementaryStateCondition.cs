using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ElementaryStateCondition : EffectCondition
	{
		private ISingleEntitySelector m_selector;

		private ElementaryStates m_elementaryState;

		public ISingleEntitySelector selector => m_selector;

		public ElementaryStates elementaryState => m_elementaryState;

		public override string ToString()
		{
			return $"{m_selector} is {m_elementaryState}";
		}

		public new static ElementaryStateCondition FromJsonToken(JToken token)
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
			ElementaryStateCondition elementaryStateCondition = new ElementaryStateCondition();
			elementaryStateCondition.PopulateFromJson(jsonObject);
			return elementaryStateCondition;
		}

		public static ElementaryStateCondition FromJsonProperty(JObject jsonObject, string propertyName, ElementaryStateCondition defaultValue = null)
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
			m_selector = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "selector");
			m_elementaryState = (ElementaryStates)Serialization.JsonTokenValue<int>(jsonObject, "elementaryState", 0);
		}

		public override bool IsValid(DynamicValueContext context)
		{
			return selector.EnumerateEntities(context).All(delegate(IEntity e)
			{
				IEntityWithElementaryState entityWithElementaryState = e as IEntityWithElementaryState;
				return (entityWithElementaryState != null && entityWithElementaryState.HasElementaryState(elementaryState)) ? true : false;
			});
		}
	}
}
