using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CasterHeroSelector : EntitySelectorForCast, ISingleEntitySelector, IEntitySelector, ITargetSelector, IEditableContent, ISingleTargetSelector
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public new static CasterHeroSelector FromJsonToken(JToken token)
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
			CasterHeroSelector casterHeroSelector = new CasterHeroSelector();
			casterHeroSelector.PopulateFromJson(jsonObject);
			return casterHeroSelector;
		}

		public static CasterHeroSelector FromJsonProperty(JObject jsonObject, string propertyName, CasterHeroSelector defaultValue = null)
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
		}

		public bool TryGetEntity<T>(DynamicValueContext context, out T entity) where T : class, IEntity
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext == null)
			{
				entity = null;
				return false;
			}
			if (!dynamicValueFightContext.fightStatus.TryGetEntity(dynamicValueFightContext.playerId, out PlayerStatus entityStatus))
			{
				entity = null;
				return false;
			}
			T val = entityStatus.heroStatus as T;
			if (val == null)
			{
				entity = null;
				return false;
			}
			entity = val;
			return true;
		}

		public override IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			if (TryGetEntity(context, out IEntity entity))
			{
				yield return entity;
			}
		}
	}
}
