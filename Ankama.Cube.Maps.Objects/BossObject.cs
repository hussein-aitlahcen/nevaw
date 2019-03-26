using Ankama.Animations;
using Ankama.Animations.Events;
using Ankama.AssetManagement;
using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Maps.Objects
{
	public abstract class BossObject : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField]
		protected Animator2D m_animator2D;

		[SerializeField]
		protected PlayableDirector m_playableDirector;

		[Header("Anim/Timeline")]
		[SerializeField]
		protected TimelineAssetDictionary m_timelineAssetDictionary;

		private CharacterAnimationCallback m_animationCallback;

		private bool m_hasTimeline;

		private bool m_loaded;

		protected abstract string spawnAnimation
		{
			get;
		}

		protected abstract string deathAnimation
		{
			get;
		}

		public abstract void GoToIdle();

		protected unsafe void Awake()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			m_animationCallback = new CharacterAnimationCallback(m_animator2D);
			m_animator2D.add_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_animator2D.add_AnimationLooped(new AnimationLoopedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_playableDirector.set_playableAsset(null);
			CameraHandler.AddMapRotationListener(OnMapRotationChanged);
			this.StartCoroutine(Load());
		}

		protected IEnumerator Load()
		{
			while (!AssetManager.get_isInitialized())
			{
				yield return null;
			}
			yield return LoadTimelines();
			m_loaded = true;
		}

		protected unsafe virtual void OnDestroy()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			CameraHandler.RemoveMapRotationListener(OnMapRotationChanged);
			if (null != m_animator2D)
			{
				m_animator2D.remove_AnimationLooped(new AnimationLoopedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_animator2D.remove_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			if (m_animationCallback != null)
			{
				m_animationCallback.Release();
				m_animationCallback = null;
			}
			Clear();
			UnloadTimelines();
			m_loaded = false;
		}

		protected virtual IEnumerator LoadTimelines()
		{
			foreach (KeyValuePair<string, TimelineAsset> item in (Dictionary<string, TimelineAsset>)m_timelineAssetDictionary)
			{
				yield return TimelineUtility.LoadTimelineResources(item.Value);
			}
		}

		protected virtual void UnloadTimelines()
		{
			foreach (KeyValuePair<string, TimelineAsset> item in (Dictionary<string, TimelineAsset>)m_timelineAssetDictionary)
			{
				TimelineUtility.UnloadTimelineResources(item.Value);
			}
		}

		protected void Clear()
		{
			m_playableDirector.Stop();
			m_playableDirector.set_playableAsset(null);
		}

		public IEnumerator Spawn()
		{
			PlayAnimation(spawnAnimation, GoToIdle);
			yield break;
		}

		public IEnumerator Die()
		{
			PlayAnimation(deathAnimation);
			int num = default(int);
			if (m_animator2D.CurrentAnimationHasLabel("die", ref num))
			{
				while (!HasAnimationReachedLabel(m_animator2D, deathAnimation, "die"))
				{
					yield return null;
				}
				m_animator2D.set_paused(true);
			}
			else
			{
				Log.Warning(m_animator2D.GetDefinition().get_name() + " is missing the 'die' label in the animation named '" + deathAnimation + "'.", 151, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\BossObject.cs");
			}
		}

		private void OnAnimatorInitialized(object sender, Animator2DInitialisedEventArgs e)
		{
			m_animator2D.set_paused(false);
			GoToIdle();
		}

		private void OnAnimationLooped(object sender, AnimationLoopedEventArgs e)
		{
			if (m_hasTimeline)
			{
				m_playableDirector.set_time(0.0);
				m_playableDirector.Resume();
			}
		}

		public void PlayAnimation(string animationName, Action onComplete = null, bool loop = false, bool restart = true, bool async = false)
		{
			TimelineAsset value;
			bool flag = ((Dictionary<string, TimelineAsset>)m_timelineAssetDictionary).TryGetValue(animationName, out value);
			if (flag && null != value)
			{
				m_playableDirector.set_extrapolationMode(0);
				if (value != m_playableDirector.get_playableAsset())
				{
					m_playableDirector.Play(value);
				}
				else
				{
					if (restart || !m_animator2D.get_animationName().Equals(animationName))
					{
						m_playableDirector.set_time(0.0);
					}
					m_playableDirector.Resume();
				}
				m_hasTimeline = true;
			}
			else
			{
				if (flag)
				{
					Log.Warning("Boss named '" + this.get_gameObject().get_name() + "' has a timeline setup for key '" + animationName + "' but the actual asset is null.", 202, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\BossObject.cs");
				}
				m_playableDirector.set_time(0.0);
				m_playableDirector.Pause();
				m_hasTimeline = false;
			}
			m_animationCallback.Setup(animationName, restart, onComplete);
			m_animator2D.SetAnimation(animationName, loop, async, restart);
		}

		public static bool HasAnimationReachedLabel([NotNull] Animator2D animator2D, string animationName, [NotNull] string label)
		{
			if (!animator2D.get_reachedEndOfAnimation() && !label.Equals(animator2D.get_currentLabel(), StringComparison.OrdinalIgnoreCase))
			{
				return !animationName.Equals(animator2D.get_animationName());
			}
			return true;
		}

		private void OnMapRotationChanged(DirectionAngle previousMapRotation, DirectionAngle newMapRotation)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = this.get_transform();
			transform.set_rotation(transform.get_rotation() * (previousMapRotation.GetRotation() * newMapRotation.GetInverseRotation()));
		}

		protected IEnumerator PlayTimeline(TimelineAsset timelineAsset)
		{
			while (!m_loaded)
			{
				yield return null;
			}
			m_playableDirector.set_playableAsset(timelineAsset);
			m_playableDirector.set_time(0.0);
			m_playableDirector.set_extrapolationMode(2);
			m_playableDirector.Play();
			while (!m_playableDirector.HasReachedEndOfAnimation())
			{
				yield return null;
			}
		}

		protected BossObject()
			: this()
		{
		}
	}
}
