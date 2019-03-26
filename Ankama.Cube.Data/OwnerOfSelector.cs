using Ankama.Cube.Fight.Entities;
using Ankama.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class OwnerOfSelector : EntitySelectorForCast
	{
		private IEntitySelector m_selector;

		public IEntitySelector selector => m_selector;

		public override string ToString()
		{
			return $"owner of {selector}";
		}

		public new static OwnerOfSelector FromJsonToken(JToken token)
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
			OwnerOfSelector ownerOfSelector = new OwnerOfSelector();
			ownerOfSelector.PopulateFromJson(jsonObject);
			return ownerOfSelector;
		}

		public static OwnerOfSelector FromJsonProperty(JObject jsonObject, string propertyName, OwnerOfSelector defaultValue = null)
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
			m_selector = IEntitySelectorUtils.FromJsonProperty(jsonObject, "selector");
		}

		public override IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			DynamicValueFightContext fightContext = context as DynamicValueFightContext;
			if (fightContext != null)
			{
				List<int> ownerIds = ListPool<int>.Get(2);
				foreach (IEntity item in m_selector.EnumerateEntities(context))
				{
					IEntityWithOwner entityWithOwner = item as IEntityWithOwner;
					if (entityWithOwner != null && !ownerIds.Contains(entityWithOwner.ownerId))
					{
						ownerIds.Add(entityWithOwner.ownerId);
						if (fightContext.fightStatus.TryGetEntity(entityWithOwner.ownerId, out IEntity entityStatus))
						{
							yield return entityStatus;
						}
					}
				}
				ListPool<int>.Release(ownerIds);
			}
		}
	}
}
