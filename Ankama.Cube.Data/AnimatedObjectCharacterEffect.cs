using Ankama.Animations;
using Ankama.AssetManagement;
using Ankama.Cube.Animations;
using Ankama.Cube.Audio;
using Ankama.Cube.Fight;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using FMODUnity;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Data/Character Effects/Animated Object")]
	public sealed class AnimatedObjectCharacterEffect : CharacterEffect, ICharacterEffectWithAudioReference
	{
		[SerializeField]
		private AnimatedObjectDefinition m_animatedObjectDefinition;

		[SerializeField]
		private string m_animationName = string.Empty;

		[SerializeField]
		private bool m_appendDirectionSuffix;

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
						Log.Warning("Could not load bank named '" + bankName + "' for sound of animated character effect named '" + this.get_name() + "'.", 58, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\AnimatedObjectCharacterEffect.cs");
					}
				}
				else
				{
					Log.Warning("Could not get default bank name for sound of animated character effect named '" + this.get_name() + "'.", 63, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\AnimatedObjectCharacterEffect.cs");
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
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_animatedObjectDefinition)
			{
				Log.Warning("Tried to instantiate animated object character effect named '" + this.get_name() + "' without an animated object definition setup.", 90, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\AnimatedObjectCharacterEffect.cs");
				return null;
			}
			string animationName;
			if (!string.IsNullOrEmpty(m_animationName))
			{
				if (m_appendDirectionSuffix && contextProvider != null)
				{
					Direction direction = Direction.None;
					CharacterObjectContext characterObjectContext = contextProvider.GetTimelineContext() as CharacterObjectContext;
					if (characterObjectContext != null)
					{
						CharacterObject characterObject = characterObjectContext.characterObject;
						if (null != characterObject)
						{
							direction = characterObject.direction;
						}
					}
					animationName = ((direction != Direction.None) ? (m_animationName + (int)direction) : m_animationName);
				}
				else
				{
					animationName = m_animationName;
				}
			}
			else
			{
				animationName = string.Empty;
			}
			if (m_sound.get_isValid())
			{
				AudioManager.PlayOneShot(m_sound, parent);
			}
			return FightObjectFactory.CreateAnimatedObjectEffectInstance(m_animatedObjectDefinition, animationName, parent);
		}

		public override IEnumerator DestroyWhenFinished(Component instance)
		{
			Animator2D animator2D = instance;
			do
			{
				yield return null;
				if (null == animator2D)
				{
					yield break;
				}
			}
			while (!animator2D.get_reachedEndOfAnimation());
			FightObjectFactory.DestroyAnimatedObjectEffectInstance(animator2D);
		}
	}
}
