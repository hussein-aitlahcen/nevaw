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
	public abstract class EntitySelectorForCast : IEditableContent, ITargetSelector, ISelectorForCast, IEntitySelector
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public static EntitySelectorForCast FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject val = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			JToken val2 = default(JToken);
			if (!val.TryGetValue("type", ref val2))
			{
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class EntitySelectorForCast");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			EntitySelectorForCast entitySelectorForCast;
			switch (text)
			{
			case "UnionOfEntitiesSelector":
				entitySelectorForCast = new UnionOfEntitiesSelector();
				break;
			case "OwnerOfEffectHolderSelector":
				entitySelectorForCast = new OwnerOfEffectHolderSelector();
				break;
			case "CasterSelector":
				entitySelectorForCast = new CasterSelector();
				break;
			case "CasterHeroSelector":
				entitySelectorForCast = new CasterHeroSelector();
				break;
			case "FilteredEntitySelector":
				entitySelectorForCast = new FilteredEntitySelector();
				break;
			case "AllEntitiesSelector":
				entitySelectorForCast = new AllEntitiesSelector();
				break;
			case "OwnerOfSelector":
				entitySelectorForCast = new OwnerOfSelector();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			entitySelectorForCast.PopulateFromJson(val);
			return entitySelectorForCast;
		}

		public static EntitySelectorForCast FromJsonProperty(JObject jsonObject, string propertyName, EntitySelectorForCast defaultValue = null)
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

		public virtual void PopulateFromJson(JObject jsonObject)
		{
		}

		public abstract IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context);

		public IEnumerable<Target> EnumerateTargets(DynamicValueContext context)
		{
			int casterId = -1;
			foreach (IEntity item in EnumerateEntities(context))
			{
				if (!item.HasProperty(PropertyId.Untargetable))
				{
					yield return new Target(item);
				}
				else
				{
					if (casterId == -1)
					{
						casterId = 0;
						DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
						if (dynamicValueFightContext == null)
						{
							Log.Warning("Selector " + GetType().Name + " tried to retrieve caster but wasn't given a compatible context.", 47, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\TargetSelectors\\ISelectorForCast.cs");
							continue;
						}
						if (!dynamicValueFightContext.fightStatus.TryGetEntity(dynamicValueFightContext.playerId, out PlayerStatus entityStatus))
						{
							Log.Warning("Selector " + GetType().Name + " tried to retrieve caster but could not find it.", 41, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\TargetSelectors\\ISelectorForCast.cs");
							continue;
						}
						casterId = entityStatus.id;
					}
					IEntityWithOwner entityWithOwner = item as IEntityWithOwner;
					if (entityWithOwner != null && entityWithOwner.ownerId == casterId)
					{
						yield return new Target(item);
					}
				}
			}
		}
	}
}
