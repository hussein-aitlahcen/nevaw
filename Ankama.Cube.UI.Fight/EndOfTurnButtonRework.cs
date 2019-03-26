using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public sealed class EndOfTurnButtonRework : MonoBehaviour
	{
		public enum State
		{
			None,
			LocalPlayer,
			LocalPlayerTeam,
			OpponentTeam
		}

		internal enum TimerState
		{
			Normal,
			Warning,
			Alert
		}

		[Header("Links")]
		[SerializeField]
		private TextField m_text;

		[SerializeField]
		private RawTextField m_textTurnTime;

		[SerializeField]
		private Image m_timeFilling;

		[Header("Colors")]
		[SerializeField]
		private FightMapFeedbackColors m_colors;

		[Header("Time Limit Animation")]
		[SerializeField]
		private Animator m_timerTextAnimator;

		[SerializeField]
		private Gradient m_warningColor;

		[SerializeField]
		private Button m_button;

		[Header("Audio Events")]
		[SerializeField]
		private UnityEvent m_onEndOfTurn;

		[SerializeField]
		private UnityEvent m_onEndOfTurnBeginAlert;

		[SerializeField]
		private UnityEvent m_onEndOfTurnEndAlert;

		private static readonly int s_warningHash = Animator.StringToHash("Warning");

		private static readonly int s_normalHash = Animator.StringToHash("Normal");

		private static readonly int s_alertHash = Animator.StringToHash("Alert");

		public Action onClick;

		private int m_turnDuration;

		private State m_state;

		private bool m_running;

		private float m_turnStartTime;

		private TimerState m_currentTimerState;

		private Tween m_timerColorTween;

		private int m_previousDisplayedTime = int.MinValue;

		public void SetState(State value)
		{
			m_state = value;
			m_button.set_interactable(value == State.LocalPlayer);
			RefreshStateView();
		}

		public void StartTurn(int turnIndex, int turnDuration)
		{
			m_turnDuration = turnDuration;
			UpdateTimerState(m_turnDuration);
			m_turnStartTime = Time.get_unscaledTime();
			m_running = true;
			ResetTimer();
		}

		public void EndTurn()
		{
			ResetTimer();
			m_running = false;
		}

		public void ShowEndOfTurn()
		{
			m_onEndOfTurn.Invoke();
		}

		private unsafe void Awake()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			m_button.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			ResetTimer();
		}

		private void Update()
		{
			if (m_running)
			{
				float num = Time.get_unscaledTime() - m_turnStartTime;
				int remainingTimeInSeconds = (int)((float)m_turnDuration - num);
				RefreshTime(remainingTimeInSeconds, num);
			}
		}

		private void RefreshStateView()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			switch (m_state)
			{
			case State.None:
				m_timeFilling.set_color(new Color(0f, 0f, 0f, 0f));
				m_text.SetText(0);
				break;
			case State.LocalPlayer:
				m_timeFilling.set_color(m_colors.GetPlayerColor(PlayerType.Player));
				m_text.SetText(51179);
				break;
			case State.LocalPlayerTeam:
				m_timeFilling.set_color(m_colors.GetPlayerColor(PlayerType.Ally));
				m_text.SetText(0);
				break;
			case State.OpponentTeam:
				m_timeFilling.set_color(m_colors.GetPlayerColor(PlayerType.Opponent | PlayerType.Local));
				m_text.SetText(85537);
				break;
			default:
				throw new ArgumentOutOfRangeException("m_state", m_state, null);
			}
		}

		private void UpdateTimerState(int secondsRemaining)
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			if (m_timerTextAnimator == null)
			{
				return;
			}
			TimerState timerState = TimerState.Normal;
			if (secondsRemaining < 0 || secondsRemaining > 15)
			{
				timerState = TimerState.Normal;
			}
			else if (secondsRemaining > 5 && secondsRemaining <= 15)
			{
				timerState = TimerState.Warning;
			}
			else if (secondsRemaining <= 5 && secondsRemaining >= 0)
			{
				timerState = TimerState.Alert;
			}
			if (timerState != m_currentTimerState)
			{
				Tween timerColorTween = m_timerColorTween;
				if (timerColorTween != null)
				{
					TweenExtensions.Kill(timerColorTween, false);
				}
				m_timerColorTween = null;
				m_currentTimerState = timerState;
				switch (timerState)
				{
				case TimerState.Normal:
					m_timerTextAnimator.SetTrigger(s_normalHash);
					break;
				case TimerState.Warning:
				{
					m_timerTextAnimator.SetTrigger(s_warningHash);
					Sequence val = DOTween.Sequence();
					TweenSettingsExtensions.Insert(val, 0f, DOTweenModuleUI.DOColor(m_timeFilling, m_warningColor.Evaluate(1f), 0.5f));
					TweenSettingsExtensions.Insert(val, 0f, m_textTurnTime.DOColor(m_warningColor.Evaluate(1f), 0.5f));
					m_timerColorTween = val;
					break;
				}
				case TimerState.Alert:
					m_timerTextAnimator.SetTrigger(s_alertHash);
					m_textTurnTime.DOColor(m_warningColor.Evaluate(0f), 0.5f);
					m_timerColorTween = TweenSettingsExtensions.SetLoops<Tweener>(DOTweenModuleUI.DOColor(m_timeFilling, m_warningColor.Evaluate(0f), 0.5f), -1, 1);
					m_onEndOfTurnBeginAlert.Invoke();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void RefreshTime(int remainingTimeInSeconds, float turnTime)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			m_timeFilling.set_fillAmount(Mathf.Clamp01(turnTime / (float)Mathf.Max(1, m_turnDuration)));
			if (m_previousDisplayedTime == remainingTimeInSeconds)
			{
				return;
			}
			m_previousDisplayedTime = remainingTimeInSeconds;
			UpdateTimerState(remainingTimeInSeconds);
			if (Object.op_Implicit(m_textTurnTime))
			{
				if (remainingTimeInSeconds < 0)
				{
					m_textTurnTime.color = Color.get_white();
					m_textTurnTime.SetText("");
				}
				else
				{
					m_textTurnTime.SetText(remainingTimeInSeconds.ToString());
				}
			}
		}

		private void ResetTimer()
		{
			RefreshTime(-1, 0f);
			m_onEndOfTurnEndAlert.Invoke();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			DoClick();
		}

		public void SimulateClick()
		{
			DoClick();
		}

		private void DoClick()
		{
			if (m_state == State.LocalPlayer && !UIManager.instance.userInteractionLocked && !DragNDropListener.instance.dragging)
			{
				onClick?.Invoke();
			}
		}

		public EndOfTurnButtonRework()
			: this()
		{
		}
	}
}
