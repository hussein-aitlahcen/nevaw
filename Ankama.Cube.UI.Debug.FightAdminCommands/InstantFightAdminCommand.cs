using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public abstract class InstantFightAdminCommand : AbstractFightAdminCommand
	{
		protected InstantFightAdminCommand(string name, KeyCode key)
			: base(name, key)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public sealed override bool Handle()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			if (Input.GetKeyDown(key))
			{
				Execute();
			}
			return false;
		}

		protected abstract void Execute();

		public sealed override bool IsRunning()
		{
			return false;
		}
	}
}
