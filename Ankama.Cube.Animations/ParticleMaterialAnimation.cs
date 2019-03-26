using System;
using UnityEngine;

namespace Ankama.Cube.Animations
{
	[ExecuteInEditMode]
	public class ParticleMaterialAnimation : MonoBehaviour
	{
		[Serializable]
		private struct MaterialParam
		{
			public string parameterName;

			public AnimationCurve animationCurve;
		}

		[SerializeField]
		private ParticleSystem m_particleSystem;

		[SerializeField]
		private Renderer[] m_renderers;

		[SerializeField]
		private MaterialParam[] m_materialParams;

		private MaterialPropertyBlock m_materialPropertyBlock;

		private int[] m_ids;

		private void Awake()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			m_materialPropertyBlock = new MaterialPropertyBlock();
			m_renderers[0].GetPropertyBlock(m_materialPropertyBlock);
			m_ids = new int[m_materialParams.Length];
			for (int i = 0; i < m_ids.Length; i++)
			{
				m_ids[i] = Shader.PropertyToID(m_materialParams[i].parameterName);
			}
		}

		private void Update()
		{
			for (int i = 0; i < m_ids.Length; i++)
			{
				m_materialPropertyBlock.SetFloat(m_ids[i], m_materialParams[i].animationCurve.Evaluate(m_particleSystem.get_time()));
			}
			for (int j = 0; j < m_renderers.Length; j++)
			{
				m_renderers[j].SetPropertyBlock(m_materialPropertyBlock);
			}
		}

		public ParticleMaterialAnimation()
			: this()
		{
		}
	}
}
