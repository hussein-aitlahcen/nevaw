using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public sealed class AnimatedFightCharacterData : AnimatedBoardCharacterData
	{
		[Flags]
		public enum IdleToRunTransitionMode
		{
			None = 0x0,
			IdleToRun = 0x1,
			RunToIdle = 0x2,
			Both = 0x3
		}

		[SerializeField]
		private AnimatedFightCharacterType m_characterType;

		[SerializeField]
		private AnimatedFightCharacterActionRange m_actionRange;

		[Header("Animation Properties")]
		[SerializeField]
		private bool m_hasRangedAttackAnimations;

		[SerializeField]
		private bool m_hasDashAnimations = true;

		[SerializeField]
		private IdleToRunTransitionMode m_idleToRunTransitionMode;

		[Header("Character Effects")]
		[SerializeField]
		private CharacterEffect m_actionEffect;

		public AnimatedFightCharacterActionRange actionRange => m_actionRange;

		public AnimatedFightCharacterType characterType => m_characterType;

		public bool hasRangedAttackAnimations => m_hasRangedAttackAnimations;

		public bool hasDashAnimations => m_hasDashAnimations;

		public IdleToRunTransitionMode idleToRunTransitionMode => m_idleToRunTransitionMode;

		public CharacterEffect actionEffect => m_actionEffect;

		protected override void GatherAdditionalResourcesLoadingRoutines(List<IEnumerator> routines)
		{
			if (null != m_actionEffect)
			{
				routines.Add(m_actionEffect.Load());
			}
		}

		protected override void UnloadAdditionalResources()
		{
			if (null != m_actionEffect)
			{
				m_actionEffect.Unload();
			}
		}
	}
}
