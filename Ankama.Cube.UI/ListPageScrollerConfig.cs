using UnityEngine;

namespace Ankama.Cube.UI
{
	[CreateAssetMenu(menuName = "Waven/UI/List/ListPageScrollerConfig")]
	public class ListPageScrollerConfig : ScriptableObject
	{
		public float selectedPageAlpha;

		public float unselectedPageAlpha;

		public float durationInSecs;

		public ListPageScrollerConfig()
			: this()
		{
		}
	}
}
