using System;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	[Serializable]
	[TrackClipType(typeof(VisualEffectPlayableAsset))]
	[TrackColor(0f, 212f / 255f, 131f / 255f)]
	public sealed class VisualEffectPlayableTrackAsset : TrackAsset
	{
		public VisualEffectPlayableTrackAsset()
			: this()
		{
		}
	}
}
