using UnityEngine;

namespace Ankama.Cube.Utility
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(ParticleSystemRenderer))]
	public sealed class ParticleSystemTrailTextureFixer : MonoBehaviour
	{
		private static readonly int s_mainTexId = Shader.PropertyToID("_MainTex");

		private void OnEnable()
		{
			Apply();
		}

		private void Apply()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			ParticleSystemRenderer component = this.GetComponent<ParticleSystemRenderer>();
			Material trailMaterial = component.get_trailMaterial();
			if (!(null == trailMaterial))
			{
				MaterialPropertyBlock val = new MaterialPropertyBlock();
				component.GetPropertyBlock(val, 1);
				val.SetTexture(s_mainTexId, trailMaterial.get_mainTexture());
				component.SetPropertyBlock(val, 1);
			}
		}

		public ParticleSystemTrailTextureFixer()
			: this()
		{
		}
	}
}
