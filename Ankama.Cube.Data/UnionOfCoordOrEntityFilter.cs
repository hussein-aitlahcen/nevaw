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
	public sealed class UnionOfCoordOrEntityFilter : IEditableContent, ICoordOrEntityFilter, ICoordFilter, ITargetFilter, IEntityFilter, IUnionTargetsFilter
	{
		private ICoordOrEntityFilter m_firstFilter;

		private ICoordOrEntityFilter m_secondFilter;

		public ICoordOrEntityFilter firstFilter => m_firstFilter;

		public ICoordOrEntityFilter secondFilter => m_secondFilter;

		public override string ToString()
		{
			return $"({m_firstFilter} OR {m_secondFilter})";
		}

		public static UnionOfCoordOrEntityFilter FromJsonToken(JToken token)
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
			UnionOfCoordOrEntityFilter unionOfCoordOrEntityFilter = new UnionOfCoordOrEntityFilter();
			unionOfCoordOrEntityFilter.PopulateFromJson(jsonObject);
			return unionOfCoordOrEntityFilter;
		}

		public static UnionOfCoordOrEntityFilter FromJsonProperty(JObject jsonObject, string propertyName, UnionOfCoordOrEntityFilter defaultValue = null)
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
			m_firstFilter = ICoordOrEntityFilterUtils.FromJsonProperty(jsonObject, "firstFilter");
			m_secondFilter = ICoordOrEntityFilterUtils.FromJsonProperty(jsonObject, "secondFilter");
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

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			IEntity[] allEntities = entities.ToArray();
			IEntity[] first = m_firstFilter.Filter(allEntities, context).ToArray();
			IEntity[] array = first;
			for (int i = 0; i < array.Length; i++)
			{
				yield return array[i];
			}
			foreach (IEntity item in m_secondFilter.Filter(allEntities, context))
			{
				if (!first.Contains(item))
				{
					yield return item;
				}
			}
		}
	}
}
