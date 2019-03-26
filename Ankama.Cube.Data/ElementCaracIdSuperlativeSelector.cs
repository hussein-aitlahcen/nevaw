using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ElementCaracIdSuperlativeSelector : IEditableContent, ICaracIdSelector
	{
		private OwnerFilter m_playerSelector;

		private Superlative m_selection;

		public OwnerFilter playerSelector => m_playerSelector;

		public Superlative selection => m_selection;

		public override string ToString()
		{
			return $"Elements with {m_selection} value for {m_playerSelector}";
		}

		public static ElementCaracIdSuperlativeSelector FromJsonToken(JToken token)
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
			ElementCaracIdSuperlativeSelector elementCaracIdSuperlativeSelector = new ElementCaracIdSuperlativeSelector();
			elementCaracIdSuperlativeSelector.PopulateFromJson(jsonObject);
			return elementCaracIdSuperlativeSelector;
		}

		public static ElementCaracIdSuperlativeSelector FromJsonProperty(JObject jsonObject, string propertyName, ElementCaracIdSuperlativeSelector defaultValue = null)
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
			m_playerSelector = OwnerFilter.FromJsonProperty(jsonObject, "playerSelector");
			m_selection = (Superlative)Serialization.JsonTokenValue<int>(jsonObject, "selection", 1);
		}
	}
}
