using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class ElementPointsChangedEvent : FightEvent, IRelatedToEntity
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

		public int element
		{
			get;
			private set;
		}

		public ElementPointsChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter, int element)
			: base(FightEventData.Types.EventType.ElementPointsChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
			this.element = element;
		}

		public ElementPointsChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.ElementPointsChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
			element = proto.Int4;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				CaracId element = (CaracId)this.element;
				if (entityStatus.GetCarac(element) != valueBefore)
				{
					Log.Warning($"The previous element points value ({entityStatus.GetCarac(element)}) for player with id {concernedEntity} doesn't match the value in the event ({valueBefore}).", 20, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ElementPointsChangedEvent.cs");
				}
				entityStatus.SetCarac(element, valueAfter);
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.RefreshAvailableCompanions();
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 33, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ElementPointsChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.ElementPointsChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					switch (element)
					{
					case 14:
						view.ChangeAirElementaryPoints(valueAfter);
						break;
					case 13:
						view.ChangeEarthElementaryPoints(valueAfter);
						break;
					case 11:
						view.ChangeFireElementaryPoints(valueAfter);
						break;
					case 12:
						view.ChangeWaterElementaryPoints(valueAfter);
						break;
					default:
						throw new ArgumentException();
					}
					yield return view.UpdateAvailableCompanions();
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 70, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ElementPointsChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.ElementPointsChanged);
		}
	}
}
