using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class AnimatedObjectMechanismData : AnimatedBoardCharacterData
	{
		[Header("Animation Properties")]
		[SerializeField]
		private bool m_hasActivationAnimation;

		[Header("Character Effects")]
		[SerializeField]
		private CharacterEffect m_activationEffect;

		public bool hasActivationAnimation => m_hasActivationAnimation;

		public CharacterEffect activationEffect => m_activationEffect;

		protected override void GatherAdditionalResourcesLoadingRoutines(List<IEnumerator> routines)
		{
			if (null != m_activationEffect)
			{
				routines.Add(m_activationEffect.Load());
			}
		}

		protected override void UnloadAdditionalResources()
		{
			if (null != m_activationEffect)
			{
				m_activationEffect.Unload();
			}
		}
	}
}
