using Ankama.Animations;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.Maps.Objects
{
	public static class CharacterObjectUtility
	{
		public static bool HasAnimationEnded([NotNull] IAnimator2D animator2D, string animationName)
		{
			if (!animator2D.get_reachedEndOfAnimation())
			{
				return !animationName.Equals(animator2D.get_animationName());
			}
			return true;
		}

		public static bool HasAnimationReachedLabel([NotNull] IAnimator2D animator2D, string animationName, [NotNull] string label)
		{
			if (!animator2D.get_reachedEndOfAnimation() && !label.Equals(animator2D.get_currentLabel(), StringComparison.OrdinalIgnoreCase))
			{
				return !animationName.Equals(animator2D.get_animationName());
			}
			return true;
		}

		public static bool HasAnimationEnded([NotNull] Animator2D animator2D, CharacterAnimationInfo animationInfo)
		{
			if (!animator2D.get_reachedEndOfAnimation())
			{
				return !animationInfo.animationName.Equals(animator2D.get_animationName());
			}
			return true;
		}

		public static bool HasAnimationReachedLabel([NotNull] Animator2D animator2D, CharacterAnimationInfo animationInfo, [NotNull] string label)
		{
			if (!animator2D.get_reachedEndOfAnimation() && !label.Equals(animator2D.get_currentLabel(), StringComparison.OrdinalIgnoreCase))
			{
				return !animationInfo.animationName.Equals(animator2D.get_animationName());
			}
			return true;
		}
	}
}
