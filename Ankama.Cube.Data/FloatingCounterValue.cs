using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FloatingCounterValue : DynamicValue
	{
		private CaracId m_counterType;

		private ISingleEntitySelector m_entitySelector;

		public CaracId counterType => m_counterType;

		public ISingleEntitySelector entitySelector => m_entitySelector;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static FloatingCounterValue FromJsonToken(JToken token)
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
			FloatingCounterValue floatingCounterValue = new FloatingCounterValue();
			floatingCounterValue.PopulateFromJson(jsonObject);
			return floatingCounterValue;
		}

		public static FloatingCounterValue FromJsonProperty(JObject jsonObject, string propertyName, FloatingCounterValue defaultValue = null)
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
			m_counterType = (CaracId)Serialization.JsonTokenValue<int>(jsonObject, "counterType", 20);
			m_entitySelector = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "entitySelector");
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			IEntity entity = m_entitySelector.EnumerateEntities(context).FirstOrDefault();
			if (entity != null)
			{
				value = entity.GetCarac(m_counterType);
				return true;
			}
			value = 0;
			return false;
		}
	}
}
