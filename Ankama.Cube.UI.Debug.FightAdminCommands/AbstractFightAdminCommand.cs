using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.FightAdminProtocol;
using Ankama.Cube.States;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public abstract class AbstractFightAdminCommand
	{
		public readonly KeyCode key;

		public readonly string name;

		protected readonly FightStatus m_fightStatus;

		protected readonly FightMap m_fightMap;

		protected AbstractFightAdminCommand(string name, KeyCode key)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			this.key = key;
			this.name = name;
			m_fightStatus = FightStatus.local;
			m_fightMap = FightMap.current;
		}

		protected static void SendAdminCommand(AdminRequestCmd cmd)
		{
			UIManager instance = UIManager.instance;
			if (!(null != instance) || !instance.userInteractionLocked)
			{
				FightState.instance?.frame?.SendFightAdminCommand(cmd);
			}
		}

		protected static bool IsAltDown()
		{
			if (!Input.GetKey(308))
			{
				return Input.GetKey(307);
			}
			return true;
		}

		protected static bool IsShiftDown()
		{
			if (!Input.GetKey(304))
			{
				return Input.GetKey(303);
			}
			return true;
		}

		protected PlayerStatus GetPlayerOrOpponent(Event lastEvent = null)
		{
			if ((lastEvent != null) ? lastEvent.get_alt() : IsAltDown())
			{
				int localPlayerId = m_fightStatus.localPlayerId;
				foreach (PlayerStatus item in m_fightStatus.EnumeratePlayers())
				{
					if (item.id != localPlayerId)
					{
						return item;
					}
				}
				return null;
			}
			return m_fightStatus.GetLocalPlayer();
		}

		protected IEnumerable<Target> EnumerateEntitiesAsTargets<T>() where T : class, IEntity
		{
			return from r in m_fightStatus.EnumerateEntities<T>()
				select new Target(r);
		}

		protected IEnumerable<Target> EnumerateValidCellsFor(IEntityWithBoardPresence entity)
		{
			return from r in (entity is MechanismStatus) ? CellValidForMechanismFilter.EnumerateCells(m_fightStatus) : CellValidForCharacterFilter.EnumerateCells(m_fightStatus)
				select new Target(r);
		}

		public abstract bool Handle();

		public abstract bool IsRunning();

		protected void EndTargetingPhase()
		{
			FightStatus local = FightStatus.local;
			if (local.currentTurnPlayerId == local.localPlayerId)
			{
				if (m_fightMap.IsInTargetingPhase())
				{
					m_fightMap.SetMovementPhase();
				}
			}
			else
			{
				m_fightMap.SetNoInteractionPhase();
			}
		}
	}
}
