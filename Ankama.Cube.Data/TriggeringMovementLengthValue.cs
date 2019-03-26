using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class TriggeringMovementLengthValue : DynamicValue
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public new static TriggeringMovementLengthValue FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject jsonObject = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			TriggeringMovementLengthValue triggeringMovementLengthValue = new TriggeringMovementLengthValue();
			triggeringMovementLengthValue.PopulateFromJson(jsonObject);
			return triggeringMovementLengthValue;
		}

		public static TriggeringMovementLengthValue FromJsonProperty(JObject jsonObject, string propertyName, TriggeringMovementLengthValue defaultValue = null)
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

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			Debug.LogWarning((object)"Unable to compute TriggeringMovementLengthValue client-side. Invalid data ?");
			value = 0;
			return false;
		}

		public override bool ToString(DynamicValueContext context, out string value)
		{
			value = "0";
			return false;
		}
	}
}
