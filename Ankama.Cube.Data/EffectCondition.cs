using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class EffectCondition : IEditableContent
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public static EffectCondition FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class EffectCondition");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			EffectCondition effectCondition;
			switch (text)
			{
			case "NotCondition":
				effectCondition = new NotCondition();
				break;
			case "AndCondition":
				effectCondition = new AndCondition();
				break;
			case "OrCondition":
				effectCondition = new OrCondition();
				break;
			case "CaracValueCondition":
				effectCondition = new CaracValueCondition();
				break;
			case "NumberOfEntityCondition":
				effectCondition = new NumberOfEntityCondition();
				break;
			case "DynamicValueEvaluatorCondition":
				effectCondition = new DynamicValueEvaluatorCondition();
				break;
			case "ElementaryStateCondition":
				effectCondition = new ElementaryStateCondition();
				break;
			case "PropertyCondition":
				effectCondition = new PropertyCondition();
				break;
			case "SpellCanBeRewindCondition":
				effectCondition = new SpellCanBeRewindCondition();
				break;
			case "ClanCondition":
				effectCondition = new ClanCondition();
				break;
			case "HerdCondition":
				effectCondition = new HerdCondition();
				break;
			case "UniqueCondition":
				effectCondition = new UniqueCondition();
				break;
			case "AgonyCondition":
				effectCondition = new AgonyCondition();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			effectCondition.PopulateFromJson(val);
			return effectCondition;
		}

		public static EffectCondition FromJsonProperty(JObject jsonObject, string propertyName, EffectCondition defaultValue = null)
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

		public abstract bool IsValid(DynamicValueContext context);
	}
}
