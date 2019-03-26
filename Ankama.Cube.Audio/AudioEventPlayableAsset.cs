using Ankama.AssetManagement;
using Ankama.Cube.Animations;
using Ankama.Utilities;
using FMODUnity;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Audio
{
	[Serializable]
	public sealed class AudioEventPlayableAsset : PlayableAsset, ITimelineClipAsset, ITimelineResourcesProvider
	{
		public enum StopMode
		{
			None,
			Immediate,
			AllowFadeout
		}

		[UsedImplicitly]
		[SerializeField]
		[AudioEventReference(/*Could not decode attribute arguments.*/)]
		private string m_eventGuid;

		[UsedImplicitly]
		[SerializeField]
		private StopMode m_stopMode;

		[UsedImplicitly]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_volume = 1f;

		[UsedImplicitly]
		[SerializeField]
		private AudioEventParameterDictionary m_parameters = new AudioEventParameterDictionary();

		private bool m_loadedResources;

		public ClipCaps clipCaps => 16;

		public IEnumerator LoadResources()
		{
			if (string.IsNullOrEmpty(m_eventGuid))
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
			if (AudioManager.TryGetDefaultBankName(m_eventGuid, out string bankName))
			{
				AudioBankLoadRequest bankLoadRequest = AudioManager.LoadBankAsync(bankName);
				while (!bankLoadRequest.isDone)
				{
					yield return null;
				}
				if (AssetManagerError.op_Implicit(bankLoadRequest.error) != 0)
				{
					Log.Error($"Failed to load bank named '{bankName}': {bankLoadRequest.error}", 84, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioEventPlayableAsset.cs");
				}
				else
				{
					m_loadedResources = true;
				}
			}
			else
			{
				Log.Warning("Could not find a bank to load for event '" + m_eventGuid + "'.", 92, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioEventPlayableAsset.cs");
			}
		}

		public void UnloadResources()
		{
			if (m_loadedResources && AudioManager.isReady && AudioManager.TryGetDefaultBankName(m_eventGuid, out string bankName))
			{
				AudioManager.UnloadBank(bankName);
			}
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(m_eventGuid))
			{
				return Playable.get_Null();
			}
			Guid guid = Guid.ParseExact(m_eventGuid, "N");
			if (guid == Guid.Empty)
			{
				return Playable.get_Null();
			}
			AudioContext context = TimelineContextUtility.GetContext<AudioContext>(graph);
			AudioEventPlayableBehaviour audioEventPlayableBehaviour = new AudioEventPlayableBehaviour(guid, m_stopMode, m_volume, m_parameters, context, owner.get_transform());
			return ScriptPlayable<AudioEventPlayableBehaviour>.op_Implicit(ScriptPlayable<AudioEventPlayableBehaviour>.Create(graph, audioEventPlayableBehaviour, 0));
		}

		public AudioEventPlayableAsset()
			: this()
		{
		}
	}
}
