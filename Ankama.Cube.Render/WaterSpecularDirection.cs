using System;
using UnityEngine;

namespace Ankama.Cube.Render
{
	[Obsolete]
	[ExecuteInEditMode]
	public class WaterSpecularDirection : MonoBehaviour
	{
		internal static readonly int _SpecularDir = Shader.PropertyToID("_SpecularDir");

		[SerializeField]
		private Transform m_specularDirection;

		private void Update()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Shader.SetGlobalVector(_SpecularDir, Vector4.op_Implicit(-m_specularDirection.get_forward()));
		}

		public WaterSpecularDirection()
			: this()
		{
		}
	}
}
