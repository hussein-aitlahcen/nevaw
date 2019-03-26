using UnityEngine;

namespace Ankama.Cube.Animations
{
	[ExecuteInEditMode]
	public class ParticleTransformShake : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem m_particleSystem;

		[SerializeField]
		private Transform[] m_transforms;

		[SerializeField]
		private AnimationCurve m_curve;

		[SerializeField]
		private Vector3 m_amplitude;

		private Vector3[] m_startingPositions;

		private void Awake()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			m_startingPositions = (Vector3[])new Vector3[m_transforms.Length];
			for (int i = 0; i < m_transforms.Length; i++)
			{
				m_startingPositions[i] = m_transforms[i].get_localPosition();
			}
		}

		private void Update()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Random.get_insideUnitSphere() * m_curve.Evaluate(m_particleSystem.get_time());
			val.x *= m_amplitude.x;
			val.y *= m_amplitude.y;
			val.z *= m_amplitude.z;
			for (int i = 0; i < m_transforms.Length; i++)
			{
				m_transforms[i].set_localPosition(m_startingPositions[i] + val);
			}
		}

		public ParticleTransformShake()
			: this()
		{
		}
	}
}
