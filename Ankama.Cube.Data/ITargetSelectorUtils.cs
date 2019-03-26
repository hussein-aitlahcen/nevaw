using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class ITargetSelectorUtils
	{
		public static ITargetSelector FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class ITargetSelector");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			ITargetSelector targetSelector;
			switch (text)
			{
			case "ConditionalSelectorForCast":
				targetSelector = new ConditionalSelectorForCast();
				break;
			case "FirstCastTargetSelector":
				targetSelector = new FirstCastTargetSelector();
				break;
			case "SecondCastTargetSelector":
				targetSelector = new SecondCastTargetSelector();
				break;
			case "IndexedCastTargetSelector":
				targetSelector = new IndexedCastTargetSelector();
				break;
			case "TriggeringEventFirstCastTargetSelector":
				targetSelector = new TriggeringEventFirstCastTargetSelector();
				break;
			case "EffectHolderSelector":
				targetSelector = new EffectHolderSelector();
				break;
			case "ApplicationHolderSelector":
				targetSelector = new ApplicationHolderSelector();
				break;
			case "TriggeringEventTargetSelector":
				targetSelector = new TriggeringEventTargetSelector();
				break;
			case "ActionedEntitySelector":
				targetSelector = new ActionedEntitySelector();
				break;
			case "MechanismActivatorSelector":
				targetSelector = new MechanismActivatorSelector();
				break;
			case "SingleCoordWithConditionSelector":
				targetSelector = new SingleCoordWithConditionSelector();
				break;
			case "SingleEntityWithConditionSelector":
				targetSelector = new SingleEntityWithConditionSelector();
				break;
			case "CentralSymmetryCoordSelector":
				targetSelector = new CentralSymmetryCoordSelector();
				break;
			case "BounceEntitiesSelector":
				targetSelector = new BounceEntitiesSelector();
				break;
			case "InvokedEntityTargetSelector":
				targetSelector = new InvokedEntityTargetSelector();
				break;
			case "EntityPositionSelector":
				targetSelector = new EntityPositionSelector();
				break;
			case "CasterSpecificCompanionSelector":
				targetSelector = new CasterSpecificCompanionSelector();
				break;
			case "UnionOfCoordsSelector":
				targetSelector = new UnionOfCoordsSelector();
				break;
			case "UnionOfEntitiesSelector":
				targetSelector = new UnionOfEntitiesSelector();
				break;
			case "OwnerOfEffectHolderSelector":
				targetSelector = new OwnerOfEffectHolderSelector();
				break;
			case "CasterSelector":
				targetSelector = new CasterSelector();
				break;
			case "CasterHeroSelector":
				targetSelector = new CasterHeroSelector();
				break;
			case "FilteredEntitySelector":
				targetSelector = new FilteredEntitySelector();
				break;
			case "FilteredCoordSelector":
				targetSelector = new FilteredCoordSelector();
				break;
			case "AllEntitiesSelector":
				targetSelector = new AllEntitiesSelector();
				break;
			case "OwnerOfSelector":
				targetSelector = new OwnerOfSelector();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			targetSelector.PopulateFromJson(val);
			return targetSelector;
		}

		public static ITargetSelector FromJsonProperty(JObject jsonObject, string propertyName, ITargetSelector defaultValue = null)
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
