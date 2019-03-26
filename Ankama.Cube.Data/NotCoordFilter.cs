using Ankama.Utilities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class NotCoordFilter : IEditableContent, ICoordFilter, ITargetFilter
	{
		private ICoordFilter m_filter;

		public ICoordFilter filter => m_filter;

		public override string ToString()
		{
			return $"not ({filter})";
		}

		public static NotCoordFilter FromJsonToken(JToken token)
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
			NotCoordFilter notCoordFilter = new NotCoordFilter();
			notCoordFilter.PopulateFromJson(jsonObject);
			return notCoordFilter;
		}

		public static NotCoordFilter FromJsonProperty(JObject jsonObject, string propertyName, NotCoordFilter defaultValue = null)
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
			m_filter = ICoordFilterUtils.FromJsonProperty(jsonObject, "filter");
		}

		public IEnumerable<Coord> Filter(IEnumerable<Coord> coords, DynamicValueContext context)
		{
			List<Coord> coordList = ListPool<Coord>.Get();
			List<Coord> filtered = ListPool<Coord>.Get();
			coordList.AddRange(coords);
			filtered.AddRange(m_filter.Filter(coordList, context));
			int coordCount = coordList.Count;
			int filteredCount = filtered.Count;
			int num2;
			for (int i = 0; i < coordCount; i = num2)
			{
				Coord coord = coordList[i];
				int num = 0;
				while (true)
				{
					if (num < filteredCount)
					{
						if (coord == filtered[num])
						{
							break;
						}
						num++;
						continue;
					}
					yield return coord;
					break;
				}
				num2 = i + 1;
			}
			ListPool<Coord>.Release(coordList);
			ListPool<Coord>.Release(filtered);
		}
	}
}
