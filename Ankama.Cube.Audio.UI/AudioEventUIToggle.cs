using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUIToggle : AudioEventUILoader, IPointerEnterHandler, IEventSystemHandler
	{
		[SerializeField]
		private Toggle m_toggle;

		[SerializeField]
		private AudioReferenceWithParameters m_soundOnClick;

		[SerializeField]
		private AudioReferenceWithParameters m_soundOnOver;

		private unsafe void Awake()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			AudioManager.StartCoroutine(Load(m_soundOnOver, m_soundOnClick));
			if (null != m_toggle)
			{
				m_toggle.onValueChanged.AddListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		protected unsafe override void OnDestroy()
		{
			if (null != m_toggle)
			{
				m_toggle.onValueChanged.RemoveListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		private void OnValueChanged(bool arg0)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (m_soundOnClick.get_isValid())
			{
				AudioManager.PlayOneShot(m_soundOnClick, this.get_transform());
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (m_soundOnOver.get_isValid() && !(null == m_toggle) && m_toggle.get_interactable() && !m_toggle.get_isOn())
			{
				AudioManager.PlayOneShot(m_soundOnOver, this.get_transform());
			}
		}
	}
}
