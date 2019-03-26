using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.FightAdminProtocol;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class KillEntityCommand : ContinuousFightAdminCommand
	{
		public KillEntityCommand(KeyCode keycode)
			: base("Kill", keycode)
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
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					Kill = new AdminRequestCmd.Types.KillAdminCmd
					{
						TargetEntityId = entity.id
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
