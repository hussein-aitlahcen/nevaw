using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Debug;
using Ankama.Cube.UI.Fight.Info;
using Ankama.Cube.UI.Fight.TeamCounter;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public class FightUIFactory : ScriptableObject
	{
		[Serializable]
		private struct CastItemData
		{
			[SerializeField]
			private Material m_selectedMaterial;

			[SerializeField]
			private CastHighlight m_castHighlight;

			[SerializeField]
			private GameObject m_castingFX;

			public Material selectedMaterial => m_selectedMaterial;

			public CastHighlight castHighlight => m_castHighlight;

			public GameObject castingFX => m_castingFX;
		}

		[Serializable]
		private class ElementaryStatesSpriteDictionary : StatesDictionary<Sprite>
		{
		}

		[Serializable]
		private class ElementCaracIdSpriteDictionary : ElementCaracsDictionary<Sprite>
		{
		}

		[Serializable]
		private class SpellDataDictionary : ElementsDictionary<CastItemData>
		{
		}

		[Header("Prefabs")]
		[SerializeField]
		private PlayerUIRework m_playerUIPrefab;

		[SerializeField]
		private DebugFightUI m_debugUIPrefab;

		[SerializeField]
		private FightInfoMessageRoot m_fightInfoMessageRootPrefab;

		[SerializeField]
		private TeamPointCounter m_teamPointCounterPrefab;

		[Space(10f)]
		[SerializeField]
		private ElementCaracIdSpriteDictionary m_gaugeSprites;

		[Space(10f)]
		[SerializeField]
		private ElementaryStatesSpriteDictionary m_elementaryStatesSprites;

		[Space(10f)]
		[SerializeField]
		private SpellDataDictionary m_uiSpellCastData;

		[SerializeField]
		private CastItemData m_uiCompanionCastData;

		private static readonly Dictionary<CastHighlight, GameObjectPool> s_castHighlightPools = new Dictionary<CastHighlight, GameObjectPool>();

		private static readonly List<KeyValuePair<CastHighlight, GameObjectPool>> s_castHighlightInstances = new List<KeyValuePair<CastHighlight, GameObjectPool>>();

		public static bool isReady
		{
			get;
			private set;
		}

		public PlayerUIRework CreatePlayerUI(PlayerStatus playerStatus, Transform parent)
		{
			return Object.Instantiate<PlayerUIRework>(m_playerUIPrefab, parent);
		}

		public DebugFightUI CreateDebugUI(Transform parent)
		{
			return Object.Instantiate<DebugFightUI>(m_debugUIPrefab, parent);
		}

		public FightInfoMessageRoot CreateMessageRibbonRoot(Transform parent)
		{
			return Object.Instantiate<FightInfoMessageRoot>(m_fightInfoMessageRootPrefab, parent);
		}

		public TeamPointCounter CreateTeamPointCounter(Transform parent)
		{
			return Object.Instantiate<TeamPointCounter>(m_teamPointCounterPrefab, parent);
		}

		public static IEnumerator Load()
		{
			if (isReady)
			{
				Log.Error("Load called while the FightUIFactory is already ready.", 198, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
			}
			else
			{
				isReady = true;
			}
			yield break;
		}

		public static IEnumerator Unload()
		{
			if (isReady)
			{
				foreach (GameObjectPool value in s_castHighlightPools.Values)
				{
					value.Dispose();
				}
				s_castHighlightPools.Clear();
				s_castHighlightInstances.Clear();
				isReady = false;
			}
			yield break;
		}

		public Material GetSpellSelectedMaterial(SpellDefinition definition)
		{
			if (!((Dictionary<Element, CastItemData>)m_uiSpellCastData).TryGetValue(definition.element, out CastItemData value))
			{
				Log.Error($"No spellData assigned to element {definition.element}", 228, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
				return null;
			}
			return value.selectedMaterial;
		}

		public void Initialize(GaugeItemUI ui, CaracId element)
		{
			((Dictionary<CaracId, Sprite>)m_gaugeSprites).TryGetValue(element, out Sprite value);
			if (null == value)
			{
				Log.Error($"No sprite assigned to element {element}", 241, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
			}
			ui.SetSprite(value);
		}

		public void Initialize(Image ui, ElementaryStates elementaryStates)
		{
			if (elementaryStates == ElementaryStates.None)
			{
				ui.set_sprite(null);
			}
			else
			{
				((Dictionary<ElementaryStates, Sprite>)m_elementaryStatesSprites).TryGetValue(elementaryStates, out Sprite value);
				ui.set_sprite(value);
			}
			ui.set_enabled(ui.get_sprite() != null);
		}

		private CastHighlight GetCastHighlight<T>(T definition) where T : ICastableDefinition
		{
			SpellDefinition spellDefinition = definition as SpellDefinition;
			if (spellDefinition != null)
			{
				if (!((Dictionary<Element, CastItemData>)m_uiSpellCastData).TryGetValue(spellDefinition.element, out CastItemData value))
				{
					Log.Error($"No spellData assigned to element {spellDefinition.element}", 271, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
					return null;
				}
				return value.castHighlight;
			}
			if (definition as CompanionDefinition != null)
			{
				return m_uiCompanionCastData.castHighlight;
			}
			Log.Error("Definition type not handled: " + definition.GetType().Name, 284, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
			return null;
		}

		public CastHighlight CreateCastHighlight<T>(T item, Transform parent) where T : ICastableStatus
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			ICastableDefinition definition = item.GetDefinition();
			CastHighlight castHighlight = GetCastHighlight(definition);
			if (castHighlight == null)
			{
				Log.Error(string.Format("No {0} prefab defined for {1}: {2}", "CastHighlight", item.GetType().Name, definition), 297, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
				return null;
			}
			if (!s_castHighlightPools.TryGetValue(castHighlight, out GameObjectPool value))
			{
				value = new GameObjectPool(castHighlight.get_gameObject());
				s_castHighlightPools.Add(castHighlight, value);
			}
			CastHighlight component = value.Instantiate(parent, true).GetComponent<CastHighlight>();
			if (component != null)
			{
				s_castHighlightInstances.Add(new KeyValuePair<CastHighlight, GameObjectPool>(component, value));
				component.get_transform().set_localPosition(Vector3.get_zero());
				component.Play();
			}
			return component;
		}

		public void DestroyCellHighlight(CastHighlight cellCastHighlight)
		{
			if (cellCastHighlight == null)
			{
				return;
			}
			cellCastHighlight.Stop();
			for (int num = s_castHighlightInstances.Count - 1; num >= 0; num--)
			{
				KeyValuePair<CastHighlight, GameObjectPool> keyValuePair = s_castHighlightInstances[num];
				if (keyValuePair.Key == cellCastHighlight)
				{
					s_castHighlightInstances.RemoveAt(num);
					keyValuePair.Value.Release(cellCastHighlight.get_gameObject());
					return;
				}
			}
			Log.Error("no pool found", cellCastHighlight, 337, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
			Object.Destroy(cellCastHighlight.get_gameObject());
		}

		private GameObject GetCastFX<T>(T definition) where T : ICastableDefinition
		{
			SpellDefinition spellDefinition = definition as SpellDefinition;
			if (spellDefinition != null)
			{
				if (!((Dictionary<Element, CastItemData>)m_uiSpellCastData).TryGetValue(spellDefinition.element, out CastItemData value))
				{
					Log.Error($"No spellData assigned to element {spellDefinition.element}", 353, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
					return null;
				}
				return value.castingFX;
			}
			if (definition as CompanionDefinition != null)
			{
				return m_uiCompanionCastData.castingFX;
			}
			Log.Error("Definition type not handled: " + definition.GetType().Name, 366, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
			return null;
		}

		public GameObject CreateCastFX<T>(T item, Vector3 position, Quaternion rotation) where T : ICastableStatus
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			ICastableDefinition definition = item.GetDefinition();
			GameObject castFX = GetCastFX(definition);
			if (castFX == null)
			{
				Log.Error($"No castFX prefab defined for {item.GetType().Name}: {definition}", 376, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\Gauges\\FightUIFactory.cs");
				return null;
			}
			GameObject val = Object.Instantiate<GameObject>(castFX);
			if (val != null)
			{
				val.get_transform().set_position(position);
				val.get_transform().set_rotation(rotation);
			}
			return val;
		}

		public void DestroyCastFX(GameObject castFx)
		{
			if (castFx != null)
			{
				Object.Destroy(castFx.get_gameObject());
			}
		}

		public FightUIFactory()
			: this()
		{
		}
	}
}
