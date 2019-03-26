using Ankama.Utilities;
using FMOD.Studio;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUITriggerBetweenEvents : AudioEventUITrigger
	{
		[SerializeField]
		private STOP_MODE m_stopMode;

		[SerializeField]
		private float m_stopDelay;

		private Coroutine m_stopDelayRoutine;

		private EventInstance m_eventInstance;

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

		[PublicAPI]
		public void ActivationTrigger()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Invalid comparison between Unknown and I4
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			if (!m_sound.get_isValid())
			{
				return;
			}
			if (!m_eventInstance.isValid())
			{
				if (!AudioManager.isReady || !AudioManager.TryCreateInstance(m_sound, out m_eventInstance))
				{
					return;
				}
			}
			else
			{
				if (m_stopDelayRoutine != null)
				{
					this.StopCoroutine(m_stopDelayRoutine);
					m_stopDelayRoutine = null;
				}
				PLAYBACK_STATE val = default(PLAYBACK_STATE);
				m_eventInstance.getPlaybackState(ref val);
				if ((int)val == 3 || (int)val == 0)
				{
					return;
				}
			}
			m_eventInstance.start();
		}

		[PublicAPI]
		public void DeactivationTrigger()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (!m_eventInstance.isValid())
			{
				return;
			}
			PLAYBACK_STATE val = default(PLAYBACK_STATE);
			m_eventInstance.getPlaybackState(ref val);
			if ((int)val != 0 && (int)val != 3)
			{
				return;
			}
			if (m_stopDelay <= float.Epsilon)
			{
				if (m_stopDelayRoutine != null)
				{
					this.StopCoroutine(m_stopDelayRoutine);
					m_stopDelayRoutine = null;
				}
				m_eventInstance.stop(m_stopMode);
			}
			else if (m_stopDelayRoutine == null)
			{
				m_stopDelayRoutine = this.StartCoroutine(DelayStop());
			}
		}

		private IEnumerator DelayStop()
		{
			yield return (object)new WaitForTime(m_stopDelay);
			if (m_eventInstance.isValid())
			{
				m_eventInstance.stop(m_stopMode);
			}
			m_stopDelayRoutine = null;
		}
	}
}
