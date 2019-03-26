using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CharacterActionValue : DynamicValue
	{
		private ISingleEntitySelector m_entitySelector;

		private bool m_addRelatedBoost;

		public ISingleEntitySelector entitySelector => m_entitySelector;

		public bool addRelatedBoost => m_addRelatedBoost;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static CharacterActionValue FromJsonToken(JToken token)
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
			CharacterActionValue characterActionValue = new CharacterActionValue();
			characterActionValue.PopulateFromJson(jsonObject);
			return characterActionValue;
		}

		public static CharacterActionValue FromJsonProperty(JObject jsonObject, string propertyName, CharacterActionValue defaultValue = null)
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
			m_entitySelector = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "entitySelector");
			m_addRelatedBoost = Serialization.JsonTokenValue<bool>(jsonObject, "addRelatedBoost", false);
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			CharacterActionValueContext characterActionValueContext = context as CharacterActionValueContext;
			if (characterActionValueContext != null && characterActionValueContext.relatedCharacterActionValue.HasValue)
			{
				value = characterActionValueContext.relatedCharacterActionValue.Value;
				return true;
			}
			value = 0;
			return false;
		}
	}
}
