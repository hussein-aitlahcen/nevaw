using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	[TrackClipType(typeof(LightIntensityPlayableAsset))]
	[TrackColor(128f / 255f, 128f / 255f, 128f / 255f)]
	public sealed class LightIntensityTrackAsset : TrackAsset
	{
		public LightIntensityTrackAsset()
			: this()
		{
		}
	}
}
