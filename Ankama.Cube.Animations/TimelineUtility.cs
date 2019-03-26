using Ankama.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	public static class TimelineUtility
	{
		public static void GatherTimelineResourcesLoadRoutines(TimelineAsset timelineAsset, List<IEnumerator> list)
		{
			if (!(null == timelineAsset))
			{
				foreach (TrackAsset outputTrack in timelineAsset.GetOutputTracks())
				{
					foreach (TimelineClip clip in outputTrack.GetClips())
					{
						ITimelineResourcesProvider timelineResourcesProvider = clip.get_asset() as ITimelineResourcesProvider;
						if (timelineResourcesProvider != null)
						{
							IEnumerator item = timelineResourcesProvider.LoadResources();
							list.Add(item);
						}
					}
				}
			}
		}

		public static IEnumerator LoadTimelineResources(TimelineAsset timelineAsset)
		{
			if (!(null == timelineAsset))
			{
				int outputTrackCount = timelineAsset.get_outputTrackCount();
				if (outputTrackCount != 0)
				{
					List<IEnumerator> loadRoutine = ListPool<IEnumerator>.Get(outputTrackCount);
					foreach (TrackAsset outputTrack in timelineAsset.GetOutputTracks())
					{
						foreach (TimelineClip clip in outputTrack.GetClips())
						{
							ITimelineResourcesProvider timelineResourcesProvider = clip.get_asset() as ITimelineResourcesProvider;
							if (timelineResourcesProvider != null)
							{
								loadRoutine.Add(timelineResourcesProvider.LoadResources());
							}
						}
					}
					yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(loadRoutine.ToArray());
					ListPool<IEnumerator>.Release(loadRoutine);
				}
			}
		}

		public static void UnloadTimelineResources(TimelineAsset timelineAsset)
		{
			if (!(null == timelineAsset))
			{
				foreach (TrackAsset outputTrack in timelineAsset.GetOutputTracks())
				{
					foreach (TimelineClip clip in outputTrack.GetClips())
					{
						(clip.get_asset() as ITimelineResourcesProvider)?.UnloadResources();
					}
				}
			}
		}
	}
}
