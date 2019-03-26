using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightAdminProtocol;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class PickSpellCommand : ToggleFightAdminCommand
	{
		private readonly DebugDropperSpell m_spellDropper;

		public PickSpellCommand(KeyCode keycode, DebugDropperSpell spellDropper)
			: base("Pick Spell <i>(⇧: fill hand, Alt: opponent)</i>", keycode)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_spellDropper = spellDropper;
		}

		protected override void Start()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			m_spellDropper.OnSelected += OnSpellSelected;
			m_spellDropper.SetActive(active: true);
			m_spellDropper.SetLocalPlayerLevel();
			m_spellDropper.SetCloseKeyCode(key);
		}

		protected override void Stop()
		{
			m_spellDropper.OnSelected -= OnSpellSelected;
			m_spellDropper.SetActive(active: false);
		}

		protected override bool Update()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			if (Input.GetKey(key))
			{
				m_spellDropper.SetActive(active: false);
			}
			return m_spellDropper.get_isActiveAndEnabled();
		}

		private void OnSpellSelected(SpellDefinition definition, int level, Event lastEvent)
		{
			PlayerStatus playerOrOpponent = GetPlayerOrOpponent(lastEvent);
			int num = (!lastEvent.get_shift()) ? 1 : (GameStatus.fightDefinition.maxSpellInHand - playerOrOpponent.spellCount);
			if (num != 0)
			{
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					PickSpell = new AdminRequestCmd.Types.PickSpellCmd
					{
						PlayerEntityId = playerOrOpponent.id,
						Quantity = num,
						SpellDefinitionId = definition.get_id(),
						SpellLevel = level
					}
				});
			}
		}
	}
}
