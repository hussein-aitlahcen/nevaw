using Ankama.AssetManagement;
using Ankama.Cube.Audio;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using DG.Tweening;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Data/Attachable Effects/Floating Counter Effect")]
	public sealed class FloatingCounterEffect : ScriptableEffect, ISpellEffectOverrideProvider
	{
		[Space(10f)]
		[SerializeField]
		private FloatingCounterFloatingObject m_floatingObject;

		[SerializeField]
		private float m_radius = 0.5f;

		[SerializeField]
		private float m_height = 0.5f;

		[SerializeField]
		private float m_rotationSpeed = 100f;

		[SerializeField]
		private Vector3 m_rotationAxis = new Vector3(0f, 1f, 0f);

		[SerializeField]
		private float m_repositionDuration = 0.25f;

		[SerializeField]
		private Ease m_repositionEase = 19;

		[Space(10f)]
		[SerializeField]
		private VisualEffect m_spawnFX;

		[SerializeField]
		private Vector3 m_spawnFXOffset = new Vector3(0f, 0.5f, 0f);

		[SerializeField]
		private AudioReferenceWithParameters m_spawnSound;

		[SerializeField]
		private float m_startingAnimationDuration = 0.25f;

		[Space(10f)]
		[SerializeField]
		private float m_endAnimationDuration = 0.5f;

		[SerializeField]
		private float m_clearAnimationDuration = 0.25f;

		[Space(10f)]
		[SerializeField]
		private SpellEffectDictionary m_spellEffectOverrides;

		[NonSerialized]
		private bool m_loadedSpawnEffectAudioBank;

		public Vector3 rotation => m_rotationAxis * m_rotationSpeed;

		public float radius => m_radius;

		public float height => m_height;

		public FloatingCounterFloatingObject floatingObject => m_floatingObject;

		public VisualEffect spawnFX => m_spawnFX;

		public Vector3 spawnFXOffset => m_spawnFXOffset;

		public float startingAnimationDuration => m_startingAnimationDuration;

		public float endAnimationDuration => m_endAnimationDuration;

		public float clearAnimationDuration => m_clearAnimationDuration;

		public float repositionDuration => m_repositionDuration;

		public Ease repositionEase => m_repositionEase;

		protected override IEnumerator LoadInternal()
		{
			AudioReferenceWithParameters spawnSound = m_spawnSound;
			if (spawnSound.get_isValid())
			{
				if (AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(spawnSound), out string bankName))
				{
					AudioBankLoadRequest bankLoadRequest = AudioManager.LoadBankAsync(bankName);
					while (!bankLoadRequest.isDone)
					{
						yield return null;
					}
					if (AssetManagerError.op_Implicit(bankLoadRequest.error) == 0)
					{
						m_loadedSpawnEffectAudioBank = true;
					}
					else
					{
						Log.Warning("Could not load bank named '" + bankName + "' for sound of visual character effect named '" + this.get_name() + "'.", 87, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\FloatingCounterEffect.cs");
					}
				}
				else
				{
					Log.Warning("Could not get default bank name for sound of visual effect named '" + this.get_name() + "'.", 92, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Animations\\FloatingCounterEffect.cs");
				}
			}
			if (m_spellEffectOverrides != null)
			{
				yield return ScriptableEffect.LoadAll(((Dictionary<SpellEffectKey, SpellEffect>)m_spellEffectOverrides).Values);
			}
			m_initializationState = InitializationState.Loaded;
		}

		protected override void UnloadInternal()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			AudioReferenceWithParameters spawnSound = m_spawnSound;
			if (spawnSound.get_isValid() && m_loadedSpawnEffectAudioBank && AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(spawnSound), out string bankName))
			{
				AudioManager.UnloadBank(bankName);
				m_loadedSpawnEffectAudioBank = false;
			}
			if (m_spellEffectOverrides != null)
			{
				foreach (SpellEffect value in ((Dictionary<SpellEffectKey, SpellEffect>)m_spellEffectOverrides).Values)
				{
					value.Unload();
				}
			}
			m_initializationState = InitializationState.None;
		}

		public bool TryGetSpellEffectOverride(SpellEffectKey key, out SpellEffect spellEffect)
		{
			if (m_spellEffectOverrides == null)
			{
				spellEffect = null;
				return false;
			}
			return ((Dictionary<SpellEffectKey, SpellEffect>)m_spellEffectOverrides).TryGetValue(key, out spellEffect);
		}

		public void PlaySound(Transform transform)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			AudioManager.PlayOneShot(m_spawnSound, transform);
		}
	}
}
