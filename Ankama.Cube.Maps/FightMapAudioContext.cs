using Ankama.Cube.Audio;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using FMOD.Studio;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public sealed class FightMapAudioContext : AudioContext
	{
		private const string TurnIndex = "turnIndex";

		private const string LocalPlayerHeroLife = "Music_Health";

		private const string BossEvolutionStep = "Music_BossState";

		private int m_turnIndex;

		private float m_localPlayerHeroLife = 1f;

		private int m_bossEvolutionStep;

		public int turnIndex
		{
			get
			{
				return m_turnIndex;
			}
			set
			{
				if (m_turnIndex != value)
				{
					SetParameterValue("turnIndex", value);
					m_turnIndex = value;
				}
			}
		}

		public float localPlayerHeroLife
		{
			get
			{
				return m_localPlayerHeroLife;
			}
			set
			{
				SetParameterValue("Music_Health", value);
				m_localPlayerHeroLife = value;
			}
		}

		public int bossEvolutionStep
		{
			get
			{
				return m_bossEvolutionStep;
			}
			set
			{
				SetParameterValue("Music_BossState", value);
				m_bossEvolutionStep = value;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			FightStatus local = FightStatus.local;
			if (local != null)
			{
				m_turnIndex = local.turnIndex;
				int localPlayerId = local.localPlayerId;
				if (local.TryGetEntity((HeroStatus e) => e.ownerId == localPlayerId, out HeroStatus entityStatus))
				{
					m_localPlayerHeroLife = Mathf.Clamp01((float)entityStatus.life / (float)entityStatus.baseLife);
				}
			}
		}

		protected override void InitializeEventInstance(EventInstance eventInstance)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			eventInstance.setParameterValue("turnIndex", (float)m_turnIndex);
		}
	}
}
