using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightAdminProtocol;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class DiscardSpellsCommand : InstantFightAdminCommand
	{
		public DiscardSpellsCommand(KeyCode keycode)
			: base("Discard Spell <i>(⇧: all, Alt: opponent)</i>", keycode)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		protected override void Execute()
		{
			PlayerStatus playerOrOpponent = GetPlayerOrOpponent();
			if (playerOrOpponent != null)
			{
				int quantity = (!AbstractFightAdminCommand.IsShiftDown()) ? 1 : playerOrOpponent.spellCount;
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					DiscardSpells = new AdminRequestCmd.Types.DiscardSpellsCmd
					{
						PlayerEntityId = playerOrOpponent.id,
						Quantity = quantity
					}
				});
			}
		}
	}
}
