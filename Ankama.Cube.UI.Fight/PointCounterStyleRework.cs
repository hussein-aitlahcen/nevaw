using DG.Tweening;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	[CreateAssetMenu(menuName = "Waven/UI/Fight/PointCounterStyle")]
	public sealed class PointCounterStyleRework : ScriptableObject
	{
		[Header("Tween Parameters")]
		[SerializeField]
		private Ease m_tweenEasing = 1;

		[SerializeField]
		private float m_tweenDurationPerUnit = 0.05f;

		[SerializeField]
		private float m_maxTweenDuration = 1f;

		public Ease tweenEasing => m_tweenEasing;

		public float GetTweenDuration(int previousValue, int newValue)
		{
			return Mathf.Min(m_maxTweenDuration, (float)Math.Abs(newValue - previousValue) * m_tweenDurationPerUnit);
		}

		public PointCounterStyleRework()
			: this()
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)

	}
}
