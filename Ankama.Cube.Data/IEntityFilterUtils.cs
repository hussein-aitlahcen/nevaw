using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.EntityAddedOrRemoved
	})]
	public class IEntityFilterUtils
	{
		public static IEntityFilter FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class IEntityFilter");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			IEntityFilter entityFilter;
			switch (text)
			{
			case "NotEntityFilter":
				entityFilter = new NotEntityFilter();
				break;
			case "EntitiesWithHighestLowestCaracFilter":
				entityFilter = new EntitiesWithHighestLowestCaracFilter();
				break;
			case "CombinedEntityFilter":
				entityFilter = new CombinedEntityFilter();
				break;
			case "EffectHolderFilter":
				entityFilter = new EffectHolderFilter();
				break;
			case "ApplicationHolderFilter":
				entityFilter = new ApplicationHolderFilter();
				break;
			case "CastTargetFilter":
				entityFilter = new CastTargetFilter();
				break;
			case "LifeFilter":
				entityFilter = new LifeFilter();
				break;
			case "ArmorFilter":
				entityFilter = new ArmorFilter();
				break;
			case "RangeFilter":
				entityFilter = new RangeFilter();
				break;
			case "PropertyFilter":
				entityFilter = new PropertyFilter();
				break;
			case "PropertiesFilter":
				entityFilter = new PropertiesFilter();
				break;
			case "EntityValidForMagicalDamageFilter":
				entityFilter = new EntityValidForMagicalDamageFilter();
				break;
			case "EntityValidForMagicalHealFilter":
				entityFilter = new EntityValidForMagicalHealFilter();
				break;
			case "EntityValidForPhysicalDamageFilter":
				entityFilter = new EntityValidForPhysicalDamageFilter();
				break;
			case "EntityValidForPhysicalHealFilter":
				entityFilter = new EntityValidForPhysicalHealFilter();
				break;
			case "EntityTypeFilter":
				entityFilter = new EntityTypeFilter();
				break;
			case "CanGrowEntityFilter":
				entityFilter = new CanGrowEntityFilter();
				break;
			case "CharacterEntityFilter":
				entityFilter = new CharacterEntityFilter();
				break;
			case "OsamodasAnimalsEntityFilter":
				entityFilter = new OsamodasAnimalsEntityFilter();
				break;
			case "CharacterActionTypeFilter":
				entityFilter = new CharacterActionTypeFilter();
				break;
			case "FamilyFilter":
				entityFilter = new FamilyFilter();
				break;
			case "FloorMechanismTypeFilter":
				entityFilter = new FloorMechanismTypeFilter();
				break;
			case "TeamFilter":
				entityFilter = new TeamFilter();
				break;
			case "WoundedFilter":
				entityFilter = new WoundedFilter();
				break;
			case "OwnerFilter":
				entityFilter = new OwnerFilter();
				break;
			case "UnionOfEntityFilter":
				entityFilter = new UnionOfEntityFilter();
				break;
			case "IntoAreaOfEntityFilter":
				entityFilter = new IntoAreaOfEntityFilter();
				break;
			case "ElementaryStateFilter":
				entityFilter = new ElementaryStateFilter();
				break;
			case "EntityHasBeenCrossedOverFilter":
				entityFilter = new EntityHasBeenCrossedOverFilter();
				break;
			case "FirstTargetsFilter":
				entityFilter = new FirstTargetsFilter();
				break;
			case "RandomTargetsFilter":
				entityFilter = new RandomTargetsFilter();
				break;
			case "SpecificCompanionFilter":
				entityFilter = new SpecificCompanionFilter();
				break;
			case "SpecificSummoningFilter":
				entityFilter = new SpecificSummoningFilter();
				break;
			case "SpecificObjectMechanismFilter":
				entityFilter = new SpecificObjectMechanismFilter();
				break;
			case "AroundTargetFilter":
				entityFilter = new AroundTargetFilter();
				break;
			case "AroundSquaredTargetFilter":
				entityFilter = new AroundSquaredTargetFilter();
				break;
			case "UnionOfCoordOrEntityFilter":
				entityFilter = new UnionOfCoordOrEntityFilter();
				break;
			case "InLineTargetFilter":
				entityFilter = new InLineTargetFilter();
				break;
			case "InLineInOneDirectionTargetFilter":
				entityFilter = new InLineInOneDirectionTargetFilter();
				break;
			case "HasEmptyPathInLineToTargetFilter":
				entityFilter = new HasEmptyPathInLineToTargetFilter();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			entityFilter.PopulateFromJson(val);
			return entityFilter;
		}

		public static IEntityFilter FromJsonProperty(JObject jsonObject, string propertyName, IEntityFilter defaultValue = null)
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
