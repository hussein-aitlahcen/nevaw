using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	[TrackClipType(typeof(CameraShakePlayableAsset))]
	[TrackColor(128f / 255f, 128f / 255f, 128f / 255f)]
	public sealed class CameraShakePlayableTrackAsset : TrackAsset
	{
		public CameraShakePlayableTrackAsset()
			: this()
		{
		}
	}
}
