using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class FloatingCounterValueChangedEvent : FightEvent, IRelatedToEntity
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

		public int floatingCounterType
		{
			get;
			private set;
		}

		public int? sightProperty
		{
			get;
			private set;
		}

		public int? counterReplaced
		{
			get;
			private set;
		}

		public FloatingCounterValueChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter, int floatingCounterType, int? sightProperty, int? counterReplaced)
			: base(FightEventData.Types.EventType.FloatingCounterValueChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
			this.floatingCounterType = floatingCounterType;
			this.sightProperty = sightProperty;
			this.counterReplaced = counterReplaced;
		}

		public FloatingCounterValueChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.FloatingCounterValueChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
			floatingCounterType = proto.Int4;
			sightProperty = proto.OptInt1;
			counterReplaced = proto.OptInt2;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntity entityStatus))
			{
				if (counterReplaced.HasValue)
				{
					entityStatus.SetCarac((CaracId)counterReplaced.Value, 0);
				}
				CaracId floatingCounterType = (CaracId)this.floatingCounterType;
				entityStatus.SetCarac(floatingCounterType, valueAfter);
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntity>(concernedEntity), 27, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloatingCounterValueChangedEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IObjectWithCounterEffects objectWithCounterEffects;
				IObjectWithCounterEffects isoObject = objectWithCounterEffects = (entityStatus.view as IObjectWithCounterEffects);
				if (objectWithCounterEffects != null)
				{
					if (counterReplaced.HasValue)
					{
						yield return isoObject.RemoveFloatingCounterEffect();
					}
					CaracId floatingCounterType = (CaracId)this.floatingCounterType;
					if (!FightSpellEffectFactory.TryGetFloatingCounterEffect(floatingCounterType, (PropertyId?)sightProperty, out FloatingCounterEffect effect))
					{
						yield break;
					}
					if (null != effect)
					{
						if (valueBefore == 0 || counterReplaced.HasValue)
						{
							yield return isoObject.InitializeFloatingCounterEffect(effect, valueAfter);
						}
						else
						{
							FloatingCounterFeedback currentFloatingCounterFeedback = isoObject.GetCurrentFloatingCounterFeedback();
							currentFloatingCounterFeedback.ChangeVisual(effect);
							yield return currentFloatingCounterFeedback.SetCount(valueAfter);
						}
						if (parentEventId.HasValue)
						{
							FightSpellEffectFactory.SetupSpellEffectOverrides(effect, fightStatus.fightId, parentEventId.Value);
						}
					}
					else
					{
						Log.Error(string.Format("No prefab defined for {0} {1}.", "FloatingCounterEffect", floatingCounterType), 68, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloatingCounterValueChangedEvent.cs");
					}
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithCounterEffects>(entityStatus), 74, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloatingCounterValueChangedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 79, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloatingCounterValueChangedEvent.cs");
			}
		}
	}
}
