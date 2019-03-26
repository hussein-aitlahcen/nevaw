using FMODUnity;
using UnityEngine;

namespace Ankama.Cube.Audio.UI
{
	public abstract class AudioEventUITrigger : AudioEventUILoader
	{
		[SerializeField]
		protected AudioReferenceWithParameters m_sound;

		protected virtual void Awake()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (m_sound.get_isValid())
			{
				AudioManager.StartCoroutine(Load(m_sound));
			}
		}

		protected void PlaySound()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (m_sound.get_isValid())
			{
				AudioManager.PlayOneShot(m_sound, this.get_transform());
			}
		}
	}
}
