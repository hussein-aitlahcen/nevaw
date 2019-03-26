using System;
using UnityEngine;

namespace Ankama.Cube.Audio
{
	public sealed class AudioManagerCallbackSource : MonoBehaviour
	{
		[HideInInspector]
		[SerializeField]
		private long m_cachedStudioHandle;

		[HideInInspector]
		[SerializeField]
		private long m_cachedLowLevelStudioHandle;

		public static AudioManagerCallbackSource Create()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			AudioManagerCallbackSource audioManagerCallbackSource = Object.FindObjectOfType<AudioManagerCallbackSource>();
			if (null != audioManagerCallbackSource)
			{
				if (audioManagerCallbackSource.m_cachedStudioHandle != 0L)
				{
					AudioManager.RestoreSystemInternal((IntPtr)audioManagerCallbackSource.m_cachedStudioHandle, (IntPtr)audioManagerCallbackSource.m_cachedLowLevelStudioHandle);
				}
				return audioManagerCallbackSource;
			}
			GameObject val = new GameObject("AudioManagerCallbackSource");
			val.set_hideFlags(1);
			Object.DontDestroyOnLoad(val);
			return val.AddComponent<AudioManagerCallbackSource>();
		}

		private void Update()
		{
			AudioManager.UpdateInternal();
		}

		private void OnDisable()
		{
			AudioManager.BackupSystemInternal(out IntPtr studioHandle, out IntPtr lowLevelStudioHandle);
			m_cachedStudioHandle = (long)studioHandle;
			m_cachedLowLevelStudioHandle = (long)lowLevelStudioHandle;
		}

		private void OnDestroy()
		{
			AudioManager.ReleaseSystemInternal();
		}

		private void OnApplicationPause(bool paused)
		{
			AudioManager.PauseSystemInternal(paused);
		}

		public AudioManagerCallbackSource()
			: this()
		{
		}
	}
}
