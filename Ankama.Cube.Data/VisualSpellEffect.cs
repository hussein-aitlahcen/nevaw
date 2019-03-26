using Ankama.AssetManagement;
using Ankama.Cube.Animations;
using Ankama.Cube.Audio;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using FMODUnity;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Data/Spell Effects/Visual Effect")]
	public class VisualSpellEffect : SpellEffect, ISpellEffectWithAudioReference
	{
		[SerializeField]
		private VisualEffect m_visualEffect;

		[SerializeField]
		private Vector3 m_positionOffset = new Vector3(0f, 0.5f, 0f);

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
						Log.Warning("Could not load bank named '" + bankName + "' for sound of visual character effect named '" + this.get_name() + "'.", 52, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\VisualSpellEffect.cs");
					}
				}
				else
				{
					Log.Warning("Could not get default bank name for sound of visual effect named '" + this.get_name() + "'.", 57, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\VisualSpellEffect.cs");
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

		public override Component Instantiate(Transform parent, Quaternion rotation, Vector3 scale, FightContext fightContext, ITimelineContextProvider contextProvider)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_visualEffect)
			{
				Log.Warning("Tried to instantiate visual character effect named '" + this.get_name() + "' without a visual effect setup.", 84, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\VisualSpellEffect.cs");
				return null;
			}
			Vector3 position = parent.get_position() + m_positionOffset;
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
