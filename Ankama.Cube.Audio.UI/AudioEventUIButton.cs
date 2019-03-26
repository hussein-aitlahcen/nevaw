using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUIButton : AudioEventUILoader, IPointerEnterHandler, IEventSystemHandler
	{
		[SerializeField]
		private Button m_button;

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
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			AudioManager.StartCoroutine(Load(m_soundOnOver, m_soundOnClick));
			if (null != m_button)
			{
				m_button.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		protected unsafe override void OnDestroy()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			if (null != m_button)
			{
				m_button.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			base.OnDestroy();
		}

		private void OnButtonClicked()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (m_soundOnClick.get_isValid())
			{
				AudioManager.PlayOneShot(m_soundOnClick, this.get_transform());
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == m_button) && m_button.get_interactable() && m_soundOnOver.get_isValid())
			{
				AudioManager.PlayOneShot(m_soundOnOver, this.get_transform());
			}
		}
	}
}
