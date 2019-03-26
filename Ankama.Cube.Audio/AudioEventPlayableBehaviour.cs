using Ankama.Utilities;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Audio
{
	public sealed class AudioEventPlayableBehaviour : PlayableBehaviour
	{
		private readonly Transform m_ownerTransform;

		private readonly Guid m_eventGuid;

		private readonly AudioEventPlayableAsset.StopMode m_stopMode;

		private readonly float m_volume;

		private readonly AudioEventParameterDictionary m_parameters;

		private readonly AudioContext m_audioContext;

		private EventInstance m_eventInstance;

		[UsedImplicitly]
		public AudioEventPlayableBehaviour()
			: this()
		{
			throw new NotImplementedException();
		}

		public AudioEventPlayableBehaviour(Guid eventGuid, AudioEventPlayableAsset.StopMode stopMode, float volume, AudioEventParameterDictionary parameters, AudioContext audioContext, Transform ownerTransform)
			: this()
		{
			m_eventGuid = eventGuid;
			m_stopMode = stopMode;
			m_volume = volume;
			m_parameters = parameters;
			m_audioContext = audioContext;
			m_ownerTransform = ownerTransform;
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (m_eventInstance.isValid())
			{
				m_eventInstance.stop(1);
				m_eventInstance.release();
				m_eventInstance.clearHandle();
			}
		}

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (m_eventInstance.isValid())
			{
				StopInstance();
			}
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Invalid comparison between Unknown and I4
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (!m_eventInstance.isValid())
			{
				if (!StartInstance())
				{
					return;
				}
			}
			else if ((int)info.get_evaluationType() == 1 && PlayableExtensions.GetPreviousTime<Playable>(playable) > PlayableExtensions.GetTime<Playable>(playable))
			{
				StopInstance();
				if (!StartInstance())
				{
					return;
				}
			}
			RESULT val = m_eventInstance.setVolume(info.get_weight() * m_volume);
			if ((int)val != 0)
			{
				Log.Warning($"Failed to set event instance volume: {val}.", 112, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioEventPlayableBehaviour.cs");
			}
		}

		private bool StartInstance()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			EventInstance eventInstance = m_eventInstance;
			if (!AudioManager.isReady)
			{
				Log.Warning($"Tried to create event instance with guid {m_eventGuid} but the audio manager is not ready.", 124, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioEventPlayableBehaviour.cs");
				return false;
			}
			if (!AudioManager.TryCreateInstance(m_eventGuid, out eventInstance))
			{
				return false;
			}
			if (m_parameters != null)
			{
				foreach (KeyValuePair<string, float> item in (Dictionary<string, float>)m_parameters)
				{
					eventInstance.setParameterValue(item.Key, item.Value);
				}
			}
			if (m_audioContext != null)
			{
				m_audioContext.AddEventInstance(eventInstance);
			}
			else if (null != m_ownerTransform)
			{
				eventInstance.set3DAttributes(FMODUtility.To3DAttributes(m_ownerTransform));
			}
			eventInstance.start();
			m_eventInstance = eventInstance;
			return true;
		}

		private void StopInstance()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			switch (m_stopMode)
			{
			case AudioEventPlayableAsset.StopMode.Immediate:
				m_eventInstance.stop(1);
				break;
			case AudioEventPlayableAsset.StopMode.AllowFadeout:
				m_eventInstance.stop(0);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case AudioEventPlayableAsset.StopMode.None:
				break;
			}
			m_eventInstance.release();
			m_eventInstance.clearHandle();
		}
	}
}
