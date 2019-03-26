using Ankama.Animations;
using Ankama.Animations.Events;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Ankama.Cube.Animations
{
	[ExecuteInEditMode]
	public sealed class FloorMechanismAnimator : MonoBehaviour, IAnimator2D
	{
		public const string Spawn = "spawn";

		public const string Idle = "idle";

		public const string Detection = "detection";

		public const string AllyActivation = "ally_activation";

		public const string OpponentActivation = "opponent_activation";

		public const string Destruction = "die";

		[SerializeField]
		private AnimationClip m_spawnAnimation;

		[SerializeField]
		private AnimationClip m_idleAnimation;

		[SerializeField]
		private AnimationClip m_detectionAnimation;

		[SerializeField]
		private AnimationClip m_allyActivationAnimation;

		[SerializeField]
		private AnimationClip m_opponentActivationAnimation;

		[SerializeField]
		private AnimationClip m_destructionAnimation;

		[HideInInspector]
		[SerializeField]
		private Renderer[] m_renderers;

		private Animation m_controller;

		private AnimationClip m_currentAnimation;

		private string m_currentLabel = string.Empty;

		private Color m_colorModifier;

		private int m_sortingLayerId;

		private int m_sortingOrder;

		public bool paused
		{
			get
			{
				return m_controller.get_isPlaying();
			}
			set
			{
				if (value)
				{
					m_controller.Play(4);
				}
				else
				{
					m_controller.Stop();
				}
			}
		}

		public int frameRate
		{
			get
			{
				AnimationClip currentAnimation = m_currentAnimation;
				if (!(null == currentAnimation))
				{
					return (int)currentAnimation.get_frameRate();
				}
				return 60;
			}
			set
			{
				throw new NotImplementedException("Cannot set frame rate on an FloorMechanismAnimator component.");
			}
		}

		public string animationName
		{
			get
			{
				AnimationClip currentAnimation = m_currentAnimation;
				if (!(null == currentAnimation))
				{
					return currentAnimation.get_name();
				}
				return string.Empty;
			}
		}

		public bool animationLoops
		{
			get
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Invalid comparison between Unknown and I4
				if (null != m_controller)
				{
					return (int)m_controller.get_wrapMode() == 2;
				}
				return false;
			}
			set
			{
				if (null != m_controller)
				{
					m_controller.set_wrapMode(value ? 2 : 8);
				}
			}
		}

		public int animationFrameCount
		{
			get
			{
				AnimationClip currentAnimation = m_currentAnimation;
				if (!(null == currentAnimation))
				{
					return Mathf.CeilToInt(currentAnimation.get_length() * currentAnimation.get_frameRate());
				}
				return 0;
			}
		}

		public int currentFrame
		{
			get
			{
				AnimationClip currentAnimation = m_currentAnimation;
				if (null == currentAnimation)
				{
					return 0;
				}
				return Mathf.FloorToInt(m_controller.get_Item(m_currentAnimation.get_name()).get_time() * currentAnimation.get_frameRate());
			}
			set
			{
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Invalid comparison between Unknown and I4
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Expected O, but got Unknown
				AnimationClip currentAnimation = m_currentAnimation;
				if (null == currentAnimation)
				{
					return;
				}
				string name = m_currentAnimation.get_name();
				float length = currentAnimation.get_length();
				AnimationState val = m_controller.get_Item(name);
				val.set_time(Mathf.Clamp((float)value / currentAnimation.get_frameRate(), 0f, length));
				if ((int)m_controller.get_wrapMode() == 2)
				{
					return;
				}
				if (this.AnimationEnded != null)
				{
					bool reachedEndOfAnimation = this.reachedEndOfAnimation;
					this.reachedEndOfAnimation = Mathf.Approximately(val.get_time(), length);
					if (!reachedEndOfAnimation && this.reachedEndOfAnimation)
					{
						AnimationEndedEventArgs val2 = new AnimationEndedEventArgs(name);
						this.AnimationEnded.Invoke((object)this, val2);
					}
				}
				else
				{
					this.reachedEndOfAnimation = Mathf.Approximately(val.get_time(), length);
				}
			}
		}

		public string currentLabel => m_currentLabel;

		public bool reachedEndOfAnimation
		{
			get;
			private set;
		}

		public Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_colorModifier;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				if (!(m_colorModifier == value))
				{
					Renderer[] renderers = m_renderers;
					int num = renderers.Length;
					for (int i = 0; i < num; i++)
					{
						renderers[i].get_material().set_color(value);
					}
					m_colorModifier = value;
				}
			}
		}

		public int sortingLayerId
		{
			get
			{
				return m_sortingLayerId;
			}
			set
			{
				if (m_sortingLayerId != value)
				{
					Renderer[] renderers = m_renderers;
					int num = renderers.Length;
					for (int i = 0; i < num; i++)
					{
						renderers[i].set_sortingLayerID(value);
					}
					m_sortingLayerId = value;
				}
			}
		}

		public int sortingOrder
		{
			get
			{
				return m_sortingOrder;
			}
			set
			{
				if (m_sortingOrder != value)
				{
					Renderer[] renderers = m_renderers;
					int num = renderers.Length;
					for (int i = 0; i < num; i++)
					{
						renderers[i].set_sortingOrder(value);
					}
					m_sortingOrder = value;
				}
			}
		}

		public event Animator2DInitialisedEventHandler Initialised
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				Animator2DInitialisedEventHandler val = this.Initialised;
				Animator2DInitialisedEventHandler val2;
				do
				{
					val2 = val;
					Animator2DInitialisedEventHandler value2 = Delegate.Combine((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.Initialised, value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				Animator2DInitialisedEventHandler val = this.Initialised;
				Animator2DInitialisedEventHandler val2;
				do
				{
					val2 = val;
					Animator2DInitialisedEventHandler value2 = Delegate.Remove((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.Initialised, value2, val2);
				}
				while (val != val2);
			}
		}

		public event AnimationStartedEventHandler AnimationStarted
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				AnimationStartedEventHandler val = this.AnimationStarted;
				AnimationStartedEventHandler val2;
				do
				{
					val2 = val;
					AnimationStartedEventHandler value2 = Delegate.Combine((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.AnimationStarted, value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				AnimationStartedEventHandler val = this.AnimationStarted;
				AnimationStartedEventHandler val2;
				do
				{
					val2 = val;
					AnimationStartedEventHandler value2 = Delegate.Remove((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.AnimationStarted, value2, val2);
				}
				while (val != val2);
			}
		}

		public event AnimationLabelChangedEventHandler AnimationLabelChanged
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				AnimationLabelChangedEventHandler val = this.AnimationLabelChanged;
				AnimationLabelChangedEventHandler val2;
				do
				{
					val2 = val;
					AnimationLabelChangedEventHandler value2 = Delegate.Combine((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.AnimationLabelChanged, value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				AnimationLabelChangedEventHandler val = this.AnimationLabelChanged;
				AnimationLabelChangedEventHandler val2;
				do
				{
					val2 = val;
					AnimationLabelChangedEventHandler value2 = Delegate.Remove((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.AnimationLabelChanged, value2, val2);
				}
				while (val != val2);
			}
		}

		public event AnimationLoopedEventHandler AnimationLooped
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				AnimationLoopedEventHandler val = this.AnimationLooped;
				AnimationLoopedEventHandler val2;
				do
				{
					val2 = val;
					AnimationLoopedEventHandler value2 = Delegate.Combine((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.AnimationLooped, value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				AnimationLoopedEventHandler val = this.AnimationLooped;
				AnimationLoopedEventHandler val2;
				do
				{
					val2 = val;
					AnimationLoopedEventHandler value2 = Delegate.Remove((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.AnimationLooped, value2, val2);
				}
				while (val != val2);
			}
		}

		public event AnimationEndedEventHandler AnimationEnded
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				AnimationEndedEventHandler val = this.AnimationEnded;
				AnimationEndedEventHandler val2;
				do
				{
					val2 = val;
					AnimationEndedEventHandler value2 = Delegate.Combine((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.AnimationEnded, value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				AnimationEndedEventHandler val = this.AnimationEnded;
				AnimationEndedEventHandler val2;
				do
				{
					val2 = val;
					AnimationEndedEventHandler value2 = Delegate.Remove((Delegate)val2, (Delegate)value);
					val = Interlocked.CompareExchange(ref this.AnimationEnded, value2, val2);
				}
				while (val != val2);
			}
		}

		[PublicAPI]
		public bool HasSpawnAnimation()
		{
			return null != m_spawnAnimation;
		}

		[PublicAPI]
		public bool TryGetSpawnAnimationName([NotNull] out string animName)
		{
			if (null != m_spawnAnimation)
			{
				animName = m_spawnAnimation.get_name();
				return true;
			}
			animName = string.Empty;
			return false;
		}

		[PublicAPI]
		public bool HasIdleAnimation()
		{
			return null != m_idleAnimation;
		}

		[PublicAPI]
		public bool TryGetIdleAnimationName([NotNull] out string animName)
		{
			if (null != m_idleAnimation)
			{
				animName = m_idleAnimation.get_name();
				return true;
			}
			animName = string.Empty;
			return false;
		}

		[PublicAPI]
		public bool HasDetectionAnimation()
		{
			return null != m_detectionAnimation;
		}

		[PublicAPI]
		public bool TryGetDetectionAnimationName([NotNull] out string animName)
		{
			if (null != m_detectionAnimation)
			{
				animName = m_detectionAnimation.get_name();
				return true;
			}
			animName = string.Empty;
			return false;
		}

		[PublicAPI]
		public bool HasAllyActivationAnimation()
		{
			return null != m_allyActivationAnimation;
		}

		[PublicAPI]
		public bool TryGetAllyActivationAnimationName([NotNull] out string animName)
		{
			if (null != m_allyActivationAnimation)
			{
				animName = m_allyActivationAnimation.get_name();
				return true;
			}
			animName = string.Empty;
			return false;
		}

		[PublicAPI]
		public bool HasOpponentActivationAnimation()
		{
			return null != m_opponentActivationAnimation;
		}

		[PublicAPI]
		public bool TryGetOpponentActivationAnimationName([NotNull] out string animName)
		{
			if (null != m_opponentActivationAnimation)
			{
				animName = m_opponentActivationAnimation.get_name();
				return true;
			}
			animName = string.Empty;
			return false;
		}

		[PublicAPI]
		public bool HasDestructionAnimation()
		{
			return null != m_destructionAnimation;
		}

		[PublicAPI]
		public bool TryGetDestructionAnimationName([NotNull] out string animName)
		{
			if (null != m_destructionAnimation)
			{
				animName = m_destructionAnimation.get_name();
				return true;
			}
			animName = string.Empty;
			return false;
		}

		public Animator2DInitialisationState GetInitialisationState()
		{
			return 3;
		}

		public bool CurrentAnimationHasLabel(string labelName, out int frame)
		{
			AnimationEvent[] events = m_currentAnimation.get_events();
			int num = events.Length;
			for (int i = 0; i < num; i++)
			{
				AnimationEvent val = events[i];
				if (val.get_functionName().Equals("SetLabel") && val.get_stringParameter().Equals(labelName))
				{
					frame = Mathf.FloorToInt(val.get_time() * m_currentAnimation.get_frameRate());
					return true;
				}
			}
			frame = 0;
			return false;
		}

		public bool CurrentAnimationHasLabel(string labelName, StringComparison comparisonType, out int frame)
		{
			AnimationEvent[] events = m_currentAnimation.get_events();
			int num = events.Length;
			for (int i = 0; i < num; i++)
			{
				AnimationEvent val = events[i];
				if (val.get_functionName().Equals("SetLabel") && val.get_stringParameter().Equals(labelName, comparisonType))
				{
					frame = Mathf.FloorToInt(val.get_time() * m_currentAnimation.get_frameRate());
					return true;
				}
			}
			frame = 0;
			return false;
		}

		public void SetAnimation(string animName, bool animLoops, bool async = true, bool restart = true)
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Invalid comparison between Unknown and I4
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected O, but got Unknown
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Invalid comparison between Unknown and I4
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Expected O, but got Unknown
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Expected O, but got Unknown
			bool reachedEndOfAnimation = this.reachedEndOfAnimation;
			AnimationClip currentAnimation = m_currentAnimation;
			string text;
			if (null != currentAnimation)
			{
				text = currentAnimation.get_name();
				if (text.Equals(animName))
				{
					if (!restart)
					{
						return;
					}
					m_currentLabel = string.Empty;
					AnimationState val = m_controller.get_Item(animName);
					if (val.get_enabled())
					{
						val.set_time(0f);
					}
					else
					{
						m_controller.Play(animName, 4);
					}
					if ((int)m_controller.get_wrapMode() != 2)
					{
						this.reachedEndOfAnimation = Mathf.Approximately(0f, currentAnimation.get_length());
						if (this.AnimationEnded != null && this.reachedEndOfAnimation)
						{
							AnimationEndedEventArgs val2 = new AnimationEndedEventArgs(animName);
							this.AnimationEnded.Invoke((object)this, val2);
						}
					}
					else
					{
						this.reachedEndOfAnimation = false;
					}
					return;
				}
			}
			else
			{
				text = string.Empty;
			}
			m_currentLabel = string.Empty;
			if (!m_controller.Play(animName, 4))
			{
				Log.Warning("Could not play animation named '" + animName + "' on GameObject named '" + this.get_name() + "'.", 479, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\FloorMechanismAnimator.cs");
				return;
			}
			AnimationState val3 = m_controller.get_Item(animName);
			if (null == val3)
			{
				Log.Warning("Could not play animation named '" + animName + "' on GameObject named '" + this.get_name() + "'.", 495, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\FloorMechanismAnimator.cs");
				return;
			}
			m_currentAnimation = val3.get_clip();
			if ((int)m_controller.get_wrapMode() != 2)
			{
				this.reachedEndOfAnimation = (val3.get_time() >= val3.get_length());
			}
			else
			{
				this.reachedEndOfAnimation = false;
			}
			if (this.AnimationStarted != null)
			{
				AnimationStartedEventArgs val4 = new AnimationStartedEventArgs(animName, text, 0);
				this.AnimationStarted.Invoke((object)this, val4);
			}
			if (this.AnimationEnded != null && this.reachedEndOfAnimation && !reachedEndOfAnimation)
			{
				AnimationEndedEventArgs val5 = new AnimationEndedEventArgs(animName);
				this.AnimationEnded.Invoke((object)this, val5);
			}
		}

		public void SetLabel([NotNull] string value)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			if (null == m_currentAnimation)
			{
				Log.Warning("SetLabel called while no animation is playing.", 527, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\FloorMechanismAnimator.cs");
			}
			else if (!m_currentLabel.Equals(value))
			{
				m_currentLabel = value;
				if (this.AnimationLabelChanged != null)
				{
					AnimationLabelChangedEventArgs val = new AnimationLabelChangedEventArgs(m_currentAnimation.get_name(), string.Empty);
					this.AnimationLabelChanged.Invoke((object)this, val);
				}
			}
		}

		public void ClearLabel()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			if (null == m_currentAnimation)
			{
				Log.Warning("SetLabel called while no animation is playing.", 559, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\FloorMechanismAnimator.cs");
			}
			else if (m_currentLabel.Length != 0)
			{
				m_currentLabel = string.Empty;
				if (this.AnimationLabelChanged != null)
				{
					AnimationLabelChangedEventArgs val = new AnimationLabelChangedEventArgs(m_currentAnimation.get_name(), string.Empty);
					this.AnimationLabelChanged.Invoke((object)this, val);
				}
			}
		}

		private IEnumerable<AnimationClip> EnumerateAnimationProperties()
		{
			yield return m_spawnAnimation;
			yield return m_idleAnimation;
			yield return m_detectionAnimation;
			yield return m_allyActivationAnimation;
			yield return m_opponentActivationAnimation;
			yield return m_destructionAnimation;
		}

		private void Awake()
		{
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Expected O, but got Unknown
			m_controller = this.GetComponent<Animation>();
			if (null == m_controller)
			{
				m_controller = this.get_gameObject().AddComponent<Animation>();
				if (null != m_spawnAnimation)
				{
					m_controller.AddClip(m_spawnAnimation, m_spawnAnimation.get_name());
				}
				if (null != m_idleAnimation)
				{
					m_controller.AddClip(m_idleAnimation, m_idleAnimation.get_name());
				}
				if (null != m_detectionAnimation)
				{
					m_controller.AddClip(m_detectionAnimation, m_detectionAnimation.get_name());
				}
				if (null != m_allyActivationAnimation)
				{
					m_controller.AddClip(m_allyActivationAnimation, m_allyActivationAnimation.get_name());
				}
				if (null != m_opponentActivationAnimation)
				{
					m_controller.AddClip(m_opponentActivationAnimation, m_opponentActivationAnimation.get_name());
				}
				if (null != m_destructionAnimation)
				{
					m_controller.AddClip(m_destructionAnimation, m_destructionAnimation.get_name());
				}
			}
			else
			{
				int clipCount = m_controller.GetClipCount();
				if (clipCount > 0)
				{
					int num = 0;
					string[] array = new string[clipCount];
					IEnumerator enumerator = m_controller.GetEnumerator();
					while (enumerator.MoveNext())
					{
						AnimationState val = enumerator.Current as AnimationState;
						if (null != val)
						{
							string name = val.get_name();
							AnimationClip clip = val.get_clip();
							if (null != clip)
							{
								foreach (AnimationClip item in EnumerateAnimationProperties())
								{
									if (!(clip == item) || !item.get_name().Equals(name))
									{
										continue;
									}
									goto IL_01ec;
								}
							}
							array[num] = val.get_name();
							num++;
						}
						IL_01ec:;
					}
					for (int i = 0; i < num; i++)
					{
						m_controller.RemoveClip(array[i]);
					}
				}
				foreach (AnimationClip item2 in EnumerateAnimationProperties())
				{
					if (null != item2 && null == m_controller.get_Item(item2.get_name()))
					{
						m_controller.AddClip(item2, item2.get_name());
					}
				}
			}
			m_controller.set_playAutomatically(false);
			if (this.Initialised != null)
			{
				Animator2DInitialisedEventArgs val2 = new Animator2DInitialisedEventArgs();
				this.Initialised.Invoke((object)this, val2);
			}
		}

		private void LateUpdate()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Invalid comparison between Unknown and I4
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Expected O, but got Unknown
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			AnimationClip currentAnimation = m_currentAnimation;
			if (null == currentAnimation)
			{
				return;
			}
			Animation controller = m_controller;
			if (controller.get_isPlaying())
			{
				if ((int)controller.get_wrapMode() != 2)
				{
					if (reachedEndOfAnimation)
					{
						return;
					}
					string name = currentAnimation.get_name();
					AnimationState val = m_controller.get_Item(name);
					if (val.get_time() >= val.get_length())
					{
						reachedEndOfAnimation = true;
						if (this.AnimationEnded != null)
						{
							AnimationEndedEventArgs val2 = new AnimationEndedEventArgs(name);
							this.AnimationEnded.Invoke((object)this, val2);
						}
					}
				}
				else if (this.AnimationLooped != null)
				{
					string name2 = currentAnimation.get_name();
					AnimationState obj = m_controller.get_Item(name2);
					float time = obj.get_time();
					float length = obj.get_length();
					int num = (int)(Mathf.Max(0f, time - Time.get_deltaTime()) / length);
					if ((int)(time / length) > num)
					{
						AnimationLoopedEventArgs val3 = new AnimationLoopedEventArgs(name2);
						this.AnimationLooped.Invoke((object)this, val3);
					}
				}
			}
			else if (!reachedEndOfAnimation)
			{
				reachedEndOfAnimation = true;
				if (this.AnimationEnded != null)
				{
					AnimationEndedEventArgs val4 = new AnimationEndedEventArgs(currentAnimation.get_name());
					this.AnimationEnded.Invoke((object)this, val4);
				}
			}
		}

		private void OnDisable()
		{
			if (null != m_controller)
			{
				m_controller.Stop();
			}
		}

		public FloorMechanismAnimator()
			: this()
		{
		}

		GameObject IAnimator2D.get_gameObject()
		{
			return this.get_gameObject();
		}

		Transform IAnimator2D.get_transform()
		{
			return this.get_transform();
		}

		bool IAnimator2D.get_enabled()
		{
			return this.get_enabled();
		}

		void IAnimator2D.set_enabled(bool value)
		{
			this.set_enabled(value);
		}

		bool IAnimator2D.get_isActiveAndEnabled()
		{
			return this.get_isActiveAndEnabled();
		}
	}
}
