using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
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
	public class EntityAreaMovedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int movementType
		{
			get;
			private set;
		}

		public int direction
		{
			get;
			private set;
		}

		public IReadOnlyList<CellCoord> cells
		{
			get;
			private set;
		}

		public EntityAreaMovedEvent(int eventId, int? parentEventId, int concernedEntity, int movementType, int direction, IReadOnlyList<CellCoord> cells)
			: base(FightEventData.Types.EventType.EntityAreaMoved, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.movementType = movementType;
			this.direction = direction;
			this.cells = cells;
		}

		public EntityAreaMovedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EntityAreaMoved, proto)
		{
			concernedEntity = proto.Int1;
			movementType = proto.Int2;
			direction = proto.Int3;
			cells = (IReadOnlyList<CellCoord>)proto.CellCoordList1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				Area area = entityStatus.area;
				int count = cells.Count;
				Vector2Int val = (Vector2Int)cells[0];
				Vector2Int newRefCoords = (Vector2Int)cells[count - 1];
				entityStatus.area.MoveTo(newRefCoords);
				fightStatus.NotifyEntityAreaMoved();
				CharacterStatus characterStatus;
				if (IsMovementAction() && (characterStatus = (entityStatus as CharacterStatus)) != null)
				{
					characterStatus.actionUsed = true;
					fightStatus.NotifyEntityPlayableStateChanged();
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 46, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.EntityMoved);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			int fightId = fightStatus.fightId;
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entity))
			{
				IsoObject isoObject = entity.view;
				if (null != isoObject)
				{
					switch (movementType)
					{
					case 1:
					{
						IObjectWithMovement objectWithMovement6;
						if ((objectWithMovement6 = (isoObject as IObjectWithMovement)) != null)
						{
							yield return objectWithMovement6.Move(GetPath());
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithMovement>(entity), 71, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
						}
						break;
					}
					case 3:
					{
						IMovableObject movableObject;
						IMovableObject objectWithMovement3 = movableObject = (isoObject as IMovableObject);
						if (movableObject != null)
						{
							FightContext fightContext = fightStatus.context;
							yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.TeleportationStart, fightId, parentEventId, isoObject, fightContext);
							objectWithMovement3.Teleport(GetDestination());
							yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.TeleportationEnd, fightId, parentEventId, isoObject, fightContext);
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithMovement>(entity), 88, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
						}
						break;
					}
					case 6:
					{
						IMovableObject movableObject;
						IMovableObject objectWithMovement3 = movableObject = (isoObject as IMovableObject);
						if (movableObject != null)
						{
							Vector2Int[] path2 = GetPath();
							Quaternion pathRotation2 = GetPathRotation(path2);
							Transform transform2 = isoObject.cellObject.get_transform();
							ITimelineContextProvider contextProvider2 = isoObject as ITimelineContextProvider;
							yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.Push, fightId, parentEventId, transform2, pathRotation2, Vector3.get_one(), fightStatus.context, contextProvider2);
							yield return objectWithMovement3.Push(path2);
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IMovableObject>(entity), 108, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
						}
						break;
					}
					case 7:
					{
						IMovableObject movableObject;
						IMovableObject objectWithMovement3 = movableObject = (isoObject as IMovableObject);
						if (movableObject != null)
						{
							Vector2Int[] path2 = GetPath();
							Quaternion pathRotation = GetPathRotation(path2);
							Transform transform = isoObject.cellObject.get_transform();
							ITimelineContextProvider contextProvider = isoObject as ITimelineContextProvider;
							yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.Pull, fightId, parentEventId, transform, pathRotation, Vector3.get_one(), fightStatus.context, contextProvider);
							yield return objectWithMovement3.Pull(path2);
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IMovableObject>(entity), 128, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
						}
						break;
					}
					case 2:
					case 4:
					{
						IObjectWithMovement objectWithMovement5;
						if ((objectWithMovement5 = (isoObject as IObjectWithMovement)) != null)
						{
							Direction direction2 = (Direction)this.direction;
							yield return objectWithMovement5.MoveToAction(GetPath(), direction2);
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithMovement>(entity), 143, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
						}
						break;
					}
					case 8:
					{
						IObjectWithMovement objectWithMovement4;
						if ((objectWithMovement4 = (isoObject as IObjectWithMovement)) != null)
						{
							Direction direction = (Direction)this.direction;
							yield return objectWithMovement4.MoveToAction(GetPath(), direction, hasFollowUpAnimation: false);
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithMovement>(entity), 157, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
						}
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
					}
					if (IsMovementAction())
					{
						IObjectWithAction objectWithAction;
						if ((objectWithAction = (isoObject as IObjectWithAction)) != null)
						{
							objectWithAction.SetActionUsed(actionUsed: true, turnEnded: false);
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAction>(entity), 174, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
						}
					}
					ICharacterObject characterObject;
					if ((characterObject = (isoObject as ICharacterObject)) != null)
					{
						characterObject.CheckParentCellIndicator();
					}
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasNoView(entity), 185, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 190, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightId, EventCategory.EntityMoved);
		}

		private bool IsMovementAction()
		{
			switch (movementType)
			{
			case 1:
			case 2:
				return true;
			case 3:
			case 4:
			case 6:
			case 7:
			case 8:
				return false;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private Vector2Int[] GetPath()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			int count = cells.Count;
			Vector2Int[] array = (Vector2Int[])new Vector2Int[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = (Vector2Int)cells[i];
			}
			return array;
		}

		private Quaternion GetPathRotation(Vector2Int[] path)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			if (path.Length > 1)
			{
				return path[0].GetDirectionTo(path[1]).GetRotation();
			}
			Log.Warning($"Movement of type {movementType} sent with an invalid path length ({path.Length}).", 237, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityAreaMovedEvent.cs");
			return Quaternion.get_identity();
		}

		private Vector2Int GetDestination()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			int count = cells.Count;
			return (Vector2Int)cells[count - 1];
		}
	}
}
