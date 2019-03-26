using System;
using UnityEngine;

namespace Ankama.Cube.Render
{
	[ExecuteInEditMode]
	public class SpriteBillboard : MonoBehaviour
	{
		[SerializeField]
		private Transform[] m_transforms;

		[SerializeField]
		private bool m_lookTowardDirection;

		private Vector3[] m_previousWorldSpacePositions;

		private unsafe void OnEnable()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			Camera.onPreCull = Delegate.Combine((Delegate)Camera.onPreCull, (Delegate)new CameraCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			int num = m_transforms.Length;
			m_previousWorldSpacePositions = (Vector3[])new Vector3[num];
			for (int i = 0; i < num; i++)
			{
				m_previousWorldSpacePositions[i] = m_transforms[i].get_position();
			}
		}

		private unsafe void OnDisable()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			Camera.onPreCull = Delegate.Remove((Delegate)Camera.onPreCull, (Delegate)new CameraCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnPrecullCamera(Camera currentCamera)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			if (!currentCamera.get_isActiveAndEnabled())
			{
				return;
			}
			Transform[] transforms = m_transforms;
			int num = transforms.Length;
			Transform transform = currentCamera.get_transform();
			Vector3 forward = transform.get_forward();
			if (m_lookTowardDirection)
			{
				Vector3[] previousWorldSpacePositions = m_previousWorldSpacePositions;
				Vector3 right = transform.get_right();
				for (int i = 0; i < num; i++)
				{
					Transform obj = transforms[i];
					Vector3 position = obj.get_position();
					Vector3 val = position - previousWorldSpacePositions[i];
					previousWorldSpacePositions[i] = position;
					float num2 = Vector3.Dot(right, val);
					obj.set_forward((0f - Mathf.Sign(num2)) * forward);
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					transforms[j].set_forward(forward);
				}
			}
		}

		public SpriteBillboard()
			: this()
		{
		}
	}
}
