using Ankama.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps.VisualEffects
{
	public static class VisualEffectFactory
	{
		private const int InitialCapacity = 32;

		private static PrefabInstancePool s_pool;

		public static void Initialize()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			if (s_pool != null)
			{
				Log.Error("Initialize called but Dispose was not properly called since last call to Initialize.", 20, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\VisualEffects\\VisualEffectFactory.cs");
				s_pool.Dispose();
			}
			s_pool = new PrefabInstancePool(32);
		}

		public static void Dispose()
		{
			if (s_pool == null)
			{
				Log.Warning("Dispose called but Initialize was not properly called.", 33, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\VisualEffects\\VisualEffectFactory.cs");
				return;
			}
			s_pool.Dispose();
			s_pool = null;
		}

		public static void PreparePool([NotNull] GameObject prefab, int capacity = 2, int maxSize = 4)
		{
			if (s_pool != null)
			{
				s_pool.PreparePool(prefab, capacity, maxSize);
			}
		}

		[NotNull]
		public static VisualEffect Instantiate([NotNull] VisualEffect prefab, Vector3 position, Quaternion rotation, Vector3 scale, [CanBeNull] Transform parent)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = (s_pool != null) ? s_pool.Instantiate(prefab.get_gameObject(), position, rotation, parent) : ((null == parent) ? Object.Instantiate<GameObject>(prefab.get_gameObject(), position, rotation) : Object.Instantiate<GameObject>(prefab.get_gameObject(), position, rotation, parent));
			Transform transform = val.get_transform();
			Vector3 localScale = transform.get_localScale();
			localScale.Scale(scale);
			transform.set_localScale(localScale);
			return val.GetComponent<VisualEffect>();
		}

		public static void Release([NotNull] VisualEffect prefab, [NotNull] VisualEffect instance)
		{
			if (s_pool == null)
			{
				Object.Destroy(instance.get_gameObject());
			}
			else
			{
				s_pool.Release(prefab.get_gameObject(), instance.get_gameObject());
			}
		}
	}
}
