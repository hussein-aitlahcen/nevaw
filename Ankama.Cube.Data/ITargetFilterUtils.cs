using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class ITargetFilterUtils
	{
		public static ITargetFilter FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class ITargetFilter");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			ITargetFilter targetFilter;
			switch (text)
			{
			case "NotEntityFilter":
				targetFilter = new NotEntityFilter();
				break;
			case "NotCoordFilter":
				targetFilter = new NotCoordFilter();
				break;
			case "FirstTargetsFilter":
				targetFilter = new FirstTargetsFilter();
				break;
			case "RandomTargetsFilter":
				targetFilter = new RandomTargetsFilter();
				break;
			case "EntitiesWithHighestLowestCaracFilter":
				targetFilter = new EntitiesWithHighestLowestCaracFilter();
				break;
			case "CombinedEntityFilter":
				targetFilter = new CombinedEntityFilter();
				break;
			case "EffectHolderFilter":
				targetFilter = new EffectHolderFilter();
				break;
			case "ApplicationHolderFilter":
				targetFilter = new ApplicationHolderFilter();
				break;
			case "CastTargetFilter":
				targetFilter = new CastTargetFilter();
				break;
			case "LifeFilter":
				targetFilter = new LifeFilter();
				break;
			case "ArmorFilter":
				targetFilter = new ArmorFilter();
				break;
			case "RangeFilter":
				targetFilter = new RangeFilter();
				break;
			case "PropertyFilter":
				targetFilter = new PropertyFilter();
				break;
			case "PropertiesFilter":
				targetFilter = new PropertiesFilter();
				break;
			case "EntityValidForMagicalDamageFilter":
				targetFilter = new EntityValidForMagicalDamageFilter();
				break;
			case "EntityValidForMagicalHealFilter":
				targetFilter = new EntityValidForMagicalHealFilter();
				break;
			case "EntityValidForPhysicalDamageFilter":
				targetFilter = new EntityValidForPhysicalDamageFilter();
				break;
			case "EntityValidForPhysicalHealFilter":
				targetFilter = new EntityValidForPhysicalHealFilter();
				break;
			case "EntityTypeFilter":
				targetFilter = new EntityTypeFilter();
				break;
			case "SpecificCompanionFilter":
				targetFilter = new SpecificCompanionFilter();
				break;
			case "SpecificSummoningFilter":
				targetFilter = new SpecificSummoningFilter();
				break;
			case "SpecificObjectMechanismFilter":
				targetFilter = new SpecificObjectMechanismFilter();
				break;
			case "CanGrowEntityFilter":
				targetFilter = new CanGrowEntityFilter();
				break;
			case "CharacterEntityFilter":
				targetFilter = new CharacterEntityFilter();
				break;
			case "OsamodasAnimalsEntityFilter":
				targetFilter = new OsamodasAnimalsEntityFilter();
				break;
			case "CharacterActionTypeFilter":
				targetFilter = new CharacterActionTypeFilter();
				break;
			case "FamilyFilter":
				targetFilter = new FamilyFilter();
				break;
			case "FloorMechanismTypeFilter":
				targetFilter = new FloorMechanismTypeFilter();
				break;
			case "TeamFilter":
				targetFilter = new TeamFilter();
				break;
			case "WoundedFilter":
				targetFilter = new WoundedFilter();
				break;
			case "OwnerFilter":
				targetFilter = new OwnerFilter();
				break;
			case "UnionOfEntityFilter":
				targetFilter = new UnionOfEntityFilter();
				break;
			case "CellValidForCharacterFilter":
				targetFilter = new CellValidForCharacterFilter();
				break;
			case "CellValidForMechanismFilter":
				targetFilter = new CellValidForMechanismFilter();
				break;
			case "UnionOfCoordFilter":
				targetFilter = new UnionOfCoordFilter();
				break;
			case "AroundTargetFilter":
				targetFilter = new AroundTargetFilter();
				break;
			case "AroundSquaredTargetFilter":
				targetFilter = new AroundSquaredTargetFilter();
				break;
			case "UnionOfCoordOrEntityFilter":
				targetFilter = new UnionOfCoordOrEntityFilter();
				break;
			case "InLineTargetFilter":
				targetFilter = new InLineTargetFilter();
				break;
			case "InLineInOneDirectionTargetFilter":
				targetFilter = new InLineInOneDirectionTargetFilter();
				break;
			case "IntoAreaOfEntityFilter":
				targetFilter = new IntoAreaOfEntityFilter();
				break;
			case "ElementaryStateFilter":
				targetFilter = new ElementaryStateFilter();
				break;
			case "HasEmptyPathInLineToTargetFilter":
				targetFilter = new HasEmptyPathInLineToTargetFilter();
				break;
			case "EntityHasBeenCrossedOverFilter":
				targetFilter = new EntityHasBeenCrossedOverFilter();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			targetFilter.PopulateFromJson(val);
			return targetFilter;
		}

		public static ITargetFilter FromJsonProperty(JObject jsonObject, string propertyName, ITargetFilter defaultValue = null)
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
