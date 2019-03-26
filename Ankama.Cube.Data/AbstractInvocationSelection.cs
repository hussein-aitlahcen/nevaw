using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class AbstractInvocationSelection : IEditableContent
	{
		protected string m_referenceId;

		public string referenceId => m_referenceId;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static AbstractInvocationSelection FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class AbstractInvocationSelection");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			AbstractInvocationSelection abstractInvocationSelection;
			switch (text)
			{
			case "SummonSelection":
				abstractInvocationSelection = new SummonSelection();
				break;
			case "CompanionSelection":
				abstractInvocationSelection = new CompanionSelection();
				break;
			case "WeaponSelection":
				abstractInvocationSelection = new WeaponSelection();
				break;
			case "FloorMechanismSelection":
				abstractInvocationSelection = new FloorMechanismSelection();
				break;
			case "ObjectMechanismSelection":
				abstractInvocationSelection = new ObjectMechanismSelection();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			abstractInvocationSelection.PopulateFromJson(val);
			return abstractInvocationSelection;
		}

		public static AbstractInvocationSelection FromJsonProperty(JObject jsonObject, string propertyName, AbstractInvocationSelection defaultValue = null)
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
			m_referenceId = Serialization.JsonTokenValue<string>(jsonObject, "referenceId", "");
		}

		public string CustomToString()
		{
			return GetType().Name;
		}
	}
}
