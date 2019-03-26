using Ankama.Cube.Fight;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	public sealed class CameraShakePlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		[SerializeField]
		private AnimationCurve m_curve = new AnimationCurve();

		public ClipCaps clipCaps
		{
			get;
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			FightContext fightContext = TimelineContextUtility.GetFightContext(graph);
			if (null != fightContext)
			{
				FightStatus local = FightStatus.local;
				if (local != null && fightContext.fightId != local.fightId)
				{
					return Playable.get_Null();
				}
			}
			CameraShakePlayableBehaviour cameraShakePlayableBehaviour = new CameraShakePlayableBehaviour(m_curve);
			return ScriptPlayable<CameraShakePlayableBehaviour>.op_Implicit(ScriptPlayable<CameraShakePlayableBehaviour>.Create(graph, cameraShakePlayableBehaviour, 0));
		}

		public CameraShakePlayableAsset()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown

	}
}
