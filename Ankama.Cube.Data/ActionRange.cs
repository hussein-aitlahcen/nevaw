using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ActionRange : IEditableContent
	{
		private ILevelOnlyDependant m_min;

		private ILevelOnlyDependant m_max;

		public ILevelOnlyDependant min => m_min;

		public ILevelOnlyDependant max => m_max;

		public override string ToString()
		{
			return $"Range: {m_min} to {m_max}";
		}

		public static ActionRange FromJsonToken(JToken token)
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
			ActionRange actionRange = new ActionRange();
			actionRange.PopulateFromJson(jsonObject);
			return actionRange;
		}

		public static ActionRange FromJsonProperty(JObject jsonObject, string propertyName, ActionRange defaultValue = null)
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

		public void PopulateFromJson(JObject jsonObject)
		{
			m_min = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "min");
			m_max = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "max");
		}
	}
}
