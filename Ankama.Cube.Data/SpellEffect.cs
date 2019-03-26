using Ankama.Cube.Animations;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public abstract class SpellEffect : ScriptableEffect
	{
		public enum WaitMethod
		{
			None,
			Delay,
			Destruction
		}

		public enum OrientationMethod
		{
			None,
			Context,
			SpellEffectTarget
		}

		[SerializeField]
		private WaitMethod m_waitMethod;

		[SerializeField]
		private float m_waitDelay;

		[SerializeField]
		private OrientationMethod m_orientationMethod;

		public WaitMethod waitMethod => m_waitMethod;

		public float waitDelay => m_waitDelay;

		public OrientationMethod orientationMethod => m_orientationMethod;

		[CanBeNull]
		public abstract Component Instantiate([NotNull] Transform parent, Quaternion rotation, Vector3 scale, [CanBeNull] FightContext fightContext, [CanBeNull] ITimelineContextProvider contextProvider);

		public abstract IEnumerator DestroyWhenFinished([NotNull] Component instance);
	}
}
