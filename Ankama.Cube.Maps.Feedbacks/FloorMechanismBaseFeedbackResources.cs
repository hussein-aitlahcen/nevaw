using UnityEngine;

namespace Ankama.Cube.Maps.Feedbacks
{
	public sealed class FloorMechanismBaseFeedbackResources : ScriptableObject
	{
		public const int FourBorders = 0;

		public const int ThreeBorders = 1;

		public const int TwoBordersInCorner = 2;

		public const int TwoBordersOpposite = 3;

		public const int OneBorder = 4;

		public const int NoBorder = 5;

		[SerializeField]
		private Sprite[] m_sprites;

		[SerializeField]
		private FightMapFeedbackColors m_feedbackColors;

		public Sprite[] sprites => m_sprites;

		public FightMapFeedbackColors feedbackColors => m_feedbackColors;

		public FloorMechanismBaseFeedbackResources()
			: this()
		{
		}
	}
}
