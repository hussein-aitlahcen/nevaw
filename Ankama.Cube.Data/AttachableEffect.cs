using Ankama.AssetManagement;
using Ankama.Cube.Animations;
using Ankama.Cube.Audio;
using Ankama.Cube.Extensions;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using FMODUnity;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Data/Attachable Effects/Attachable Effect")]
	public class AttachableEffect : ScriptableEffect
	{
		[Serializable]
		private struct EffectData
		{
			[SerializeField]
			public VisualEffect visualEffect;

			[SerializeField]
			public Vector3 positionOffset;

			[SerializeField]
			public AudioReferenceWithParameters sound;

			[SerializeField]
			public float delay;
		}

		[SerializeField]
		private EffectData m_mainEffect;

		[SerializeField]
		private EffectData m_stopEffect;

		[NonSerialized]
		private bool m_loadedMainEffectAudioBank;

		[NonSerialized]
		private bool m_loadedStopEffectAudioBank;

		public AudioReferenceWithParameters mainEffectAudioReference => m_mainEffect.sound;

		public float mainEffectDelay => m_mainEffect.delay;

		public AudioReferenceWithParameters stopEffectAudioReference => m_stopEffect.sound;

		public float stopEffectDelay => m_stopEffect.delay;

		protected override IEnumerator LoadInternal()
		{
			AudioReferenceWithParameters sound = m_mainEffect.sound;
			string bankName;
			if (sound.get_isValid())
			{
				if (AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(sound), out bankName))
				{
					AudioBankLoadRequest bankLoadRequest2 = AudioManager.LoadBankAsync(bankName);
					while (!bankLoadRequest2.isDone)
					{
						yield return null;
					}
					if (AssetManagerError.op_Implicit(bankLoadRequest2.error) == 0)
					{
						m_loadedMainEffectAudioBank = true;
					}
					else
					{
						Log.Warning("Could not load bank named '" + bankName + "' for sound of visual character effect named '" + this.get_name() + "'.", 88, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\AttachableEffect.cs");
					}
				}
				else
				{
					Log.Warning("Could not get default bank name for sound of visual effect named '" + this.get_name() + "'.", 93, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\AttachableEffect.cs");
				}
				bankName = null;
			}
			AudioReferenceWithParameters sound2 = m_stopEffect.sound;
			if (sound2.get_isValid())
			{
				if (AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(sound2), out bankName))
				{
					AudioBankLoadRequest bankLoadRequest2 = AudioManager.LoadBankAsync(bankName);
					while (!bankLoadRequest2.isDone)
					{
						yield return null;
					}
					if (AssetManagerError.op_Implicit(bankLoadRequest2.error) == 0)
					{
						m_loadedStopEffectAudioBank = true;
					}
					else
					{
						Log.Warning("Could not load bank named '" + bankName + "' for sound of visual character effect named '" + this.get_name() + "'.", 115, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\AttachableEffect.cs");
					}
				}
				else
				{
					Log.Warning("Could not get default bank name for sound of visual effect named '" + this.get_name() + "'.", 120, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\AttachableEffect.cs");
				}
			}
			m_initializationState = InitializationState.Loaded;
		}

		protected override void UnloadInternal()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			AudioReferenceWithParameters sound = m_mainEffect.sound;
			if (sound.get_isValid() && m_loadedMainEffectAudioBank && AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(sound), out string bankName))
			{
				AudioManager.UnloadBank(bankName);
				m_loadedMainEffectAudioBank = false;
			}
			AudioReferenceWithParameters sound2 = m_stopEffect.sound;
			if (sound2.get_isValid() && m_loadedStopEffectAudioBank && AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(sound2), out string bankName2))
			{
				AudioManager.UnloadBank(bankName2);
				m_loadedStopEffectAudioBank = false;
			}
			m_initializationState = InitializationState.None;
		}

		[CanBeNull]
		public VisualEffect InstantiateMainEffect([NotNull] Transform parent, [CanBeNull] ITimelineContextProvider contextProvider)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			EffectData mainEffect = m_mainEffect;
			VisualEffect visualEffect = mainEffect.visualEffect;
			if (null == visualEffect)
			{
				Log.Warning("Tried to instantiate attachable effect named '" + this.get_name() + "' without a visual effect setup.", 163, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\AttachableEffect.cs");
				return null;
			}
			Vector3 position = parent.get_position() + mainEffect.positionOffset;
			Quaternion rotation = Quaternion.get_identity();
			CameraHandler current = CameraHandler.current;
			if (null != current)
			{
				rotation = current.mapRotation.GetInverseRotation();
			}
			AudioReferenceWithParameters sound = mainEffect.sound;
			if (sound.get_isValid())
			{
				AudioManager.PlayOneShot(sound, parent);
			}
			VisualEffect visualEffect2 = VisualEffectFactory.Instantiate(visualEffect, position, rotation, Vector3.get_one(), parent);
			visualEffect2.destructionOverride = OnMainEffectInstanceDestructionRequest;
			return visualEffect2;
		}

		[CanBeNull]
		public VisualEffect InstantiateStopEffect([NotNull] Transform parent, [CanBeNull] ITimelineContextProvider contextProvider)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			EffectData stopEffect = m_stopEffect;
			VisualEffect visualEffect = stopEffect.visualEffect;
			if (null == visualEffect)
			{
				return null;
			}
			Vector3 position = parent.get_position() + stopEffect.positionOffset;
			Quaternion rotation = Quaternion.get_identity();
			CameraHandler current = CameraHandler.current;
			if (null != current)
			{
				rotation = current.mapRotation.GetInverseRotation();
			}
			AudioReferenceWithParameters sound = stopEffect.sound;
			if (sound.get_isValid())
			{
				AudioManager.PlayOneShot(sound, parent);
			}
			VisualEffect visualEffect2 = VisualEffectFactory.Instantiate(visualEffect, position, rotation, Vector3.get_one(), parent);
			visualEffect2.destructionOverride = OnStopEffectInstanceDestructionRequest;
			return visualEffect2;
		}

		private void OnMainEffectInstanceDestructionRequest(VisualEffect instance)
		{
			VisualEffectFactory.Release(m_mainEffect.visualEffect, instance);
		}

		private void OnStopEffectInstanceDestructionRequest(VisualEffect instance)
		{
			VisualEffectFactory.Release(m_stopEffect.visualEffect, instance);
		}
	}
}
