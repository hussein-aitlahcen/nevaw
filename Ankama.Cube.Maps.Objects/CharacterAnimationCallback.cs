using Ankama.Animations;
using Ankama.Animations.Events;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.Maps.Objects
{
	public class CharacterAnimationCallback
	{
		private readonly IAnimator2D m_animator2D;

		private string m_animationName;

		private bool m_animationStarted;

		private Action m_onComplete;

		private Action m_onCancel;

		private Action m_onStart;

		public unsafe CharacterAnimationCallback([NotNull] IAnimator2D animator2D)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			m_animator2D = animator2D;
			m_animator2D.add_AnimationStarted(new AnimationStartedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_animator2D.add_AnimationEnded(new AnimationEndedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void Setup([NotNull] string animationName, bool restart, [CanBeNull] Action onComplete = null, [CanBeNull] Action onCancel = null, [CanBeNull] Action onStart = null)
		{
			m_onCancel?.Invoke();
			if (restart || !animationName.Equals(m_animationName))
			{
				m_animationName = animationName;
				m_animationStarted = false;
			}
			m_onComplete = onComplete;
			m_onCancel = onCancel;
			m_onStart = onStart;
		}

		public void ChangeAnimationName(string value)
		{
			m_animationName = value;
		}

		public unsafe void Release()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			m_animator2D.remove_AnimationStarted(new AnimationStartedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_animator2D.remove_AnimationEnded(new AnimationEndedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnAnimationStarted(object sender, AnimationStartedEventArgs e)
		{
			if (e.animation.Equals(m_animationName))
			{
				m_animationStarted = true;
				m_onStart?.Invoke();
				return;
			}
			m_onCancel?.Invoke();
			m_animationStarted = false;
			m_onComplete = null;
			m_onCancel = null;
			m_onStart = null;
		}

		private void OnAnimationEnded(object sender, AnimationEndedEventArgs e)
		{
			if (m_animationStarted && e.animation.Equals(m_animationName))
			{
				m_onComplete?.Invoke();
				m_onComplete = null;
				m_onCancel = null;
				m_onStart = null;
			}
		}
	}
}
