using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.Audio.UI
{
	public class AudioEventUITriggerOnDrag : AudioEventUITrigger, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
	{
		[SerializeField]
		protected AudioReferenceWithParameters m_soundOnDragStart;

		[SerializeField]
		protected AudioReferenceWithParameters m_soundOnDragEnd;

		[SerializeField]
		private STOP_MODE m_stopMode;

		private EventInstance m_dragEventInstance;

		protected override void Awake()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			AudioManager.StartCoroutine(Load(m_sound, m_soundOnDragStart, m_soundOnDragEnd));
		}

		protected override void OnDestroy()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			m_dragEventInstance.stop(1);
			if (m_dragEventInstance.isValid())
			{
				m_dragEventInstance.release();
				m_dragEventInstance.clearHandle();
			}
			base.OnDestroy();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (m_soundOnDragStart.get_isValid())
			{
				AudioManager.PlayOneShot(m_soundOnDragStart, this.get_transform());
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (m_sound.get_isValid() && !m_dragEventInstance.isValid() && (m_dragEventInstance.isValid() || AudioManager.TryCreateInstance(m_sound, this.get_transform(), out m_dragEventInstance)))
			{
				m_dragEventInstance.start();
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (m_soundOnDragEnd.get_isValid())
			{
				if (m_dragEventInstance.isValid())
				{
					m_dragEventInstance.stop(m_stopMode);
				}
				AudioManager.PlayOneShot(m_soundOnDragEnd, this.get_transform());
			}
		}
	}
}
