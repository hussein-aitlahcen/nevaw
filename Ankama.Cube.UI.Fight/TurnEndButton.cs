using Ankama.Cube.UI.Components;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public class TurnEndButton : MonoBehaviour
	{
		[Header("Links")]
		[SerializeField]
		private Button m_button;

		[SerializeField]
		private Image m_timeFilling;

		[SerializeField]
		private TextField m_text;

		[SerializeField]
		private UISpriteTextRenderer m_chronoText;

		[Header("Colors")]
		[SerializeField]
		private Color m_myTurnColor;

		[SerializeField]
		private Color m_opponentTurnColor;

		private float m_turnStartTime;

		private float m_turnDuration;

		private bool m_isDirty;

		private bool m_running;

		private int m_previousSecond = -1;

		private Sequence m_sequence;

		public void SetTurnDuration(float turnDuration)
		{
			m_turnDuration = turnDuration;
		}

		public void StartTurn(bool isLocalPlayerTurn, bool isFightEnded)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			SetInteractable(isLocalPlayerTurn && !isFightEnded);
			m_chronoText.get_gameObject().SetActive(false);
			m_text.get_gameObject().SetActive(true);
			if (isLocalPlayerTurn)
			{
				m_timeFilling.set_color(m_myTurnColor);
				m_text.SetText(51179);
			}
			else
			{
				m_timeFilling.set_color(m_opponentTurnColor);
				m_text.SetText(85537);
			}
			m_previousSecond = -1;
			m_turnStartTime = Time.get_unscaledTime();
			m_running = true;
			m_isDirty = true;
		}

		public void Stop()
		{
			SetInteractable(value: false);
			m_running = false;
			m_isDirty = true;
			m_chronoText.get_gameObject().SetActive(false);
			m_text.get_gameObject().SetActive(true);
		}

		public void EndTurn()
		{
			SetInteractable(value: false);
			m_isDirty = true;
		}

		public void EndTeamTurn()
		{
			SetInteractable(value: false);
			m_running = false;
			m_isDirty = true;
		}

		public void AddListener(UnityAction call)
		{
			m_button.get_onClick().AddListener(call);
		}

		public void SimulateClick()
		{
			InputUtility.SimulateClickOn(m_button);
		}

		private void Awake()
		{
			StartTurn(isLocalPlayerTurn: false, isFightEnded: true);
			m_timeFilling.set_fillAmount(0f);
		}

		private void Update()
		{
			if (m_running || m_isDirty)
			{
				RefreshTimeFilling();
				m_isDirty = false;
			}
		}

		private void SetInteractable(bool value)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			m_button.set_interactable(value);
			Color color = m_text.color;
			color.a = (value ? 1f : 0.6f);
			m_text.color = color;
		}

		private unsafe void RefreshTimeFilling()
		{
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			if (!m_running || Mathf.Approximately(m_turnDuration, 0f))
			{
				m_timeFilling.set_fillAmount(0f);
				return;
			}
			float num = Time.get_unscaledTime() - m_turnStartTime;
			m_timeFilling.set_fillAmount(Mathf.Clamp01(num / m_turnDuration));
			if (m_turnDuration - num <= 10f)
			{
				m_chronoText.get_gameObject().SetActive(true);
				m_text.get_gameObject().SetActive(false);
				int num2 = (int)(m_turnDuration - num);
				if (m_previousSecond != num2)
				{
					m_chronoText.text = num2.ToString();
					m_chronoText.get_transform().set_localScale(Vector3.get_one() * 5f);
					if (m_sequence != null && TweenExtensions.IsActive(m_sequence))
					{
						TweenExtensions.Kill(m_sequence, false);
					}
					SetAlpha(0f);
					m_sequence = DOTween.Sequence();
					TweenSettingsExtensions.Insert(m_sequence, 0f, DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, 0.3f));
					TweenSettingsExtensions.Insert(m_sequence, 0f, TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(_003C_003Ec._003C_003E9__20_1 ?? (_003C_003Ec._003C_003E9__20_1 = new DOGetter<Vector3>((object)_003C_003Ec._003C_003E9, (IntPtr)(void*)/*OpCode not supported: LdFtn*/)), new DOSetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), Vector3.get_one(), 0.3f), 0f));
				}
				m_previousSecond = num2;
			}
			else
			{
				m_chronoText.get_gameObject().SetActive(false);
				m_text.get_gameObject().SetActive(true);
			}
		}

		private void SetAlpha(float value)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Color color = m_chronoText.get_color();
			color.a = value;
			m_chronoText.set_color(color);
		}

		public TurnEndButton()
			: this()
		{
		}
	}
}
