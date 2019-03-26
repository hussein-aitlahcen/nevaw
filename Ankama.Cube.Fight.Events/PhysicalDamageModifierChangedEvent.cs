using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class PhysicalDamageModifierChangedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int valueBefore
		{
			get;
			private set;
		}

		public int valueAfter
		{
			get;
			private set;
		}

		public PhysicalDamageModifierChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter)
			: base(FightEventData.Types.EventType.PhysicalDamageModifierChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
		}

		public PhysicalDamageModifierChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.PhysicalDamageModifierChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				entityStatus.SetCarac(CaracId.PhysicalDamageModifier, valueAfter);
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 20, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PhysicalDamageModifierChangedEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IsoObject view = entityStatus.view;
				if (null != view)
				{
					IObjectWithAction objectWithAction;
					if ((objectWithAction = (view as IObjectWithAction)) != null)
					{
						objectWithAction.SetPhysicalDamageBoost(valueAfter);
					}
					else
					{
						Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAction>(entityStatus), 37, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PhysicalDamageModifierChangedEvent.cs");
					}
					CellObject cellObject = view.cellObject;
					if (null != cellObject)
					{
						if (valueAfter > valueBefore)
						{
							yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.PhysicalDamageGain, fightStatus.fightId, parentEventId, view, fightStatus.context);
							ValueChangedFeedback.Launch(valueAfter - valueBefore, ValueChangedFeedback.Type.Action, cellObject.get_transform());
						}
						else
						{
							yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.PhysicalDamageLoss, fightStatus.fightId, parentEventId, view, fightStatus.context);
							ValueChangedFeedback.Launch(valueAfter - valueBefore, ValueChangedFeedback.Type.Action, cellObject.get_transform());
						}
					}
					else
					{
						Log.Warning("Tried to apply a damage modifier on target named " + view.get_name() + " (" + ((object)view).GetType().Name + ") but the target is no longer on the board.", 56, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PhysicalDamageModifierChangedEvent.cs");
					}
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasNoView(entityStatus), 61, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PhysicalDamageModifierChangedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 66, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PhysicalDamageModifierChangedEvent.cs");
			}
		}
	}
}
