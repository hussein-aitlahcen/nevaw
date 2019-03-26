using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public sealed class CastHighlight : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem m_particleSystem;

		public void Play()
		{
			m_particleSystem.Play();
		}

		public void Stop()
		{
			m_particleSystem.Stop();
		}

		public CastHighlight()
			: this()
		{
		}
	}
}
