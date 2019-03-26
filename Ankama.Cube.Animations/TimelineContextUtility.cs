using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Animations
{
	public static class TimelineContextUtility
	{
		public const string FightContextPropertyName = "fight";

		public const string ContextPropertyName = "context";

		public static FightContext GetFightContext(PlayableGraph graph)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			PropertyName val = default(PropertyName);
			val._002Ector("fight");
			bool flag = default(bool);
			return graph.GetResolver().GetReferenceValue(val, ref flag) as FightContext;
		}

		public static void SetFightContext([NotNull] PlayableDirector playableDirector, FightContext fightContext)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			PropertyName val = default(PropertyName);
			val._002Ector("fight");
			playableDirector.SetReferenceValue(val, fightContext);
		}

		public static void ClearFightContext([NotNull] PlayableDirector playableDirector)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			PropertyName val = default(PropertyName);
			val._002Ector("fight");
			playableDirector.ClearReferenceValue(val);
		}

		public static T GetContext<T>(PlayableGraph graph) where T : class, ITimelineContext
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			PropertyName val = default(PropertyName);
			val._002Ector("context");
			bool flag = default(bool);
			ITimelineContextProvider timelineContextProvider = graph.GetResolver().GetReferenceValue(val, ref flag) as ITimelineContextProvider;
			if (timelineContextProvider == null)
			{
				return null;
			}
			return timelineContextProvider.GetTimelineContext() as T;
		}

		public static void SetContextProvider([NotNull] PlayableDirector playableDirector, [NotNull] ITimelineContextProvider provider)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			PropertyName val = default(PropertyName);
			val._002Ector("context");
			Object timelineBinding = provider.GetTimelineBinding();
			playableDirector.SetReferenceValue(val, timelineBinding);
		}

		public static void ClearContextProvider([NotNull] PlayableDirector playableDirector)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			PropertyName val = default(PropertyName);
			val._002Ector("context");
			playableDirector.ClearReferenceValue(val);
		}
	}
}
