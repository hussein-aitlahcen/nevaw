using Ankama.AssetManagement;
using Ankama.Cube.Animations;
using Ankama.Cube.Audio;
using Ankama.Cube.Extensions;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using FMODUnity;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Data/Character Effects/Visual Effect")]
	public sealed class VisualCharacterEffect : CharacterEffect, ICharacterEffectWithAudioReference
	{
		private enum OrientationMethod
		{
			None,
			Context
		}

		[SerializeField]
		private VisualEffect m_visualEffect;

		[SerializeField]
		private Vector3 m_positionOffset = Vector3.get_zero();

		[SerializeField]
		private OrientationMethod m_orientationMethod;

		[SerializeField]
		private AudioReferenceWithParameters m_sound;

		[NonSerialized]
		private bool m_loadedAudioBank;

		protected override IEnumerator LoadInternal()
		{
			if (m_sound.get_isValid())
			{
				if (AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(m_sound), out string bankName))
				{
					AudioBankLoadRequest bankLoadRequest = AudioManager.LoadBankAsync(bankName);
					while (!bankLoadRequest.isDone)
					{
						yield return null;
					}
					if (AssetManagerError.op_Implicit(bankLoadRequest.error) == 0)
					{
						m_loadedAudioBank = true;
					}
					else
					{
						Log.Warning("Could not load bank named '" + bankName + "' for sound of visual character effect named '" + this.get_name() + "'.", 63, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\VisualCharacterEffect.cs");
					}
				}
				else
				{
					Log.Warning("Could not get default bank name for sound of visual effect named '" + this.get_name() + "'.", 68, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\VisualCharacterEffect.cs");
				}
			}
			m_initializationState = InitializationState.Loaded;
		}

		protected override void UnloadInternal()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (m_sound.get_isValid() && m_loadedAudioBank && AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(m_sound), out string bankName))
			{
				AudioManager.UnloadBank(bankName);
				m_loadedAudioBank = false;
			}
			m_initializationState = InitializationState.None;
		}

		public override Component Instantiate(Transform parent, ITimelineContextProvider contextProvider)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_visualEffect)
			{
				Log.Warning("Tried to instantiate visual character effect named '" + this.get_name() + "' without a visual effect setup.", 95, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\VisualCharacterEffect.cs");
				return null;
			}
			Vector3 position = parent.get_position() + m_positionOffset;
			Quaternion rotation = Quaternion.get_identity();
			Vector3 scale = Vector3.get_one();
			switch (m_orientationMethod)
			{
			case OrientationMethod.None:
			{
				CameraHandler current = CameraHandler.current;
				if (null != current)
				{
					rotation = current.mapRotation.GetInverseRotation();
				}
				break;
			}
			case OrientationMethod.Context:
				if (contextProvider != null)
				{
					(contextProvider.GetTimelineContext() as VisualEffectContext)?.GetVisualEffectTransformation(out rotation, out scale);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (m_sound.get_isValid())
			{
				AudioManager.PlayOneShot(m_sound, parent);
			}
			VisualEffect visualEffect = VisualEffectFactory.Instantiate(m_visualEffect, position, rotation, scale, parent);
			visualEffect.destructionOverride = OnInstanceDestructionRequest;
			return visualEffect;
		}

		public override IEnumerator DestroyWhenFinished(Component instance)
		{
			yield break;
		}

		private void OnInstanceDestructionRequest(VisualEffect instance)
		{
			VisualEffectFactory.Release(m_visualEffect, instance);
		}
	}
}
