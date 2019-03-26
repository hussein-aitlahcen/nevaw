using System.Collections;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	public interface ITimelineAssetProvider
	{
		IEnumerator LoadTimelineResources();

		void UnloadTimelineResources();

		bool HasTimelineAsset(string key);

		bool TryGetTimelineAsset(string key, out TimelineAsset timelineAsset);
	}
}
