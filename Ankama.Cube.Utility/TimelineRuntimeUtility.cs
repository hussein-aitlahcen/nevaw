using System.Collections.Generic;
using UnityEngine.Playables;

namespace Ankama.Cube.Utility
{
	public static class TimelineRuntimeUtility
	{
		public static IEnumerable<T> EnumerateBehaviours<T>(PlayableGraph playableGraph) where T : class, IPlayableBehaviour, new()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (playableGraph.IsValid())
			{
				int rootPlayableCount = playableGraph.GetRootPlayableCount();
				int num;
				for (int rootPlayableIndex = 0; rootPlayableIndex < rootPlayableCount; rootPlayableIndex = num)
				{
					Playable rootPlayable = playableGraph.GetRootPlayable(rootPlayableIndex);
					foreach (T item in EnumerateBehaviours<T>(rootPlayable))
					{
						yield return item;
					}
					num = rootPlayableIndex + 1;
				}
			}
		}

		public static IEnumerable<T> EnumerateBehaviours<T>(Playable playable) where T : class, IPlayableBehaviour, new()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (PlayableExtensions.IsValid<Playable>(playable))
			{
				int inputCount = PlayableExtensions.GetInputCount<Playable>(playable);
				int num;
				for (int inputIndex = 0; inputIndex < inputCount; inputIndex = num)
				{
					Playable input = PlayableExtensions.GetInput<Playable>(playable, inputIndex);
					foreach (T item in EnumerateBehaviours<T>(input))
					{
						yield return item;
					}
					num = inputIndex + 1;
				}
				if (playable.GetPlayableType() == typeof(T))
				{
					yield return ((ScriptPlayable<T>)playable).GetBehaviour();
				}
			}
		}
	}
}
