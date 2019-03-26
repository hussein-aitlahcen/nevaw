using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUISlider : AudioEventUILoader, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler
	{
		[SerializeField]
		private Slider m_slider;

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
			if (m_soundOnClick.get_isValid() && !(null == m_slider) && m_slider.get_interactable())
			{
				AudioManager.PlayOneShot(m_soundOnClick, this.get_transform());
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (m_soundOnOver.get_isValid() && !(null == m_slider) && m_slider.get_interactable())
			{
				AudioManager.PlayOneShot(m_soundOnOver, this.get_transform());
			}
		}
	}
}
