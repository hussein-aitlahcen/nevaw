using Ankama.Cube.Fight.Entities;
using Ankama.Utilities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class OwnerOfEffectHolderSelector : EntitySelectorForCast, ISingleEntitySelector, IEntitySelector, ITargetSelector, IEditableContent, ISingleTargetSelector
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public new static OwnerOfEffectHolderSelector FromJsonToken(JToken token)
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
			OwnerOfEffectHolderSelector ownerOfEffectHolderSelector = new OwnerOfEffectHolderSelector();
			ownerOfEffectHolderSelector.PopulateFromJson(jsonObject);
			return ownerOfEffectHolderSelector;
		}

		public static OwnerOfEffectHolderSelector FromJsonProperty(JObject jsonObject, string propertyName, OwnerOfEffectHolderSelector defaultValue = null)
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

		public override IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			if (TryGetEntity(context, out IEntity entity))
			{
				yield return entity;
			}
		}

		public bool TryGetEntity<T>(DynamicValueContext context, out T entity) where T : class, IEntity
		{
			CharacterActionValueContext characterActionValueContext = context as CharacterActionValueContext;
			if (characterActionValueContext != null)
			{
				int ownerId = characterActionValueContext.relatedCharacterStatus.ownerId;
				return characterActionValueContext.fightStatus.TryGetEntity(ownerId, out entity);
			}
			Log.Error($"Cannot use Owner Of Effect Holder in {context}", 29, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\TargetSelectors\\OwnerOfEffectHolderSelector.cs");
			entity = null;
			return false;
		}
	}
}
