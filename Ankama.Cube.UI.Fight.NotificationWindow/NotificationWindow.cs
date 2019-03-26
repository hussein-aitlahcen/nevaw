using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.NotificationWindow
{
	public class NotificationWindow : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[SerializeField]
		private RawTextField m_messageField;

		[SerializeField]
		private Button m_closeButton;

		[SerializeField]
		private NotificationWindowStyle m_style;

		private NotificationWindowState m_state;

		private Tween m_tween;

		private float m_endTime;

		public event Action<NotificationWindow> OnClosed;

		private unsafe void Awake()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			m_closeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_canvasGroup.set_alpha(0f);
			this.get_transform().set_localScale(new Vector3(0.9f, 0.9f, 1f));
			m_state = NotificationWindowState.OPENING;
		}

		public unsafe void Open(string message)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			m_messageField.SetText(message);
			if (m_state == NotificationWindowState.OPENING)
			{
				Sequence val = DOTween.Sequence();
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 1f, m_style.fadeInDuration), m_style.fadeInEase));
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.get_transform(), 1f, m_style.fadeInDuration), m_style.scaleFadeInEase));
				TweenSettingsExtensions.OnKill<Sequence>(val, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_tween = val;
			}
		}

		private void OnOpenComplete()
		{
			m_state = NotificationWindowState.OPENED;
			m_tween = null;
			m_endTime = Time.get_realtimeSinceStartup() + m_style.displayDuration;
		}

		private void Update()
		{
			if (m_state == NotificationWindowState.OPENED && m_endTime <= Time.get_realtimeSinceStartup())
			{
				Close();
			}
		}

		public unsafe void Close()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			if (m_state != NotificationWindowState.CLOSING)
			{
				m_state = NotificationWindowState.CLOSING;
				Tween tween = m_tween;
				if (tween != null)
				{
					TweenExtensions.Kill(tween, false);
				}
				Sequence val = DOTween.Sequence();
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 0f, m_style.fadeOutDuration), m_style.fadeOutEase));
				TweenSettingsExtensions.OnKill<Sequence>(val, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_tween = val;
			}
		}

		private void OnCloseComplete()
		{
			this.OnClosed?.Invoke(this);
		}

		public NotificationWindow()
			: this()
		{
		}
	}
}
