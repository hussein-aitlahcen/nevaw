using UnityEngine.Playables;

namespace Ankama.Cube.Extensions
{
	public static class PlayableDirectorExtensions
	{
		public static bool HasReachedEndOfAnimation(this PlayableDirector director)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			PlayableGraph playableGraph = director.get_playableGraph();
			if (playableGraph.IsValid())
			{
				return playableGraph.IsDone();
			}
			return true;
		}
	}
}
