using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class EntityAnimationEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int animationKey
		{
			get;
			private set;
		}

		public IReadOnlyList<CellCoord> additionalCoords
		{
			get;
			private set;
		}

		public EntityAnimationEvent(int eventId, int? parentEventId, int concernedEntity, int animationKey, IReadOnlyList<CellCoord> additionalCoords)
			: base(FightEventData.Types.EventType.EntityAnimation, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.animationKey = animationKey;
			this.additionalCoords = additionalCoords;
		}

		public EntityAnimationEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EntityAnimation, proto)
		{
			concernedEntity = proto.Int1;
			animationKey = proto.Int2;
			additionalCoords = (IReadOnlyList<CellCoord>)proto.CellCoordList1;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			switch (animationKey)
			{
			case 1:
				return DoAction(fightStatus);
			case 2:
				return DoRangedAction(fightStatus);
			case 3:
				return DoActivation(fightStatus);
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private IEnumerator DoAction(FightStatus fightStatus)
		{
			IEntityWithBoardPresence entityStatus;
			IObjectWithAction objectWithAction;
			if (additionalCoords.Count < 1)
			{
				Log.Error(string.Format("{0} with key {1} has not supplied an additional coordinate.", "EntityAnimationEvent", EntityAnimationKey.Attack), 35, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAnimationEvent.cs");
			}
			else if (!fightStatus.TryGetEntity(concernedEntity, out entityStatus))
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 41, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAnimationEvent.cs");
			}
			else if ((objectWithAction = (entityStatus.view as IObjectWithAction)) != null)
			{
				Vector2Int coords = objectWithAction.cellObject.coords;
				yield return objectWithAction.DoAction(coords, (Vector2Int)additionalCoords[0]);
			}
			else
			{
				Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAction>(entityStatus), 52, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAnimationEvent.cs");
			}
		}

		private IEnumerator DoRangedAction(FightStatus fightStatus)
		{
			IEntityWithBoardPresence entityStatus;
			IObjectWithAction objectWithAction;
			if (additionalCoords.Count < 1)
			{
				Log.Error(string.Format("{0} with key {1} has not supplied an additional coordinate.", "EntityAnimationEvent", EntityAnimationKey.Attack), 60, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAnimationEvent.cs");
			}
			else if (!fightStatus.TryGetEntity(concernedEntity, out entityStatus))
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 66, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAnimationEvent.cs");
			}
			else if ((objectWithAction = (entityStatus.view as IObjectWithAction)) != null)
			{
				Vector2Int coords = objectWithAction.cellObject.coords;
				yield return objectWithAction.DoRangedAction(coords, (Vector2Int)additionalCoords[0]);
			}
			else
			{
				Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAction>(entityStatus), 77, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAnimationEvent.cs");
			}
		}

		private IEnumerator DoActivation(FightStatus fightStatus)
		{
			IObjectWithActivationAnimation objectWithActivationAnimation;
			if (!fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 85, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAnimationEvent.cs");
			}
			else if ((objectWithActivationAnimation = (entityStatus.view as IObjectWithActivationAnimation)) != null)
			{
				Vector2Int coords = objectWithActivationAnimation.cellObject.coords;
				yield return objectWithActivationAnimation.AnimateActivation(coords);
			}
			else
			{
				Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithActivationAnimation>(entityStatus), 96, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAnimationEvent.cs");
			}
		}
	}
}
