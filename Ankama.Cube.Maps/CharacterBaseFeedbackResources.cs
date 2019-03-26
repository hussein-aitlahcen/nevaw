using UnityEngine;

namespace Ankama.Cube.Maps
{
	public sealed class CharacterBaseFeedbackResources : ScriptableObject
	{
		[SerializeField]
		private Sprite m_baseSprite;

		[SerializeField]
		private Sprite m_attackedSprite;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_notPlayableAlpha = 1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_actionUsedAlpha = 0.75f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_actionAvailableAlpha = 1f;

		[SerializeField]
		private FightMapFeedbackColors m_feedbackColors;

		public Sprite baseSprite => m_baseSprite;

		public Sprite attackedSprite => m_attackedSprite;

		public float notPlayableAlpha => m_notPlayableAlpha;

		public float actionUsedAlpha => m_actionUsedAlpha;

		public float actionAvailableAlpha => m_actionAvailableAlpha;

		public FightMapFeedbackColors feedbackColors => m_feedbackColors;

		public CharacterBaseFeedbackResources()
			: this()
		{
		}
	}
}
