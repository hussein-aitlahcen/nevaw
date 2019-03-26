using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class DynamicValue : IEditableContent
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public static DynamicValue FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class DynamicValue");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			DynamicValue dynamicValue;
			switch (text)
			{
			case "ConstIntegerValue":
				dynamicValue = new ConstIntegerValue();
				break;
			case "SimpleDiceValue":
				dynamicValue = new SimpleDiceValue();
				break;
			case "EntityCountValue":
				dynamicValue = new EntityCountValue();
				break;
			case "ElementCaracValue":
				dynamicValue = new ElementCaracValue();
				break;
			case "BoardEntityCaracValue":
				dynamicValue = new BoardEntityCaracValue();
				break;
			case "ReserveCaracValue":
				dynamicValue = new ReserveCaracValue();
				break;
			case "SumValue":
				dynamicValue = new SumValue();
				break;
			case "MultValue":
				dynamicValue = new MultValue();
				break;
			case "ClampValue":
				dynamicValue = new ClampValue();
				break;
			case "ConditionalValue":
				dynamicValue = new ConditionalValue();
				break;
			case "NegativeValue":
				dynamicValue = new NegativeValue();
				break;
			case "TriggeringEventValue":
				dynamicValue = new TriggeringEventValue();
				break;
			case "TriggeringMovementLengthValue":
				dynamicValue = new TriggeringMovementLengthValue();
				break;
			case "SpellsPlayedThisTurnValue":
				dynamicValue = new SpellsPlayedThisTurnValue();
				break;
			case "SpellsInHandValue":
				dynamicValue = new SpellsInHandValue();
				break;
			case "CharacterActionValue":
				dynamicValue = new CharacterActionValue();
				break;
			case "ThisSpellActionPointsCostValue":
				dynamicValue = new ThisSpellActionPointsCostValue();
				break;
			case "ThisSpellReservePointsCostValue":
				dynamicValue = new ThisSpellReservePointsCostValue();
				break;
			case "ThisSpellElementsPointsCostValue":
				dynamicValue = new ThisSpellElementsPointsCostValue();
				break;
			case "FloatingCounterValue":
				dynamicValue = new FloatingCounterValue();
				break;
			case "TotalActivationValueOfThisAssemblage":
				dynamicValue = new TotalActivationValueOfThisAssemblage();
				break;
			case "DistanceValue":
				dynamicValue = new DistanceValue();
				break;
			case "LinearLevelBasedDynamicValue":
				dynamicValue = new LinearLevelBasedDynamicValue();
				break;
			case "ConstIntLevelBasedDynamicValue":
				dynamicValue = new ConstIntLevelBasedDynamicValue();
				break;
			case "DynamicValueLevelBasedDynamicValue":
				dynamicValue = new DynamicValueLevelBasedDynamicValue();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			dynamicValue.PopulateFromJson(val);
			return dynamicValue;
		}

		public static DynamicValue FromJsonProperty(JObject jsonObject, string propertyName, DynamicValue defaultValue = null)
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

		public abstract bool GetValue(DynamicValueContext context, out int value);

		public virtual bool ToString(DynamicValueContext context, out string value)
		{
			int value2;
			bool value3 = GetValue(context, out value2);
			value = value2.ToString();
			return value3;
		}
	}
}
