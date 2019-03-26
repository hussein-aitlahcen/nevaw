using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Fight.Events;
using Ankama.Cube.Network;
using Ankama.Cube.Protocols.FightProtocol;
using Ankama.Cube.UI;
using Google.Protobuf;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight
{
	public sealed class FightFrame : CubeMessageFrame
	{
		private readonly List<FightEvent> m_fightEventBuffer = new List<FightEvent>();

		public Action<int> onOtherPlayerLeftFight;

		public Action<FightSnapshot> onFightSnapshot;

		public FightFrame()
		{
			base.WhenReceiveEnqueue<FightEventsEvent>((Action<FightEventsEvent>)OnFightEventsEvent);
			base.WhenReceiveEnqueue<CommandHandledEvent>((Action<CommandHandledEvent>)OnCommandHandledEvent);
			base.WhenReceiveEnqueue<PlayerLeftFightEvent>((Action<PlayerLeftFightEvent>)OnPlayerLeftFightEvent);
			base.WhenReceiveEnqueue<FightSnapshotEvent>((Action<FightSnapshotEvent>)OnFightSnapshotEvent);
		}

		private void OnPlayerLeftFightEvent(PlayerLeftFightEvent obj)
		{
			onOtherPlayerLeftFight?.Invoke(obj.PlayerId);
		}

		private void OnFightSnapshotEvent(FightSnapshotEvent snapshotEvent)
		{
			if (onFightSnapshot != null)
			{
				RepeatedField<FightSnapshot> fightsSnapshots = snapshotEvent.FightsSnapshots;
				int count = fightsSnapshots.get_Count();
				for (int i = 0; i < count; i++)
				{
					onFightSnapshot(fightsSnapshots.get_Item(i));
				}
			}
		}

		private void OnFightEventsEvent(FightEventsEvent obj)
		{
			m_fightEventBuffer.Clear();
			RepeatedField<FightEventData> events = obj.Events;
			int count = events.get_Count();
			for (int i = 0; i < count; i++)
			{
				FightEventData proto = events.get_Item(i);
				m_fightEventBuffer.Add(FightEventFactory.FromProto(proto));
			}
			FightLogicExecutor.ProcessFightEvents(obj.FightId, m_fightEventBuffer);
		}

		private static void OnCommandHandledEvent(CommandHandledEvent obj)
		{
			UIManager instance = UIManager.instance;
			if (null != instance)
			{
				instance.ReleaseUserInteractionLock();
			}
			switch (FightCastManager.currentCastType)
			{
			case FightCastManager.CurrentCastType.None:
				break;
			case FightCastManager.CurrentCastType.Spell:
				FightCastManager.StopCastingSpell(cancelled: true);
				break;
			case FightCastManager.CurrentCastType.Companion:
				FightCastManager.StopInvokingCompanion(cancelled: true);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void SendFightAdminCommand(IMessage cmd)
		{
			SendCommandAndLockUserInteraction(cmd);
		}

		public void SendEntityMovement(int entityId, Vector2Int[] path)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			MoveEntityCmd moveEntityCmd = new MoveEntityCmd
			{
				EntityId = entityId
			};
			int num = path.Length;
			for (int i = 1; i < num; i++)
			{
				Vector2Int value = path[i];
				moveEntityCmd.Path.Add(value.ToCellCoord());
			}
			SendCommandAndLockUserInteraction(moveEntityCmd);
		}

		public void SendEntityAttack(int attackerId, Vector2Int[] path, int defenderId)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			MoveEntityCmd moveEntityCmd = new MoveEntityCmd
			{
				EntityId = attackerId,
				EntityToAttackId = defenderId
			};
			int num = path.Length;
			for (int i = 1; i < num; i++)
			{
				Vector2Int value = path[i];
				moveEntityCmd.Path.Add(value.ToCellCoord());
			}
			SendCommandAndLockUserInteraction(moveEntityCmd);
		}

		public void SendSpell(int spellInstanceId, Target[] targets)
		{
			PlaySpellCmd playSpellCmd = new PlaySpellCmd
			{
				SpellId = spellInstanceId
			};
			int num = targets.Length;
			for (int i = 0; i < num; i++)
			{
				Target target = targets[i];
				playSpellCmd.CastTargets.Add(target.ToCastTarget());
			}
			SendCommandAndLockUserInteraction(playSpellCmd);
		}

		public void SendInvokeCompanion(int companionDefId, Coord coord)
		{
			InvokeCompanionCmd cmd = new InvokeCompanionCmd
			{
				CompanionDefId = companionDefId,
				Coords = coord.ToCellCoord()
			};
			SendCommandAndLockUserInteraction(cmd);
		}

		public void SendGiveCompanion(int fightId, int playerId, int companionDefinitionId)
		{
			GiveCompanionCmd cmd = new GiveCompanionCmd
			{
				CompanionDefId = companionDefinitionId,
				TargetFightId = fightId,
				TargetPlayerId = playerId
			};
			SendCommandAndLockUserInteraction(cmd);
		}

		public void SendUseReserve()
		{
			UseReserveCmd cmd = new UseReserveCmd();
			SendCommandAndLockUserInteraction(cmd);
		}

		public void SendResign()
		{
			ResignCmd cmd = new ResignCmd();
			SendCommandAndLockUserInteraction(cmd);
		}

		public void SendTurnEnd(int turnIndex)
		{
			EndOfTurnCmd cmd = new EndOfTurnCmd
			{
				TurnIndex = turnIndex
			};
			SendCommandAndLockUserInteraction(cmd);
		}

		public void SendPlayerReady()
		{
			PlayerReadyCmd message = new PlayerReadyCmd();
			m_connection.Write(message);
		}

		public void SendLeave()
		{
			LeaveCmd message = new LeaveCmd();
			m_connection.Write(message);
		}

		public void SendFightSnapshotRequest()
		{
			GetFightSnapshotCmd message = new GetFightSnapshotCmd();
			m_connection.Write(message);
		}

		private void SendCommandAndLockUserInteraction(IMessage cmd)
		{
			UIManager instance = UIManager.instance;
			if (null != instance)
			{
				instance.LockUserInteraction();
			}
			m_connection.Write(cmd);
		}

		public override void Dispose()
		{
			onFightSnapshot = null;
			onOtherPlayerLeftFight = null;
			base.Dispose();
		}
	}
}
