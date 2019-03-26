using Ankama.Cube.Audio;
using Ankama.Cube.Data;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Animations
{
	public sealed class CellObjectAnimationPlayableBehaviour : PlayableBehaviour
	{
		private readonly AbstractFightMap m_fightMap;

		private readonly CellObjectAnimationParameters m_parameters;

		private readonly Vector2Int m_origin;

		private readonly Quaternion m_rotation;

		private readonly float m_strength;

		private EventInstance m_eventInstance;

		public CellObjectAnimationPlayableBehaviour([NotNull] AbstractFightMap fightMap, [NotNull] CellObjectAnimationParameters parameters, Vector2Int origin, Quaternion rotation, float strength)
			: this()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			m_fightMap = fightMap;
			m_parameters = parameters;
			m_origin = origin;
			m_rotation = rotation;
			m_strength = strength;
		}

		[UsedImplicitly]
		public CellObjectAnimationPlayableBehaviour()
			: this()
		{
		}

		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_fightMap)
			{
				return;
			}
			AudioReferenceWithParameters sound = m_parameters.sound;
			if (sound.get_isValid())
			{
				AbstractFightMap fightMap = m_fightMap;
				Vector2Int origin = m_origin;
				int x = origin.get_x();
				origin = m_origin;
				if (AudioManager.TryCreateInstance(transform: (!fightMap.TryGetCellObject(x, origin.get_y(), out CellObject cellObject)) ? null : cellObject.get_transform(), audioReference: sound, eventInstance: out m_eventInstance))
				{
					m_eventInstance.setParameterValue("Strength", m_strength);
					m_eventInstance.start();
				}
				else
				{
					Log.Warning("Failed to create event instance for cell object animation parameters named '" + m_parameters.get_name() + "'.", 109, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\CellObjectAnimationPlayableBehaviour.cs");
				}
			}
		}

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (m_eventInstance.isValid())
			{
				m_eventInstance.stop(0);
				m_eventInstance.release();
				m_eventInstance.clearHandle();
			}
		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == m_fightMap))
			{
				float time = (float)PlayableExtensions.GetTime<Playable>(playable);
				m_fightMap.ApplyCellObjectAnimation(m_parameters, m_origin, m_rotation, m_strength, time);
			}
		}
	}
}
