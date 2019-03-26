using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class GetCloser : MoveVector
	{
		private ISingleCoordSelector m_to;

		public ISingleCoordSelector to => m_to;

		public override string ToString()
		{
			return $"GetCloser to {m_to}";
		}

		public new static GetCloser FromJsonToken(JToken token)
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
			GetCloser getCloser = new GetCloser();
			getCloser.PopulateFromJson(jsonObject);
			return getCloser;
		}

		public static GetCloser FromJsonProperty(JObject jsonObject, string propertyName, GetCloser defaultValue = null)
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
			m_to = ISingleCoordSelectorUtils.FromJsonProperty(jsonObject, "to");
		}
	}
}
