using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	public sealed class LightIntensityPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		[SerializeField]
		private AnimationCurve m_curve = new AnimationCurve();

		public ClipCaps clipCaps
		{
			get;
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			LightIntensityPlayableBehaviour lightIntensityPlayableBehaviour = new LightIntensityPlayableBehaviour(m_curve);
			return ScriptPlayable<LightIntensityPlayableBehaviour>.op_Implicit(ScriptPlayable<LightIntensityPlayableBehaviour>.Create(graph, lightIntensityPlayableBehaviour, 0));
		}

		public LightIntensityPlayableAsset()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown

	}
}
