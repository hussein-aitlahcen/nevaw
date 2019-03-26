using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Events
{
	public class CompanionAddedInReserveEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public IReadOnlyList<int> companionDefId
		{
			get;
			private set;
		}

		public IReadOnlyList<int> levels
		{
			get;
			private set;
		}

		public CompanionAddedInReserveEvent(int eventId, int? parentEventId, int concernedEntity, IReadOnlyList<int> companionDefId, IReadOnlyList<int> levels)
			: base(FightEventData.Types.EventType.CompanionAddedInReserve, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.companionDefId = companionDefId;
			this.levels = levels;
		}

		public CompanionAddedInReserveEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.CompanionAddedInReserve, proto)
		{
			concernedEntity = proto.Int1;
			companionDefId = (IReadOnlyList<int>)proto.IntList1;
			levels = (IReadOnlyList<int>)proto.IntList2;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				entityStatus.SetAvailableCompanions(companionDefId, levels);
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					int count = companionDefId.Count;
					for (int i = 0; i < count; i++)
					{
						view.AddCompanionStatus(companionDefId[i], levels[i], i);
					}
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 32, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionAddedInReserveEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (!(null != view))
				{
					yield break;
				}
				int count = companionDefId.Count;
				if (count > 0)
				{
					IEnumerator[] array = new IEnumerator[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = view.AddCompanion(companionDefId[i], levels[i], i);
					}
					yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(array);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 59, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionAddedInReserveEvent.cs");
			}
		}
	}
}
