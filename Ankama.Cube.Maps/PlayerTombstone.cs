using Ankama.Cube.Data;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public sealed class PlayerTombstone : MonoBehaviour
	{
		[SerializeField]
		private float m_maxTiltAngle = 10f;

		private void Awake()
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			CameraHandler current = CameraHandler.current;
			DirectionAngle directionAngle = (null != current) ? current.mapRotation : DirectionAngle.None;
			float num = 180f - 45f * (float)directionAngle;
			float num2 = 0f - m_maxTiltAngle + 2f * m_maxTiltAngle * Random.get_value();
			this.get_transform().set_rotation(Quaternion.AngleAxis(num, Vector3.get_up()) * Quaternion.AngleAxis(num2, this.get_transform().get_right()) * this.get_transform().get_rotation());
		}

		public PlayerTombstone()
			: this()
		{
		}
	}
}
