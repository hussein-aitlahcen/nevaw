using UnityEngine;

namespace Ankama.Cube.Maps.VisualEffects
{
	public class ParticleAnimationEvent : MonoBehaviour
	{
		[SerializeField]
		private VisualEffect[] m_visualEffects;

		public void Play(int index)
		{
			m_visualEffects[index].Play();
		}

		public void Stop(int index)
		{
			m_visualEffects[index].Stop();
		}

		public ParticleAnimationEvent()
			: this()
		{
		}
	}
}
