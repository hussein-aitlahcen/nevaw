using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class AssemblageChangedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public IReadOnlyList<int> allEntities
		{
			get;
			private set;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithAssemblage entityStatus))
			{
				entityStatus.assemblingIds = allEntities;
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithAssemblage>(concernedEntity), 20, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\AssemblageChangedEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithAssemblage entityStatus))
			{
				IObjectWithAssemblage objectWithAssemblage;
				if ((objectWithAssemblage = (entityStatus.view as IObjectWithAssemblage)) != null)
				{
					IEnumerable<Vector2Int> otherObjectInAssemblagePositions = EnumerateOtherObjectsInAssemblagePositions(fightStatus, concernedEntity, allEntities);
					objectWithAssemblage.RefreshAssemblage(otherObjectInAssemblagePositions);
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAssemblage>(entityStatus), 35, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\AssemblageChangedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithAssemblage>(concernedEntity), 40, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\AssemblageChangedEvent.cs");
			}
			yield break;
		}

		private static IEnumerable<Vector2Int> EnumerateOtherObjectsInAssemblagePositions(FightStatus fightStatus, int concernedEntity, IReadOnlyList<int> entitiesInAssemblage)
		{
			int entityCountInAssemblage = entitiesInAssemblage.Count;
			int num2;
			for (int i = 0; i < entityCountInAssemblage; i = num2)
			{
				int num = entitiesInAssemblage[i];
				if (num != concernedEntity)
				{
					if (fightStatus.TryGetEntity(num, out IEntityWithAssemblage entityStatus))
					{
						IObjectWithAssemblage objectWithAssemblage;
						if ((objectWithAssemblage = (entityStatus.view as IObjectWithAssemblage)) != null)
						{
							yield return objectWithAssemblage.cellObject.coords;
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAssemblage>(entityStatus), 65, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\AssemblageChangedEvent.cs");
						}
					}
					else
					{
						Log.Error(FightEventErrors.EntityNotFound<IEntityWithAssemblage>(num), 70, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\AssemblageChangedEvent.cs");
					}
				}
				num2 = i + 1;
			}
		}

		public AssemblageChangedEvent(int eventId, int? parentEventId, int concernedEntity, IReadOnlyList<int> allEntities)
			: base(FightEventData.Types.EventType.AssemblageChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.allEntities = allEntities;
		}

		public AssemblageChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.AssemblageChanged, proto)
		{
			concernedEntity = proto.Int1;
			allEntities = (IReadOnlyList<int>)proto.IntList1;
		}
	}
}
