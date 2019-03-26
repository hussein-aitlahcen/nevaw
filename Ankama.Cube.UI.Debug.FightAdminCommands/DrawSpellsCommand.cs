using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightAdminProtocol;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class DrawSpellsCommand : InstantFightAdminCommand
	{
		public DrawSpellsCommand(KeyCode keycode)
			: base("Draw Spell <i>(⇧: fill hand, Alt: opponent)</i>", keycode)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		protected override void Execute()
		{
			PlayerStatus playerOrOpponent = GetPlayerOrOpponent();
			if (playerOrOpponent != null)
			{
				int quantity = (!AbstractFightAdminCommand.IsShiftDown()) ? 1 : (GameStatus.fightDefinition.maxSpellInHand - playerOrOpponent.spellCount);
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					DrawSpells = new AdminRequestCmd.Types.DrawSpellsCmd
					{
						PlayerEntityId = playerOrOpponent.id,
						Quantity = quantity
					}
				});
			}
		}
	}
}
