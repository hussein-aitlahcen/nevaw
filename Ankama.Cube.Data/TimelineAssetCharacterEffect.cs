using Ankama.Cube.Animations;
using Ankama.Cube.Fight;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Data/Character Effects/Timeline Asset")]
	public sealed class TimelineAssetCharacterEffect : CharacterEffect, ICharacterEffectWithTimeline
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

		public override Component Instantiate(Transform parent, ITimelineContextProvider contextProvider)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_timelineAsset)
			{
				Log.Warning("Tried to instantiate timeline asset character effect named '" + this.get_name() + "' without a timeline asset setup.", 44, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\TimelineAssetCharacterEffect.cs");
				return null;
			}
			Quaternion rotation = Quaternion.get_identity();
			Vector3 scale = Vector3.get_one();
			VisualEffectContext visualEffectContext;
			if (contextProvider != null && (visualEffectContext = (contextProvider.GetTimelineContext() as VisualEffectContext)) != null)
			{
				visualEffectContext.GetVisualEffectTransformation(out rotation, out scale);
			}
			return FightObjectFactory.CreateTimelineAssetEffectInstance(m_timelineAsset, parent, rotation, scale, null, contextProvider);
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
			FightObjectFactory.DestroyTimelineAssetEffectInstance(playableDirector, clearFightContext: false);
		}
	}
}
