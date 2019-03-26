using Ankama.Cube.Animations;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public abstract class CharacterEffect : ScriptableEffect
	{
		[CanBeNull]
		public abstract Component Instantiate([NotNull] Transform parent, [CanBeNull] ITimelineContextProvider contextProvider);

		public abstract IEnumerator DestroyWhenFinished([NotNull] Component instance);
	}
}
