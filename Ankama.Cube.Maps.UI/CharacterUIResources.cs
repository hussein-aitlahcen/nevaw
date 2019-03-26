using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	public sealed class CharacterUIResources : ScriptableObject
	{
		[Header("Action Type Icons")]
		[SerializeField]
		private Sprite m_actionAttackIcon;

		[SerializeField]
		private Sprite m_actionRangedAttackIcon;

		[SerializeField]
		private Sprite m_actionHealIcon;

		[SerializeField]
		private Sprite m_actionRangedHealIcon;

		[Header("Elementary State Icons")]
		[SerializeField]
		private Sprite m_elementaryStateMuddyIcon;

		[SerializeField]
		private Sprite m_elementaryStateOiledIcon;

		[SerializeField]
		private Sprite m_elementaryStateVentilatedIcon;

		[SerializeField]
		private Sprite m_elementaryStateWetIcon;

		[Header("Attack Target Feedback Icons")]
		[SerializeField]
		private Sprite m_attackTargetFeedbackIcon;

		[SerializeField]
		private Sprite m_attackTargetSelectedFeedbackIcon;

		[SerializeField]
		private Sprite m_healTargetFeedbackIcon;

		[SerializeField]
		private Sprite m_healTargetSelectedFeedbackIcon;

		[Header("Map Cell Indicators")]
		[SerializeField]
		private Sprite m_mapCellIndicatorDeathIcon;

		public Sprite actionAttackIcon => m_actionAttackIcon;

		public Sprite actionRangedAttackIcon => m_actionRangedAttackIcon;

		public Sprite actionHealIcon => m_actionHealIcon;

		public Sprite actionRangedHealIcon => m_actionRangedHealIcon;

		public Sprite elementaryStateMuddyIcon => m_elementaryStateMuddyIcon;

		public Sprite elementaryStateOiledIcon => m_elementaryStateOiledIcon;

		public Sprite elementaryStateVentilatedIcon => m_elementaryStateVentilatedIcon;

		public Sprite elementaryStateWetIcon => m_elementaryStateWetIcon;

		public Sprite attackTargetFeedbackIcon => m_attackTargetFeedbackIcon;

		public Sprite attackTargetSelectedFeedbackIcon => m_attackTargetSelectedFeedbackIcon;

		public Sprite healTargetFeedbackIcon => m_healTargetFeedbackIcon;

		public Sprite healTargetSelectedFeedbackIcon => m_healTargetSelectedFeedbackIcon;

		public Sprite mapCellIndicatorDeathIcon => m_mapCellIndicatorDeathIcon;

		public CharacterUIResources()
			: this()
		{
		}
	}
}
