using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightAdminProtocol;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public class GiveElementPointsCommand : InstantFightAdminCommand
	{
		public GiveElementPointsCommand(KeyCode keycode)
			: base("Give Elements <i>(⇧: 10, Alt: opponent)</i>", keycode)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		protected override void Execute()
		{
			PlayerStatus playerOrOpponent = GetPlayerOrOpponent();
			if (playerOrOpponent != null)
			{
				int quantity = (!AbstractFightAdminCommand.IsShiftDown()) ? 1 : 10;
				AbstractFightAdminCommand.SendAdminCommand(new AdminRequestCmd
				{
					GainElementPoints = new AdminRequestCmd.Types.GainElementPointsCmd
					{
						PlayerEntityId = playerOrOpponent.id,
						Quantity = quantity
					}
				});
			}
		}
	}
}
