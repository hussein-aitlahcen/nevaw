using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Events
{
	public class FloorMechanismActivationEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int activator
		{
			get;
			private set;
		}

		public IReadOnlyList<int> entitiesInAssemblage
		{
			get;
			private set;
		}

		public FloorMechanismActivationEvent(int eventId, int? parentEventId, int concernedEntity, int activator, IReadOnlyList<int> entitiesInAssemblage)
			: base(FightEventData.Types.EventType.FloorMechanismActivation, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.activator = activator;
			this.entitiesInAssemblage = entitiesInAssemblage;
		}

		public FloorMechanismActivationEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.FloorMechanismActivation, proto)
		{
			concernedEntity = proto.Int1;
			activator = proto.Int2;
			entitiesInAssemblage = (IReadOnlyList<int>)proto.IntList1;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out FloorMechanismStatus entityStatus))
			{
				if (fightStatus.TryGetEntity(activator, out IEntityWithTeam entityStatus2))
				{
					if (entityStatus2.teamId == entityStatus.teamId)
					{
						IObjectWithActivation objectWithActivation;
						if ((objectWithActivation = (entityStatus.view as IObjectWithActivation)) != null)
						{
							yield return objectWithActivation.ActivatedByAlly();
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithActivation>(entityStatus), 26, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismActivationEvent.cs");
						}
						yield break;
					}
					IReadOnlyList<int> entitiesInAssemblage = this.entitiesInAssemblage;
					int count = entitiesInAssemblage.Count;
					if (count == 1)
					{
						IObjectWithActivation objectWithActivation2;
						if ((objectWithActivation2 = (entityStatus.view as IObjectWithActivation)) != null)
						{
							yield return objectWithActivation2.ActivatedByOpponent();
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithActivation>(entityStatus), 41, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismActivationEvent.cs");
						}
						yield break;
					}
					IEnumerator[] array = new IEnumerator[count];
					for (int i = 0; i < count; i++)
					{
						if (fightStatus.TryGetEntity(entitiesInAssemblage[i], out FloorMechanismStatus entityStatus3))
						{
							IObjectWithActivation objectWithActivation3;
							if ((objectWithActivation3 = (entityStatus3.view as IObjectWithActivation)) != null)
							{
								array[i] = objectWithActivation3.ActivatedByOpponent();
							}
							else
							{
								Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithActivation>(entityStatus3), 57, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismActivationEvent.cs");
							}
						}
						else
						{
							Log.Error(FightEventErrors.EntityNotFound<FloorMechanismStatus>(entitiesInAssemblage[i]), 62, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismActivationEvent.cs");
						}
					}
					yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(array);
				}
				else
				{
					Log.Error(FightEventErrors.EntityNotFound<IEntityWithTeam>(activator), 72, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismActivationEvent.cs");
				}
			}
			else
			{
				Log.Error($"Could not find entity with id {concernedEntity}.", 77, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismActivationEvent.cs");
			}
		}
	}
}
