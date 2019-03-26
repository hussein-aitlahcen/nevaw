using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class MovementPointsChangedEvent : FightEvent, IRelatedToEntity
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

		public MovementPointsChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter)
			: base(FightEventData.Types.EventType.MovementPointsChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
		}

		public MovementPointsChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.MovementPointsChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithMovement entityStatus))
			{
				if (entityStatus.GetCarac(CaracId.MovementPoints) != valueBefore)
				{
					Log.Warning($"The previous movement points value ({entityStatus.GetCarac(CaracId.MovementPoints)}) for entity with id {concernedEntity} doesn't match the value in the event ({valueBefore}).", 18, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MovementPointsChangedEvent.cs");
				}
				entityStatus.SetCarac(CaracId.MovementPoints, valueAfter);
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithMovement>(concernedEntity), 25, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MovementPointsChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.MovementPointsChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			int fightId = fightStatus.fightId;
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithMovement entityStatus))
			{
				IsoObject isoObject = entityStatus.view;
				if (null != isoObject)
				{
					IObjectWithMovement objectWithMovement;
					if ((objectWithMovement = (isoObject as IObjectWithMovement)) != null)
					{
						objectWithMovement.SetMovementPoints(valueAfter);
					}
					else
					{
						Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithMovement>(entityStatus), 46, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MovementPointsChangedEvent.cs");
					}
					if (valueAfter > valueBefore)
					{
						yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.MovementPointGain, fightId, parentEventId, isoObject, fightStatus.context);
						ValueChangedFeedback.Launch(valueAfter - valueBefore, ValueChangedFeedback.Type.Movement, isoObject.cellObject.get_transform());
					}
					else
					{
						yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.MovementPointLoss, fightId, parentEventId, isoObject, fightStatus.context);
						ValueChangedFeedback.Launch(valueAfter - valueBefore, ValueChangedFeedback.Type.Movement, isoObject.cellObject.get_transform());
					}
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasNoView(entityStatus), 62, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MovementPointsChangedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithMovement>(concernedEntity), 67, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MovementPointsChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightId, EventCategory.MovementPointsChanged);
		}
	}
}
