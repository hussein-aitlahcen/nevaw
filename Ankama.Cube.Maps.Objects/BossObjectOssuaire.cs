using Ankama.Cube.Animations;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

namespace Ankama.Cube.Maps.Objects
{
	public class BossObjectOssuaire : BossObject, IBossEvolution, IBossSpell
	{
		protected const string IdleAnim0 = "phase_1";

		protected const string IdleAnim1 = "phase_2";

		protected const string IdleAnim2 = "phase_3";

		[Header("FX Anim")]
		[SerializeField]
		private TimelineAsset m_transition01;

		[SerializeField]
		private TimelineAsset m_transition12;

		[SerializeField]
		private TimelineAsset m_transition21;

		[SerializeField]
		private TimelineAsset m_transition10;

		[SerializeField]
		private TimelineAsset m_transition20;

		[SerializeField]
		private TimelineAsset m_launchSpell;

		private int m_level;

		protected override string spawnAnimation => "spawn";

		protected override string deathAnimation => "hit1";

		protected override IEnumerator LoadTimelines()
		{
			yield return base.LoadTimelines();
			yield return TimelineUtility.LoadTimelineResources(m_transition01);
			yield return TimelineUtility.LoadTimelineResources(m_transition12);
			yield return TimelineUtility.LoadTimelineResources(m_transition21);
			yield return TimelineUtility.LoadTimelineResources(m_transition10);
			yield return TimelineUtility.LoadTimelineResources(m_transition20);
			yield return TimelineUtility.LoadTimelineResources(m_launchSpell);
		}

		protected override void UnloadTimelines()
		{
			base.UnloadTimelines();
			TimelineUtility.UnloadTimelineResources(m_transition01);
			TimelineUtility.UnloadTimelineResources(m_transition12);
			TimelineUtility.UnloadTimelineResources(m_transition21);
			TimelineUtility.UnloadTimelineResources(m_transition10);
			TimelineUtility.UnloadTimelineResources(m_transition20);
			TimelineUtility.UnloadTimelineResources(m_launchSpell);
		}

		public override void GoToIdle()
		{
			PlayAnimation(GetIdleAnim(m_level), null, loop: true, restart: false);
		}

		public IEnumerator PlayLevelChangeAnim(int valueBefore, int valueAfter)
		{
			if (m_level != valueBefore)
			{
				Log.Warning($"Level desynchro !! client : {m_level} / server : {valueBefore} ", 64, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\BossObjectOssuaire.cs");
			}
			m_level = valueAfter;
			TimelineAsset levelTimeline = GetLevelTimeline(valueBefore, valueAfter);
			yield return PlayTimeline(levelTimeline);
			GoToIdle();
		}

		public void OnLevelChangeAnimEvent()
		{
			m_animator2D.SetAnimation(GetIdleAnim(m_level), true, true, false);
		}

		public IEnumerator PlaySpellAnim(int spellId)
		{
			yield return PlayTimeline(m_launchSpell);
			GoToIdle();
		}

		private string GetIdleAnim(int level)
		{
			switch (level)
			{
			case 0:
				return "phase_1";
			case 1:
				return "phase_2";
			case 2:
				return "phase_3";
			default:
				Log.Warning($"Cannot find idle anim for level {level}", 101, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\BossObjectOssuaire.cs");
				return "phase_1";
			}
		}

		private TimelineAsset GetLevelTimeline(int previous, int next)
		{
			if (next > previous)
			{
				switch (next)
				{
				case 1:
					return m_transition01;
				case 2:
					return m_transition12;
				}
			}
			else
			{
				switch (next)
				{
				case 0:
					if (previous != 1)
					{
						return m_transition20;
					}
					return m_transition10;
				case 1:
					return m_transition21;
				}
			}
			Log.Warning($"Cannot find anim for leveling {previous} -> {next}", 125, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\BossObjectOssuaire.cs");
			return m_transition01;
		}
	}
}
