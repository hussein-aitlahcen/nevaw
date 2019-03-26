using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.States;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class FightEndedEvent : FightEvent
	{
		private readonly List<IEntityWithBoardPresence> m_removedEntities = new List<IEntityWithBoardPresence>();

		public int? winningTeamId
		{
			get;
			private set;
		}

		public IReadOnlyList<int> winningPlayers
		{
			get;
			private set;
		}

		public FightEndedEvent(int eventId, int? parentEventId, int? winningTeamId, IReadOnlyList<int> winningPlayers)
			: base(FightEventData.Types.EventType.FightEnded, eventId, parentEventId)
		{
			this.winningTeamId = winningTeamId;
			this.winningPlayers = winningPlayers;
		}

		public FightEndedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.FightEnded, proto)
		{
			winningTeamId = proto.OptInt1;
			winningPlayers = (IReadOnlyList<int>)proto.IntList1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			fightStatus.endReason = GetEndReason(fightStatus);
			switch (GameStatus.fightType)
			{
			case FightType.TeamVersus:
				foreach (IEntityWithBoardPresence item in fightStatus.EnumerateEntities<IEntityWithBoardPresence>())
				{
					IEntityWithOwner entityWithOwner;
					if ((entityWithOwner = (item as IEntityWithOwner)) == null || entityWithOwner.teamId != winningTeamId)
					{
						m_removedEntities.Add(item);
						fightStatus.RemoveEntity(item.id);
					}
				}
				break;
			case FightType.BossFight:
				foreach (IEntityWithBoardPresence item2 in fightStatus.EnumerateEntities<IEntityWithBoardPresence>())
				{
					m_removedEntities.Add(item2);
					fightStatus.RemoveEntity(item2.id);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (fightStatus == FightStatus.local)
			{
				FightMap current3 = FightMap.current;
				if (null != current3)
				{
					current3.Stop();
				}
				FightUIRework instance = FightUIRework.instance;
				if (null != instance)
				{
					instance.SetResignButtonEnabled(value: false);
				}
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			MonoBehaviour monoBehaviour = Main.monoBehaviour;
			List<IEntityWithBoardPresence> removedEntities = m_removedEntities;
			int count = removedEntities.Count;
			for (int i = 0; i < count; i++)
			{
				IEntityWithBoardPresence entity = removedEntities[i];
				MonoBehaviourExtensions.StartCoroutineImmediateSafe(monoBehaviour, RemoveEntityFromBoard(entity), null);
			}
			if (GameStatus.fightType == FightType.BossFight && fightStatus.endReason == FightStatusEndReason.Lose)
			{
				FightMap current = FightMap.current;
				if (null != current)
				{
					if (fightStatus.TryGetEntity((PlayerStatus p) => p.teamIndex == GameStatus.localPlayerTeamIndex, out PlayerStatus entityStatus))
					{
						HeroStatus heroStatus = entityStatus.heroStatus;
						if (heroStatus != null)
						{
							Vector2Int refCoord = heroStatus.area.refCoord;
							current.AddHeroLostFeedback(refCoord);
						}
					}
					MonoBehaviourExtensions.StartCoroutineImmediateSafe(monoBehaviour, current.ClearMonsterSpawnCells(fightStatus.fightId), null);
				}
			}
			if (fightStatus == FightStatus.local && !GameStatus.hasEnded)
			{
				yield return DisplayFightResultFeedback(fightStatus);
			}
		}

		private FightStatusEndReason GetEndReason(FightStatus fightStatus)
		{
			int count = winningPlayers.Count;
			if (count == 0)
			{
				return FightStatusEndReason.Draw;
			}
			int localPlayerId = fightStatus.localPlayerId;
			for (int i = 0; i < count; i++)
			{
				if (winningPlayers[i] == localPlayerId)
				{
					return FightStatusEndReason.Win;
				}
			}
			return FightStatusEndReason.Lose;
		}

		private static IEnumerator DisplayFightResultFeedback(FightStatus fightStatus)
		{
			switch (GameStatus.fightType)
			{
			case FightType.None:
			case FightType.Versus:
				yield break;
			case FightType.BossFight:
				if (fightStatus.endReason != FightStatusEndReason.Lose)
				{
					yield break;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case FightType.TeamVersus:
				break;
			}
			FightState instance = FightState.instance;
			if (instance != null)
			{
				yield return instance.ShowFightEndFeedback(fightStatus.endReason);
			}
			else
			{
				Log.Error("Could not find fight state.", 189, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FightEndedEvent.cs");
			}
		}

		private static IEnumerator RemoveEntityFromBoard(IEntityWithBoardPresence entity)
		{
			IsoObject view = entity.view;
			ICharacterObject characterObject;
			if ((characterObject = (view as ICharacterObject)) != null)
			{
				yield return characterObject.Die();
			}
			view.DetachFromCell();
			view.Destroy();
		}
	}
}
