using System;
using UnityEngine.Timeline;

namespace Ankama.Cube.UI
{
	[Serializable]
	public struct UIAnimationDescription
	{
		public string Name;

		public TimelineAsset animation;
	}
}
