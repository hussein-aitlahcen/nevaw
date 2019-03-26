using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.FightAdminProtocol;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class ApplyElementaryStateCommand : ContinuousFightAdminCommand
	{
		private readonly DebugSelectorElementaryState m_elementaryStateSelector;

		public ApplyElementaryStateCommand(KeyCode keycode, DebugSelectorElementaryState elementaryStateSelector)
			: base("Set Elementary State <i>(⇧: remove)</i>", keycode)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_elementaryStateSelector = elementaryStateSelector;
		}

		protected override void Start()
		{
			m_elementaryStateSelector.SetActive(active: true);
			SetTargetingPhase();
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Combine(fightMap.onTargetSelected, new Action<Target?>(OnEntitySelected));
		}

		protected override void Stop()
		{
			m_elementaryStateSelector.SetActive(active: false);
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap.onTargetSelected, new Action<Target?>(OnEntitySelected));
			EndTargetingPhase();
		}

		private void OnEntitySelected(Target? obj)
		{
			IEntity entity = obj?.entity;
			if (entity != null)
			{
				SetTargetingPhase();
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					SetElementaryState = new AdminRequestCmd.Types.SetElementaryStateAdminCmd
					{
						TargetEntityId = entity.id,
						ElementaryStateId = (int)(AbstractFightAdminCommand.IsShiftDown() ? ElementaryStates.None : m_elementaryStateSelector.selected)
					}
				});
			}
		}

		private void SetTargetingPhase()
		{
			m_fightMap.SetTargetingPhase(EnumerateEntitiesAsTargets<IEntity>());
		}
	}
}
