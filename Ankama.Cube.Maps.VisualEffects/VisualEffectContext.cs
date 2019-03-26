using Ankama.Cube.Audio;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.VisualEffects
{
	public abstract class VisualEffectContext : AudioContext
	{
		protected readonly List<VisualEffect> m_visualEffectInstances = new List<VisualEffect>();

		[NotNull]
		public abstract Transform transform
		{
			get;
		}

		public abstract void GetVisualEffectTransformation(out Quaternion rotation, out Vector3 scale);

		public void AddVisualEffect([NotNull] VisualEffect visualEffect)
		{
			m_visualEffectInstances.Add(visualEffect);
		}

		public void RemoveVisualEffect([NotNull] VisualEffect visualEffect)
		{
			m_visualEffectInstances.Remove(visualEffect);
		}
	}
}
