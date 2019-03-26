using Ankama.Cube.Maps.Objects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	[TrackClipType(typeof(CharacterObjectColorModifierPlayableAsset))]
	[TrackColor(236f / 255f, 0.05882353f, 7f / 85f)]
	public sealed class CharacterObjectColorModifierPlayableTrackAsset : TrackAsset
	{
		[SerializeField]
		private bool m_writeDefaults = true;

		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			CharacterObjectContext context = TimelineContextUtility.GetContext<CharacterObjectContext>(graph);
			if (context == null)
			{
				DummyPlayableBehaviour dummyPlayableBehaviour = new DummyPlayableBehaviour();
				return ScriptPlayable<DummyPlayableBehaviour>.op_Implicit(ScriptPlayable<DummyPlayableBehaviour>.Create(graph, dummyPlayableBehaviour, 0));
			}
			CharacterObjectColorModifierPlayableAsset defaultsSource = m_writeDefaults ? GetLastClip() : null;
			CharacterObjectColorModifierPlayableMixer characterObjectColorModifierPlayableMixer = new CharacterObjectColorModifierPlayableMixer(context, go, this.GetClips(), defaultsSource);
			return ScriptPlayable<CharacterObjectColorModifierPlayableMixer>.op_Implicit(ScriptPlayable<CharacterObjectColorModifierPlayableMixer>.Create(graph, characterObjectColorModifierPlayableMixer, inputCount));
		}

		private CharacterObjectColorModifierPlayableAsset GetLastClip()
		{
			List<TimelineClip> clips = base.m_Clips;
			int count = clips.Count;
			if (count <= 0)
			{
				return null;
			}
			return (CharacterObjectColorModifierPlayableAsset)clips[count - 1].get_asset();
		}

		public CharacterObjectColorModifierPlayableTrackAsset()
			: this()
		{
		}
	}
}
