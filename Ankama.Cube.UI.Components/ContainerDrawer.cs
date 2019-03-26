using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public class ContainerDrawer : MonoBehaviour
	{
		[SerializeField]
		private RectMask2D m_mask;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[SerializeField]
		private RectTransform m_content;

		[SerializeField]
		private RectTransform m_hiddenPositionRect;

		[SerializeField]
		public bool open;

		private ContainerDrawerState m_state;

		private Tween m_tween;

		private Vector3 m_basePosition;

		private Vector3 m_hiddenPosition;

		private void Awake()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			m_mask.set_enabled(false);
			m_canvasGroup.set_alpha(1f);
			m_content.get_gameObject().SetActive(open);
			m_state = ((!open) ? ContainerDrawerState.Closed : ContainerDrawerState.Opened);
			m_basePosition = m_content.get_localPosition();
			m_hiddenPosition = m_hiddenPositionRect.get_localPosition();
		}

		public unsafe void Open(bool forceImmediate = false)
		{
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			if (forceImmediate)
			{
				Tween tween = m_tween;
				if (tween != null)
				{
					tween.onKill.Invoke();
				}
				OnClosed();
			}
			else if (m_state != 0 && m_state != ContainerDrawerState.Opening)
			{
				Tween tween2 = m_tween;
				if (tween2 != null)
				{
					TweenExtensions.Kill(tween2, false);
				}
				m_state = ContainerDrawerState.Opening;
				m_mask.set_enabled(true);
				m_canvasGroup.set_alpha(0f);
				m_content.get_gameObject().SetActive(true);
				m_content.get_transform().set_localPosition(m_hiddenPosition);
				Sequence val = DOTween.Sequence();
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 1f, 0.3f), 19));
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(m_content, m_basePosition, 0.3f, false), 19));
				TweenSettingsExtensions.OnKill<Sequence>(val, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_tween = val;
			}
		}

		private void OnOpened()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			m_state = ContainerDrawerState.Opened;
			m_mask.set_enabled(false);
			m_canvasGroup.set_alpha(1f);
			m_content.set_localPosition(m_basePosition);
			m_tween = null;
		}

		public unsafe void Close(bool forceImmediate = false)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			if (forceImmediate)
			{
				Tween tween = m_tween;
				if (tween != null)
				{
					tween.onKill.Invoke();
				}
				OnClosed();
			}
			else if (m_state != ContainerDrawerState.Closed && m_state != ContainerDrawerState.Closing)
			{
				Tween tween2 = m_tween;
				if (tween2 != null)
				{
					TweenExtensions.Kill(tween2, false);
				}
				m_state = ContainerDrawerState.Closing;
				m_mask.set_enabled(true);
				m_canvasGroup.set_alpha(1f);
				m_content.get_transform().set_localPosition(m_basePosition);
				Sequence val = DOTween.Sequence();
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 0f, 0.3f), 19));
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(m_content, m_hiddenPosition, 0.3f, false), 19));
				TweenSettingsExtensions.OnKill<Sequence>(val, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_tween = val;
			}
		}

		private void OnClosed()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			m_state = ContainerDrawerState.Closed;
			m_mask.set_enabled(false);
			m_canvasGroup.set_alpha(0f);
			m_content.set_localPosition(m_hiddenPosition);
			m_content.get_gameObject().SetActive(false);
			m_tween = null;
		}

		public void Switch()
		{
			if (m_state == ContainerDrawerState.Opened || m_state == ContainerDrawerState.Opening)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		public ContainerDrawer()
			: this()
		{
		}
	}
}
