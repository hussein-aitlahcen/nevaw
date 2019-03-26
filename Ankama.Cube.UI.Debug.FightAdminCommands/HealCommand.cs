using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.FightAdminProtocol;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class HealCommand : ContinuousFightAdminCommand
	{
		public HealCommand(KeyCode keycode)
			: base("Heal <i>(⇧: 10), Alt: physical)</i>", keycode)
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
					Heal = new AdminRequestCmd.Types.HealAdminCmd
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
