using UnityEngine;

public static class ObjectUtility
{
	public static void Destroy(Object o)
	{
		if (!(null == o))
		{
			Object.Destroy(o);
		}
	}
}
