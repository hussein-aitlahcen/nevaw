using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public class CastingModeParameters : ScriptableObject
	{
		[SerializeField]
		private Color m_disabledColor = new Color(0.6f, 0.6f, 0.6f, 1f);

		[SerializeField]
		private float m_disableFadeDuration = 0.3f;

		[SerializeField]
		private float m_castInvalidPopupDuration = 2f;

		[Header("Enter In Hand")]
		[SerializeField]
		private Vector3 m_enterInHandOffset = new Vector3(0f, 10f, 0f);

		[SerializeField]
		private float m_enterInHandDuration = 0.3f;

		[SerializeField]
		public Ease enterInHandEase = 18;

		[Header("Pointer over")]
		[SerializeField]
		private Vector3 m_selectOffset = new Vector3(0f, 10f, 0f);

		[SerializeField]
		private float m_selectDuration = 0.1f;

		[SerializeField]
		public Ease selectEase = 18;

		[Header("Start drag")]
		[SerializeField]
		private float m_dragThreshold = 50f;

		[SerializeField]
		private Vector2 m_startDragPivot = new Vector2(0.5f, 0.5f);

		[SerializeField]
		private float m_startDragRotation;

		[SerializeField]
		private float m_startDragScale = 0.8f;

		[SerializeField]
		private float m_startDragDuration = 1f;

		[SerializeField]
		public Ease startDragEase = 18;

		[Header("Dragging")]
		[SerializeField]
		private Vector2 m_movePivot = new Vector2(0.5f, 0f);

		[SerializeField]
		private float m_moveRotation;

		[SerializeField]
		private float m_moveScale = 1f;

		[SerializeField]
		private float m_moveDuration = 0.07f;

		[SerializeField]
		public Ease moveEase = 18;

		[Header("Targeting")]
		[SerializeField]
		private Vector3 m_cellTargetOffset = new Vector3(0f, 1.5f, 0f);

		[SerializeField]
		private Vector2 m_snapPivot = new Vector2(0.5f, 0.5f);

		[SerializeField]
		private float m_snapRotation;

		[SerializeField]
		private float m_snapScale = 0.7f;

		[SerializeField]
		private float m_uiSnapScale = 0.7f;

		[SerializeField]
		public Ease snapEase = 18;

		[Space(10f)]
		[SerializeField]
		private float m_snapMoveDuration = 0.05f;

		[SerializeField]
		public Ease snapMoveEase = 18;

		[Space(10f)]
		[SerializeField]
		private float m_snapRotationDuration = 0.05f;

		[SerializeField]
		public Ease snapRotationEase = 18;

		[Space(15f)]
		[SerializeField]
		private float m_snapReleaseCellThreshold = 50f;

		[SerializeField]
		private float m_snapReleaseWaitingDuration = 0.5f;

		[SerializeField]
		private float m_snapReleaseDuration = 0.05f;

		[SerializeField]
		public Ease snapReleaseEase = 17;

		[Header("Release")]
		[SerializeField]
		private float m_returnDuration = 0.1f;

		[SerializeField]
		public Ease returnEase = 18;

		[Header("Throw")]
		[SerializeField]
		public float noiseSpeed = 0.1f;

		[SerializeField]
		public float noiseAmount = 0.1f;

		[Header("Remove")]
		[SerializeField]
		private float m_fadeDuration = 0.1f;

		[SerializeField]
		public Ease fadeEase = 18;

		[Header("Misc.")]
		[SerializeField]
		private float m_infoFadeDuration = 0.1f;

		[SerializeField]
		private float m_opponentPlayingDuration = 0.15f;

		[SerializeField]
		private float m_opponentCastPlayingDuration = 0.15f;

		public Color disabledColor => m_disabledColor;

		public float disableFadeDuration => m_disableFadeDuration;

		public float castInvalidPopupDuration => m_castInvalidPopupDuration;

		public Vector3 enterInHandOffset => m_enterInHandOffset;

		public float enterInHandDuration => m_enterInHandDuration;

		public Vector3 selectOffset => m_selectOffset;

		public float selectDuration => m_selectDuration;

		public float dragThreshold => m_dragThreshold;

		public Vector2 startDragPivot => m_startDragPivot;

		public float startDragRotation => m_startDragRotation;

		public float startDragScale => m_startDragScale;

		public float startDragDuration => m_startDragDuration;

		public Vector2 movePivot => m_movePivot;

		public float moveRotation => m_moveRotation;

		public float moveScale => m_moveScale;

		public float moveDuration => m_moveDuration;

		public Vector3 cellTargetOffset => m_cellTargetOffset;

		public Vector2 snapPivot => m_snapPivot;

		public float snapRotation => m_snapRotation;

		public float snapScale => m_snapScale;

		public float uiSnapScale => m_uiSnapScale;

		public float snapMoveDuration => m_snapMoveDuration;

		public float snapRotationDuration => m_snapRotationDuration;

		public float snapReleaseDuration => m_snapReleaseDuration;

		public float snapReleaseCellThreshold => m_snapReleaseCellThreshold;

		public float snapReleaseWaitingDuration => m_snapReleaseWaitingDuration;

		public float returnDuration => m_returnDuration;

		public float fadeDuration => m_fadeDuration;

		public float infoFadeDuration => m_infoFadeDuration;

		public float opponentPlayingDuration => m_opponentPlayingDuration;

		public float opponentCastPlayingDuration => m_opponentCastPlayingDuration;

		public CastingModeParameters()
			: this()
		{
		}//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)

	}
}
