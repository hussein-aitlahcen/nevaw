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
	public sealed class UnionOfEntityFilter : IEditableContent, IEntityFilter, ITargetFilter, IUnionTargetsFilter
	{
		private IEntityFilter m_firstFilter;

		private IEntityFilter m_secondFilter;

		public IEntityFilter firstFilter => m_firstFilter;

		public IEntityFilter secondFilter => m_secondFilter;

		public override string ToString()
		{
			return $"({m_firstFilter} OR {m_secondFilter})";
		}

		public static UnionOfEntityFilter FromJsonToken(JToken token)
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
			UnionOfEntityFilter unionOfEntityFilter = new UnionOfEntityFilter();
			unionOfEntityFilter.PopulateFromJson(jsonObject);
			return unionOfEntityFilter;
		}

		public static UnionOfEntityFilter FromJsonProperty(JObject jsonObject, string propertyName, UnionOfEntityFilter defaultValue = null)
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
			m_firstFilter = IEntityFilterUtils.FromJsonProperty(jsonObject, "firstFilter");
			m_secondFilter = IEntityFilterUtils.FromJsonProperty(jsonObject, "secondFilter");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			IEntity[] allEntities = entities.ToArray();
			IEntity[] first = m_firstFilter.Filter(allEntities, context).ToArray();
			for (int i = 0; i < first.Length; i++)
			{
				yield return first[i];
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
