using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class ISingleEntitySelectorUtils
	{
		public static ISingleEntitySelector FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class ISingleEntitySelector");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			ISingleEntitySelector singleEntitySelector;
			switch (text)
			{
			case "FirstCastTargetSelector":
				singleEntitySelector = new FirstCastTargetSelector();
				break;
			case "SecondCastTargetSelector":
				singleEntitySelector = new SecondCastTargetSelector();
				break;
			case "IndexedCastTargetSelector":
				singleEntitySelector = new IndexedCastTargetSelector();
				break;
			case "TriggeringEventFirstCastTargetSelector":
				singleEntitySelector = new TriggeringEventFirstCastTargetSelector();
				break;
			case "EffectHolderSelector":
				singleEntitySelector = new EffectHolderSelector();
				break;
			case "OwnerOfEffectHolderSelector":
				singleEntitySelector = new OwnerOfEffectHolderSelector();
				break;
			case "ApplicationHolderSelector":
				singleEntitySelector = new ApplicationHolderSelector();
				break;
			case "TriggeringEventTargetSelector":
				singleEntitySelector = new TriggeringEventTargetSelector();
				break;
			case "ActionedEntitySelector":
				singleEntitySelector = new ActionedEntitySelector();
				break;
			case "MechanismActivatorSelector":
				singleEntitySelector = new MechanismActivatorSelector();
				break;
			case "CasterSelector":
				singleEntitySelector = new CasterSelector();
				break;
			case "CasterHeroSelector":
				singleEntitySelector = new CasterHeroSelector();
				break;
			case "SingleEntityWithConditionSelector":
				singleEntitySelector = new SingleEntityWithConditionSelector();
				break;
			case "InvokedEntityTargetSelector":
				singleEntitySelector = new InvokedEntityTargetSelector();
				break;
			case "CasterSpecificCompanionSelector":
				singleEntitySelector = new CasterSpecificCompanionSelector();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			singleEntitySelector.PopulateFromJson(val);
			return singleEntitySelector;
		}

		public static ISingleEntitySelector FromJsonProperty(JObject jsonObject, string propertyName, ISingleEntitySelector defaultValue = null)
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
