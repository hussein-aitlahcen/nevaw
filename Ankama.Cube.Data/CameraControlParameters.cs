using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public sealed class CameraControlParameters : ScriptableObject
	{
		[Header("Pan")]
		[SerializeField]
		private float m_panTweenDuration = 0.25f;

		[SerializeField]
		private Ease m_panTweenEase = 9;

		[Header("Zoom")]
		[SerializeField]
		private float m_zoomTweenMaxDuration = 0.5f;

		[SerializeField]
		private Ease m_zoomTweenEase = 9;

		public float panTweenDuration => m_panTweenDuration;

		public Ease panTweenEase => m_panTweenEase;

		public float zoomTweenMaxDuration => m_zoomTweenMaxDuration;

		public Ease zoomTweenEase => m_zoomTweenEase;

		public CameraControlParameters()
			: this()
		{
		}//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)

	}
}
