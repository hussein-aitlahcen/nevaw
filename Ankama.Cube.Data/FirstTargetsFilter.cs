using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FirstTargetsFilter : IEditableContent, ICoordOrEntityFilter, ICoordFilter, ITargetFilter, IEntityFilter
	{
		private DynamicValue m_count;

		public DynamicValue count => m_count;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static FirstTargetsFilter FromJsonToken(JToken token)
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
			FirstTargetsFilter firstTargetsFilter = new FirstTargetsFilter();
			firstTargetsFilter.PopulateFromJson(jsonObject);
			return firstTargetsFilter;
		}

		public static FirstTargetsFilter FromJsonProperty(JObject jsonObject, string propertyName, FirstTargetsFilter defaultValue = null)
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
			m_count = DynamicValue.FromJsonProperty(jsonObject, "count");
		}

		public IEnumerable<Coord> Filter(IEnumerable<Coord> coords, DynamicValueContext context)
		{
			m_count.GetValue(context, out int left);
			if (left != 0)
			{
				foreach (Coord coord in coords)
				{
					yield return coord;
					int num = left - 1;
					left = num;
					if (left == 0)
					{
						break;
					}
				}
			}
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			m_count.GetValue(context, out int left);
			if (left != 0)
			{
				foreach (IEntity entity in entities)
				{
					yield return entity;
					int num = left - 1;
					left = num;
					if (left == 0)
					{
						break;
					}
				}
			}
		}
	}
}
