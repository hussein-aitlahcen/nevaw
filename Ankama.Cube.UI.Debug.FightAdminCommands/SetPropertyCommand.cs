using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.FightAdminProtocol;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class SetPropertyCommand : ContinuousFightAdminCommand
	{
		private readonly DebugSelectorProperty m_propertySelector;

		public SetPropertyCommand(KeyCode keycode, DebugSelectorProperty propertySelector)
			: base("Set Property <i>(⇧: remove)</i>", keycode)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_propertySelector = propertySelector;
		}

		protected override void Start()
		{
			m_propertySelector.SetActive(active: true);
			SetTargetingPhase();
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Combine(fightMap.onTargetSelected, new Action<Target?>(OnEntitySelected));
		}

		protected override void Stop()
		{
			m_propertySelector.SetActive(active: false);
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap.onTargetSelected, new Action<Target?>(OnEntitySelected));
			EndTargetingPhase();
		}

		private void OnEntitySelected(Target? obj)
		{
			IEntity entity = obj?.entity;
			if (entity != null)
			{
				bool active = !AbstractFightAdminCommand.IsShiftDown();
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					SetProperty = new AdminRequestCmd.Types.SetPropertyCmd
					{
						TargetEntityId = entity.id,
						PropertyId = (int)m_propertySelector.selected,
						Active = active
					}
				});
				SetTargetingPhase();
			}
		}

		private void SetTargetingPhase()
		{
			m_fightMap.SetTargetingPhase(EnumerateEntitiesAsTargets<IEntity>());
		}
	}
}
