using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public sealed class SpawnCellObject : MonoBehaviour
	{
		[SerializeField]
		private Transform m_rotationTransform;

		public void SetDirection(Direction direction)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Quaternion rotation = m_rotationTransform.get_rotation();
			Vector3 eulerAngles = rotation.get_eulerAngles();
			eulerAngles.y = 0f;
			m_rotationTransform.set_rotation(direction.GetRotation() * Quaternion.Euler(eulerAngles));
		}

		public SpawnCellObject()
			: this()
		{
		}
	}
}
