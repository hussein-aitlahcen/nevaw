using System;
using System.Collections;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUITriggerOnEnable : AudioEventUITrigger
	{
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
				PlaySound();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
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
				PlaySound();
			}
		}
	}
}
