using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Render
{
	[ExecuteInEditMode]
	public class ParticleEmitOnTriggerEvent : MonoBehaviour
	{
		[Serializable]
		private struct ParticleSystemEmit
		{
			public ParticleSystem pSystem;

			public int emitCount;

			[HideInInspector]
			public Transform trsf;
		}

		[SerializeField]
		private ParticleSystem m_particleSystemListener;

		[SerializeField]
		private ParticleSystemEmit[] m_particleSystemEmits;

		private int pSystemsCount;

		private List<Particle> particles = new List<Particle>();

		private void Awake()
		{
			pSystemsCount = m_particleSystemEmits.Length;
			for (int i = 0; i < pSystemsCount; i++)
			{
				m_particleSystemEmits[i].trsf = m_particleSystemEmits[i].pSystem.get_transform();
			}
		}

		private void OnParticleTrigger()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			int triggerParticles = ParticlePhysicsExtensions.GetTriggerParticles(m_particleSystemListener, 2, particles);
			for (int i = 0; i < triggerParticles; i++)
			{
				Particle val = particles[i];
				Vector3 position = val.get_position();
				for (int j = 0; j < pSystemsCount; j++)
				{
					ParticleSystemEmit particleSystemEmit = m_particleSystemEmits[j];
					particleSystemEmit.trsf.set_position(position);
					particleSystemEmit.pSystem.Emit(particleSystemEmit.emitCount);
				}
			}
		}

		public ParticleEmitOnTriggerEvent()
			: this()
		{
		}
	}
}
