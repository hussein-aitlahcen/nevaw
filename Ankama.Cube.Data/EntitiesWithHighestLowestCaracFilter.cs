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
	public sealed class EntitiesWithHighestLowestCaracFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private Superlative m_superlative;

		private CaracId m_carac;

		public Superlative superlative => m_superlative;

		public CaracId carac => m_carac;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static EntitiesWithHighestLowestCaracFilter FromJsonToken(JToken token)
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
			EntitiesWithHighestLowestCaracFilter entitiesWithHighestLowestCaracFilter = new EntitiesWithHighestLowestCaracFilter();
			entitiesWithHighestLowestCaracFilter.PopulateFromJson(jsonObject);
			return entitiesWithHighestLowestCaracFilter;
		}

		public static EntitiesWithHighestLowestCaracFilter FromJsonProperty(JObject jsonObject, string propertyName, EntitiesWithHighestLowestCaracFilter defaultValue = null)
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
			m_superlative = (Superlative)Serialization.JsonTokenValue<int>(jsonObject, "superlative", 1);
			m_carac = (CaracId)Serialization.JsonTokenValue<int>(jsonObject, "carac", 1);
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			IEntity[] array = entities.ToArray();
			List<IEntity> list = new List<IEntity>();
			switch (m_superlative)
			{
			case Superlative.Biggest:
			{
				int num2 = int.MinValue;
				IEntity[] array2 = array;
				foreach (IEntity entity2 in array2)
				{
					int carac2 = entity2.GetCarac(this.carac);
					if (carac2 >= num2)
					{
						if (carac2 == num2)
						{
							list.Add(entity2);
							continue;
						}
						list.Clear();
						list.Add(entity2);
						num2 = carac2;
					}
				}
				break;
			}
			case Superlative.Smallest:
			{
				int num = int.MaxValue;
				IEntity[] array2 = array;
				foreach (IEntity entity in array2)
				{
					int carac = entity.GetCarac(this.carac);
					if (carac <= num)
					{
						if (carac == num)
						{
							list.Add(entity);
							continue;
						}
						list.Clear();
						list.Add(entity);
						num = carac;
					}
				}
				break;
			}
			default:
				Debug.LogError((object)"Unknown Superlative: {m_superlative");
				break;
			}
			return list;
		}
	}
}
