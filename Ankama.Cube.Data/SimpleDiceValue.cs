using Ankama.Utilities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SimpleDiceValue : DynamicValue
	{
		private int m_dice;

		public int dice => m_dice;

		public override string ToString()
		{
			return $"value of D{dice}";
		}

		public new static SimpleDiceValue FromJsonToken(JToken token)
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
			SimpleDiceValue simpleDiceValue = new SimpleDiceValue();
			simpleDiceValue.PopulateFromJson(jsonObject);
			return simpleDiceValue;
		}

		public static SimpleDiceValue FromJsonProperty(JObject jsonObject, string propertyName, SimpleDiceValue defaultValue = null)
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
			m_dice = Serialization.JsonTokenValue<int>(jsonObject, "dice", 0);
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			Log.Warning("Should not be call in client. Returning max value", 57, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\DynamicValues\\DynamicValue.cs");
			value = m_dice;
			return false;
		}

		public override bool ToString(DynamicValueContext context, out string value)
		{
			value = m_dice.ToString();
			return false;
		}
	}
}
