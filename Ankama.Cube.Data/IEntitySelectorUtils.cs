using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class IEntitySelectorUtils
	{
		public static IEntitySelector FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class IEntitySelector");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			IEntitySelector entitySelector;
			switch (text)
			{
			case "BounceEntitiesSelector":
				entitySelector = new BounceEntitiesSelector();
				break;
			case "FirstCastTargetSelector":
				entitySelector = new FirstCastTargetSelector();
				break;
			case "SecondCastTargetSelector":
				entitySelector = new SecondCastTargetSelector();
				break;
			case "IndexedCastTargetSelector":
				entitySelector = new IndexedCastTargetSelector();
				break;
			case "TriggeringEventFirstCastTargetSelector":
				entitySelector = new TriggeringEventFirstCastTargetSelector();
				break;
			case "EffectHolderSelector":
				entitySelector = new EffectHolderSelector();
				break;
			case "OwnerOfEffectHolderSelector":
				entitySelector = new OwnerOfEffectHolderSelector();
				break;
			case "ApplicationHolderSelector":
				entitySelector = new ApplicationHolderSelector();
				break;
			case "TriggeringEventTargetSelector":
				entitySelector = new TriggeringEventTargetSelector();
				break;
			case "ActionedEntitySelector":
				entitySelector = new ActionedEntitySelector();
				break;
			case "MechanismActivatorSelector":
				entitySelector = new MechanismActivatorSelector();
				break;
			case "CasterSelector":
				entitySelector = new CasterSelector();
				break;
			case "CasterHeroSelector":
				entitySelector = new CasterHeroSelector();
				break;
			case "SingleEntityWithConditionSelector":
				entitySelector = new SingleEntityWithConditionSelector();
				break;
			case "InvokedEntityTargetSelector":
				entitySelector = new InvokedEntityTargetSelector();
				break;
			case "CasterSpecificCompanionSelector":
				entitySelector = new CasterSpecificCompanionSelector();
				break;
			case "UnionOfEntitiesSelector":
				entitySelector = new UnionOfEntitiesSelector();
				break;
			case "FilteredEntitySelector":
				entitySelector = new FilteredEntitySelector();
				break;
			case "AllEntitiesSelector":
				entitySelector = new AllEntitiesSelector();
				break;
			case "OwnerOfSelector":
				entitySelector = new OwnerOfSelector();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			entitySelector.PopulateFromJson(val);
			return entitySelector;
		}

		public static IEntitySelector FromJsonProperty(JObject jsonObject, string propertyName, IEntitySelector defaultValue = null)
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
