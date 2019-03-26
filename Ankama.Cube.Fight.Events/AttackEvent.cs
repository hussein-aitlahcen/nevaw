using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class AttackEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int target
		{
			get;
			private set;
		}

		public CellCoord attackerCoord
		{
			get;
			private set;
		}

		public CellCoord targetCoord
		{
			get;
			private set;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out CharacterStatus entityStatus))
			{
				IObjectWithAction objectWithAction;
				if ((objectWithAction = (entityStatus.view as IObjectWithAction)) != null)
				{
					Vector2Int position = (Vector2Int)attackerCoord;
					Vector2Int target = (Vector2Int)targetCoord;
					yield return objectWithAction.DoAction(position, target);
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAction>(entityStatus), 27, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\AttackEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<CharacterStatus>(concernedEntity), 32, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\AttackEvent.cs");
			}
		}

		public AttackEvent(int eventId, int? parentEventId, int concernedEntity, int target, CellCoord attackerCoord, CellCoord targetCoord)
			: base(FightEventData.Types.EventType.Attack, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.target = target;
			this.attackerCoord = attackerCoord;
			this.targetCoord = targetCoord;
		}

		public AttackEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.Attack, proto)
		{
			concernedEntity = proto.Int1;
			target = proto.Int2;
			attackerCoord = proto.CellCoord1;
			targetCoord = proto.CellCoord2;
		}
	}
}
