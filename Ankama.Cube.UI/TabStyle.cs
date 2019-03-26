using System;
using UnityEngine;

namespace Ankama.Cube.UI
{
	[CreateAssetMenu(menuName = "Waven/UI/Buttons/TabStyle", order = 2000)]
	public class TabStyle : ScriptableObject
	{
		[Serializable]
		public struct TabState
		{
			[SerializeField]
			public Color backgroundColor;

			[SerializeField]
			public Color borderColor;
		}

		[SerializeField]
		public float transitionDuration = 0.1f;

		[SerializeField]
		public TabState normal;

		[SerializeField]
		public TabState highlight;

		[SerializeField]
		public TabState pressed;

		[SerializeField]
		public TabState disable;

		[SerializeField]
		public TabState selected;

		public TabStyle()
			: this()
		{
		}
	}
}
