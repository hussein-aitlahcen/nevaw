using Ankama.AssetManagement;
using Ankama.Cube.Audio;
using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	public sealed class CellObjectAnimationPlayableAsset : PlayableAsset, ITimelineClipAsset, ITimelineResourcesProvider
	{
		public enum OrientationMethod
		{
			None,
			Context,
			Director
		}

		[SerializeField]
		private CellObjectAnimationParameters m_parameters;

		[SerializeField]
		private float m_strength = 1f;

		[SerializeField]
		private OrientationMethod m_orientationMethod;

		[SerializeField]
		private Vector2Int m_offset = Vector2Int.get_zero();

		private bool m_loadedResources;

		public ClipCaps clipCaps
		{
			get;
		}

		public IEnumerator LoadResources()
		{
			if (null == m_parameters)
			{
				yield break;
			}
			AudioReferenceWithParameters sound = m_parameters.sound;
			if (!sound.get_isValid())
			{
				yield break;
			}
			while (!AudioManager.isReady)
			{
				if (AssetManagerError.op_Implicit(AudioManager.error) != 0)
				{
					yield break;
				}
				yield return null;
			}
			if (AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(sound), out string bankName))
			{
				AudioBankLoadRequest bankLoadRequest = AudioManager.LoadBankAsync(bankName);
				while (!bankLoadRequest.isDone)
				{
					yield return null;
				}
				if (AssetManagerError.op_Implicit(bankLoadRequest.error) != 0)
				{
					Log.Error($"Failed to load bank named '{bankName}': {bankLoadRequest.error}", 80, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\CellObjectAnimationPlayableAsset.cs");
				}
				else
				{
					m_loadedResources = true;
				}
			}
			else
			{
				Log.Warning("Could not find a bank to load sound for cell object animation parameters named '" + m_parameters.get_name() + "'.", 88, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\CellObjectAnimationPlayableAsset.cs");
			}
		}

		public void UnloadResources()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (m_loadedResources && AudioManager.isReady && AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(m_parameters.sound), out string bankName))
			{
				AudioManager.UnloadBank(bankName);
			}
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_parameters || !m_parameters.isValid || Mathf.Approximately(m_strength, 0f))
			{
				return Playable.get_Null();
			}
			Transform parent = owner.get_transform().get_parent();
			if (null == parent)
			{
				return Playable.get_Null();
			}
			CellObject componentInParent = parent.GetComponentInParent<CellObject>();
			if (null == componentInParent)
			{
				return Playable.get_Null();
			}
			AbstractFightMap abstractFightMap = componentInParent.parentMap as AbstractFightMap;
			if (null == abstractFightMap)
			{
				return Playable.get_Null();
			}
			Vector2Int val = componentInParent.coords;
			Quaternion rotation;
			switch (m_orientationMethod)
			{
			case OrientationMethod.None:
				rotation = Quaternion.get_identity();
				break;
			case OrientationMethod.Context:
			{
				VisualEffectContext context = TimelineContextUtility.GetContext<VisualEffectContext>(graph);
				if (context == null)
				{
					rotation = Quaternion.get_identity();
					break;
				}
				context.GetVisualEffectTransformation(out rotation, out Vector3 _);
				val += m_offset.Rotate(rotation);
				break;
			}
			case OrientationMethod.Director:
				rotation = owner.get_transform().get_rotation();
				val += m_offset.Rotate(rotation);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			CellObjectAnimationPlayableBehaviour cellObjectAnimationPlayableBehaviour = new CellObjectAnimationPlayableBehaviour(abstractFightMap, m_parameters, val, rotation, m_strength);
			return ScriptPlayable<CellObjectAnimationPlayableBehaviour>.op_Implicit(ScriptPlayable<CellObjectAnimationPlayableBehaviour>.Create(graph, cellObjectAnimationPlayableBehaviour, 0));
		}

		public CellObjectAnimationPlayableAsset()
			: this()
		{
		}//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)

	}
}
