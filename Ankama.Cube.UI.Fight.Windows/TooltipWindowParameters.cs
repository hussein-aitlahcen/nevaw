using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI.Fight.Windows
{
	[CreateAssetMenu(menuName = "Waven/UI/Tooltip parameters")]
	public class TooltipWindowParameters : ScriptableObject
	{
		[SerializeField]
		private float m_moveDuration = 0.1f;

		[SerializeField]
		private Ease m_moveEase = 18;

		[Header("Open")]
		[SerializeField]
		private float m_openDelay;

		[SerializeField]
		private float m_openDuration = 0.5f;

		[SerializeField]
		private Ease m_openEase = 32;

		[Header("Close")]
		[SerializeField]
		private float m_closeDuration = 0.5f;

		[SerializeField]
		private Ease m_closeEase = 32;

		public float moveDuration => m_moveDuration;

		public Ease moveEase => m_moveEase;

		public float openDelay => m_openDelay;

		public float openDuration => m_openDuration;

		public Ease openEase => m_openEase;

		public float closeDuration => m_closeDuration;

		public Ease closeEase => m_closeEase;

		public TooltipWindowParameters()
			: this()
		{
		}//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)

	}
}
