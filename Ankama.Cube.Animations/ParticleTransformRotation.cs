using System;
using UnityEngine;

namespace Ankama.Cube.Animations
{
	[ExecuteInEditMode]
	public class ParticleTransformRotation : MonoBehaviour
	{
		private enum RotationSpace
		{
			Self,
			World,
			Camera
		}

		[SerializeField]
		private ParticleSystem m_particleSystem;

		[SerializeField]
		private Transform[] m_transforms;

		[SerializeField]
		private RotationSpace m_rotationSpace;

		[SerializeField]
		private AnimationCurve m_curve;

		[SerializeField]
		private Vector3 m_axis;

		[SerializeField]
		private Vector3 m_offsetRotation;

		private unsafe void OnEnable()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			if (m_rotationSpace == RotationSpace.Camera)
			{
				Camera.onPreCull = Delegate.Combine((Delegate)Camera.onPreCull, (Delegate)new CameraCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		private unsafe void OnDisable()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			if (m_rotationSpace == RotationSpace.Camera)
			{
				Camera.onPreCull = Delegate.Remove((Delegate)Camera.onPreCull, (Delegate)new CameraCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		private void Update()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (m_rotationSpace == RotationSpace.Camera)
			{
				return;
			}
			Quaternion val = Quaternion.Euler(m_axis * m_curve.Evaluate(m_particleSystem.get_time()) + m_offsetRotation);
			if (m_rotationSpace == RotationSpace.Self)
			{
				for (int i = 0; i < m_transforms.Length; i++)
				{
					m_transforms[i].set_localRotation(val);
				}
			}
			else
			{
				for (int j = 0; j < m_transforms.Length; j++)
				{
					m_transforms[j].set_rotation(val);
				}
			}
		}

		private void OnPrecullCamera(Camera camera)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if (camera.get_isActiveAndEnabled())
			{
				Quaternion rotation = Quaternion.LookRotation(-camera.get_transform().get_forward(), camera.get_transform().get_up()) * Quaternion.Euler(m_axis * m_curve.Evaluate(m_particleSystem.get_time()));
				for (int i = 0; i < m_transforms.Length; i++)
				{
					m_transforms[i].set_rotation(rotation);
				}
			}
		}

		public ParticleTransformRotation()
			: this()
		{
		}
	}
}
