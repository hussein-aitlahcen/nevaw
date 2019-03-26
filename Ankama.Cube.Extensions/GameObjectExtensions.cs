using Ankama.Utilities;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public static class GameObjectExtensions
	{
		public static Canvas GetRootCanvas(this GameObject go)
		{
			List<Canvas> list = ListPool<Canvas>.Get();
			go.GetComponentsInParent<Canvas>(false, list);
			Canvas result = null;
			if (list.Count != 0)
			{
				result = list[0];
			}
			ListPool<Canvas>.Release(list);
			return result;
		}

		public static void SetHideFlagsRecursively(this GameObject gameObject, HideFlags value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			gameObject.set_hideFlags(value);
			Transform transform = gameObject.get_transform();
			int childCount = transform.get_childCount();
			for (int i = 0; i < childCount; i++)
			{
				transform.GetChild(i).get_gameObject().SetHideFlagsRecursively(value);
			}
		}

		public static void SetLayerRecursively([NotNull] this GameObject gameObject, int value)
		{
			gameObject.set_layer(value);
			Transform transform = gameObject.get_transform();
			int childCount = transform.get_childCount();
			for (int i = 0; i < childCount; i++)
			{
				transform.GetChild(i).get_gameObject().SetLayerRecursively(value);
			}
		}
	}
}
