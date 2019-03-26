using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.VisualEffects
{
	[SelectionBase]
	[ExecuteInEditMode]
	public abstract class VisualEffect : MonoBehaviour
	{
		private static readonly int s_colorPropertyId = Shader.PropertyToID("_Color");

		[SerializeField]
		private bool m_playOnAwake = true;

		[SerializeField]
		private VisualEffectDestroyMethod m_destroyMethod;

		[SerializeField]
		private float m_delayedDestructionSeconds = 1f;

		[SerializeField]
		private List<Renderer> m_renderers = new List<Renderer>();

		[SerializeField]
		private bool m_hasParentGroup;

		private MaterialPropertyBlock m_colorModifierPropertyBlock;

		private Coroutine m_destroyMethodCoroutine;

		[NonSerialized]
		public Action<VisualEffect> destructionOverride;

		public VisualEffectState state
		{
			get;
			protected set;
		} = VisualEffectState.Stopped;


		public VisualEffectDestroyMethod destroyMethod => m_destroyMethod;

		public bool hasParentGroup => m_hasParentGroup;

		[PublicAPI]
		public abstract bool IsAlive();

		[PublicAPI]
		public void Play()
		{
			state = VisualEffectState.Playing;
			if (!this.get_isActiveAndEnabled())
			{
				return;
			}
			PlayInternal();
			if (Application.get_isPlaying())
			{
				if (m_destroyMethodCoroutine != null)
				{
					this.StopCoroutine(m_destroyMethodCoroutine);
					m_destroyMethodCoroutine = null;
				}
				switch (m_destroyMethod)
				{
				case VisualEffectDestroyMethod.None:
				case VisualEffectDestroyMethod.WhenStopped:
				case VisualEffectDestroyMethod.WhenStoppedAndFinished:
					break;
				case VisualEffectDestroyMethod.AfterDelay:
					m_destroyMethodCoroutine = this.StartCoroutine(DelayedDestructionCheckRoutine());
					break;
				case VisualEffectDestroyMethod.WhenFinished:
					m_destroyMethodCoroutine = this.StartCoroutine(AutomaticDestructionCheckRoutine());
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		[PublicAPI]
		public void Pause()
		{
			if (state == VisualEffectState.Playing)
			{
				state = VisualEffectState.Paused;
				if (this.get_isActiveAndEnabled())
				{
					PauseInternal();
				}
			}
		}

		[PublicAPI]
		public void Stop(VisualEffectStopMethod stopMethod = VisualEffectStopMethod.Stop)
		{
			state = VisualEffectState.Stopped;
			if (!this.get_isActiveAndEnabled())
			{
				return;
			}
			StopInternal(stopMethod);
			if (m_destroyMethod == VisualEffectDestroyMethod.WhenStoppedAndFinished)
			{
				if (stopMethod == VisualEffectStopMethod.StopAndClear)
				{
					if (destructionOverride != null)
					{
						destructionOverride(this);
					}
					else
					{
						Object.Destroy(this.get_gameObject());
					}
					return;
				}
				if (m_destroyMethodCoroutine != null)
				{
					this.StopCoroutine(m_destroyMethodCoroutine);
				}
				m_destroyMethodCoroutine = this.StartCoroutine(AutomaticDestructionCheckRoutine());
			}
			else if (m_destroyMethod == VisualEffectDestroyMethod.WhenStopped)
			{
				if (destructionOverride != null)
				{
					destructionOverride(this);
				}
				else
				{
					Object.Destroy(this.get_gameObject());
				}
			}
		}

		[PublicAPI]
		public void Clear()
		{
			if (this.get_isActiveAndEnabled())
			{
				ClearInternal();
			}
		}

		[PublicAPI]
		public virtual void SetSortingOrder(int value)
		{
			List<Renderer> renderers = m_renderers;
			int count = renderers.Count;
			for (int i = 0; i < count; i++)
			{
				Renderer val = renderers[i];
				if (null != val)
				{
					val.set_sortingOrder(value);
				}
			}
		}

		[PublicAPI]
		public virtual void SetColorModifier(Color color)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			MaterialPropertyBlock val = m_colorModifierPropertyBlock;
			if (val == null)
			{
				val = (m_colorModifierPropertyBlock = new MaterialPropertyBlock());
			}
			val.SetColor(s_colorPropertyId, color);
			List<Renderer> renderers = m_renderers;
			int count = renderers.Count;
			for (int i = 0; i < count; i++)
			{
				Renderer val2 = renderers[i];
				if (null != val2)
				{
					SpriteRenderer val3 = val2 as SpriteRenderer;
					if (null != val3)
					{
						val3.set_color(color);
					}
					else
					{
						val2.SetPropertyBlock(val);
					}
				}
			}
		}

		protected abstract void PlayInternal();

		protected abstract void PauseInternal();

		protected abstract void StopInternal(VisualEffectStopMethod stopMethod);

		protected abstract void ClearInternal();

		internal void GroupPlayedInternal()
		{
			state = VisualEffectState.Playing;
			if (this.get_isActiveAndEnabled())
			{
				PlayInternal();
			}
		}

		internal void GroupPausedInternal()
		{
			state = VisualEffectState.Paused;
			if (this.get_isActiveAndEnabled())
			{
				PauseInternal();
			}
		}

		internal void GroupStoppedInternal(VisualEffectStopMethod stopMethod)
		{
			state = VisualEffectState.Stopped;
			if (this.get_isActiveAndEnabled())
			{
				StopInternal(stopMethod);
			}
		}

		internal void GroupClearedInternal()
		{
			if (this.get_isActiveAndEnabled())
			{
				ClearInternal();
			}
		}

		protected IEnumerator AutomaticDestructionCheckRoutine()
		{
			do
			{
				yield return null;
			}
			while (IsAlive());
			if (destructionOverride != null)
			{
				destructionOverride(this);
			}
			else
			{
				Object.Destroy(this.get_gameObject());
			}
		}

		protected IEnumerator DelayedDestructionCheckRoutine()
		{
			float timeLeft = m_delayedDestructionSeconds;
			do
			{
				yield return null;
				if (state != VisualEffectState.Paused)
				{
					timeLeft -= Time.get_deltaTime();
				}
			}
			while (timeLeft > 0f);
			if (destructionOverride != null)
			{
				destructionOverride(this);
			}
			else
			{
				Object.Destroy(this.get_gameObject());
			}
		}

		private void Awake()
		{
			if (m_playOnAwake)
			{
				Play();
			}
		}

		private void OnEnable()
		{
			if (state == VisualEffectState.Playing)
			{
				Play();
			}
		}

		private void OnDisable()
		{
			if (!m_hasParentGroup)
			{
				if (state != VisualEffectState.Stopped)
				{
					StopInternal(VisualEffectStopMethod.StopAndClear);
				}
				else
				{
					ClearInternal();
				}
			}
		}

		protected VisualEffect()
			: this()
		{
		}
	}
}
