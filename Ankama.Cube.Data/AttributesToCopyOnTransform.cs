using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class AttributesToCopyOnTransform : IEditableContent
	{
		private bool m_life;

		private bool m_armor;

		private bool m_elementaryState;

		private bool m_alreadyActioned;

		public bool life => m_life;

		public bool armor => m_armor;

		public bool elementaryState => m_elementaryState;

		public bool alreadyActioned => m_alreadyActioned;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static AttributesToCopyOnTransform FromJsonToken(JToken token)
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
			AttributesToCopyOnTransform attributesToCopyOnTransform = new AttributesToCopyOnTransform();
			attributesToCopyOnTransform.PopulateFromJson(jsonObject);
			return attributesToCopyOnTransform;
		}

		public static AttributesToCopyOnTransform FromJsonProperty(JObject jsonObject, string propertyName, AttributesToCopyOnTransform defaultValue = null)
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
			m_life = Serialization.JsonTokenValue<bool>(jsonObject, "life", false);
			m_armor = Serialization.JsonTokenValue<bool>(jsonObject, "armor", false);
			m_elementaryState = Serialization.JsonTokenValue<bool>(jsonObject, "elementaryState", false);
			m_alreadyActioned = Serialization.JsonTokenValue<bool>(jsonObject, "alreadyActioned", false);
		}
	}
}
