using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class Cost : IEditableContent
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public static Cost FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class Cost");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			Cost cost;
			switch (text)
			{
			case "ActionPointsCost":
				cost = new ActionPointsCost();
				break;
			case "DrainActionPointsCost":
				cost = new DrainActionPointsCost();
				break;
			case "ReservePointsCost":
				cost = new ReservePointsCost();
				break;
			case "DrainReservePointsCost":
				cost = new DrainReservePointsCost();
				break;
			case "ElementPointsCost":
				cost = new ElementPointsCost();
				break;
			case "DrainElementsPoints":
				cost = new DrainElementsPoints();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			cost.PopulateFromJson(val);
			return cost;
		}

		public static Cost FromJsonProperty(JObject jsonObject, string propertyName, Cost defaultValue = null)
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

		public virtual void PopulateFromJson(JObject jsonObject)
		{
		}

		protected abstract CastValidity InternalCheckValidity(PlayerStatus status, DynamicValueContext castTargetContext);

		public CastValidity CheckValidity(PlayerStatus status, DynamicValueContext castTargetContext)
		{
			return InternalCheckValidity(status, castTargetContext);
		}

		public abstract void UpdateModifications(ref GaugesModification modifications, PlayerStatus player, DynamicValueContext context);
	}
}
