using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class StepIndicator : MonoBehaviour
	{
		public enum State
		{
			None,
			Disable,
			Enable
		}

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[SerializeField]
		private Image m_background;

		[SerializeField]
		private StepIndicatorData m_data;

		private State m_state;

		private float m_scale;

		protected void OnEnable()
		{
			if (m_state != 0)
			{
				this.StartCoroutine(DelayUpdateVisual());
			}
		}

		public void SetState(State state, bool tween)
		{
			if (m_state != state)
			{
				m_state = state;
				if (this.get_isActiveAndEnabled())
				{
					UpdateVisual(tween);
				}
			}
		}

		private IEnumerator DelayUpdateVisual()
		{
			yield return null;
			UpdateVisual(tween: false);
		}

		private unsafe void UpdateVisual(bool tween)
		{
			switch (m_state)
			{
			case State.Disable:
				if (tween)
				{
					DOVirtual.Float(1f, 0f, m_data.transitionDuration, new TweenCallback<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				else
				{
					LerpState(0f);
				}
				break;
			case State.Enable:
				if (tween)
				{
					DOVirtual.Float(0f, 1f, m_data.transitionDuration, new TweenCallback<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				else
				{
					LerpState(1f);
				}
				break;
			}
		}

		private unsafe void LerpState(float value)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			StepIndicatorData.StateData disableState = m_data.disableState;
			StepIndicatorData.StateData enableState = m_data.enableState;
			float alpha = Mathf.Lerp(disableState.alpha, enableState.alpha, value);
			m_scale = Mathf.Lerp(disableState.scale, enableState.scale, value);
			m_canvasGroup.set_alpha(alpha);
			m_background.get_transform().set_localScale(Vector3.get_one() * m_scale);
			RectTransform val = this.get_transform() as RectTransform;
			RectTransform val2 = m_background.get_transform() as RectTransform;
			Rect rect = val.get_rect();
			float num = rect.get_width() / m_scale;
			rect = val.get_rect();
			float num2 = num - rect.get_width();
			val2.set_sizeDelta(Vector2.get_zero());
			val2.set_offsetMax(new Vector2(num2 / 2f, ((IntPtr)(void*)val2.get_offsetMax()).y));
			val2.set_offsetMin(new Vector2((0f - num2) / 2f + (28f - 28f * m_scale), ((IntPtr)(void*)val2.get_offsetMin()).y));
		}

		public StepIndicator()
			: this()
		{
		}
	}
}
