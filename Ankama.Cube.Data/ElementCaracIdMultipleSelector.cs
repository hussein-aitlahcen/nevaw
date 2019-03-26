using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ElementCaracIdMultipleSelector : IEditableContent, ICaracIdSelector
	{
		private List<CaracId> m_elements;

		public IReadOnlyList<CaracId> elements => m_elements;

		public override string ToString()
		{
			return "(" + string.Join(", ", m_elements) + ")";
		}

		public static ElementCaracIdMultipleSelector FromJsonToken(JToken token)
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
			ElementCaracIdMultipleSelector elementCaracIdMultipleSelector = new ElementCaracIdMultipleSelector();
			elementCaracIdMultipleSelector.PopulateFromJson(jsonObject);
			return elementCaracIdMultipleSelector;
		}

		public static ElementCaracIdMultipleSelector FromJsonProperty(JObject jsonObject, string propertyName, ElementCaracIdMultipleSelector defaultValue = null)
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
			m_elements = Serialization.JsonArrayAsList<CaracId>(jsonObject, "elements");
		}
	}
}
