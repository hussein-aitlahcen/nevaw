using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class ActionPointsChangedEvent : FightEvent, IRelatedToEntity
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

		public bool isCost
		{
			get;
			private set;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				if (entityStatus.actionPoints != valueBefore)
				{
					Log.Warning($"The previous action points value ({entityStatus.actionPoints}) for player with id {concernedEntity} doesn't match the value in the event ({valueBefore}).", 17, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ActionPointsChangedEvent.cs");
				}
				entityStatus.SetCarac(CaracId.ActionPoints, valueAfter);
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.RefreshAvailableActions(recomputeSpellCosts: false);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 30, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ActionPointsChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.ActionPointsChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.ChangeActionPoints(valueAfter);
					yield return view.UpdateAvailableActions(recomputeSpellCosts: false);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 50, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ActionPointsChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.ActionPointsChanged);
		}

		public ActionPointsChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter, bool isCost)
			: base(FightEventData.Types.EventType.ActionPointsChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
			this.isCost = isCost;
		}

		public ActionPointsChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.ActionPointsChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
			isCost = proto.Bool1;
		}
	}
}
