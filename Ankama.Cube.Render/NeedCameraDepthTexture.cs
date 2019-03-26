using System;
using UnityEngine;

namespace Ankama.Cube.Render
{
	[Obsolete]
	[ExecuteInEditMode]
	public class NeedCameraDepthTexture : MonoBehaviour
	{
		[SerializeField]
		private DepthTextureMode m_mode;

		protected void OnEnable()
		{
		}

		protected void OnDisable()
		{
		}

		public NeedCameraDepthTexture()
			: this()
		{
		}
	}
}
