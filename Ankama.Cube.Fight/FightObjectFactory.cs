using Ankama.Animations;
using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Fight
{
	public class FightObjectFactory : ScriptableObject
	{
		private static FightObjectFactory s_instance;

		[Header("Character Prefabs")]
		[UsedImplicitly]
		[SerializeField]
		private GameObject m_heroCharacterPrefab;

		[UsedImplicitly]
		[SerializeField]
		private GameObject m_companionCharacterPrefab;

		[UsedImplicitly]
		[SerializeField]
		private GameObject m_summoningCharacterPrefab;

		[UsedImplicitly]
		[SerializeField]
		private GameObject m_objectMechanismPrefab;

		[UsedImplicitly]
		[SerializeField]
		private GameObject m_floorMechanismPrefab;

		[Header("Character Effects")]
		[UsedImplicitly]
		[SerializeField]
		private GameObject m_animatedObjectEffectPrefab;

		[UsedImplicitly]
		[SerializeField]
		private GameObject m_timelineAssetEffectPrefab;

		[Header("Feedbacks")]
		[UsedImplicitly]
		[SerializeField]
		private GameObject m_valueChangedFeedbackPrefab;

		private static GameObjectPool s_companionCharacterPool;

		private static GameObjectPool s_summoningCharacterPool;

		private static GameObjectPool s_objectMechanismCharacterPool;

		private static GameObjectPool s_floorMechanismCharacterPool;

		private static GameObjectPool s_animatedObjectEffectPool;

		private static GameObjectPool s_timelineAssetEffectPool;

		private static GameObjectPool s_valueChangedFeedbackPool;

		private static readonly Dictionary<Transform, int> s_valueChangedFeedbackCountPerTransform = new Dictionary<Transform, int>();

		public static bool isReady
		{
			get;
			private set;
		}

		public static IEnumerator Load()
		{
			if (isReady)
			{
				Log.Error("Load called while the fight object factory is already ready.", 61, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				yield break;
			}
			AssetBundleLoadRequest bundleRequest = AssetManager.LoadAssetBundle("core/factories/fight_object_factory");
			while (!bundleRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleRequest.get_error()) != 0)
			{
				Log.Error(string.Format("Error while loading bundle: {0} error={1}", "core/factories/fight_object_factory", bundleRequest.get_error()), 74, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				yield break;
			}
			AllAssetsLoadRequest<FightObjectFactory> assetLoadRequest = AssetManager.LoadAllAssetsAsync<FightObjectFactory>("core/factories/fight_object_factory");
			while (!assetLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetLoadRequest.get_error()) != 0)
			{
				Log.Error(string.Format("Error while loading asset: {0} error={1}", "FightObjectFactory", assetLoadRequest.get_error()), 85, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				yield break;
			}
			s_instance = assetLoadRequest.get_assets()[0];
			s_companionCharacterPool = new GameObjectPool(s_instance.m_companionCharacterPrefab, 2);
			s_summoningCharacterPool = new GameObjectPool(s_instance.m_summoningCharacterPrefab, 2);
			s_objectMechanismCharacterPool = new GameObjectPool(s_instance.m_objectMechanismPrefab, 2);
			s_floorMechanismCharacterPool = new GameObjectPool(s_instance.m_floorMechanismPrefab, 2);
			s_animatedObjectEffectPool = new GameObjectPool(s_instance.m_animatedObjectEffectPrefab, 2);
			s_timelineAssetEffectPool = new GameObjectPool(s_instance.m_timelineAssetEffectPrefab, 4);
			s_valueChangedFeedbackPool = new GameObjectPool(s_instance.m_valueChangedFeedbackPrefab, 4);
			isReady = true;
		}

		public static IEnumerator Unload()
		{
			if (isReady)
			{
				isReady = false;
				s_instance = null;
				DisposePool(ref s_companionCharacterPool);
				DisposePool(ref s_summoningCharacterPool);
				DisposePool(ref s_objectMechanismCharacterPool);
				DisposePool(ref s_floorMechanismCharacterPool);
				DisposePool(ref s_animatedObjectEffectPool);
				DisposePool(ref s_timelineAssetEffectPool);
				DisposePool(ref s_valueChangedFeedbackPool);
				s_valueChangedFeedbackCountPerTransform.Clear();
				AssetBundleUnloadRequest unloadRequest = AssetManager.UnloadAssetBundle("core/factories/fight_object_factory");
				while (!unloadRequest.get_isDone())
				{
					yield return null;
				}
			}
		}

		private static void DisposePool(ref GameObjectPool pool)
		{
			if (pool != null)
			{
				pool.Dispose();
				pool = null;
			}
		}

		public static HeroCharacterObject CreateHeroCharacterObject(WeaponDefinition definition, int x, int y, Direction direction)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			if (null == s_instance)
			{
				Log.Error("CreateHeroCharacterObject called while the factory is not ready.", 175, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			if (!FightMap.current.TryGetCellObject(x, y, out CellObject cellObject))
			{
				Log.Error(string.Format("{0} called with an invalid position {1}, {2}.", "CreateHeroCharacterObject", x, y), 184, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			Transform transform = cellObject.get_transform();
			Vector3 position = transform.get_position();
			position.y += 0.5f;
			HeroCharacterObject component = Object.Instantiate<GameObject>(s_instance.m_heroCharacterPrefab, position, Quaternion.get_identity(), transform).GetComponent<HeroCharacterObject>();
			component.InitializeDefinitionAndArea(definition, x, y);
			component.SetCellObject(cellObject);
			component.direction = direction;
			return component;
		}

		public static CompanionCharacterObject CreateCompanionCharacterObject(CompanionDefinition definition, int x, int y, Direction direction)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (null == s_instance)
			{
				Log.Error("CreateCompanionCharacterObject called while the factory is not ready.", 206, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			if (!FightMap.current.TryGetCellObject(x, y, out CellObject cellObject))
			{
				Log.Error(string.Format("{0} called with an invalid position {1}, {2}.", "CreateCompanionCharacterObject", x, y), 215, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			Transform transform = cellObject.get_transform();
			Vector3 position = transform.get_position();
			position.y += 0.5f;
			CompanionCharacterObject component = s_companionCharacterPool.Instantiate(position, Quaternion.get_identity(), transform).GetComponent<CompanionCharacterObject>();
			component.InitializeDefinitionAndArea(definition, x, y);
			component.SetCellObject(cellObject);
			component.direction = direction;
			return component;
		}

		public static void ReleaseCompanionCharacterObject([NotNull] CompanionCharacterObject instance)
		{
			if (s_companionCharacterPool != null)
			{
				s_companionCharacterPool.Release(instance.get_gameObject());
			}
		}

		public static SummoningCharacterObject CreateSummoningCharacterObject(SummoningDefinition definition, int x, int y, Direction direction)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (null == s_instance)
			{
				Log.Error("CreateSummoningCharacterObject called while the factory is not ready.", 245, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			if (!FightMap.current.TryGetCellObject(x, y, out CellObject cellObject))
			{
				Log.Error(string.Format("{0} called with an invalid position {1}, {2}.", "CreateSummoningCharacterObject", x, y), 254, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			Transform transform = cellObject.get_transform();
			Vector3 position = transform.get_position();
			position.y += 0.5f;
			SummoningCharacterObject component = s_summoningCharacterPool.Instantiate(position, Quaternion.get_identity(), transform).GetComponent<SummoningCharacterObject>();
			component.InitializeDefinitionAndArea(definition, x, y);
			component.SetCellObject(cellObject);
			component.direction = direction;
			return component;
		}

		public static void ReleaseSummoningCharacterObject([NotNull] SummoningCharacterObject instance)
		{
			if (s_summoningCharacterPool != null)
			{
				s_summoningCharacterPool.Release(instance.get_gameObject());
			}
		}

		public static ObjectMechanismObject CreateObjectMechanismObject(ObjectMechanismDefinition definition, int x, int y)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (null == s_instance)
			{
				Log.Error("CreateObjectMechanismObject called while the factory is not ready.", 284, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			if (!FightMap.current.TryGetCellObject(x, y, out CellObject cellObject))
			{
				Log.Error(string.Format("{0} called with an invalid position {1}, {2}.", "CreateObjectMechanismObject", x, y), 293, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			Transform transform = cellObject.get_transform();
			Vector3 position = transform.get_position();
			position.y += 0.5f;
			ObjectMechanismObject component = s_objectMechanismCharacterPool.Instantiate(position, Quaternion.get_identity(), transform).GetComponent<ObjectMechanismObject>();
			component.InitializeDefinitionAndArea(definition, x, y);
			component.SetCellObject(cellObject);
			return component;
		}

		public static void ReleaseObjectMechanismObject([NotNull] ObjectMechanismObject instance)
		{
			if (s_objectMechanismCharacterPool != null)
			{
				s_objectMechanismCharacterPool.Release(instance.get_gameObject());
			}
		}

		public static FloorMechanismObject CreateFloorMechanismObject(FloorMechanismDefinition definition, int x, int y)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (null == s_instance)
			{
				Log.Error("CreateFloorMechanismObject called while the factory is not ready.", 322, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			if (!FightMap.current.TryGetCellObject(x, y, out CellObject cellObject))
			{
				Log.Error(string.Format("{0} called with an invalid position {1}, {2}.", "CreateFloorMechanismObject", x, y), 331, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			Transform transform = cellObject.get_transform();
			Vector3 position = transform.get_position();
			position.y += 0.5f;
			FloorMechanismObject component = s_floorMechanismCharacterPool.Instantiate(position, Quaternion.get_identity(), transform).GetComponent<FloorMechanismObject>();
			component.InitializeDefinitionAndArea(definition, x, y);
			component.SetCellObject(cellObject);
			return component;
		}

		public static void ReleaseFloorMechanismObject([NotNull] FloorMechanismObject instance)
		{
			if (s_floorMechanismCharacterPool != null)
			{
				s_floorMechanismCharacterPool.Release(instance.get_gameObject());
			}
		}

		public static Animator2D CreateAnimatedObjectEffectInstance([NotNull] AnimatedObjectDefinition definition, string animationName, [NotNull] Transform parent)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (null == definition)
			{
				throw new NullReferenceException();
			}
			if (null == s_instance)
			{
				Log.Error("CreateAnimatedObjectEffectInstance called while the factory is not ready.", 367, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			Animator2D component = s_animatedObjectEffectPool.Instantiate(parent.get_position(), Quaternion.get_identity(), parent).GetComponent<Animator2D>();
			if (!string.IsNullOrEmpty(animationName))
			{
				component.SetAnimation(animationName, false, false, true);
			}
			else
			{
				component.set_animationLoops(false);
			}
			component.SetDefinition(definition, null, (Graphic[])null);
			return component;
		}

		public static void DestroyAnimatedObjectEffectInstance([NotNull] Animator2D instance)
		{
			if (null != s_instance)
			{
				s_animatedObjectEffectPool.Release(instance.get_gameObject());
			}
			else
			{
				Object.Destroy(instance.get_gameObject());
			}
		}

		public static PlayableDirector CreateTimelineAssetEffectInstance([NotNull] TimelineAsset timelineAsset, [NotNull] Transform parent, Quaternion rotation, Vector3 scale, [CanBeNull] FightContext fightContext, [CanBeNull] ITimelineContextProvider contextProvider)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (null == timelineAsset)
			{
				throw new NullReferenceException();
			}
			if (null == s_instance)
			{
				Log.Error("CreateTimelineAssetEffectInstance called while the factory is not ready.", 412, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				return null;
			}
			GameObject obj = s_timelineAssetEffectPool.Instantiate(parent.get_position(), rotation, parent);
			Vector3 localScale = obj.get_transform().get_localScale();
			localScale.Scale(scale);
			obj.get_transform().set_localScale(localScale);
			PlayableDirector component = obj.GetComponent<PlayableDirector>();
			if (null != fightContext)
			{
				TimelineContextUtility.SetFightContext(component, fightContext);
			}
			if (contextProvider != null)
			{
				TimelineContextUtility.SetContextProvider(component, contextProvider);
			}
			component.Play(timelineAsset, 2);
			return component;
		}

		public static void DestroyTimelineAssetEffectInstance(PlayableDirector instance, bool clearFightContext)
		{
			if (clearFightContext)
			{
				TimelineContextUtility.ClearFightContext(instance);
			}
			TimelineContextUtility.ClearContextProvider(instance);
			instance.set_playableAsset(null);
			if (null != s_instance)
			{
				s_timelineAssetEffectPool.Release(instance.get_gameObject());
			}
			else
			{
				Object.Destroy(instance.get_gameObject());
			}
		}

		public static ValueChangedFeedback CreateValueChangedFeedback(Transform parentTransform, out int instanceCountInTransform)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			if (s_valueChangedFeedbackPool == null)
			{
				Log.Error("CreateValueChangedFeedback called while the factory is not ready.", 465, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightObjectFactory.cs");
				instanceCountInTransform = 0;
				return null;
			}
			CameraHandler current = CameraHandler.current;
			Vector3 val;
			Quaternion val2;
			if (null != current)
			{
				val = parentTransform.get_position();
				val2 = Quaternion.LookRotation(current.get_transform().get_forward(), Vector3.get_up());
			}
			else
			{
				Transform transform = s_valueChangedFeedbackPool.get_prefab().get_transform();
				val = parentTransform.get_position() + transform.get_localPosition();
				val2 = transform.get_localRotation();
			}
			s_valueChangedFeedbackCountPerTransform.TryGetValue(parentTransform, out instanceCountInTransform);
			s_valueChangedFeedbackCountPerTransform[parentTransform] = instanceCountInTransform + 1;
			return s_valueChangedFeedbackPool.Instantiate(val, val2, parentTransform).GetComponent<ValueChangedFeedback>();
		}

		public static void ReleaseValueChangedFeedback(GameObject instance)
		{
			if (s_valueChangedFeedbackPool != null)
			{
				Transform parent = instance.get_transform().get_parent();
				if (null != parent && s_valueChangedFeedbackCountPerTransform.TryGetValue(parent, out int value))
				{
					s_valueChangedFeedbackCountPerTransform[parent] = value - 1;
				}
				s_valueChangedFeedbackPool.Release(instance);
			}
		}

		public FightObjectFactory()
			: this()
		{
		}
	}
}
