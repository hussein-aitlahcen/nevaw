using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class ICoordSelectorUtils
	{
		public static ICoordSelector FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class ICoordSelector");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			ICoordSelector coordSelector;
			switch (text)
			{
			case "FirstCastTargetSelector":
				coordSelector = new FirstCastTargetSelector();
				break;
			case "SecondCastTargetSelector":
				coordSelector = new SecondCastTargetSelector();
				break;
			case "IndexedCastTargetSelector":
				coordSelector = new IndexedCastTargetSelector();
				break;
			case "TriggeringEventFirstCastTargetSelector":
				coordSelector = new TriggeringEventFirstCastTargetSelector();
				break;
			case "SingleCoordWithConditionSelector":
				coordSelector = new SingleCoordWithConditionSelector();
				break;
			case "CentralSymmetryCoordSelector":
				coordSelector = new CentralSymmetryCoordSelector();
				break;
			case "EntityPositionSelector":
				coordSelector = new EntityPositionSelector();
				break;
			case "UnionOfCoordsSelector":
				coordSelector = new UnionOfCoordsSelector();
				break;
			case "FilteredCoordSelector":
				coordSelector = new FilteredCoordSelector();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			coordSelector.PopulateFromJson(val);
			return coordSelector;
		}

		public static ICoordSelector FromJsonProperty(JObject jsonObject, string propertyName, ICoordSelector defaultValue = null)
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
