using Ankama.Cube.Animations;
using Ankama.Cube.Fight;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Data/Spell Effects/Timeline Effect")]
	public class TimelineAssetSpellEffect : SpellEffect, ISpellEffectWithTimeline
	{
		[SerializeField]
		private TimelineAsset m_timelineAsset;

		protected override IEnumerator LoadInternal()
		{
			yield return TimelineUtility.LoadTimelineResources(m_timelineAsset);
			m_initializationState = InitializationState.Loaded;
		}

		protected override void UnloadInternal()
		{
			TimelineUtility.UnloadTimelineResources(m_timelineAsset);
			m_initializationState = InitializationState.None;
		}

		public override Component Instantiate(Transform parent, Quaternion rotation, Vector3 scale, FightContext fightContext, ITimelineContextProvider contextProvider)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_timelineAsset)
			{
				Log.Warning("Tried to instantiate timeline asset spell effect named '" + this.get_name() + "' without a timeline asset setup.", 42, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\TimelineAssetSpellEffect.cs");
				return null;
			}
			return FightObjectFactory.CreateTimelineAssetEffectInstance(m_timelineAsset, parent, rotation, scale, fightContext, contextProvider);
		}

		public override IEnumerator DestroyWhenFinished(Component instance)
		{
			PlayableDirector playableDirector = instance;
			PlayableGraph playableGraph;
			do
			{
				yield return null;
				if (null == playableDirector)
				{
					yield break;
				}
				playableGraph = playableDirector.get_playableGraph();
			}
			while (playableGraph.IsValid() && !playableGraph.IsDone());
			FightObjectFactory.DestroyTimelineAssetEffectInstance(playableDirector, clearFightContext: true);
		}
	}
}
