using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.EntityAddedOrRemoved,
		EventCategory.EntityMoved
	})]
	public class ICoordFilterUtils
	{
		public static ICoordFilter FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class ICoordFilter");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			ICoordFilter coordFilter;
			switch (text)
			{
			case "NotCoordFilter":
				coordFilter = new NotCoordFilter();
				break;
			case "CellValidForCharacterFilter":
				coordFilter = new CellValidForCharacterFilter();
				break;
			case "CellValidForMechanismFilter":
				coordFilter = new CellValidForMechanismFilter();
				break;
			case "UnionOfCoordFilter":
				coordFilter = new UnionOfCoordFilter();
				break;
			case "FirstTargetsFilter":
				coordFilter = new FirstTargetsFilter();
				break;
			case "RandomTargetsFilter":
				coordFilter = new RandomTargetsFilter();
				break;
			case "AroundTargetFilter":
				coordFilter = new AroundTargetFilter();
				break;
			case "AroundSquaredTargetFilter":
				coordFilter = new AroundSquaredTargetFilter();
				break;
			case "UnionOfCoordOrEntityFilter":
				coordFilter = new UnionOfCoordOrEntityFilter();
				break;
			case "InLineTargetFilter":
				coordFilter = new InLineTargetFilter();
				break;
			case "InLineInOneDirectionTargetFilter":
				coordFilter = new InLineInOneDirectionTargetFilter();
				break;
			case "HasEmptyPathInLineToTargetFilter":
				coordFilter = new HasEmptyPathInLineToTargetFilter();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			coordFilter.PopulateFromJson(val);
			return coordFilter;
		}

		public static ICoordFilter FromJsonProperty(JObject jsonObject, string propertyName, ICoordFilter defaultValue = null)
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
