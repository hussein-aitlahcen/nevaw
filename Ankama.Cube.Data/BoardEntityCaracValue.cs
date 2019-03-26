using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.EntityAddedOrRemoved
	})]
	public sealed class BoardEntityCaracValue : DynamicValue
	{
		private IEntityFilter m_entity;

		private CaracId m_carac;

		public IEntityFilter entity => m_entity;

		public CaracId carac => m_carac;

		public override string ToString()
		{
			return m_carac.ToString();
		}

		public new static BoardEntityCaracValue FromJsonToken(JToken token)
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
			BoardEntityCaracValue boardEntityCaracValue = new BoardEntityCaracValue();
			boardEntityCaracValue.PopulateFromJson(jsonObject);
			return boardEntityCaracValue;
		}

		public static BoardEntityCaracValue FromJsonProperty(JObject jsonObject, string propertyName, BoardEntityCaracValue defaultValue = null)
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
			m_entity = IEntityFilterUtils.FromJsonProperty(jsonObject, "entity");
			m_carac = (CaracId)Serialization.JsonTokenValue<int>(jsonObject, "carac", 0);
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null)
			{
				IEnumerable<IEntityWithBoardPresence> entities = dynamicValueFightContext.fightStatus.EnumerateEntities<IEntityWithBoardPresence>();
				int num = 0;
				foreach (IEntity item in m_entity.Filter(entities, context))
				{
					num += item.GetCarac(m_carac);
				}
				value = num;
				return true;
			}
			value = 0;
			return false;
		}
	}
}
