using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUIButtonLoopOnHighlight : AudioEventUILoader, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		[SerializeField]
		private Button m_button;

		[SerializeField]
		private AudioReferenceWithParameters m_soundOnOver;

		[SerializeField]
		private STOP_MODE m_stopMode;

		private EventInstance m_eventInstance;

		private unsafe void Awake()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			AudioManager.StartCoroutine(Load(m_soundOnOver));
			if (null != m_button)
			{
				m_button.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		private void OnDisable()
		{
			StopSound();
		}

		protected unsafe override void OnDestroy()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			if (null != m_button)
			{
				m_button.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			if (m_eventInstance.isValid())
			{
				m_eventInstance.stop(1);
				m_eventInstance.release();
				m_eventInstance.clearHandle();
			}
			base.OnDestroy();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (m_soundOnOver.get_isValid() && !(null == m_button) && m_button.get_interactable() && (m_eventInstance.isValid() || (AudioManager.isReady && AudioManager.TryCreateInstance(m_soundOnOver, this.get_transform(), out m_eventInstance))))
			{
				m_eventInstance.start();
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			StopSound();
		}

		private void OnButtonClicked()
		{
			StopSound();
		}

		private void StopSound()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (m_eventInstance.isValid())
			{
				m_eventInstance.stop(m_stopMode);
			}
		}
	}
}
