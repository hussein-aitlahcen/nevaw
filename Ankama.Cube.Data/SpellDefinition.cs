using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Fight;
using Ankama.Utilities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpellDefinition : EditableData, ICastableDefinition, IDefinitionWithTooltip, IDefinitionWithDescription, IDefinitionWithPrecomputedData, ISpellEffectOverrideProvider
	{
		private enum ResourceLoadingState
		{
			None,
			Loading,
			Loaded
		}

		private List<EventCategory> m_eventsInvalidatingCost;

		private List<EventCategory> m_eventsInvalidatingCasting;

		private PrecomputedData m_precomputedData;

		private SpellType m_spellType;

		private God m_god;

		private Element m_element;

		private List<SpellTag> m_tags;

		[LocalizedString("SPELL_{id}_NAME", "Spells", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[LocalizedString("SPELL_{id}_DESCRIPTION", "Spells", 3)]
		[SerializeField]
		private int m_i18nDescriptionId;

		private List<GaugeValue> m_gaugeToModifyOnSpellPlay;

		private List<Cost> m_costs;

		private ICastTargetDefinition m_castTarget;

		private List<SpellEffectInstantiationData> m_spellEffectData;

		[SerializeField]
		private AssetReference m_illustrationReference;

		[SerializeField]
		private SpellEffectReferenceDictionary m_spellEffectOverrideReferences = new SpellEffectReferenceDictionary();

		[NonSerialized]
		private ResourceLoadingState m_resourceLoadingState;

		[NonSerialized]
		private SpellEffect[] m_spellEffects;

		[NonSerialized]
		private Dictionary<SpellEffectKey, SpellEffect> m_spellEffectOverrides;

		[SerializeField]
		private ElementaryStates m_elementaryStates = ElementaryStates.None;

		public IReadOnlyList<EventCategory> eventsInvalidatingCost => m_eventsInvalidatingCost;

		public IReadOnlyList<EventCategory> eventsInvalidatingCasting => m_eventsInvalidatingCasting;

		public PrecomputedData precomputedData => m_precomputedData;

		public SpellType spellType => m_spellType;

		public God god => m_god;

		public Element element => m_element;

		public IReadOnlyList<SpellTag> tags => m_tags;

		public int i18nNameId => m_i18nNameId;

		public int i18nDescriptionId => m_i18nDescriptionId;

		public IReadOnlyList<GaugeValue> gaugeToModifyOnSpellPlay => m_gaugeToModifyOnSpellPlay;

		public IReadOnlyList<Cost> costs => m_costs;

		public ICastTargetDefinition castTarget => m_castTarget;

		public IReadOnlyList<SpellEffectInstantiationData> spellEffectData => m_spellEffectData;

		public AssetReference illustrationReference => m_illustrationReference;

		public string illustrationBundleName => "core/ui/spells";

		public ElementaryStates elementaryStates => m_elementaryStates;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
			m_eventsInvalidatingCost = Serialization.JsonArrayAsList<EventCategory>(jsonObject, "eventsInvalidatingCost");
			m_eventsInvalidatingCasting = Serialization.JsonArrayAsList<EventCategory>(jsonObject, "eventsInvalidatingCasting");
			m_precomputedData = PrecomputedData.FromJsonProperty(jsonObject, "precomputedData");
			m_spellType = (SpellType)Serialization.JsonTokenValue<int>(jsonObject, "spellType", 1);
			m_god = (God)Serialization.JsonTokenValue<int>(jsonObject, "god", 0);
			m_element = (Element)Serialization.JsonTokenValue<int>(jsonObject, "element", 0);
			m_tags = Serialization.JsonArrayAsList<SpellTag>(jsonObject, "tags");
			JArray val = Serialization.JsonArray(jsonObject, "gaugeToModifyOnSpellPlay");
			m_gaugeToModifyOnSpellPlay = new List<GaugeValue>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_gaugeToModifyOnSpellPlay.Add(GaugeValue.FromJsonToken(item));
				}
			}
			JArray val2 = Serialization.JsonArray(jsonObject, "costs");
			m_costs = new List<Cost>((val2 != null) ? val2.get_Count() : 0);
			if (val2 != null)
			{
				foreach (JToken item2 in val2)
				{
					m_costs.Add(Cost.FromJsonToken(item2));
				}
			}
			m_castTarget = ICastTargetDefinitionUtils.FromJsonProperty(jsonObject, "castTarget");
			JArray val3 = Serialization.JsonArray(jsonObject, "spellEffectData");
			m_spellEffectData = new List<SpellEffectInstantiationData>((val3 != null) ? val3.get_Count() : 0);
			if (val3 != null)
			{
				foreach (JToken item3 in val3)
				{
					m_spellEffectData.Add(SpellEffectInstantiationData.FromJsonToken(item3));
				}
			}
		}

		public SpellEffect GetSpellEffect(int index)
		{
			if (index >= m_spellEffects.Length)
			{
				return null;
			}
			return m_spellEffects[index];
		}

		public bool TryGetSpellEffectOverride(SpellEffectKey key, out SpellEffect spellEffect)
		{
			if (m_spellEffectOverrides == null)
			{
				spellEffect = null;
				return false;
			}
			return m_spellEffectOverrides.TryGetValue(key, out spellEffect);
		}

		public IEnumerator LoadResources()
		{
			switch (m_resourceLoadingState)
			{
			case ResourceLoadingState.Loaded:
				break;
			case ResourceLoadingState.Loading:
				do
				{
					yield return null;
				}
				while (m_resourceLoadingState == ResourceLoadingState.Loading);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case ResourceLoadingState.None:
			{
				if (!FightSpellEffectFactory.isReady)
				{
					break;
				}
				m_resourceLoadingState = ResourceLoadingState.Loading;
				int spellEffectReferenceCount = m_spellEffectData.Count;
				int count = ((Dictionary<SpellEffectKey, AssetReference>)m_spellEffectOverrideReferences).Count;
				int num = spellEffectReferenceCount + count;
				AssetLoadRequest<SpellEffect>[] loadRequests = new AssetLoadRequest<SpellEffect>[num];
				IEnumerator[] spellEffectLoadRequests = new IEnumerator[num];
				int i;
				for (i = 0; i < spellEffectReferenceCount; i++)
				{
					string spellEffect = m_spellEffectData[i].spellEffect;
					if (spellEffect.Length > 0)
					{
						loadRequests[i] = AssetManager.LoadAssetAsync<SpellEffect>(spellEffect, "core/spells/effects");
						continue;
					}
					Log.Warning($"Spell named '{this.get_displayName()}' has an unassigned spell effect at index {i}.", 118, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Spells\\SpellDefinition.cs");
					loadRequests[i] = null;
				}
				foreach (AssetReference value in ((Dictionary<SpellEffectKey, AssetReference>)m_spellEffectOverrideReferences).Values)
				{
					AssetReference current = value;
					if (current.get_hasValue())
					{
						loadRequests[i] = current.LoadFromAssetBundleAsync<SpellEffect>("core/spells/effects");
					}
					else
					{
						loadRequests[i] = null;
					}
					i++;
				}
				yield return EnumeratorUtility.ParallelRecursiveSafeExecution((IEnumerator[])loadRequests);
				SpellEffect[] spellEffects = new SpellEffect[spellEffectReferenceCount];
				for (i = 0; i < spellEffectReferenceCount; i++)
				{
					AssetLoadRequest<SpellEffect> val = loadRequests[i];
					if (val == null)
					{
						spellEffects[i] = null;
						continue;
					}
					if (AssetManagerError.op_Implicit(val.get_error()) == 0)
					{
						spellEffectLoadRequests[i] = (spellEffects[i] = val.get_asset()).Load();
						continue;
					}
					spellEffects[i] = null;
					Log.Error($"Could not load spell effect for spell {this.get_name()}: {val.get_error()}", 159, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Spells\\SpellDefinition.cs");
				}
				Dictionary<SpellEffectKey, SpellEffect> spellEffectOverrides = new Dictionary<SpellEffectKey, SpellEffect>(((Dictionary<SpellEffectKey, AssetReference>)m_spellEffectOverrideReferences).Count, SpellEffectKeyComparer.instance);
				foreach (SpellEffectKey key in ((Dictionary<SpellEffectKey, AssetReference>)m_spellEffectOverrideReferences).Keys)
				{
					AssetLoadRequest<SpellEffect> val2 = loadRequests[i];
					if (val2 == null)
					{
						spellEffectOverrides.Add(key, null);
					}
					else if (AssetManagerError.op_Implicit(val2.get_error()) == 0)
					{
						SpellEffect asset = val2.get_asset();
						spellEffectOverrides.Add(key, asset);
						spellEffectLoadRequests[i] = asset.Load();
					}
					else
					{
						spellEffectOverrides.Add(key, null);
						Log.Error($"Could not load spell effect override for key {key} for spell {this.get_name()}: {val2.get_error()}", 183, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Spells\\SpellDefinition.cs");
					}
					i++;
				}
				yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(spellEffectLoadRequests);
				m_spellEffects = spellEffects;
				m_spellEffectOverrides = spellEffectOverrides;
				m_resourceLoadingState = ResourceLoadingState.Loaded;
				FightSpellEffectFactory.NotifySpellDefinitionLoaded(this);
				break;
			}
			}
		}

		public void UnloadResources()
		{
			if (m_resourceLoadingState == ResourceLoadingState.None)
			{
				return;
			}
			if (m_spellEffects != null)
			{
				int num = m_spellEffects.Length;
				for (int i = 0; i < num; i++)
				{
					SpellEffect spellEffect = m_spellEffects[i];
					if (null != spellEffect)
					{
						spellEffect.Unload();
					}
				}
				m_spellEffects = null;
			}
			if (m_spellEffectOverrides != null)
			{
				foreach (SpellEffect value in m_spellEffectOverrides.Values)
				{
					if (null != value)
					{
						value.Unload();
					}
				}
				m_spellEffectOverrides.Clear();
				m_spellEffectOverrides = null;
			}
			m_resourceLoadingState = ResourceLoadingState.None;
		}

		public int? GetBaseCost(int level)
		{
			SpellDefinitionContext context = new SpellDefinitionContext(this, level);
			return GetCost(context);
		}

		public int? GetCost(DynamicValueContext context)
		{
			int? num = null;
			foreach (Cost cost in m_costs)
			{
				ActionPointsCost actionPointsCost = cost as ActionPointsCost;
				if (actionPointsCost != null)
				{
					if (!num.HasValue)
					{
						num = 0;
					}
					actionPointsCost.value.GetValue(context, out int value);
					num += value;
				}
			}
			return num;
		}

		public SpellDefinition()
			: this()
		{
		}
	}
}
