using FMOD.Studio;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Audio.UI
{
	public class AudioEventUIPlayWhileEnabled : AudioEventUITrigger
	{
		[SerializeField]
		private STOP_MODE m_stopMode;

		private EventInstance m_eventInstance;

		private void OnEnable()
		{
			switch (m_initializationState)
			{
			case InitializationState.None:
			case InitializationState.Error:
				break;
			case InitializationState.Loading:
				AudioManager.StartCoroutine(WaitAndPlay());
				break;
			case InitializationState.Loaded:
				Play();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void OnDisable()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (m_eventInstance.isValid())
			{
				m_eventInstance.stop(m_stopMode);
			}
		}

		protected override void OnDestroy()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (m_eventInstance.isValid())
			{
				m_eventInstance.stop(1);
				m_eventInstance.release();
				m_eventInstance.clearHandle();
			}
			base.OnDestroy();
		}

		private IEnumerator WaitAndPlay()
		{
			do
			{
				yield return null;
			}
			while (m_initializationState == InitializationState.Loading);
			if (m_initializationState == InitializationState.Loaded)
			{
				Play();
			}
		}

		protected void Play()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (m_sound.get_isValid() && (m_eventInstance.isValid() || (AudioManager.isReady && AudioManager.TryCreateInstance(m_sound, this.get_transform(), out m_eventInstance))))
			{
				m_eventInstance.start();
			}
		}
	}
}
