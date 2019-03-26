using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class ISingleTargetSelectorUtils
	{
		public static ISingleTargetSelector FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class ISingleTargetSelector");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			ISingleTargetSelector singleTargetSelector;
			switch (text)
			{
			case "FirstCastTargetSelector":
				singleTargetSelector = new FirstCastTargetSelector();
				break;
			case "SecondCastTargetSelector":
				singleTargetSelector = new SecondCastTargetSelector();
				break;
			case "IndexedCastTargetSelector":
				singleTargetSelector = new IndexedCastTargetSelector();
				break;
			case "TriggeringEventFirstCastTargetSelector":
				singleTargetSelector = new TriggeringEventFirstCastTargetSelector();
				break;
			case "EffectHolderSelector":
				singleTargetSelector = new EffectHolderSelector();
				break;
			case "OwnerOfEffectHolderSelector":
				singleTargetSelector = new OwnerOfEffectHolderSelector();
				break;
			case "ApplicationHolderSelector":
				singleTargetSelector = new ApplicationHolderSelector();
				break;
			case "TriggeringEventTargetSelector":
				singleTargetSelector = new TriggeringEventTargetSelector();
				break;
			case "ActionedEntitySelector":
				singleTargetSelector = new ActionedEntitySelector();
				break;
			case "MechanismActivatorSelector":
				singleTargetSelector = new MechanismActivatorSelector();
				break;
			case "CasterSelector":
				singleTargetSelector = new CasterSelector();
				break;
			case "CasterHeroSelector":
				singleTargetSelector = new CasterHeroSelector();
				break;
			case "SingleCoordWithConditionSelector":
				singleTargetSelector = new SingleCoordWithConditionSelector();
				break;
			case "SingleEntityWithConditionSelector":
				singleTargetSelector = new SingleEntityWithConditionSelector();
				break;
			case "CentralSymmetryCoordSelector":
				singleTargetSelector = new CentralSymmetryCoordSelector();
				break;
			case "InvokedEntityTargetSelector":
				singleTargetSelector = new InvokedEntityTargetSelector();
				break;
			case "EntityPositionSelector":
				singleTargetSelector = new EntityPositionSelector();
				break;
			case "CasterSpecificCompanionSelector":
				singleTargetSelector = new CasterSpecificCompanionSelector();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			singleTargetSelector.PopulateFromJson(val);
			return singleTargetSelector;
		}

		public static ISingleTargetSelector FromJsonProperty(JObject jsonObject, string propertyName, ISingleTargetSelector defaultValue = null)
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
