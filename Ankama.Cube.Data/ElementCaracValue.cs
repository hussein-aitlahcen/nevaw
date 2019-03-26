using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ElementCaracValue : DynamicValue
	{
		private OwnerFilter m_player;

		private CaracId m_elementType;

		public OwnerFilter player => m_player;

		public CaracId elementType => m_elementType;

		public override string ToString()
		{
			return "<element>";
		}

		public new static ElementCaracValue FromJsonToken(JToken token)
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
			ElementCaracValue elementCaracValue = new ElementCaracValue();
			elementCaracValue.PopulateFromJson(jsonObject);
			return elementCaracValue;
		}

		public static ElementCaracValue FromJsonProperty(JObject jsonObject, string propertyName, ElementCaracValue defaultValue = null)
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
			m_player = OwnerFilter.FromJsonProperty(jsonObject, "player");
			m_elementType = (CaracId)Serialization.JsonTokenValue<int>(jsonObject, "elementType", 12);
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null)
			{
				IEnumerable<IEntity> entities = dynamicValueFightContext.fightStatus.EnumerateEntities<PlayerStatus>();
				int num = 0;
				foreach (IEntity item in m_player.Filter(entities, context))
				{
					num += item.GetCarac(m_elementType);
				}
				value = num;
				return true;
			}
			value = 0;
			return false;
		}
	}
}
