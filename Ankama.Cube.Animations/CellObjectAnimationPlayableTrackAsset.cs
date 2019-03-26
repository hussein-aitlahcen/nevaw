using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	[TrackClipType(typeof(CellObjectAnimationPlayableAsset))]
	[TrackColor(128f / 255f, 128f / 255f, 128f / 255f)]
	public sealed class CellObjectAnimationPlayableTrackAsset : TrackAsset
	{
		public CellObjectAnimationPlayableTrackAsset()
			: this()
		{
		}
	}
}
