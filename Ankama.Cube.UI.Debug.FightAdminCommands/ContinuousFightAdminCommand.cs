using Ankama.Cube.Fight;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Debug.FightAdminCommands
{
	public abstract class ContinuousFightAdminCommand : AbstractFightAdminCommand
	{
		private bool m_isRunning;

		protected ContinuousFightAdminCommand(string name, KeyCode key)
			: base(name, key)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
		}

		protected virtual void Stop()
		{
		}

		public sealed override bool IsRunning()
		{
			return m_isRunning;
		}

		public override bool Handle()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			bool key = Input.GetKey(base.key);
			if (!m_isRunning)
			{
				if (!key)
				{
					return false;
				}
				m_isRunning = true;
				switch (FightCastManager.currentCastType)
				{
				case FightCastManager.CurrentCastType.Spell:
					FightCastManager.StopCastingSpell(cancelled: true);
					break;
				case FightCastManager.CurrentCastType.Companion:
					FightCastManager.StopInvokingCompanion(cancelled: true);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				case FightCastManager.CurrentCastType.None:
					break;
				}
				Start();
				Update();
			}
			else if (key)
			{
				Update();
			}
			else
			{
				m_isRunning = false;
				Stop();
			}
			return m_isRunning;
		}
	}
}
