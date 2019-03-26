using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class UnionOfCoordFilter : IEditableContent, ICoordFilter, ITargetFilter, IUnionTargetsFilter
	{
		private ICoordFilter m_firstFilter;

		private ICoordFilter m_secondFilter;

		public ICoordFilter firstFilter => m_firstFilter;

		public ICoordFilter secondFilter => m_secondFilter;

		public override string ToString()
		{
			return $"({m_firstFilter} OR {m_secondFilter})";
		}

		public static UnionOfCoordFilter FromJsonToken(JToken token)
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
			UnionOfCoordFilter unionOfCoordFilter = new UnionOfCoordFilter();
			unionOfCoordFilter.PopulateFromJson(jsonObject);
			return unionOfCoordFilter;
		}

		public static UnionOfCoordFilter FromJsonProperty(JObject jsonObject, string propertyName, UnionOfCoordFilter defaultValue = null)
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
			m_firstFilter = ICoordFilterUtils.FromJsonProperty(jsonObject, "firstFilter");
			m_secondFilter = ICoordFilterUtils.FromJsonProperty(jsonObject, "secondFilter");
		}

		public IEnumerable<Coord> Filter(IEnumerable<Coord> coords, DynamicValueContext context)
		{
			Coord[] allCoords = coords.ToArray();
			Coord[] first = m_firstFilter.Filter(allCoords, context).ToArray();
			for (int i = 0; i < first.Length; i++)
			{
				yield return first[i];
			}
			foreach (Coord item in m_secondFilter.Filter(allCoords, context))
			{
				if (!first.Contains(item))
				{
					yield return item;
				}
			}
		}
	}
}
