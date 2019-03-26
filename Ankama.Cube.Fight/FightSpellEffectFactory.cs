using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight
{
	public class FightSpellEffectFactory : ScriptableObject
	{
		private struct SpellEffectOverrideData
		{
			private readonly ISpellEffectOverrideProvider m_spellDefinition;

			private readonly int m_eventId;

			public SpellEffectOverrideData(ISpellEffectOverrideProvider spellDefinition, int eventId)
			{
				m_spellDefinition = spellDefinition;
				m_eventId = eventId;
			}

			public bool TryGetSpellEffectOverride(SpellEffectKey key, int parentEventId, out SpellEffect spellEffect)
			{
				if (m_spellDefinition == null || parentEventId != m_eventId)
				{
					spellEffect = null;
					return false;
				}
				return m_spellDefinition.TryGetSpellEffectOverride(key, out spellEffect);
			}
		}

		private const string BundleName = "core/spells/effects";

		private static FightSpellEffectFactory s_instance;

		[SerializeField]
		private SpellEffectReferenceDictionary m_genericSpellEffects = new SpellEffectReferenceDictionary();

		[SerializeField]
		private PropertyEffectReferenceDictionary m_propertyEffects = new PropertyEffectReferenceDictionary();

		[SerializeField]
		private FloatingCounterEffectReferenceDictionary m_floatingCounterEffects = new FloatingCounterEffectReferenceDictionary();

		[SerializeField]
		private SightEffectReferenceDictionary m_sightEffects = new SightEffectReferenceDictionary();

		[SerializeField]
		private FloatingCounterFeedback m_floatingCounterFeedbackPrefab;

		private static Dictionary<SpellEffectKey, SpellEffect> s_spellEffectCache;

		private static Dictionary<PropertyId, AttachableEffect> s_propertyEffectCache;

		private static Dictionary<CaracId, FloatingCounterEffect> s_floatingCounterEffectCache;

		private static Dictionary<PropertyId, FloatingCounterEffect> s_sightEffectCache;

		private static GameObjectPool s_floatingCounterFeedbackPool;

		private static SpellEffectOverrideData[] s_currentSpellEffectOverrideData;

		private static List<SpellDefinition> s_loadedSpellDefinitions;

		public static bool isReady
		{
			get;
			private set;
		}

		public static IEnumerator Load(int fightCount)
		{
			if (isReady)
			{
				Log.Error("Load called while the fight object factory is already ready.", 91, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
				yield break;
			}
			AssetBundleLoadRequest bundleRequest = AssetManager.LoadAssetBundle("core/spells/effects");
			while (!bundleRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleRequest.get_error()) != 0)
			{
				Log.Error(string.Format("Error while loading bundle '{0}': {1}", "core/spells/effects", bundleRequest.get_error()), 103, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
				yield break;
			}
			AllAssetsLoadRequest<FightSpellEffectFactory> assetLoadRequest = AssetManager.LoadAllAssetsAsync<FightSpellEffectFactory>("core/spells/effects");
			while (!assetLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetLoadRequest.get_error()) != 0)
			{
				Log.Error(string.Format("Error while loading asset {0}: {1}", "FightSpellEffectFactory", assetLoadRequest.get_error()), 115, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
				yield break;
			}
			s_instance = assetLoadRequest.get_assets()[0];
			SpellEffectReferenceDictionary genericSpellEffects = s_instance.m_genericSpellEffects;
			Dictionary<SpellEffectKey, SpellEffect> spellEffectCache = s_spellEffectCache = new Dictionary<SpellEffectKey, SpellEffect>(((Dictionary<SpellEffectKey, AssetReference>)genericSpellEffects).Count, SpellEffectKeyComparer.instance);
			yield return PreloadEffectAssets(genericSpellEffects, spellEffectCache, "core/spells/effects");
			yield return LoadEffectsResources(spellEffectCache);
			PropertyEffectReferenceDictionary propertyEffects = s_instance.m_propertyEffects;
			Dictionary<PropertyId, AttachableEffect> propertyEffectCache = s_propertyEffectCache = new Dictionary<PropertyId, AttachableEffect>(((Dictionary<PropertyId, AssetReference>)propertyEffects).Count, PropertyIdComparer.instance);
			yield return PreloadEffectAssets(propertyEffects, propertyEffectCache, "core/spells/effects");
			yield return LoadEffectsResources(propertyEffectCache);
			SightEffectReferenceDictionary sightEffects = s_instance.m_sightEffects;
			Dictionary<PropertyId, FloatingCounterEffect> sightEffectCache = s_sightEffectCache = new Dictionary<PropertyId, FloatingCounterEffect>(((Dictionary<PropertyId, AssetReference>)sightEffects).Count, PropertyIdComparer.instance);
			yield return PreloadEffectAssets(sightEffects, sightEffectCache, "core/spells/effects");
			yield return LoadEffectsResources(sightEffectCache);
			FloatingCounterEffectReferenceDictionary floatingCounterEffects = s_instance.m_floatingCounterEffects;
			Dictionary<CaracId, FloatingCounterEffect> floatingCounterEffectCache = s_floatingCounterEffectCache = new Dictionary<CaracId, FloatingCounterEffect>(((Dictionary<CaracId, AssetReference>)floatingCounterEffects).Count, CaracIdComparer.instance);
			yield return PreloadEffectAssets(floatingCounterEffects, floatingCounterEffectCache, "core/spells/effects");
			yield return LoadEffectsResources(floatingCounterEffectCache);
			s_floatingCounterFeedbackPool = new GameObjectPool(s_instance.m_floatingCounterFeedbackPrefab.get_gameObject());
			s_loadedSpellDefinitions = new List<SpellDefinition>(24);
			s_currentSpellEffectOverrideData = new SpellEffectOverrideData[fightCount];
			isReady = true;
		}

		public static IEnumerator Unload()
		{
			if (isReady)
			{
				isReady = false;
				if (s_spellEffectCache != null)
				{
					UnloadCache(s_spellEffectCache);
					s_spellEffectCache = null;
				}
				if (s_propertyEffectCache != null)
				{
					UnloadCache(s_propertyEffectCache);
					s_propertyEffectCache = null;
				}
				if (s_sightEffectCache != null)
				{
					UnloadCache(s_sightEffectCache);
					s_sightEffectCache = null;
				}
				if (s_floatingCounterEffectCache != null)
				{
					UnloadCache(s_floatingCounterEffectCache);
					s_floatingCounterEffectCache = null;
				}
				if (s_floatingCounterFeedbackPool != null)
				{
					s_floatingCounterFeedbackPool.Clear();
					s_floatingCounterFeedbackPool = null;
				}
				if (s_loadedSpellDefinitions != null)
				{
					foreach (SpellDefinition s_loadedSpellDefinition in s_loadedSpellDefinitions)
					{
						if (null != s_loadedSpellDefinition)
						{
							s_loadedSpellDefinition.UnloadResources();
						}
					}
					s_loadedSpellDefinitions.Clear();
					s_loadedSpellDefinitions = null;
				}
				s_currentSpellEffectOverrideData = null;
				s_instance = null;
				AssetBundleUnloadRequest unloadRequest = AssetManager.UnloadAssetBundle("core/factories/fight_object_factory");
				while (!unloadRequest.get_isDone())
				{
					yield return null;
				}
			}
		}

		public static void NotifySpellDefinitionLoaded(SpellDefinition spellDefinition)
		{
			if (s_loadedSpellDefinitions == null)
			{
				Log.Error("NotifySpellDefinitionLoaded called while the factory is not ready.", 231, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
			}
			else
			{
				s_loadedSpellDefinitions.Add(spellDefinition);
			}
		}

		public static void SetupSpellEffectOverrides(ISpellEffectOverrideProvider definition, int fightId, int eventId)
		{
			if (s_currentSpellEffectOverrideData == null)
			{
				Log.Error("SetupSpellEffectOverrides called while the factory is not ready.", 242, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
			}
			else
			{
				s_currentSpellEffectOverrideData[fightId] = new SpellEffectOverrideData(definition, eventId);
			}
		}

		public static void ClearSpellEffectOverrides(int fightId)
		{
			if (s_currentSpellEffectOverrideData != null)
			{
				s_currentSpellEffectOverrideData[fightId] = new SpellEffectOverrideData(null, 0);
			}
		}

		public static bool TryGetSpellEffect(SpellEffectKey key, int fightId, int? parentEventId, out SpellEffect spellEffect)
		{
			if (s_spellEffectCache == null || s_currentSpellEffectOverrideData == null)
			{
				Log.Error("TryGetSpellEffect called while the factory is not ready.", 270, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
				spellEffect = null;
				return false;
			}
			if (parentEventId.HasValue && s_currentSpellEffectOverrideData[fightId].TryGetSpellEffectOverride(key, parentEventId.Value, out spellEffect))
			{
				return true;
			}
			return s_spellEffectCache.TryGetValue(key, out spellEffect);
		}

		public static IEnumerator PlayGenericEffect(SpellEffectKey key, int fightId, int? parentEventId, [NotNull] IsoObject target, [CanBeNull] FightContext fightContext)
		{
			if (s_spellEffectCache == null || s_currentSpellEffectOverrideData == null)
			{
				Log.Error("PlayGenericEffect called while the factory is not ready.", 290, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
			}
			else
			{
				if (((!parentEventId.HasValue || !s_currentSpellEffectOverrideData[fightId].TryGetSpellEffectOverride(key, parentEventId.Value, out SpellEffect value)) && !s_spellEffectCache.TryGetValue(key, out value)) || null == value)
				{
					yield break;
				}
				CellObject cellObject = target.cellObject;
				if (null == cellObject)
				{
					Log.Warning($"Tried to play generic effect {key} on target named {target.get_name()} ({((object)target).GetType().Name}) but the target is no longer on the board.", 313, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
					yield break;
				}
				Transform transform = cellObject.get_transform();
				Quaternion rotation = Quaternion.get_identity();
				Vector3 scale = Vector3.get_one();
				ITimelineContextProvider timelineContextProvider = target as ITimelineContextProvider;
				switch (value.orientationMethod)
				{
				case SpellEffect.OrientationMethod.None:
				{
					CameraHandler current = CameraHandler.current;
					if (null != current)
					{
						rotation = current.mapRotation.GetInverseRotation();
					}
					break;
				}
				case SpellEffect.OrientationMethod.Context:
				{
					VisualEffectContext visualEffectContext;
					if (timelineContextProvider != null && (visualEffectContext = (timelineContextProvider.GetTimelineContext() as VisualEffectContext)) != null)
					{
						visualEffectContext.GetVisualEffectTransformation(out rotation, out scale);
					}
					break;
				}
				case SpellEffect.OrientationMethod.SpellEffectTarget:
					Log.Warning($"Spell effect named '{value.get_name()}' orientation method is {SpellEffect.OrientationMethod.SpellEffectTarget} but is not played from a spell.", 346, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				yield return PlaySpellEffect(value, transform, rotation, scale, 0f, fightContext, timelineContextProvider);
			}
		}

		public static IEnumerator PlayGenericEffect(SpellEffectKey key, int fightId, int? parentEventId, [NotNull] Transform parent, Quaternion rotation, Vector3 scale, [CanBeNull] FightContext fightContext, [CanBeNull] ITimelineContextProvider contextProvider)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			SpellEffect spellEffect;
			if (s_spellEffectCache == null || s_currentSpellEffectOverrideData == null)
			{
				Log.Error("PlayGenericEffect called while the factory is not ready.", 361, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
			}
			else if (((parentEventId.HasValue && s_currentSpellEffectOverrideData[fightId].TryGetSpellEffectOverride(key, parentEventId.Value, out spellEffect)) || s_spellEffectCache.TryGetValue(key, out spellEffect)) && !(null == spellEffect))
			{
				yield return PlaySpellEffect(spellEffect, parent, rotation, scale, 0f, fightContext, contextProvider);
			}
		}

		public static IEnumerator PlaySpellEffect([NotNull] SpellEffect spellEffect, Vector2Int coords, [NotNull] SpellEffectInstantiationData instantiationData, [NotNull] CastTargetContext castTargetContext)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!FightMap.current.TryGetCellObject(coords.get_x(), coords.get_y(), out CellObject cellObject))
			{
				yield break;
			}
			Transform transform = cellObject.get_transform();
			FightContext context = castTargetContext.fightStatus.context;
			ITimelineContextProvider timelineContextProvider = null;
			Quaternion rotation = Quaternion.get_identity();
			Vector3 scale = Vector3.get_one();
			switch (spellEffect.orientationMethod)
			{
			case SpellEffect.OrientationMethod.None:
			{
				CameraHandler current = CameraHandler.current;
				if (null != current)
				{
					rotation = current.mapRotation.GetInverseRotation();
				}
				break;
			}
			case SpellEffect.OrientationMethod.Context:
				if (cellObject.TryGetIsoObject(out CharacterObject isoObject))
				{
					timelineContextProvider = isoObject;
					VisualEffectContext visualEffectContext;
					if ((visualEffectContext = (timelineContextProvider.GetTimelineContext() as VisualEffectContext)) != null)
					{
						visualEffectContext.GetVisualEffectTransformation(out rotation, out scale);
					}
				}
				else
				{
					Log.Warning($"Spell effect named '{spellEffect.get_name()}' orientation method is {SpellEffect.OrientationMethod.Context} but context provider could not be found.", 426, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
				}
				break;
			case SpellEffect.OrientationMethod.SpellEffectTarget:
				rotation = instantiationData.GetOrientation(coords, castTargetContext);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			float delayOverDistance = instantiationData.GetDelayOverDistance(coords);
			yield return PlaySpellEffect(spellEffect, transform, rotation, scale, delayOverDistance, context, timelineContextProvider);
		}

		public static IEnumerator PlaySpellEffect([NotNull] SpellEffect spellEffect, [NotNull] IsoObject view, [NotNull] SpellEffectInstantiationData instantiationData, [NotNull] CastTargetContext castTargetContext)
		{
			CellObject cellObject = view.cellObject;
			if (null == cellObject)
			{
				Log.Warning("Tried to play spell effect " + spellEffect.get_name() + " on target named " + view.get_name() + " (" + ((object)view).GetType().Name + ") but the target is no longer on the board.", 449, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
				yield break;
			}
			Transform transform = cellObject.get_transform();
			FightContext context = castTargetContext.fightStatus.context;
			ITimelineContextProvider timelineContextProvider = view as ITimelineContextProvider;
			Quaternion rotation = Quaternion.get_identity();
			Vector3 scale = Vector3.get_one();
			switch (spellEffect.orientationMethod)
			{
			case SpellEffect.OrientationMethod.None:
			{
				CameraHandler current = CameraHandler.current;
				if (null != current)
				{
					rotation = current.mapRotation.GetInverseRotation();
				}
				break;
			}
			case SpellEffect.OrientationMethod.Context:
			{
				VisualEffectContext visualEffectContext;
				if (timelineContextProvider != null && (visualEffectContext = (timelineContextProvider.GetTimelineContext() as VisualEffectContext)) != null)
				{
					visualEffectContext.GetVisualEffectTransformation(out rotation, out scale);
				}
				break;
			}
			case SpellEffect.OrientationMethod.SpellEffectTarget:
				rotation = instantiationData.GetOrientation(cellObject.coords, castTargetContext);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			float delayOverDistance = instantiationData.GetDelayOverDistance(cellObject.coords);
			yield return PlaySpellEffect(spellEffect, transform, rotation, scale, delayOverDistance, context, timelineContextProvider);
		}

		public static IEnumerator PlaySpellEffect([NotNull] SpellEffect spellEffect, [NotNull] Transform transform, Quaternion rotation, Vector3 scale, float delay, [CanBeNull] FightContext fightContext, [CanBeNull] ITimelineContextProvider contextProvider)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (delay > 0f)
			{
				yield return (object)new WaitForTime(delay);
			}
			Component instance = spellEffect.Instantiate(transform, rotation, scale, fightContext, contextProvider);
			if (!(null != instance))
			{
				yield break;
			}
			switch (spellEffect.waitMethod)
			{
			case SpellEffect.WaitMethod.None:
			{
				MonoBehaviour current2 = FightMap.current;
				if (null != current2)
				{
					current2.StartCoroutine(spellEffect.DestroyWhenFinished(instance));
				}
				break;
			}
			case SpellEffect.WaitMethod.Delay:
				yield return (object)new WaitForTime(spellEffect.waitDelay);
				if (null != instance)
				{
					MonoBehaviour current = FightMap.current;
					if (null != current)
					{
						current.StartCoroutine(spellEffect.DestroyWhenFinished(instance));
					}
				}
				break;
			case SpellEffect.WaitMethod.Destruction:
				yield return spellEffect.DestroyWhenFinished(instance);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static bool TryGetPropertyEffect(PropertyId propertyId, out AttachableEffect attachableEffect)
		{
			return s_propertyEffectCache.TryGetValue(propertyId, out attachableEffect);
		}

		public static bool TryGetFloatingCounterEffect(CaracId counterId, PropertyId? propertyId, out FloatingCounterEffect floatingEffectCounter)
		{
			if (counterId == CaracId.FloatingCounterSight && propertyId.HasValue)
			{
				return s_sightEffectCache.TryGetValue(propertyId.Value, out floatingEffectCounter);
			}
			return s_floatingCounterEffectCache.TryGetValue(counterId, out floatingEffectCounter);
		}

		public static FloatingCounterFeedback InstantiateFloatingCounterFeedback(Transform parent)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return s_floatingCounterFeedbackPool.Instantiate(parent.get_position(), parent.get_rotation(), parent).GetComponent<FloatingCounterFeedback>();
		}

		public static void DestroyFloatingCounterFeedback([NotNull] FloatingCounterFeedback instance)
		{
			instance.Clear();
			s_floatingCounterFeedbackPool.Release(instance.get_gameObject());
		}

		private static IEnumerator PreloadEffectAssets<K, V>(SerializableDictionaryLogic<K, AssetReference> effects, Dictionary<K, V> effectCache, string bundleName) where V : ScriptableEffect
		{
			int count = ((Dictionary<K, AssetReference>)effects).Count;
			if (count != 0)
			{
				AssetLoadRequest<V>[] loadRequests = new AssetLoadRequest<V>[count];
				int num = 0;
				foreach (AssetReference value in ((Dictionary<K, AssetReference>)effects).Values)
				{
					AssetReference current = value;
					if (current.get_hasValue())
					{
						loadRequests[num] = current.LoadFromAssetBundleAsync<V>(bundleName);
					}
					num++;
				}
				yield return EnumeratorUtility.ParallelRecursiveImmediateExecution((IEnumerator[])loadRequests);
				num = 0;
				foreach (K key in ((Dictionary<K, AssetReference>)effects).Keys)
				{
					AssetLoadRequest<V> val = loadRequests[num];
					num++;
					if (val != null)
					{
						if (AssetManagerError.op_Implicit(val.get_error()) != 0)
						{
							Log.Error($"Failed to load effect for '{key}': {val.get_error()}", 608, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightSpellEffectFactory.cs");
						}
						else
						{
							effectCache.Add(key, val.get_asset());
						}
					}
				}
			}
		}

		private static IEnumerator LoadEffectsResources<K, V>(Dictionary<K, V> effectCache) where V : ScriptableEffect
		{
			yield return ScriptableEffect.LoadAll(effectCache.Values);
		}

		private static void UnloadCache<K, V>(Dictionary<K, V> effectCache) where V : ScriptableEffect
		{
			foreach (V value in effectCache.Values)
			{
				if (null != value)
				{
					value.Unload();
				}
			}
			effectCache.Clear();
		}

		public FightSpellEffectFactory()
			: this()
		{
		}
	}
}
