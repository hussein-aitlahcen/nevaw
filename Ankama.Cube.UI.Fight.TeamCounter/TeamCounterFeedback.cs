using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.TeamCounter
{
	public class TeamCounterFeedback : MonoBehaviour
	{
		[SerializeField]
		private MaskableGraphic m_target;

		[SerializeField]
		private float m_tweenDuration;

		private Sequence m_sequence;

		public unsafe void PlayFeedback()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			if (m_sequence != null && TweenExtensions.IsActive(m_sequence))
			{
				TweenExtensions.Kill(m_sequence, false);
			}
			this.get_transform().set_localScale(Vector3.get_one());
			SetAlpha(1f);
			m_target.set_enabled(true);
			m_sequence = DOTween.Sequence();
			TweenSettingsExtensions.Insert(m_sequence, 0f, DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, m_tweenDuration));
			TweenSettingsExtensions.Insert(m_sequence, 0f, TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(_003C_003Ec._003C_003E9__2_0 ?? (_003C_003Ec._003C_003E9__2_0 = new DOGetter<Vector3>((object)_003C_003Ec._003C_003E9, (IntPtr)(void*)/*OpCode not supported: LdFtn*/)), new DOSetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new Vector3(5f, 5f, 5f), m_tweenDuration), 0f));
			TweenSettingsExtensions.OnComplete<Sequence>(m_sequence, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void SetAlpha(float value)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Color color = m_target.get_color();
			color.a = value;
			m_target.set_color(color);
		}

		private unsafe float TweenAlphaGetter()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((IntPtr)(void*)m_target.get_color()).a;
		}

		private void OnTweenComplete()
		{
			SetOff();
		}

		public void SetOff()
		{
			m_target.set_enabled(false);
		}

		public TeamCounterFeedback()
			: this()
		{
		}
	}
}
