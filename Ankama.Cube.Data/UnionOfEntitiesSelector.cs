using Ankama.Cube.Fight.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class UnionOfEntitiesSelector : EntitySelectorForCast
	{
		private IEntitySelector m_first;

		private IEntitySelector m_second;

		public IEntitySelector first => m_first;

		public IEntitySelector second => m_second;

		public override string ToString()
		{
			return $"({m_first} OR {m_second})";
		}

		public new static UnionOfEntitiesSelector FromJsonToken(JToken token)
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
			UnionOfEntitiesSelector unionOfEntitiesSelector = new UnionOfEntitiesSelector();
			unionOfEntitiesSelector.PopulateFromJson(jsonObject);
			return unionOfEntitiesSelector;
		}

		public static UnionOfEntitiesSelector FromJsonProperty(JObject jsonObject, string propertyName, UnionOfEntitiesSelector defaultValue = null)
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
			m_first = IEntitySelectorUtils.FromJsonProperty(jsonObject, "first");
			m_second = IEntitySelectorUtils.FromJsonProperty(jsonObject, "second");
		}

		public override IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			HashSet<IEntity> entitiesSet = new HashSet<IEntity>();
			foreach (IEntity e in m_first.EnumerateEntities(context))
			{
				yield return e;
				entitiesSet.Add(e);
			}
			foreach (IEntity item in m_second.EnumerateEntities(context))
			{
				if (entitiesSet.Add(item))
				{
					yield return item;
				}
			}
		}
	}
}
