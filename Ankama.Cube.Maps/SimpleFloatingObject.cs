using Ankama.Cube.Data;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class SimpleFloatingObject : MonoBehaviour
	{
		[SerializeField]
		private SimpleFloatingObjectData m_data;

		private unsafe void Update()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			Transform parent = this.get_transform().get_parent();
			Vector3 val = parent.InverseTransformDirection(Vector3.get_up());
			Vector3 val2 = parent.InverseTransformDirection(Vector3.get_right());
			Vector3 val3 = parent.InverseTransformDirection(Vector3.get_forward());
			SimpleFloatingObjectData data = m_data;
			Vector3 position = this.get_transform().get_position();
			float time = Time.get_time();
			float num = (Mathf.PerlinNoise(((IntPtr)(void*)position).x + time * data.verticalSpeed, ((IntPtr)(void*)position).y) - 0.5f) * 2f * data.verticalNoise;
			float num2 = (Mathf.PerlinNoise(((IntPtr)(void*)position).x + time * data.rotationSpeed, ((IntPtr)(void*)position).y) - 0.5f) * 2f * data.rotationNoise;
			float num3 = (Mathf.PerlinNoise(((IntPtr)(void*)position).x, ((IntPtr)(void*)position).y + time * data.rotationSpeed) - 0.5f) * 2f * data.rotationNoise;
			this.get_transform().set_localPosition(val * num);
			this.get_transform().set_localRotation(Quaternion.Euler(val2 * num2 + val3 * num3));
		}

		public SimpleFloatingObject()
			: this()
		{
		}
	}
}
