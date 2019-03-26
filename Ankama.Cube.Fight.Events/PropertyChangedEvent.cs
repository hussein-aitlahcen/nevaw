using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class PropertyChangedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int propertyId
		{
			get;
			private set;
		}

		public bool active
		{
			get;
			private set;
		}

		public int? propertyReplaced
		{
			get;
			private set;
		}

		public PropertyChangedEvent(int eventId, int? parentEventId, int concernedEntity, int propertyId, bool active, int? propertyReplaced)
			: base(FightEventData.Types.EventType.PropertyChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.propertyId = propertyId;
			this.active = active;
			this.propertyReplaced = propertyReplaced;
		}

		public PropertyChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.PropertyChanged, proto)
		{
			concernedEntity = proto.Int1;
			propertyId = proto.Int2;
			active = proto.Bool1;
			propertyReplaced = proto.OptInt1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			PropertyId propertyId = (PropertyId)this.propertyId;
			if (fightStatus.TryGetEntity(concernedEntity, out EntityStatus entityStatus))
			{
				if (propertyReplaced.HasValue)
				{
					PropertyId value = (PropertyId)propertyReplaced.Value;
					if (value != propertyId)
					{
						entityStatus.RemoveProperty(value);
						PropertyStatusChanged(fightStatus, value);
						entityStatus.AddProperty(propertyId);
						PropertyStatusChanged(fightStatus, propertyId);
					}
				}
				else
				{
					if (active)
					{
						entityStatus.AddProperty(propertyId);
					}
					else
					{
						entityStatus.RemoveProperty(propertyId);
					}
					PropertyStatusChanged(fightStatus, propertyId);
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<EntityStatus>(concernedEntity), 49, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PropertyChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.PropertyChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			PropertyId property = (PropertyId)propertyId;
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				ICharacterObject characterObject2;
				ICharacterObject characterObject = characterObject2 = (entityStatus.view as ICharacterObject);
				if (characterObject2 != null)
				{
					if (propertyReplaced.HasValue)
					{
						PropertyId removedProperty = (PropertyId)propertyReplaced.Value;
						if (removedProperty != property)
						{
							if (PropertiesUtility.IsSightProperty(property))
							{
								yield return TryChangeSightVisual(characterObject, property);
							}
							else
							{
								yield return RemovePropertyFromView(characterObject, removedProperty);
								yield return PropertyViewChanged(fightStatus, removedProperty);
								yield return AddPropertyToView(characterObject, property);
								yield return PropertyViewChanged(fightStatus, property);
							}
						}
					}
					else
					{
						if (active)
						{
							if (PropertiesUtility.IsSightProperty(property))
							{
								yield return TryChangeSightVisual(characterObject, property);
							}
							else
							{
								yield return AddPropertyToView(characterObject, property);
							}
						}
						else if (PropertiesUtility.IsSightProperty(property))
						{
							yield return TryChangeSightVisual(characterObject, null);
						}
						else
						{
							yield return RemovePropertyFromView(characterObject, property);
						}
						yield return PropertyViewChanged(fightStatus, property);
					}
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 113, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PropertyChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.PropertyChanged);
		}

		private static IEnumerator AddPropertyToView(ICharacterObject characterObject, PropertyId property)
		{
			AttachableEffect attachableEffect;
			if (FightSpellEffectFactory.isReady && FightSpellEffectFactory.TryGetPropertyEffect(property, out attachableEffect) && null != attachableEffect)
			{
				yield return characterObject.AddPropertyEffect(attachableEffect, property);
			}
		}

		private static IEnumerator RemovePropertyFromView(ICharacterObject characterObject, PropertyId property)
		{
			if (FightSpellEffectFactory.isReady && FightSpellEffectFactory.TryGetPropertyEffect(property, out AttachableEffect attachableEffect))
			{
				yield return characterObject.RemovePropertyEffect(attachableEffect, property);
			}
		}

		private void PropertyStatusChanged(FightStatus fightStatus, PropertyId property)
		{
			if (property == PropertyId.PlaySpellForbidden && fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.RefreshAvailableActions(recomputeSpellCosts: false);
				}
			}
			if (PropertiesUtility.PreventsAction(property))
			{
				fightStatus.NotifyEntityPlayableStateChanged();
			}
		}

		private IEnumerator PropertyViewChanged(FightStatus fightStatus, PropertyId property)
		{
			if (property == PropertyId.PlaySpellForbidden && fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					yield return view.UpdateAvailableActions(recomputeSpellCosts: false);
				}
			}
		}

		private static IEnumerator TryChangeSightVisual(ICharacterObject characterObject, PropertyId? property)
		{
			IObjectWithCounterEffects objectWithCounterEffects;
			if ((objectWithCounterEffects = (characterObject as IObjectWithCounterEffects)) != null)
			{
				if (FightSpellEffectFactory.isReady && FightSpellEffectFactory.TryGetFloatingCounterEffect(CaracId.FloatingCounterSight, property, out FloatingCounterEffect floatingEffectCounter))
				{
					yield return objectWithCounterEffects.ChangeFloatingCounterEffect(floatingEffectCounter);
				}
			}
			else
			{
				Log.Warning(string.Format("Try to set a {0} on {1}, which is not a {2}", "SightPropertyId", characterObject, "IObjectWithCounterEffects"), 209, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PropertyChangedEvent.cs");
			}
		}
	}
}
