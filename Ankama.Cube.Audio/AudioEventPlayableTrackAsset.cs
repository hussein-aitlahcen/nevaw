using System;
using UnityEngine.Timeline;

namespace Ankama.Cube.Audio
{
	[Serializable]
	[TrackClipType(typeof(AudioEventPlayableAsset))]
	[TrackColor(0f, 131f / 255f, 212f / 255f)]
	public sealed class AudioEventPlayableTrackAsset : TrackAsset
	{
		public AudioEventPlayableTrackAsset()
			: this()
		{
		}
	}
}
