using DG.Tweening;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.DeckMaker
{
	public class CastableDragNDropElement : CastableDnd
	{
		public unsafe Tween PlayCastImmediate(Vector3 worldPosition, Transform parent)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			InitMove();
			Tween animationTween = m_animationTween;
			if (animationTween != null)
			{
				TweenExtensions.Kill(animationTween, false);
			}
			m_content.SetParent(parent, true);
			m_content.set_position(worldPosition);
			m_canvasGroup.set_alpha(0f);
			m_subContent.set_localRotation(Quaternion.Euler(0f, 0f, 45f));
			m_subContent.set_anchoredPosition(new Vector2(0f, 200f));
			Sequence val = DOTween.Sequence();
			TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 1f, 1f), 18));
			TweenSettingsExtensions.OnKill<Sequence>(val, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_animationTween = val;
			return m_animationTween;
		}

		private void OnEndPlayCastImmediate()
		{
			m_canvasGroup.set_alpha(1f);
		}

		public unsafe Tween EndCastImmediate()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			return TweenSettingsExtensions.OnKill<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 0f, 0.25f), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnEndCastImmediate()
		{
			m_canvasGroup.set_alpha(0f);
			m_content.SetParent(m_contentParent, false);
		}

		protected unsafe override Tween OnEnterTargetTween()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			Sequence obj = DOTween.Sequence();
			TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalRotate(m_subContent, new Vector3(0f, 0f, 45f), 0.5f, 0), 18));
			TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(m_subContent, new Vector2(0f, 200f), 0.5f, false), 18));
			TweenSettingsExtensions.OnKill<Sequence>(obj, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			return obj;
		}

		private void EndEnterCastSequence()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			m_subContent.set_localRotation(Quaternion.Euler(0f, 0f, 45f));
			m_subContent.set_anchoredPosition(new Vector2(0f, 200f));
		}

		protected unsafe override Tween OnExitTargetTween()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			Sequence obj = DOTween.Sequence();
			TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalRotate(m_subContent, Vector3.get_zero(), 0.5f, 0), 18));
			TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(m_subContent, Vector2.get_zero(), 0.5f, false), 18));
			return TweenSettingsExtensions.OnKill<Sequence>(obj, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void EndExitCastTarget()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			m_subContent.set_localRotation(Quaternion.Euler(0f, 0f, 0f));
			m_subContent.set_anchoredPosition(Vector2.get_zero());
		}

		protected override Tween OnPointerEnterTween()
		{
			return TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(m_content, 1.1f, 0.3f), 27);
		}

		protected override Tween OnPointerExitTween()
		{
			return TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(m_content, 1f, 0.3f), 27);
		}
	}
}
