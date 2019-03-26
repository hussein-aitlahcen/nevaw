using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.VisualEffects
{
	[SelectionBase]
	[ExecuteInEditMode]
	public class AnimationVisualEffect : VisualEffect
	{
		public enum StopMode
		{
			Natural,
			CrossFade,
			Skip
		}

		[SerializeField]
		private Animation m_animationController;

		[SerializeField]
		private StopMode m_stopMode;

		[SerializeField]
		private float m_endCrossFadeDuration = 0.5f;

		[SerializeField]
		private AnimationClip m_startClip;

		[SerializeField]
		private AnimationClip m_idleClip;

		[SerializeField]
		private AnimationClip m_endClip;

		private Coroutine m_playQueuedCoroutine;

		private Coroutine m_stopQueuedCoroutine;

		public override bool IsAlive()
		{
			if (null == m_animationController)
			{
				return false;
			}
			if (base.state == VisualEffectState.Stopped && m_playQueuedCoroutine == null && !m_animationController.get_isPlaying())
			{
				return m_stopQueuedCoroutine != null;
			}
			return true;
		}

		protected override void PlayInternal()
		{
			if (null == m_animationController)
			{
				return;
			}
			if (m_playQueuedCoroutine != null)
			{
				this.StopCoroutine(m_playQueuedCoroutine);
				m_playQueuedCoroutine = null;
			}
			if (m_stopQueuedCoroutine != null)
			{
				this.StopCoroutine(m_stopQueuedCoroutine);
				m_stopQueuedCoroutine = null;
			}
			if (base.state != VisualEffectState.Paused)
			{
				SetSpeed(1f);
				return;
			}
			if (null != m_startClip)
			{
				m_animationController.Play(m_startClip.get_name(), 4);
				if (null != m_idleClip)
				{
					m_playQueuedCoroutine = this.StartCoroutine(PlayQueued(m_idleClip.get_name()));
				}
			}
			else if (null != m_idleClip)
			{
				m_animationController.Play(m_idleClip.get_name(), 4);
			}
			else
			{
				Log.Warning("AnimationVisualEffect named '" + this.get_name() + "' doesn't have a Start animation nor an Idle animation.", 104, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\VisualEffects\\AnimationVisualEffect.cs");
			}
			if (base.destroyMethod == VisualEffectDestroyMethod.WhenFinished)
			{
				m_stopQueuedCoroutine = this.StartCoroutine(StopQueued());
			}
		}

		protected override void PauseInternal()
		{
			if (!(null == m_animationController))
			{
				SetSpeed(0f);
			}
		}

		protected override void StopInternal(VisualEffectStopMethod stopMethod)
		{
			if (null == m_animationController)
			{
				return;
			}
			if (m_playQueuedCoroutine != null)
			{
				this.StopCoroutine(m_playQueuedCoroutine);
				m_playQueuedCoroutine = null;
			}
			if (m_stopQueuedCoroutine != null)
			{
				this.StopCoroutine(m_stopQueuedCoroutine);
				m_stopQueuedCoroutine = null;
			}
			if (base.state == VisualEffectState.Paused)
			{
				SetSpeed(1f);
			}
			if (null == m_endClip)
			{
				m_animationController.Stop();
				return;
			}
			switch (m_stopMode)
			{
			case StopMode.Natural:
				m_playQueuedCoroutine = this.StartCoroutine(PlayQueued(m_endClip.get_name()));
				break;
			case StopMode.CrossFade:
				m_animationController.CrossFade(m_endClip.get_name(), m_endCrossFadeDuration);
				break;
			case StopMode.Skip:
				m_animationController.Play(m_endClip.get_name(), 4);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		protected override void ClearInternal()
		{
		}

		private IEnumerator PlayQueued(string clipName)
		{
			AnimationState currentAnimationState = null;
			if (null != m_startClip && m_animationController.IsPlaying(m_startClip.get_name()))
			{
				currentAnimationState = m_animationController.get_Item(m_startClip.get_name());
			}
			else if (null != m_idleClip && m_animationController.IsPlaying(m_idleClip.get_name()))
			{
				currentAnimationState = m_animationController.get_Item(m_idleClip.get_name());
			}
			else if (null != m_endClip && m_animationController.IsPlaying(m_endClip.get_name()))
			{
				currentAnimationState = m_animationController.get_Item(m_endClip.get_name());
			}
			if (null != currentAnimationState)
			{
				WrapMode wrapMode = currentAnimationState.get_wrapMode();
				switch ((int)wrapMode)
				{
				case 1:
					do
					{
						yield return null;
					}
					while (currentAnimationState.get_enabled());
					break;
				case 2:
				{
					float normalizedTime2 = currentAnimationState.get_normalizedTime();
					float normalizedLoopThreshold2 = Mathf.Ceil(normalizedTime2);
					if (!Mathf.Approximately(normalizedLoopThreshold2, normalizedTime2))
					{
						do
						{
							yield return null;
						}
						while (currentAnimationState.get_enabled() && currentAnimationState.get_normalizedTime() < normalizedLoopThreshold2);
					}
					break;
				}
				case 4:
				{
					float normalizedTime = currentAnimationState.get_normalizedTime();
					float normalizedLoopThreshold2 = 2f * Mathf.Ceil(0.5f * normalizedTime);
					if (!Mathf.Approximately(normalizedLoopThreshold2, normalizedTime))
					{
						do
						{
							yield return null;
						}
						while (currentAnimationState.get_enabled() && currentAnimationState.get_normalizedTime() < normalizedLoopThreshold2);
					}
					break;
				}
				case 0:
					do
					{
						yield return null;
					}
					while (currentAnimationState.get_enabled());
					break;
				case 8:
					do
					{
						yield return null;
					}
					while (currentAnimationState.get_enabled() && currentAnimationState.get_normalizedTime() <= 1f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			m_animationController.Play(clipName);
			m_playQueuedCoroutine = null;
		}

		private IEnumerator StopQueued()
		{
			while (m_playQueuedCoroutine != null || m_animationController.get_isPlaying())
			{
				yield return null;
			}
			m_stopQueuedCoroutine = null;
			Stop();
		}

		private void SetSpeed(float value)
		{
			if (null != m_startClip)
			{
				m_animationController.get_Item(m_startClip.get_name()).set_speed(value);
			}
			if (null != m_idleClip)
			{
				m_animationController.get_Item(m_idleClip.get_name()).set_speed(value);
			}
			if (null != m_endClip)
			{
				m_animationController.get_Item(m_endClip.get_name()).set_speed(value);
			}
		}
	}
}
