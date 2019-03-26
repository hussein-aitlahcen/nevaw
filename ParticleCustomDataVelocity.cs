using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleCustomDataVelocity : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem m_particleSystem;

	[SerializeField]
	private ParticleSystemCustomData customData;

	private List<Vector4> m_customData = new List<Vector4>();

	private Particle[] m_particles;

	private int m_maxParticlesCount;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		MainModule main = m_particleSystem.get_main();
		m_maxParticlesCount = main.get_maxParticles();
		m_particles = (Particle[])new Particle[m_maxParticlesCount];
	}

	private void Reset()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		m_particleSystem = this.GetComponent<ParticleSystem>();
		MainModule main = m_particleSystem.get_main();
		m_maxParticlesCount = main.get_maxParticles();
		m_particles = (Particle[])new Particle[m_maxParticlesCount];
	}

	private void Update()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		m_particleSystem.GetCustomParticleData(m_customData, customData);
		if (m_particles != null)
		{
			int particles = m_particleSystem.GetParticles(m_particles);
			for (int i = 0; i < particles; i++)
			{
				m_customData[i] = Vector4.op_Implicit(m_particles[i].get_totalVelocity());
			}
			m_particleSystem.SetCustomParticleData(m_customData, customData);
		}
	}

	public ParticleCustomDataVelocity()
		: this()
	{
	}
}
