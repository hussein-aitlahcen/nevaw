using Ankama.Cube.Audio;
using Ankama.Cube.Player;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class AudioCategory : OptionCategory
	{
		[SerializeField]
		protected Slider m_generalSlider;

		[SerializeField]
		protected Slider m_musicSlider;

		[SerializeField]
		protected Slider m_effectSlider;

		[SerializeField]
		protected Slider m_uiSlider;

		protected void Awake()
		{
		}

		protected unsafe void OnEnable()
		{
			AudioManager.TryGetVolume(AudioBusIdentifier.Master, out float volume);
			AudioManager.TryGetVolume(AudioBusIdentifier.Music, out float volume2);
			AudioManager.TryGetVolume(AudioBusIdentifier.SFX, out float volume3);
			AudioManager.TryGetVolume(AudioBusIdentifier.UI, out float volume4);
			m_generalSlider.set_value(volume);
			m_musicSlider.set_value(volume2);
			m_effectSlider.set_value(volume3);
			m_uiSlider.set_value(volume4);
			m_generalSlider.get_onValueChanged().AddListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_musicSlider.get_onValueChanged().AddListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_effectSlider.get_onValueChanged().AddListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_uiSlider.get_onValueChanged().AddListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		protected unsafe void OnDisable()
		{
			m_generalSlider.get_onValueChanged().RemoveListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_musicSlider.get_onValueChanged().RemoveListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_effectSlider.get_onValueChanged().RemoveListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		protected void OnGeneralSliderChanged(float value)
		{
			AudioManager.SetVolume(AudioBusIdentifier.Master, value);
			PlayerPreferences.audioMasterVolume = value;
		}

		protected void OnMusicSliderChanged(float value)
		{
			AudioManager.SetVolume(AudioBusIdentifier.Music, value);
			PlayerPreferences.audioMusicVolume = value;
		}

		protected void OnEffectSliderChanged(float value)
		{
			AudioManager.SetVolume(AudioBusIdentifier.SFX, value);
			PlayerPreferences.audioFxVolume = value;
		}

		protected void OnUISliderChanged(float value)
		{
			AudioManager.SetVolume(AudioBusIdentifier.UI, value);
			PlayerPreferences.audioUiVolume = value;
		}

		protected void OnMuteWhenInactiveDropdownChanged(int value)
		{
		}
	}
}
