using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Animations
{
	[ExecuteInEditMode]
	public class ParticleAnimation : MonoBehaviour
	{
		[SerializeField]
		private Animation m_animation;

		[SerializeField]
		private ParticleSystem m_particleSystem;

		private void OnEnable()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			if (m_animation.GetClipCount() != 0)
			{
				IEnumerator enumerator = m_animation.GetEnumerator();
				if (enumerator.MoveNext())
				{
					AnimationState val = enumerator.Current;
					m_animation.Play(val.get_name());
				}
			}
		}

		private void OnDisable()
		{
			m_animation.Stop();
		}

		public ParticleAnimation()
			: this()
		{
		}
	}
}
