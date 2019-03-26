using Ankama.AssetManagement;
using Ankama.Cube.Audio;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using FMODUnity;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Maps/Monster Spawn Cell Definition")]
	public sealed class MonsterSpawnCellDefinition : ScriptableObject
	{
		private const int PoolCapacity = 8;

		[SerializeField]
		private GameObject m_prefab;

		[SerializeField]
		private VisualEffect m_appearanceEffect;

		[SerializeField]
		private AudioReference m_appearanceSound;

		[SerializeField]
		private float m_appearanceDelay = 0.25f;

		[SerializeField]
		private VisualEffect m_disappearanceEffect;

		[SerializeField]
		private AudioReference m_disappearanceSound;

		[SerializeField]
		private float m_disappearanceDelay = 0.25f;

		private GameObjectPool m_prefabPool;

		private bool m_loadedAudioBank;

		public VisualEffect appearanceEffect => m_appearanceEffect;

		public AudioReference appearanceSound => m_appearanceSound;

		public float appearanceDelay => m_appearanceDelay;

		public VisualEffect disappearanceEffect => m_disappearanceEffect;

		public AudioReference disappearanceSound => m_disappearanceSound;

		public float disappearanceDelay => m_disappearanceDelay;

		public IEnumerator Initialize()
		{
			if (null == m_prefab)
			{
				yield break;
			}
			m_prefabPool = new GameObjectPool(m_prefab, 8);
			if (!m_appearanceSound.get_isValid() || !AudioManager.isReady)
			{
				yield break;
			}
			if (AudioManager.TryGetDefaultBankName(m_appearanceSound, out string bankName))
			{
				AudioBankLoadRequest bankLoadRequest = AudioManager.LoadBankAsync(bankName);
				while (!bankLoadRequest.isDone)
				{
					yield return null;
				}
				if (AssetManagerError.op_Implicit(bankLoadRequest.error) != 0)
				{
					Log.Error($"Failed to load bank named '{bankName}': {bankLoadRequest.error}", 105, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Maps\\MonsterSpawnCellDefinition.cs");
				}
				else
				{
					m_loadedAudioBank = true;
				}
			}
			else
			{
				Log.Warning($"Could not find a bank to load for event '{m_appearanceSound}'.", 113, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Maps\\MonsterSpawnCellDefinition.cs");
			}
		}

		public void Release()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (m_prefabPool != null)
			{
				m_prefabPool.Dispose();
				m_prefabPool = null;
			}
			if (m_loadedAudioBank && m_appearanceSound.get_isValid() && AudioManager.isReady && AudioManager.TryGetDefaultBankName(m_appearanceSound, out string bankName))
			{
				AudioManager.UnloadBank(bankName);
			}
		}

		[CanBeNull]
		public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (m_prefabPool == null)
			{
				Log.Error("Missing prefab for Monster SpawnCell Definition named '" + this.get_name() + "'.", 145, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Maps\\MonsterSpawnCellDefinition.cs");
				return null;
			}
			return m_prefabPool.Instantiate(position, rotation, parent);
		}

		public void DestroyInstance([NotNull] GameObject instance)
		{
			if (m_prefabPool == null)
			{
				Object.Destroy(instance);
			}
			else
			{
				m_prefabPool.Release(instance);
			}
		}

		public MonsterSpawnCellDefinition()
			: this()
		{
		}
	}
}
