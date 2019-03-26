using UnityEngine;

namespace Ankama.Cube.Animations
{
	public class GameObjectNoiseMovement : MonoBehaviour
	{
		[SerializeField]
		private bool m_enable_X;

		[SerializeField]
		private bool m_enable_Y;

		[SerializeField]
		private bool m_enable_Z;

		[SerializeField]
		private Vector3 m_noiseSpeed;

		[SerializeField]
		private Vector3 m_noiseStrength;

		private Transform m_trsf;

		private void Awake()
		{
			m_trsf = this.get_transform();
		}

		private void Update()
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			Vector3 localPosition = default(Vector3);
			localPosition._002Ector(0f, 0f, 0f);
			if (m_enable_X)
			{
				localPosition.x = Mathf.PerlinNoise(0f, Time.get_time() * m_noiseSpeed.x) * m_noiseStrength.x;
			}
			if (m_enable_Y)
			{
				localPosition.y = Mathf.PerlinNoise(0.3f, Time.get_time() * m_noiseSpeed.y) * m_noiseStrength.y;
			}
			if (m_enable_Z)
			{
				localPosition.z = Mathf.PerlinNoise(0.7f, Time.get_time() * m_noiseSpeed.z) * m_noiseStrength.z;
			}
			m_trsf.set_localPosition(localPosition);
		}

		public GameObjectNoiseMovement()
			: this()
		{
		}
	}
}
