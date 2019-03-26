using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUIDropDown : AudioEventUILoader, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler
	{
		[SerializeField]
		private Selectable m_selectable;

		[SerializeField]
		private AudioReferenceWithParameters m_soundOnClick;

		[SerializeField]
		private AudioReferenceWithParameters m_soundOnOver;

		private void Awake()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			AudioManager.StartCoroutine(Load(m_soundOnOver, m_soundOnClick));
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == m_selectable) && m_selectable.get_interactable() && m_soundOnClick.get_isValid())
			{
				AudioManager.PlayOneShot(m_soundOnClick, this.get_transform());
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == m_selectable) && m_selectable.get_interactable() && m_soundOnOver.get_isValid())
			{
				AudioManager.PlayOneShot(m_soundOnOver, this.get_transform());
			}
		}
	}
}
