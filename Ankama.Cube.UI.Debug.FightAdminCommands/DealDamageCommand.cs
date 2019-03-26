using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.FightAdminProtocol;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class DealDamageCommand : ContinuousFightAdminCommand
	{
		public DealDamageCommand(KeyCode keycode)
			: base("Attack (magical) <i>(⇧: 10, Alt: physical)</i>", keycode)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		protected override void Start()
		{
			m_fightMap.SetTargetingPhase(EnumerateEntitiesAsTargets<IEntityWithLife>());
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Combine(fightMap.onTargetSelected, new Action<Target?>(OnUpdateTarget));
		}

		private static void OnUpdateTarget(Target? obj)
		{
			IEntity entity = obj?.entity;
			if (entity != null)
			{
				int quantity = (!AbstractFightAdminCommand.IsShiftDown()) ? 1 : 10;
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					DealDamage = new AdminRequestCmd.Types.DealDamageAdminCmd
					{
						TargetEntityId = entity.id,
						Magical = !AbstractFightAdminCommand.IsAltDown(),
						Quantity = quantity
					}
				});
			}
		}

		protected override void Stop()
		{
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap.onTargetSelected, new Action<Target?>(OnUpdateTarget));
			EndTargetingPhase();
		}
	}
}
