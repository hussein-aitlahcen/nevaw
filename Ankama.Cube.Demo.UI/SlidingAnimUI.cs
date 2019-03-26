using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	[Serializable]
	public struct SlidingAnimUI
	{
		[SerializeField]
		public SlidingAnimUIConfig openConfig;

		[SerializeField]
		public SlidingAnimUIConfig closeConfig;

		[SerializeField]
		public List<CanvasGroup> elements;

		private Sequence m_transitionTweenSequence;

		public Sequence PlayAnim(bool open, SlidingSide side, bool reverseElementOrder = false)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			if (m_transitionTweenSequence != null && TweenExtensions.IsActive(m_transitionTweenSequence))
			{
				TweenExtensions.Kill(m_transitionTweenSequence, false);
			}
			SlidingAnimUIConfig slidingAnimUIConfig = open ? openConfig : closeConfig;
			m_transitionTweenSequence = DOTween.Sequence();
			float num = slidingAnimUIConfig.delay;
			for (int i = 0; i < elements.Count; i++)
			{
				int index = reverseElementOrder ? (elements.Count - 1 - i) : i;
				CanvasGroup val = elements[index];
				RectTransform val2 = val.get_transform() as RectTransform;
				Vector2 anchoredPosition = val2.get_anchoredPosition();
				Vector2 anchorOffset = slidingAnimUIConfig.anchorOffset;
				Vector2 val3 = (side == SlidingSide.Right) ? (anchoredPosition + anchorOffset) : (anchoredPosition - anchorOffset);
				Vector2 anchoredPosition2 = open ? val3 : anchoredPosition;
				Vector2 val4 = open ? anchoredPosition : val3;
				if (open)
				{
					val.set_alpha(0f);
				}
				val2.set_anchoredPosition(anchoredPosition2);
				TweenSettingsExtensions.Insert(m_transitionTweenSequence, num, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(val2, val4, slidingAnimUIConfig.duration, false), slidingAnimUIConfig.positionCurve));
				TweenSettingsExtensions.Insert(m_transitionTweenSequence, num, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(val, slidingAnimUIConfig.endAlpha, slidingAnimUIConfig.duration), slidingAnimUIConfig.alphaCurve));
				num += slidingAnimUIConfig.elementDelayOffset;
			}
			return m_transitionTweenSequence;
		}
	}
}
