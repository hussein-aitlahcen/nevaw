using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.VisualEffects
{
	[SelectionBase]
	[ExecuteInEditMode]
	public sealed class ParticleSystemsVisualEffect : VisualEffect
	{
		[Serializable]
		private struct StartupParticleSystemData : IEquatable<StartupParticleSystemData>
		{
			[SerializeField]
			public int particleSystemIndex;

			[SerializeField]
			private ParticleSystemRingBufferMode m_ringBufferMode;

			public static bool Handles(ParticleSystem particleSystem)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Invalid comparison between Unknown and I4
				MainModule main = particleSystem.get_main();
				return (int)main.get_ringBufferMode() > 0;
			}

			public StartupParticleSystemData(int index, ParticleSystem particleSystem)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				MainModule main = particleSystem.get_main();
				particleSystemIndex = index;
				m_ringBufferMode = main.get_ringBufferMode();
			}

			public void Apply([NotNull] ParticleSystem particleSystem)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				MainModule main = particleSystem.get_main();
				main.set_ringBufferMode(m_ringBufferMode);
			}

			public static bool operator ==(StartupParticleSystemData lhs, StartupParticleSystemData rhs)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				if (lhs.particleSystemIndex == rhs.particleSystemIndex)
				{
					return lhs.m_ringBufferMode == rhs.m_ringBufferMode;
				}
				return false;
			}

			public static bool operator !=(StartupParticleSystemData lhs, StartupParticleSystemData rhs)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				if (lhs.particleSystemIndex == rhs.particleSystemIndex)
				{
					return lhs.m_ringBufferMode != rhs.m_ringBufferMode;
				}
				return true;
			}

			public bool Equals(StartupParticleSystemData other)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				if (particleSystemIndex == other.particleSystemIndex)
				{
					return m_ringBufferMode == other.m_ringBufferMode;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				object obj2;
				if (obj != null && (obj2 = obj) is StartupParticleSystemData)
				{
					StartupParticleSystemData startupParticleSystemData = (StartupParticleSystemData)obj2;
					if (particleSystemIndex == startupParticleSystemData.particleSystemIndex)
					{
						return m_ringBufferMode == startupParticleSystemData.m_ringBufferMode;
					}
					return false;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return particleSystemIndex;
			}
		}

		[SerializeField]
		private List<ParticleSystem> m_particleSystems = new List<ParticleSystem>();

		[SerializeField]
		private List<StartupParticleSystemData> m_startupData = new List<StartupParticleSystemData>();

		public override bool IsAlive()
		{
			List<ParticleSystem> particleSystems = m_particleSystems;
			int count = particleSystems.Count;
			for (int i = 0; i < count; i++)
			{
				ParticleSystem val = particleSystems[i];
				if (null != val && val.IsAlive(false))
				{
					return true;
				}
			}
			return false;
		}

		protected override void PlayInternal()
		{
			List<ParticleSystem> particleSystems = m_particleSystems;
			List<StartupParticleSystemData> startupData = m_startupData;
			int count = startupData.Count;
			for (int i = 0; i < count; i++)
			{
				StartupParticleSystemData startupParticleSystemData = startupData[i];
				ParticleSystem val = particleSystems[startupParticleSystemData.particleSystemIndex];
				if (null != val)
				{
					startupParticleSystemData.Apply(val);
				}
			}
			int count2 = particleSystems.Count;
			for (int j = 0; j < count2; j++)
			{
				ParticleSystem val2 = particleSystems[j];
				if (null != val2)
				{
					val2.Play(false);
				}
			}
		}

		protected override void PauseInternal()
		{
			List<ParticleSystem> particleSystems = m_particleSystems;
			int count = particleSystems.Count;
			for (int i = 0; i < count; i++)
			{
				ParticleSystem val = particleSystems[i];
				if (null != val)
				{
					val.Pause(false);
				}
			}
		}

		protected override void StopInternal(VisualEffectStopMethod stopMethod)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			List<ParticleSystem> particleSystems = m_particleSystems;
			ParticleSystemStopBehavior val = stopMethod;
			int count = particleSystems.Count;
			for (int i = 0; i < count; i++)
			{
				ParticleSystem val2 = particleSystems[i];
				if (null != val2)
				{
					MainModule main = val2.get_main();
					if ((int)main.get_ringBufferMode() != 0)
					{
						main.set_ringBufferMode(0);
					}
					val2.Stop(false, val);
				}
			}
		}

		protected override void ClearInternal()
		{
			List<ParticleSystem> particleSystems = m_particleSystems;
			int count = particleSystems.Count;
			for (int i = 0; i < count; i++)
			{
				ParticleSystem val = particleSystems[i];
				if (null != val)
				{
					val.Clear();
				}
			}
		}
	}
}
