using FMODUnity;
using UnityEngine;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioUIMusicController : MonoBehaviour
	{
		[SerializeField]
		private AudioReferenceWithParameters m_music;

		private void Awake()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (m_music.get_isValid())
			{
				AudioManager.StartCoroutine(AudioManager.StartUIMusic(m_music));
			}
		}

		private void OnDestroy()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (m_music.get_isValid())
			{
				AudioManager.StartCoroutine(AudioManager.StopUIMusic(m_music));
			}
		}

		public AudioUIMusicController()
			: this()
		{
		}
	}
}
