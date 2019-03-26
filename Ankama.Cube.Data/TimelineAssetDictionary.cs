using Ankama.Cube.Animations;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class TimelineAssetDictionary : SerializableDictionary<string, TimelineAsset>
	{
		public TimelineAssetDictionary()
			: base((IEqualityComparer<string>)StringComparer.Ordinal)
		{
		}

		public void GatherLoadRoutines(List<IEnumerator> list)
		{
			foreach (TimelineAsset value in base.Values)
			{
				TimelineUtility.GatherTimelineResourcesLoadRoutines(value, list);
			}
		}

		public void Unload()
		{
			foreach (TimelineAsset value in base.Values)
			{
				TimelineUtility.UnloadTimelineResources(value);
			}
		}
	}
}
