using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.FightAdminProtocol;
using DataEditor;
using System;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class InvokeCreatureCommand : ToggleFightAdminCommand
	{
		private readonly DebugDropperCreature m_creatureDropper;

		private bool m_targeting;

		private int m_level;

		public InvokeCreatureCommand(KeyCode keycode, DebugDropperCreature creatureDropper)
			: base("Invoke Creature <i>(Alt: opponent)</i>", keycode)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_creatureDropper = creatureDropper;
		}

		protected override void Start()
		{
			m_targeting = false;
			m_creatureDropper.OnSelected += OnDefinitionSelected;
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Combine(fightMap.onTargetSelected, new Action<Target?>(InvokeCreatureAt));
			m_creatureDropper.SetCloseKeyCode(27);
			m_creatureDropper.SetSelected(null);
			m_creatureDropper.SetActive(active: true);
			m_creatureDropper.SetLocalPlayerLevel();
		}

		private void OnDefinitionSelected(CharacterDefinition definition, int level, Event lastEvent)
		{
			m_creatureDropper.SetSelected(definition);
			m_level = level;
		}

		protected override bool Update()
		{
			if (m_fightStatus.localPlayerId != m_fightStatus.currentTurnPlayerId)
			{
				return false;
			}
			if (m_creatureDropper.selected != null && !m_targeting)
			{
				m_targeting = true;
			}
			if (m_targeting)
			{
				CharacterDefinition selected = m_creatureDropper.selected;
				m_fightMap.SetTargetingPhase(from r in CellValidForCharacterFilter.EnumerateCells(m_fightStatus)
					select new Target(r));
			}
			return m_creatureDropper.get_isActiveAndEnabled();
		}

		private void InvokeCreatureAt(Target? target)
		{
			CharacterDefinition selected = m_creatureDropper.selected;
			if (target.HasValue && selected != null)
			{
				AdminRequestCmd adminRequestCmd = CreateRequest(GetPlayerOrOpponent(), selected, m_level, target.Value.coord.ToCellCoord());
				if (adminRequestCmd != null)
				{
					AbstractFightAdminCommand.SendAdminCommand(adminRequestCmd);
				}
			}
			m_targeting = false;
			m_creatureDropper.SetActive(active: false);
		}

		protected override void Stop()
		{
			m_creatureDropper.OnSelected -= OnDefinitionSelected;
			m_creatureDropper.SetSelected(null);
			m_creatureDropper.SetActive(active: false);
			m_targeting = false;
			EndTargetingPhase();
			FightMap fightMap = m_fightMap;
			fightMap.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap.onTargetSelected, new Action<Target?>(InvokeCreatureAt));
		}

		private static AdminRequestCmd CreateRequest(PlayerStatus owner, EditableData definition, int level, CellCoord coord)
		{
			if (definition is CompanionDefinition)
			{
				return new AdminRequestCmd
				{
					InvokeCompanion = new AdminRequestCmd.Types.InvokeCompanionAdminCmd
					{
						OwnerEntityId = owner.id,
						DefinitionId = definition.get_id(),
						CompanionLevel = level,
						Destination = coord
					}
				};
			}
			if (definition is SummoningDefinition)
			{
				return new AdminRequestCmd
				{
					InvokeSummoning = new AdminRequestCmd.Types.InvokeSummoningAdminCmd
					{
						OwnerEntityId = owner.id,
						DefinitionId = definition.get_id(),
						SummoningLevel = level,
						Destination = coord
					}
				};
			}
			return null;
		}
	}
}
