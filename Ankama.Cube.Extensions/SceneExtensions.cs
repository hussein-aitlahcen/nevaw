using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ankama.Cube.Extensions
{
	public static class SceneExtensions
	{
		public static T GetComponentInRootsChildren<T>(this Scene scene) where T : MonoBehaviour
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			int num = rootGameObjects.Length;
			for (int i = 0; i < num; i++)
			{
				T componentInChildren = rootGameObjects[i].GetComponentInChildren<T>();
				if ((object)componentInChildren != null)
				{
					return componentInChildren;
				}
			}
			return default(T);
		}
	}
}
