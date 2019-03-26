using Ankama.Cube.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.UI
{
	[RequireComponent(typeof(PlayableDirector))]
	public class UIAnimationDirector : MonoBehaviour
	{
		[SerializeField]
		private List<UIAnimationDescription> m_animations;

		private PlayableDirector m_playableDirector;

		public IEnumerator Load()
		{
			foreach (UIAnimationDescription animation in m_animations)
			{
				yield return TimelineUtility.LoadTimelineResources(animation.animation);
			}
		}

		public void Unload()
		{
			foreach (UIAnimationDescription animation in m_animations)
			{
				TimelineUtility.UnloadTimelineResources(animation.animation);
			}
		}

		public PlayableDirector GetDirector()
		{
			if (m_playableDirector == null)
			{
				m_playableDirector = this.GetComponent<PlayableDirector>();
			}
			return m_playableDirector;
		}

		public TimelineAsset GetAnimation(string name)
		{
			foreach (UIAnimationDescription animation in m_animations)
			{
				if (string.Equals(name, animation.Name, StringComparison.OrdinalIgnoreCase))
				{
					return animation.animation;
				}
			}
			return null;
		}

		public List<UIAnimationDescription> GetAnimations()
		{
			return m_animations;
		}

		public void SetAnimation(int i, UIAnimationDescription animations)
		{
			m_animations[i] = animations;
		}

		public void AddAnimation(string newAnimation, TimelineAsset o)
		{
			UIAnimationDescription item = default(UIAnimationDescription);
			item.Name = newAnimation;
			item.animation = o;
			m_animations.Add(item);
		}

		public void DeleteAnimation(int index)
		{
			m_animations.RemoveAt(index);
		}

		public void EditAnimation(TimelineAsset timelineAsset)
		{
			if (m_playableDirector == null)
			{
				m_playableDirector = this.GetComponent<PlayableDirector>();
			}
			m_playableDirector.set_playableAsset(timelineAsset);
		}

		public UIAnimationDirector()
			: this()
		{
		}
	}
}
