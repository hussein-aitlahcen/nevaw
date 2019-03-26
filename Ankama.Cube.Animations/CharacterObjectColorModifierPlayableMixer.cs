using Ankama.Cube.Maps.Objects;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	public sealed class CharacterObjectColorModifierPlayableMixer : PlayableBehaviour
	{
		private readonly CharacterObjectContext m_characterObjectContext;

		private readonly GameObject m_owner;

		private readonly IEnumerable<TimelineClip> m_clips;

		private readonly CharacterObjectColorModifierPlayableAsset m_defaultsSource;

		private CharacterObject m_characterObject;

		private CharacterObjectColorModifierPlayableAsset m_currentClip;

		[UsedImplicitly]
		public CharacterObjectColorModifierPlayableMixer()
			: this()
		{
		}

		public CharacterObjectColorModifierPlayableMixer([NotNull] CharacterObjectContext characterObjectContext, GameObject owner, IEnumerable<TimelineClip> clips, CharacterObjectColorModifierPlayableAsset defaultsSource)
			: this()
		{
			m_characterObjectContext = characterObjectContext;
			m_owner = owner;
			m_clips = clips;
			m_defaultsSource = defaultsSource;
		}

		public override void OnGraphStart(Playable playable)
		{
			CharacterObject characterObject = m_characterObjectContext.characterObject;
			if (null == characterObject)
			{
				if (null == m_owner)
				{
					return;
				}
				characterObject = m_owner.GetComponent<CharacterObject>();
				if (null == characterObject)
				{
					return;
				}
			}
			m_characterObject = characterObject;
		}

		public override void OnGraphStop(Playable playable)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_characterObject)
			{
				if (m_characterObjectContext == null)
				{
					return;
				}
				m_characterObject = m_characterObjectContext.characterObject;
				if (null == m_characterObject)
				{
					return;
				}
			}
			if (null != m_defaultsSource)
			{
				Color endColor = m_defaultsSource.endColor;
				m_characterObject.SetColorModifier(endColor);
			}
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == m_characterObject))
			{
				PlayableGraph graph = PlayableExtensions.GetGraph<Playable>(playable);
				double time = PlayableExtensions.GetTime<Playable>(graph.GetRootPlayable(0));
				foreach (TimelineClip clip in m_clips)
				{
					double start = clip.get_start();
					if (time < start)
					{
						if (clip.IsPreExtrapolatedTime(time))
						{
							CharacterObjectColorModifierPlayableAsset characterObjectColorModifierPlayableAsset = (CharacterObjectColorModifierPlayableAsset)clip.get_asset();
							m_characterObject.SetColorModifier(characterObjectColorModifierPlayableAsset.startColor);
							m_currentClip = characterObjectColorModifierPlayableAsset;
							return;
						}
					}
					else
					{
						double end = clip.get_end();
						if (time <= end)
						{
							CharacterObjectColorModifierPlayableAsset characterObjectColorModifierPlayableAsset2 = (CharacterObjectColorModifierPlayableAsset)clip.get_asset();
							float num = Mathf.InverseLerp((float)start, (float)end, (float)time);
							Color colorModifier = Color.Lerp(characterObjectColorModifierPlayableAsset2.startColor, characterObjectColorModifierPlayableAsset2.endColor, num);
							m_characterObject.SetColorModifier(colorModifier);
							m_currentClip = characterObjectColorModifierPlayableAsset2;
							return;
						}
						if (clip.IsPostExtrapolatedTime(time))
						{
							CharacterObjectColorModifierPlayableAsset characterObjectColorModifierPlayableAsset3 = (CharacterObjectColorModifierPlayableAsset)clip.get_asset();
							m_characterObject.SetColorModifier(characterObjectColorModifierPlayableAsset3.endColor);
							m_currentClip = characterObjectColorModifierPlayableAsset3;
							return;
						}
					}
				}
				if (null != m_currentClip)
				{
					m_characterObject.SetColorModifier(m_currentClip.endColor);
					m_currentClip = null;
				}
			}
		}
	}
}
