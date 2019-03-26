using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.FightAdminProtocol;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class TeleportCommand : ContinuousFightAdminCommand
	{
		private Tuple<IEntityWithBoardPresence, Coord> m_movingEntity;

		private IEntityWithBoardPresence m_selectedEntity;

		public TeleportCommand(KeyCode keycode)
			: base("Teleport", keycode)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		protected override void Start()
		{
			SetTargetingPhase(null);
		}

		private void OnEntitySelected(Target? obj)
		{
			if (!obj.HasValue)
			{
				return;
			}
			Target value = obj.Value;
			if (value.type == Target.Type.Entity)
			{
				IEntityWithBoardPresence entityWithBoardPresence = value.entity as IEntityWithBoardPresence;
				if (entityWithBoardPresence != null)
				{
					SetTargetingPhase(entityWithBoardPresence);
				}
			}
		}

		private void OnDestinationSelected(Target? obj)
		{
			if (!obj.HasValue)
			{
				return;
			}
			Target value = obj.Value;
			if (value.type == Target.Type.Coord)
			{
				if (m_selectedEntity == null)
				{
					SetTargetingPhase(null);
					return;
				}
				Coord coord = value.coord;
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					Teleport = new AdminRequestCmd.Types.TeleportAdminCmd
					{
						TargetEntityId = m_selectedEntity.id,
						Destination = coord.ToCellCoord()
					}
				});
				m_movingEntity = new Tuple<IEntityWithBoardPresence, Coord>(m_selectedEntity, coord);
				m_selectedEntity = null;
				FightMap fightMap = m_fightMap;
				fightMap.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap.onTargetSelected, new Action<Target?>(OnDestinationSelected));
				EndTargetingPhase();
			}
		}

		protected override void Update()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (m_movingEntity != null)
			{
				IEntityWithBoardPresence item = m_movingEntity.Item1;
				if (m_movingEntity.Item2.Equals(item.area.refCoord))
				{
					SetTargetingPhase(null);
					m_movingEntity = null;
				}
			}
		}

		protected override void Stop()
		{
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap.onTargetSelected, new Action<Target?>(OnEntitySelected));
			FightMap fightMap2 = m_fightMap;
			fightMap2.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap2.onTargetSelected, new Action<Target?>(OnDestinationSelected));
			EndTargetingPhase();
			m_movingEntity = null;
		}

		private void SetTargetingPhase(IEntityWithBoardPresence entity)
		{
			if (entity == null)
			{
				m_fightMap.SetTargetingPhase(EnumerateEntitiesAsTargets<IEntityWithBoardPresence>());
				FightMap fightMap = m_fightMap;
				fightMap.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap.onTargetSelected, new Action<Target?>(OnDestinationSelected));
				FightMap fightMap2 = m_fightMap;
				fightMap2.onTargetSelected = (Action<Target?>)Delegate.Combine(fightMap2.onTargetSelected, new Action<Target?>(OnEntitySelected));
			}
			else
			{
				m_fightMap.SetTargetingPhase(EnumerateValidCellsFor(entity));
				FightMap fightMap3 = m_fightMap;
				fightMap3.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap3.onTargetSelected, new Action<Target?>(OnEntitySelected));
				FightMap fightMap4 = m_fightMap;
				fightMap4.onTargetSelected = (Action<Target?>)Delegate.Combine(fightMap4.onTargetSelected, new Action<Target?>(OnDestinationSelected));
			}
			m_selectedEntity = entity;
		}
	}
}
