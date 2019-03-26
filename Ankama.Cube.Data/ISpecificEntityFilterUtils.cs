using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class ISpecificEntityFilterUtils
	{
		public static ISpecificEntityFilter FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject val = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			JToken val2 = default(JToken);
			if (!val.TryGetValue("type", ref val2))
			{
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class ISpecificEntityFilter");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			ISpecificEntityFilter specificEntityFilter;
			switch (text)
			{
			case "SpecificCompanionFilter":
				specificEntityFilter = new SpecificCompanionFilter();
				break;
			case "SpecificSummoningFilter":
				specificEntityFilter = new SpecificSummoningFilter();
				break;
			case "SpecificObjectMechanismFilter":
				specificEntityFilter = new SpecificObjectMechanismFilter();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			specificEntityFilter.PopulateFromJson(val);
			return specificEntityFilter;
		}

		public static ISpecificEntityFilter FromJsonProperty(JObject jsonObject, string propertyName, ISpecificEntityFilter defaultValue = null)
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
	}
}
