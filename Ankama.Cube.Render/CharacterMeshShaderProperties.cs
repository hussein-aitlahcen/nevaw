using Ankama.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Maps;
using System;
using UnityEngine;

namespace Ankama.Cube.Render
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Animator2D))]
	public class CharacterMeshShaderProperties : MonoBehaviour
	{
		private const string PropertyName = "_CharacterParams";

		private static readonly int s_propertyIndex = Shader.PropertyToID("_CharacterParams");

		private static Camera s_currentCamera;

		private static Vector3 s_vector;

		private Animator2D m_animator2D;

		private void OnEnable()
		{
			m_animator2D = this.GetComponent<Animator2D>();
			if (null == m_animator2D)
			{
				this.set_enabled(false);
				return;
			}
			if (null == s_currentCamera)
			{
				s_currentCamera = Camera.get_main();
				if (null == s_currentCamera)
				{
					return;
				}
			}
			CameraHandler.AddMapRotationListener(OnMapRotationChanged);
		}

		private void OnDisable()
		{
			CameraHandler.RemoveMapRotationListener(OnMapRotationChanged);
		}

		private unsafe void OnMapRotationChanged(DirectionAngle previousMapRotation, DirectionAngle newMapRotation)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			float y = ((IntPtr)(void*)s_currentCamera.get_transform().get_eulerAngles()).y;
			s_vector = Quaternion.Euler(30f, y, 0f) * Vector3.get_forward() / 0.5f;
			Refresh();
		}

		private void OnWillRenderObject()
		{
			if (this.get_transform().get_hasChanged())
			{
				Refresh();
				this.get_transform().set_hasChanged(false);
			}
		}

		private unsafe void Refresh()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = s_vector;
			Vector4 val2 = default(Vector4);
			val2._002Ector(((IntPtr)(void*)val).x, ((IntPtr)(void*)val).y, ((IntPtr)(void*)val).z, ((IntPtr)(void*)this.get_transform().get_position()).y);
			MaterialPropertyBlock propertyBlock = m_animator2D.GetPropertyBlock();
			propertyBlock.SetVector(s_propertyIndex, val2);
			m_animator2D.SetPropertyBlock(propertyBlock);
		}

		public CharacterMeshShaderProperties()
			: this()
		{
		}
	}
}
